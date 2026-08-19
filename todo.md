# TODO

## Context

`CDS.ScriptChat.WinForms` wanted to render AI chat replies (Markdown prose,
occasionally with a table) inside a scrolling transcript made of many small
message bubbles — one bubble per turn, growing for the life of a session.
`MarkdownViewer` was the first thing tried, since it already does GitHub-
flavoured rendering well. It didn't fit that particular shape of consumer, for
three reasons that came out of actually trying to wire it into a
`ChatTurnView` (see `CDS.ScriptChat.WinForms/src/CDS.ScriptChat.WinForms/ChatTurnView.cs`
and its `Designer.cs` for the control being embedded into):

1. **The toolbar is always on.** `MarkdownViewer.Designer.cs` docks a Home /
   Back / Forward panel above the `WebView2` unconditionally. There's no
   property to hide it, and the fields (`panel1`, `btnHome`, `btnBack`,
   `btnForward`) are private. A single-turn chat bubble has nowhere to "go
   home" to, so this chrome is pure dead weight for that use case (and
   presumably for anyone else embedding the viewer inside a larger layout
   rather than using it as a standalone pane).

2. **No way to learn the rendered content height.** A host that wants to
   size itself to its content (a `FlowLayoutPanel` full of message bubbles is
   exactly this) has no signal for how tall the HTML actually rendered.
   WebView2 doesn't report this on its own, and `MarkdownViewer` doesn't
   surface anything today — you have to reach into the page yourself via
   `ExecuteScriptAsync`, which the control doesn't expose a hook for.

3. **One instance per message is a heavy default.** The control assumes a
   small number of long-lived instances (a docs pane, a help panel) — each
   one owns a `SemaphoreSlim`, a `TempHtmlFileManager`, and (unless the host
   explicitly shares `Options.UserDataFolder`/`ProfileName`) its own WebView2
   profile. Spinning up a fresh instance per chat turn, potentially dozens of
   times in a session, was enough of an open question that it stopped the
   integration before it started — nobody had measured whether that's
   actually viable.

None of this is a criticism of `MarkdownViewer` for what it was built for
(rendering a full document, README-style, in its own pane) — it's a mismatch
for "one of many small, cheap, auto-sizing instances embedded in someone
else's layout". `CDS.ScriptChat.WinForms` ended up solving its immediate need
with its own control instead (`MarkdownTextBox`, in that repo — a
`RichTextBox` subclass driven by Markdig, no WebView2, no navigation chrome,
reports its own preferred height directly). It only handles a subset of
Markdown (no Mermaid, no MathJax, tables rendered as a best-effort monospaced
grid rather than a real HTML table) — it's a text-formatting control, not a
document viewer, so that's an acceptable trade for that use case, not a
replacement for `MarkdownViewer`.

## Proposed changes

Each item below is independent and additive — see the "compatibility" note
on each for why it shouldn't need to break `MarkdownViewer`'s current public
surface or the Demo app.

### 1. Make the navigation toolbar optional

- **Reason:** it only makes sense when there's a "home" document and
  somewhere to navigate to/from. A viewer embedded as a fragment inside a
  larger UI (a chat bubble, a preview pane, a details panel) has neither.
- **Desired outcome:** something like `MarkdownViewerOptions.ShowNavigationToolbar`
  (`bool`, default `true`). When `false`, hide `panel1` and let `webView`
  dock to fill the whole control. Given the toolbar buttons are currently
  private fields with no public accessors, this likely needs the visibility
  toggle to live inside `MarkdownViewer` itself (e.g. reacting to the option
  in `ApplyThemeToWebView`-style setup, or a new `ApplyToolbarVisibility`
  step called from the constructor and whenever `Options` changes).
- **Compatibility:** default `true` reproduces today's behaviour for every
  existing consumer. Only a consumer that opts in sees anything different.

### 2. Report rendered content height back to the host

- **Reason:** a host that wants to size its own container to the content
  (rather than fixing a height up front) has no way to ask the control how
  tall its content actually rendered.
- **Desired outcome:** a new event — e.g. `event EventHandler<int> ContentHeightChanged`
  — raised after each successful navigation/render, plus perhaps an on-demand
  `Task<int> GetContentHeightAsync()`. Worth spiking both `ExecuteScriptAsync("document.body.scrollHeight")`
  polled once after `NavigationCompleted`, and a `ResizeObserver` in the
  injected HTML that posts back through `postMessage`/`WebMessageReceived`
  (the latter is more robust to content that keeps changing size after the
  initial paint — web fonts, async-rendered Mermaid diagrams, MathJax
  reflow — all of which this control already deals with).
- **Compatibility:** purely additive (new event/method) — no change to
  anything that exists today.

### 3. Document (or improve) the cost of many short-lived instances

- **Reason:** nobody has measured what "one `MarkdownViewer` per chat
  message, created and disposed continuously for a session" actually costs
  in memory and startup latency. That uncertainty is what stopped the
  `CDS.ScriptChat.WinForms` integration rather than a concrete number ruling
  it out.
- **Desired outcome:** at minimum, a short section in `docs/viewer.md`
  giving a ballpark (memory per instance, time to first paint) and stating
  plainly whether "many short-lived instances" is a supported scenario or
  not. If it isn't, the doc should say what to do instead — share a
  `CoreWebView2Environment`/profile across instances via the existing
  `Options.UserDataFolder`/`ProfileName`, or reuse a pool of viewers rather
  than creating one per item. If sharing isn't fully wired up today, that's
  the follow-up implementation task; the doc is the part that unblocks
  someone else's decision either way.
- **Compatibility:** doc-only, or additive if a pooling/sharing API gets
  added.

### 4. Decide whether a lighter, non-WebView2 control belongs in this solution

- **Reason:** `CDS.ScriptChat.WinForms` built `MarkdownTextBox` (Markdig +
  `RichTextBox`, no browser process, no navigation chrome, reports its own
  preferred height synchronously off `RichTextBox.ContentsResized`) to solve
  its immediate need rather than wait on the above. That's a reasonable
  "good enough" formatting control for prose-in-a-list scenarios generally,
  not something specific to chat.
- **Desired outcome:** a deliberate decision, not an accident of "whichever
  repo happened to need it first" — either (a) leave it where it is, in
  `CDS.ScriptChat.WinForms`, and this solution stays WebView2-only, or (b)
  pull a generalised version of it into `CDS.Markdown` as a second control
  (e.g. `MarkdownTextBox` alongside `MarkdownViewer`) so every consumer gets
  the lightweight option for free. If you go with (b), the version in
  `CDS.ScriptChat.WinForms` is a working reference for the block/inline
  coverage it handles (headings, bold/italic, inline and fenced code, lists
  incl. nesting, block quotes, thematic breaks, and tables rendered as a
  padded monospaced grid) and what it deliberately doesn't attempt (Mermaid,
  MathJax, real HTML tables, images beyond an italic placeholder, clickable
  links).
- **Compatibility:** a wholly new class either way — zero risk to
  `MarkdownViewer` consumers regardless of which way this goes.

**Resolved: option (b).** `MarkdownTextBox` was pulled into `CDS.Markdown` (see
`CDS.Markdown/MarkdownTextBox.cs`), generalised from the `CDS.ScriptChat.WinForms` original
(same behaviour and coverage, just the namespace and one doc comment referencing `ChatTurnView`
changed to something generic). Covered by `UnitTests/MarkdownTextBoxTests.cs` and a `Demo` menu
entry (`Demo/FormMarkdownTextBoxDemo.cs`, also reachable via `Demo.exe --textbox` for UI-automation
tests) verified live via `UiTests/MarkdownViewerUiTests.cs`'s `MarkdownTextBoxUiTests`. Documented
in `docs/viewer.md`'s new "MarkdownTextBox: a Lighter Alternative" section and in `README.md`.

### 5. Bring in FlaUI for UI-automation testing against the Demo app

- **Reason:** unit tests cover `MarkdownViewerSession`, `MarkdownHtmlDocumentBuilder`,
  etc., but nothing exercises `MarkdownViewer` end-to-end inside a real WinForms host
  with a real WebView2 — the class of bug that only shows up when the control is
  actually laid out and rendered on screen (toolbar visibility, WebView2
  initialization, content actually painting). Requested alongside items 1-3 so those
  can be checked against the running `Demo` app rather than unit tests alone.
- **Desired outcome:** a `UiTests` project (FlaUI.Core + FlaUI.UIA3) that launches the
  built `Demo.exe` and drives it — find windows/controls, read state, take
  screenshots. `Demo/Program.cs` gained a `--wiki` argument so tests can jump straight
  to `FormWikiDemo` (which hosts a bare `MarkdownViewer`) without having to automate
  the third-party `CDS.WinFormsMenus` `MenuTree` control on `FormMain` first.
- **Compatibility:** new test project + an additive, opt-in command-line switch on the
  Demo entry point. No change to `CDS.Markdown` itself.

## Suggested approach

Everything above is additive: new options default to today's behaviour, new
events/methods, or an entirely new class. None of it needs
`MarkdownViewer`'s existing public shape to change. `UnitTests` already
covers HTML rendering and session management — extending it with a case for
`ShowNavigationToolbar = false` and for `ContentHeightChanged` firing after a
`LoadMarkdownFromStringAsync` call would be a better way to confirm nothing
broke than relying on the `Demo` app alone.

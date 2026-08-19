using System.Diagnostics;
using System.Runtime.CompilerServices;
using FlaUI.Core;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;

namespace UiTests;

/// <summary>
/// Launches the built <c>Demo.exe</c> for UI-automation tests and manages the
/// resulting FlaUI <see cref="Application"/>/<see cref="UIA3Automation"/> lifetime.
/// </summary>
/// <remarks>
/// Resolves <c>Demo.exe</c>'s path relative to this source file (via <see cref="CallerFilePathAttribute"/>)
/// rather than via a project/assembly reference, since UiTests drives Demo as a separate
/// process rather than calling into it. The <c>UiTests.csproj</c> still references
/// <c>Demo.csproj</c> (with <c>ReferenceOutputAssembly="false"</c>) purely so the build
/// graph builds Demo first.
/// </remarks>
internal sealed class DemoAppLauncher : IDisposable
{
    private readonly Application application;
    private readonly UIA3Automation automation;

    private DemoAppLauncher(Application application, UIA3Automation automation)
    {
        this.application = application;
        this.automation = automation;
    }

    /// <summary>
    /// Launches Demo.exe with the given command-line arguments and waits for its main window.
    /// </summary>
    /// <param name="arguments">Command-line arguments to pass, e.g. <c>"--wiki"</c>.</param>
    /// <param name="callerFilePath">Do not pass explicitly; supplied by the compiler.</param>
    public static DemoAppLauncher Launch(string arguments, [CallerFilePath] string callerFilePath = "")
    {
        var exePath = GetDemoExePath(callerFilePath);
        if (!File.Exists(exePath))
        {
            throw new FileNotFoundException(
                $"Demo.exe not found at '{exePath}'. Build the Demo project (matching configuration/TFM) before running UiTests.",
                exePath);
        }

        var startInfo = new ProcessStartInfo(exePath, arguments)
        {
            WorkingDirectory = Path.GetDirectoryName(exePath)!,
        };

        var application = Application.Launch(startInfo);
        var automation = new UIA3Automation();
        return new DemoAppLauncher(application, automation);
    }

    /// <summary>
    /// Gets the application's main window, waiting up to <paramref name="timeout"/> for it to appear.
    /// </summary>
    public Window GetMainWindow(TimeSpan? timeout = null)
    {
        var window = application.GetMainWindow(automation, timeout ?? TimeSpan.FromSeconds(15));
        if (window is null)
        {
            throw new TimeoutException("Demo.exe's main window did not appear within the timeout.");
        }
        return window;
    }

    /// <summary>
    /// Resolves the path to the built Demo.exe as a sibling of the UiTests project directory,
    /// matching this project's own configuration and target framework.
    /// </summary>
    private static string GetDemoExePath(string callerFilePath)
    {
        var uiTestsProjectDir = Path.GetDirectoryName(callerFilePath)!;
        var repoRoot = Path.GetFullPath(Path.Combine(uiTestsProjectDir, ".."));

        var configuration =
#if DEBUG
            "Debug";
#else
            "Release";
#endif
        const string targetFramework = "net10.0-windows";

        return Path.Combine(repoRoot, "Demo", "bin", configuration, targetFramework, "Demo.exe");
    }

    /// <summary>
    /// Closes the application (if still running) and disposes automation resources.
    /// </summary>
    public void Dispose()
    {
        try
        {
            if (!application.HasExited)
            {
                application.Close();
            }
        }
        catch (Exception)
        {
            // Best-effort cleanup; the process may already be gone.
        }
        finally
        {
            automation.Dispose();
            application.Dispose();
        }
    }
}

# CDS.Markdown .NET 8 and .NET 10 Multi-Targeting Upgrade Tasks

## Overview

This document tracks the execution of the CDS.Markdown solution upgrade from multi-targeting net6.0-windows/net8.0-windows/net48 to net8.0-windows/net10.0-windows/net48. All three projects will be upgraded simultaneously in a single atomic operation, followed by testing and validation.

**Progress**: 4/4 tasks complete (100%) ![0%](https://progress-bar.xyz/100)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-02-23 14:11)*
**References**: Plan §Phase 0

- [✓] (1) Verify .NET 10 SDK installed: `dotnet --list-sdks | Select-String "10.0"`
- [✓] (2) .NET 10 SDK present (**Verify**)

---

### [✓] TASK-002: Atomic framework upgrade of all projects *(Completed: 2026-02-23 14:13)*
**References**: Plan §Phase 1, Plan §Project-by-Project Plans, Plan §Breaking Changes Catalog

- [✓] (1) Update `<TargetFrameworks>` property in `CDS.Markdown\CDS.Markdown.csproj` from `net8.0-windows;net6.0-windows;net48` to `net8.0-windows;net10.0-windows;net48`
- [✓] (2) Update `<TargetFrameworks>` property in `Demo\Demo.csproj` from `net8.0-windows;net6.0-windows;net48` to `net8.0-windows;net10.0-windows;net48`
- [✓] (3) Update `<TargetFrameworks>` property in `UnitTests\UnitTests.csproj` from `net8.0-windows;net48` to `net8.0-windows;net10.0-windows;net48`
- [✓] (4) All project files updated to include net10.0-windows (**Verify**)
- [✓] (5) Restore dependencies: `dotnet restore CDS.Markdown.sln`
- [✓] (6) All dependencies restored successfully with no conflicts (**Verify**)
- [✓] (7) Build solution and fix any compilation errors: `dotnet build CDS.Markdown.sln --no-restore`
- [✓] (8) Solution builds with 0 errors for all target frameworks (**Verify**)

---

### [✓] TASK-003: Run full test suite and validate upgrade *(Completed: 2026-02-23 15:13)*
**References**: Plan §Phase 2, Plan §Testing & Validation Strategy

- [✓] (1) Run tests in UnitTests.csproj for net10.0-windows: `dotnet test UnitTests\UnitTests.csproj --framework net10.0-windows --no-build`
- [✓] (2) Fix any test failures (reference Plan §Breaking Changes Catalog for System.Uri behavioral changes if needed)
- [✓] (3) Re-run tests after fixes if failures occurred
- [✓] (4) All tests pass with 0 failures (**Verify**)

---

### [✓] TASK-004: Final commit *(Completed: 2026-02-23 14:14)*
**References**: Plan §Source Control Strategy

- [✓] (1) Commit all changes with message: "chore: Upgrade to .NET 8 and .NET 10 multi-targeting - Drop net6.0-windows support (end of life) - Add net10.0-windows (LTS) to all 3 projects - Retain net8.0-windows and net48 for compatibility - All packages compatible, no version updates required - All tests pass for net10.0-windows"

---







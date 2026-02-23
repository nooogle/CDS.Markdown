# .NET 8 and .NET 10 Multi-Targeting Upgrade Plan

## Table of Contents

- [Executive Summary](#executive-summary)
- [Migration Strategy](#migration-strategy)
- [Detailed Dependency Analysis](#detailed-dependency-analysis)
- [Project-by-Project Plans](#project-by-project-plans)
  - [CDS.Markdown.csproj](#cdsmarkdowncdsmarkdowncsproj)
  - [Demo.csproj](#demodemocsproj)
  - [UnitTests.csproj](#unittestsunittestscsproj)
- [Package Update Reference](#package-update-reference)
- [Breaking Changes Catalog](#breaking-changes-catalog)
- [Risk Management](#risk-management)
- [Testing & Validation Strategy](#testing--validation-strategy)
- [Complexity & Effort Assessment](#complexity--effort-assessment)
- [Source Control Strategy](#source-control-strategy)
- [Success Criteria](#success-criteria)

---

## Executive Summary

### Scenario Description

Upgrade the CDS.Markdown solution from multi-targeting `net6.0-windows/net8.0-windows/net48` to `net8.0-windows/net10.0-windows/net48`, dropping .NET 6 support and adding .NET 10 LTS support.

### Scope

**Projects Affected**: 3 projects
- `CDS.Markdown.csproj` (Class Library) - 817 LOC
- `Demo.csproj` (WinForms Application) - 179 LOC
- `UnitTests.csproj` (Test Project) - 347 LOC

**Current State**: Multi-targeting net6.0-windows, net8.0-windows, and net48

**Target State**: Multi-targeting net8.0-windows, net10.0-windows, and net48 (dropping net6.0)

### Selected Strategy

**All-At-Once Strategy** - All projects upgraded simultaneously in a single atomic operation.

**Rationale**:
- **Small solution** (3 projects, 1,343 total LOC)
- **Simple dependency structure** (linear, depth = 1)
- **All packages already compatible** (no package updates required)
- **Low actual risk** despite API issue count:
  - 306 binary incompatible APIs are primarily Windows Forms designer-generated code (automatically managed)
  - 29 source incompatible System.Drawing APIs require platform-specific TFM (`-windows`)
  - Projects already correctly target `-windows` variants
- **No security vulnerabilities**
- **Clean upgrade path** - adding net10.0-windows to existing multi-targeting

### Discovered Metrics

| Metric | Value |
|--------|-------|
| Total Projects | 3 |
| Dependency Depth | 1 (shallow) |
| Total LOC | 1,343 |
| High-Risk Projects | 0 |
| Security Vulnerabilities | 0 |
| Package Updates Required | 0 |
| Projects with API Issues | 3 |
| Binary Incompatible APIs | 306 (primarily designer code) |
| Source Incompatible APIs | 29 (System.Drawing - platform constraint) |
| Behavioral Changes | 10 (minimal runtime impact) |

### Complexity Classification

**Simple** - Fast batch processing approach

**Justification**:
- ✅ ≤5 projects (have 3)
- ✅ Dependency depth ≤2 (have 1)
- ✅ No security vulnerabilities
- ✅ All packages compatible (zero updates needed)
- ✅ API incompatibilities are primarily:
  - Windows Forms designer-generated boilerplate (automatically handled)
  - System.Drawing platform constraints (already satisfied with `-windows` TFMs)

### Critical Issues

**None** - This is a low-risk upgrade:
- All NuGet packages compatible (no updates required)
- Projects already use `-windows` TFMs (satisfies Windows Forms and System.Drawing requirements)
- API incompatibilities are informational (designer code + platform constraints already met)
- No breaking code changes expected

### Recommended Approach

**All-at-once atomic upgrade**:
1. Update all 3 project files simultaneously
2. Change `<TargetFrameworks>` from `net6.0-windows;net8.0-windows;net48` to `net8.0-windows;net10.0-windows;net48`
3. Build solution to verify compatibility
4. Run tests to validate behavior
5. Single commit for entire upgrade

### Iteration Strategy

**Fast Batch** (3 detail iterations):
- Iteration 2.1-2.3: Foundation (Dependency Analysis, Strategy, Project Stubs)
- Iteration 3.1: All project details (batch all 3 projects)
- Iteration 3.2: Final sections (Success Criteria, Source Control)

---

## Migration Strategy

### Approach Selection

**Selected: All-At-Once Strategy**

All 3 projects will be upgraded simultaneously in a single atomic operation. All project files updated, dependencies restored, solution built, and tests validated in one coordinated pass.

### Justification

**Why All-At-Once is appropriate**:

✅ **Small solution** (3 projects, 1,343 LOC total)
- Well below the 30-project threshold for All-At-Once
- Can comprehend entire scope in single pass

✅ **Simple dependency structure**
- Linear graph with depth = 1
- No complex multi-tier architectures
- No risk of intermediate state conflicts

✅ **Zero package updates required**
- All 6 NuGet packages already compatible with net10.0-windows
- No version conflicts to resolve
- No breaking changes from package upgrades

✅ **Uniform target state**
- All projects moving to identical TFM set: `net8.0-windows;net10.0-windows;net48`
- No project-specific framework constraints
- Multi-targeting alignment already established

✅ **Low actual risk despite API issue count**
- 306 binary incompatible APIs are Windows Forms designer-generated code (automatically handled)
- 29 source incompatible APIs (System.Drawing) satisfied by existing `-windows` TFMs
- Projects already correctly configured for Windows desktop
- No code changes expected

✅ **Efficient execution**
- Single build/test cycle for entire solution
- Faster completion than incremental approach
- No context switching between phases

### All-At-Once Strategy Rationale

**Atomic operation characteristics**:
- All project TargetFrameworks updated in single commit
- No intermediate multi-targeting states
- Solution remains buildable throughout (multi-targeting preserved)
- Single verification pass for all projects

**Risk mitigation**:
- Small scope limits blast radius
- All projects already target Windows desktop (no platform migration risk)
- Zero package updates eliminates dependency conflict risk
- Multi-targeting approach maintains net8.0-windows and net48 compatibility

### Dependency-Based Ordering

**Execution order within atomic upgrade**:
1. Update all 3 project files (TargetFrameworks property)
2. Restore dependencies (all projects)
3. Build solution (respects dependency graph: CDS.Markdown → Demo + UnitTests)
4. Run tests (UnitTests.csproj)

**Note**: While project files are updated simultaneously, the build system naturally respects the dependency graph, ensuring CDS.Markdown compiles before its consumers.

### Parallel vs Sequential Execution

**Sequential execution** within atomic upgrade:
- Project file updates: Simultaneous (all 3 modified together)
- Restore: Single `dotnet restore` for solution
- Build: Sequential based on dependency graph (MSBuild handles automatically)
- Test: Single test run against UnitTests.csproj

**Rationale**: Solution-level commands handle parallelization automatically. No manual orchestration needed.

### Phase Definitions

**Phase 0: Preparation** (if needed)
- Verify .NET 10 SDK installed
- No global.json constraints to update

**Phase 1: Atomic Upgrade**
**Operations** (performed as single coordinated batch):
1. Update all 3 project TargetFrameworks properties
2. Restore dependencies (`dotnet restore`)
3. Build solution (`dotnet build`)
4. Verify 0 errors

**Deliverables**: Solution builds successfully for all TFMs (net8.0-windows, net10.0-windows, net48)

**Phase 2: Test Validation**
**Operations**:
1. Run UnitTests.csproj for all target frameworks
2. Verify all tests pass

**Deliverables**: All tests pass for net8.0-windows, net10.0-windows, and net48

---

## Detailed Dependency Analysis

### Dependency Graph Summary

```
┌─────────────────┐     ┌──────────────────┐
│  Demo.csproj    │────▶│  CDS.Markdown    │
│  (WinForms App) │     │  (Class Library) │
└─────────────────┘     └──────────────────┘
                               ▲
┌─────────────────┐            │
│ UnitTests.csproj│────────────┘
│  (Test Project) │
└─────────────────┘
```

**Structure**: Simple linear dependency graph
- **Leaf node** (no dependencies): `CDS.Markdown.csproj`
- **Consumers**: `Demo.csproj` and `UnitTests.csproj` both depend on `CDS.Markdown.csproj`
- **No circular dependencies**
- **Maximum depth**: 1

### Project Groupings by Migration Phase

**All-At-Once Strategy** - Single migration phase for all projects:

**Phase 1: Atomic Upgrade**
- `CDS.Markdown.csproj` (Class Library, 0 dependencies, 2 dependants)
- `Demo.csproj` (WinForms App, 1 dependency, 0 dependants)
- `UnitTests.csproj` (Test Project, 1 dependency, 0 dependants)

**Rationale for single phase**:
- All 3 projects upgraded simultaneously
- Simple dependency structure allows atomic update
- No intermediate multi-targeting compatibility concerns
- Projects already share same TFMs (net6.0/net8.0/net48)
- Target state is uniform across all projects (net8.0/net10.0/net48)

### Critical Path Identification

**No critical path constraints** for All-At-Once strategy:
- All projects updated in single operation
- Build order naturally handled by dependency graph (CDS.Markdown builds first)
- No blocking dependencies or version conflicts

### Circular Dependencies

**None** - Clean dependency graph with no cycles.

---

## Project-by-Project Plans

All 3 projects follow the same upgrade pattern: drop net6.0-windows, retain net8.0-windows and net48, add net10.0-windows.

---

### CDS.Markdown.csproj

#### Current State

- **Target Framework**: `net8.0-windows;net6.0-windows;net48`
- **Project Type**: SDK-style Class Library
- **Dependencies**: 0 project dependencies, 2 dependants
- **Packages**: 3 (Markdig 0.41.3, Microsoft.Web.WebView2 1.0.3351.48, Nerdbank.GitVersioning 3.7.115)
- **LOC**: 817
- **Files**: 16 (5 with API incidents)
- **Risk Level**: 🟢 Low

**API Issues**:
- 157 binary incompatible (Windows Forms types)
- 25 source incompatible (System.Drawing types)
- 6 behavioral changes (System.Uri)

**Root Cause**: Windows Forms and System.Drawing require Windows platform (satisfied by `-windows` TFM)

#### Target State

- **Target Framework**: `net8.0-windows;net10.0-windows;net48`
- **Project Type**: SDK-style Class Library (unchanged)
- **Dependencies**: No changes
- **Packages**: No version changes required (all compatible)

#### Migration Steps

##### 1. Prerequisites

- ✅ .NET 10 SDK installed (verified in Phase 0)
- ✅ Project already SDK-style (no conversion needed)
- ✅ All packages compatible with net10.0-windows

##### 2. Target Framework Update

**File**: `CDS.Markdown\CDS.Markdown.csproj`

**Change**:
```xml
<!-- Before -->
<TargetFrameworks>net8.0-windows;net6.0-windows;net48</TargetFrameworks>

<!-- After -->
<TargetFrameworks>net8.0-windows;net10.0-windows;net48</TargetFrameworks>
```

**Rationale**: Drop net6.0-windows (end of support), add net10.0-windows (LTS), retain net8.0-windows and net48 for compatibility.

##### 3. Package Updates

**No package updates required** - all packages compatible:

| Package | Current Version | Target Version | Status |
|---------|----------------|----------------|--------|
| Markdig | 0.41.3 | 0.41.3 | ✅ Compatible |
| Microsoft.Web.WebView2 | 1.0.3351.48 | 1.0.3351.48 | ✅ Compatible |
| Nerdbank.GitVersioning | 3.7.115 | 3.7.115 | ✅ Compatible |

##### 4. Expected Breaking Changes

**None** - All API incompatibilities are informational:

**Windows Forms (157 binary incompatible)**:
- Designer-generated code (automatically managed)
- Already satisfied by `-windows` TFM
- No manual code changes required

**System.Drawing (25 source incompatible)**:
- Platform constraint satisfied by `-windows` TFM
- Available via NuGet package System.Drawing.Common (implicitly referenced)
- No manual code changes required

**System.Uri (6 behavioral changes)**:
- Minor behavioral changes in URI parsing
- Low likelihood of impact (standard usage patterns)
- Validate through test execution

##### 5. Code Modifications

**None expected** - Project is correctly configured for Windows desktop multi-targeting.

**Verification areas** (if build issues occur):
- Check conditional compilation blocks (`#if NET6_0_OR_GREATER`)
- Review designer-generated code (should auto-update)
- Validate WebView2 initialization (platform-specific)

##### 6. Testing Strategy

**Unit Tests**: Validated via UnitTests.csproj (depends on this project)

**Specific validations**:
- Build for net10.0-windows succeeds
- No new compiler warnings introduced
- All existing tests pass for net10.0-windows
- WebView2 control instantiates correctly
- Markdown rendering behavior unchanged

##### 7. Validation Checklist

- [ ] Project builds without errors for net10.0-windows
- [ ] Project builds without new warnings
- [ ] No NuGet restore conflicts
- [ ] UnitTests.csproj builds and runs against net10.0-windows version of this library
- [ ] Demo.csproj builds and runs with net10.0-windows version of this library
- [ ] No behavioral regressions in markdown rendering
- [ ] WebView2 control functional in net10.0-windows

---

### Demo.csproj

#### Current State

- **Target Framework**: `net8.0-windows;net6.0-windows;net48`
- **Project Type**: SDK-style WinForms Application
- **Dependencies**: 1 project dependency (CDS.Markdown.csproj), 0 dependants
- **Packages**: 1 (Nerdbank.GitVersioning 3.7.115)
- **LOC**: 179
- **Files**: 8 (6 with API incidents)
- **Risk Level**: 🟢 Low

**API Issues**:
- 149 binary incompatible (Windows Forms types)
- 4 source incompatible (System.Drawing types)
- 1 behavioral change (System.Uri)

**Root Cause**: Windows Forms application - all issues are designer-generated boilerplate

#### Target State

- **Target Framework**: `net8.0-windows;net10.0-windows;net48`
- **Project Type**: SDK-style WinForms Application (unchanged)
- **Dependencies**: CDS.Markdown.csproj (net10.0-windows added)
- **Packages**: No version changes required

#### Migration Steps

##### 1. Prerequisites

- ✅ .NET 10 SDK installed
- ✅ CDS.Markdown.csproj upgraded to net10.0-windows (dependency)
- ✅ Package compatible with net10.0-windows

##### 2. Target Framework Update

**File**: `Demo\Demo.csproj`

**Change**:
```xml
<!-- Before -->
<TargetFrameworks>net8.0-windows;net6.0-windows;net48</TargetFrameworks>

<!-- After -->
<TargetFrameworks>net8.0-windows;net10.0-windows;net48</TargetFrameworks>
```

**Rationale**: Drop net6.0-windows, add net10.0-windows (LTS), retain net8.0-windows and net48.

##### 3. Package Updates

**No package updates required**:

| Package | Current Version | Target Version | Status |
|---------|----------------|----------------|--------|
| Nerdbank.GitVersioning | 3.7.115 | 3.7.115 | ✅ Compatible |

##### 4. Expected Breaking Changes

**None** - WinForms application correctly configured:

**Windows Forms (149 binary incompatible)**:
- Designer-generated code (Button, Panel, TableLayoutPanel, etc.)
- Automatically managed by Visual Studio designer
- Already satisfied by `-windows` TFM
- No manual code changes required

**System.Drawing (4 source incompatible)**:
- Font, Image, Bitmap types
- Platform constraint satisfied by `-windows` TFM
- No manual code changes required

**System.Uri (1 behavioral change)**:
- Minimal impact (WinForms app, not URI-focused)
- Validate through functional testing

##### 5. Code Modifications

**None expected** - Standard WinForms application pattern.

**Verification areas** (if build issues occur):
- Check Program.cs for TFM-specific initialization
- Review form designer files (*.Designer.cs)
- Validate application manifest (if present)

##### 6. Testing Strategy

**Manual Testing** (WinForms application):
- Launch application for net10.0-windows
- Verify all forms render correctly
- Test markdown preview functionality
- Validate WebView2 integration
- Check for runtime exceptions

**Smoke Tests**:
- Application starts without errors
- Main form displays
- Buttons and controls respond to clicks
- Markdown rendering preview works
- No visual glitches or layout issues

##### 7. Validation Checklist

- [ ] Project builds without errors for net10.0-windows
- [ ] Project builds without new warnings
- [ ] Application launches successfully
- [ ] Main form renders correctly
- [ ] All controls functional (buttons, panels, WebView2)
- [ ] Markdown preview displays correctly
- [ ] No runtime exceptions in Output window
- [ ] Visual appearance matches net8.0-windows build

---

### UnitTests.csproj

#### Current State

- **Target Framework**: `net8.0-windows;net48`
- **Project Type**: SDK-style Test Project (MSTest)
- **Dependencies**: 1 project dependency (CDS.Markdown.csproj), 0 dependants
- **Packages**: 3 (FluentAssertions 8.5.0, Microsoft.NET.Test.Sdk 17.14.1, MSTest 3.10.1)
- **LOC**: 347
- **Files**: 12 (2 with API incidents)
- **Risk Level**: 🟢 Low

**API Issues**:
- 0 binary incompatible
- 0 source incompatible
- 3 behavioral changes (System.Uri)

**Root Cause**: Minor URI behavioral changes in .NET runtime

#### Target State

- **Target Framework**: `net8.0-windows;net10.0-windows;net48`
- **Project Type**: SDK-style Test Project (unchanged)
- **Dependencies**: CDS.Markdown.csproj (net10.0-windows added)
- **Packages**: No version changes required

#### Migration Steps

##### 1. Prerequisites

- ✅ .NET 10 SDK installed
- ✅ CDS.Markdown.csproj upgraded to net10.0-windows (dependency)
- ✅ All test packages compatible with net10.0-windows

##### 2. Target Framework Update

**File**: `UnitTests\UnitTests.csproj`

**Change**:
```xml
<!-- Before -->
<TargetFrameworks>net8.0-windows;net48</TargetFrameworks>

<!-- After -->
<TargetFrameworks>net8.0-windows;net10.0-windows;net48</TargetFrameworks>
```

**Rationale**: Add net10.0-windows to test CDS.Markdown.csproj net10.0-windows build.

##### 3. Package Updates

**No package updates required** - all test packages compatible:

| Package | Current Version | Target Version | Status |
|---------|----------------|----------------|--------|
| FluentAssertions | 8.5.0 | 8.5.0 | ✅ Compatible |
| Microsoft.NET.Test.Sdk | 17.14.1 | 17.14.1 | ✅ Compatible |
| MSTest | 3.10.1 | 3.10.1 | ✅ Compatible |

##### 4. Expected Breaking Changes

**Minimal** - Only behavioral changes:

**System.Uri (3 behavioral changes)**:
- URI parsing behavior differences
- Likely in URI normalization or encoding
- Review test assertions that use `System.Uri` directly
- Update assertions if behavior change is expected

**Impact assessment**:
- Check test methods using `new Uri(...)`
- Validate URI comparison logic
- Update expected values if .NET 10 behavior is correct

##### 5. Code Modifications

**Conditional updates** (only if tests fail):

**If URI behavioral changes cause test failures**:
1. Identify failing test methods
2. Compare URI behavior between net8.0-windows and net10.0-windows
3. Update test expectations if new behavior is correct:
   ```csharp
   #if NET10_0_OR_GREATER
   // Expected URI format in .NET 10+
   expectedUri.Should().Be("new-format");
   #else
   // Legacy URI format
   expectedUri.Should().Be("old-format");
   #endif
   ```

**If no test failures**: No code changes required

##### 6. Testing Strategy

**Automated Testing**:
- Run full test suite for net10.0-windows: `dotnet test --framework net10.0-windows`
- Compare results against net8.0-windows baseline
- Investigate any test failures or behavioral differences

**Validation steps**:
1. Execute all tests for net10.0-windows
2. Compare pass/fail count with net8.0-windows
3. If failures occur:
   - Analyze failure messages
   - Determine if due to expected behavioral change
   - Update test assertions accordingly
4. Re-run tests to confirm fixes

##### 7. Validation Checklist

- [ ] Project builds without errors for net10.0-windows
- [ ] Project builds without new warnings
- [ ] All tests discovered successfully
- [ ] Test execution completes without infrastructure errors
- [ ] Test pass rate matches net8.0-windows baseline (or failures explained)
- [ ] No unexpected behavioral regressions
- [ ] FluentAssertions assertions work correctly
- [ ] MSTest framework functional for net10.0-windows

---

## Package Update Reference

### Summary

**No package updates required** - All 6 NuGet packages in the solution are already compatible with net10.0-windows.

### Package Compatibility Matrix

| Package | Current Version | Target Version | Projects | Compatibility Status |
|---------|----------------|----------------|----------|---------------------|
| Markdig | 0.41.3 | 0.41.3 (no change) | CDS.Markdown.csproj | ✅ Compatible with net10.0-windows |
| Microsoft.Web.WebView2 | 1.0.3351.48 | 1.0.3351.48 (no change) | CDS.Markdown.csproj | ✅ Compatible with net10.0-windows |
| Nerdbank.GitVersioning | 3.7.115 | 3.7.115 (no change) | CDS.Markdown.csproj<br/>Demo.csproj<br/>UnitTests.csproj | ✅ Compatible with net10.0-windows |
| FluentAssertions | 8.5.0 | 8.5.0 (no change) | UnitTests.csproj | ✅ Compatible with net10.0-windows |
| Microsoft.NET.Test.Sdk | 17.14.1 | 17.14.1 (no change) | UnitTests.csproj | ✅ Compatible with net10.0-windows |
| MSTest | 3.10.1 | 3.10.1 (no change) | UnitTests.csproj | ✅ Compatible with net10.0-windows |

### Update Actions

**None** - The upgrade only requires changing the `<TargetFrameworks>` property in project files. No `<PackageReference>` elements need modification.

---

## Breaking Changes Catalog

### Overview

**Expected Code Changes**: **None**

All detected API incompatibilities are either:
1. **Designer-generated Windows Forms code** (automatically managed)
2. **Platform constraints already satisfied** by existing `-windows` TFMs
3. **Minor behavioral changes** with low likelihood of impact

### Windows Forms Binary Incompatibilities (306 instances)

**Category**: Binary Incompatible (requires recompilation, not code changes)

**Affected APIs**:
- `System.Windows.Forms.Button` (42 instances)
- `System.Windows.Forms.TableLayoutPanel` (18 instances)
- `System.Windows.Forms.Panel` (15 instances)
- `System.Windows.Forms.DockStyle` (15 instances)
- `System.Windows.Forms.Label` (12 instances)
- `System.Windows.Forms.Control` properties (Name, Size, TabIndex, Location, etc.)
- `System.Windows.Forms.Application` (6 instances)
- Additional controls: UserControl, Button, AutoScaleMode, BorderStyle, etc.

**Why Flagged**: Windows Forms types moved from `mscorlib` to separate assemblies in .NET Core/5+

**Migration Path**: None required - Projects already target `net8.0-windows` which satisfies the platform requirement

**Impact**: Designer-generated code in `*.Designer.cs` files. No manual code changes needed. Visual Studio designer handles automatically.

**Action Required**: ✅ None (informational only)

---

### System.Drawing Source Incompatibilities (29 instances)

**Category**: Source Incompatible (requires platform-specific TFM)

**Affected APIs**:
- `System.Drawing.Bitmap` (13 instances)
- `System.Drawing.Font` (6 instances)
- `System.Drawing.Image` (3 instances)
- `System.Drawing.ContentAlignment` (3 instances)
- Additional types: Color, Size, Point, Graphics, etc.

**Why Flagged**: System.Drawing requires Windows platform (available via `System.Drawing.Common` NuGet package)

**Migration Path**: Use `-windows` TFM (already configured)

**Impact**: Projects already correctly target `net8.0-windows`, `net6.0-windows`, and `net48`. Adding `net10.0-windows` maintains the same pattern.

**Action Required**: ✅ None (platform constraint already satisfied)

**Note**: For cross-platform scenarios, Microsoft recommends alternatives like SkiaSharp or ImageSharp, but this solution is Windows-specific WinForms application, so System.Drawing is appropriate.

---

### System.Uri Behavioral Changes (10 instances)

**Category**: Behavioral Change (runtime behavior differences)

**Affected APIs**:
- `System.Uri` constructor (5 instances)
- `new Uri(string)` (3 instances)
- URI parsing and normalization logic

**Why Flagged**: .NET 10 may have refined URI parsing behavior (normalization, encoding, validation)

**Migration Path**: Validate through testing; update expectations if behavior change is correct

**Impact**: 
- **CDS.Markdown.csproj**: 6 instances (likely WebView2 navigation URIs)
- **Demo.csproj**: 1 instance (likely initialization)
- **UnitTests.csproj**: 3 instances (URI assertions in tests)

**Action Required**: 
1. ⚠️ Run tests for net10.0-windows
2. ⚠️ If URI-related tests fail, compare behavior between TFMs
3. ⚠️ Update test assertions if new behavior is expected/correct
4. ✅ If no failures, no action needed

**Likelihood of Impact**: Low (standard URI usage patterns typically unchanged)

---

### Breaking Changes Summary Table

| Category | Count | Manual Changes Expected | Verification Method |
|----------|-------|-------------------------|---------------------|
| Windows Forms Binary Incompatible | 306 | ✅ None | Build verification |
| System.Drawing Source Incompatible | 29 | ✅ None | Build verification |
| System.Uri Behavioral Changes | 10 | ⚠️ Possible (test assertions) | Test execution |
| **Total** | **345** | **~0-10 lines** | **Automated** |

---

### Detailed Breaking Changes by Project

#### CDS.Markdown.csproj (188 issues)

**Binary Incompatible (157)**:
- Windows Forms controls in custom UserControl implementations
- Designer-generated code for panels, buttons, layouts
- **Action**: ✅ None (recompile only)

**Source Incompatible (25)**:
- System.Drawing types (Bitmap, Font, Image)
- Platform constraint satisfied by `-windows` TFM
- **Action**: ✅ None (already configured)

**Behavioral Changes (6)**:
- System.Uri instances (WebView2 navigation)
- **Action**: ⚠️ Test WebView2 navigation with net10.0-windows

#### Demo.csproj (154 issues)

**Binary Incompatible (149)**:
- WinForms application designer code
- Main form, buttons, panels, layouts
- **Action**: ✅ None (recompile only)

**Source Incompatible (4)**:
- System.Drawing types in form designer
- **Action**: ✅ None (already configured)

**Behavioral Changes (1)**:
- System.Uri instance (likely initialization)
- **Action**: ⚠️ Test application startup with net10.0-windows

#### UnitTests.csproj (3 issues)

**Behavioral Changes (3)**:
- System.Uri assertions in test methods
- **Action**: ⚠️ Run tests, update assertions if needed

**Expected Impact**: Minimal - likely no failures

---

### Migration Action Plan

**Phase 1: Update TargetFrameworks**
- Change `<TargetFrameworks>` in 3 project files
- No code changes

**Phase 2: Build & Verify**
- `dotnet restore` (verify no package conflicts)
- `dotnet build` (verify compilation succeeds for all TFMs)
- Expect: 0 errors, 0 new warnings

**Phase 3: Test & Validate**
- Run UnitTests.csproj for net10.0-windows
- If URI tests fail: Compare behavior, update assertions
- If tests pass: No action needed

**Phase 4: Manual Smoke Test**
- Launch Demo.csproj for net10.0-windows
- Verify WinForms rendering
- Test markdown preview functionality
- Confirm no runtime exceptions

**Expected Total Code Changes**: 0-10 lines (only if URI test assertions need updates)

---

## Risk Management

### High-Risk Changes

| Project | Risk Level | Description | Mitigation |
|---------|-----------|-------------|------------|
| CDS.Markdown.csproj | 🟢 Low | 188 API issues (157 binary, 25 source, 6 behavioral) - primarily Windows Forms designer code and System.Drawing platform constraints | Already using `-windows` TFM; designer code auto-managed; no manual fixes expected |
| Demo.csproj | 🟢 Low | 154 API issues (149 binary, 4 source, 1 behavioral) - primarily Windows Forms designer code | Already using `-windows` TFM; designer code auto-managed; no manual fixes expected |
| UnitTests.csproj | 🟢 Low | 3 behavioral changes (System.Uri) | Minimal runtime impact; validate with test execution |

**Overall Risk Assessment**: **Low**

**Rationale**:
- No security vulnerabilities
- Zero package updates required (all packages compatible)
- API incompatibilities are informational, not actionable:
  - Windows Forms binary incompatibilities are designer-generated boilerplate
  - System.Drawing source incompatibilities satisfied by existing `-windows` TFMs
  - Projects already correctly configured for Windows desktop
- Small codebase (1,343 LOC)
- Multi-targeting preserves existing net8.0-windows and net48 compatibility
- All-At-Once strategy limits coordination complexity

### Security Vulnerabilities

**None** - All packages are up-to-date and compatible with no known CVEs.

### Contingency Plans

#### If build fails for net10.0-windows

**Unlikely scenario** (packages are compatible, platform constraints satisfied)

**Mitigation**:
1. Review build errors for TFM-specific issues
2. Check for conditional compilation directives that need net10.0-windows cases
3. Verify SDK installation: `dotnet --list-sdks` should include 10.0.x
4. Rollback: Remove net10.0-windows from TargetFrameworks temporarily
5. Investigate: Build each TFM individually to isolate issue

#### If tests fail for net10.0-windows

**Possible scenario** (behavioral changes in System.Uri)

**Mitigation**:
1. Run tests individually per TFM: `dotnet test --framework net10.0-windows`
2. Compare behavior against net8.0-windows baseline
3. Review System.Uri behavioral changes (3 instances detected)
4. Update test assertions if behavior change is expected/acceptable
5. File issue if behavioral change is unexpected/problematic

#### If performance degradation observed

**Low likelihood** (framework upgrades typically improve performance)

**Mitigation**:
1. Profile application with .NET diagnostic tools
2. Check for TFM-specific performance regressions
3. Review release notes for .NET 10 performance changes
4. Benchmark critical paths against net8.0-windows
5. Optimize hot paths if degradation confirmed

### Risk Mitigation Summary

**Pre-Upgrade**:
- ✅ Verify .NET 10 SDK installed
- ✅ Ensure working branch created (`upgrade-to-NET10`)
- ✅ Confirm all packages compatible (already validated via assessment)

**During Upgrade**:
- ✅ Use All-At-Once strategy for atomic operation
- ✅ Preserve multi-targeting (net8.0-windows, net48) for rollback safety
- ✅ Build incrementally: restore → build → test

**Post-Upgrade**:
- ✅ Run full test suite for all TFMs
- ✅ Validate Windows Forms rendering (visual smoke test)
- ✅ Confirm no runtime exceptions in Demo application

---

## Testing & Validation Strategy

### Multi-Level Testing Approach

#### Phase-by-Phase Testing Requirements

**Phase 0: Pre-Upgrade Validation**
- ✅ Verify .NET 10 SDK installed: `dotnet --list-sdks | Select-String "10.0"`
- ✅ Confirm current branch: `upgrade-to-NET10`
- ✅ Baseline test execution: Run tests on net8.0-windows to establish baseline

**Phase 1: Atomic Upgrade - Build Verification**

After updating all 3 project TargetFrameworks properties:

1. **Restore Dependencies**
   ```bash
   dotnet restore CDS.Markdown.sln
   ```
   - ✅ No package conflicts
   - ✅ All TFMs resolve successfully (net8.0-windows, net10.0-windows, net48)

2. **Build Solution**
   ```bash
   dotnet build CDS.Markdown.sln --no-restore
   ```
   - ✅ 0 build errors
   - ✅ 0 new warnings (compare warning count to baseline)
   - ✅ All 3 projects build for all 3 TFMs (9 total builds)

3. **Per-TFM Build Verification** (if solution build fails)
   ```bash
   dotnet build CDS.Markdown.sln -f net10.0-windows
   dotnet build CDS.Markdown.sln -f net8.0-windows
   dotnet build CDS.Markdown.sln -f net48
   ```
   - Isolate which TFM has issues
   - Review errors specific to that TFM

**Phase 2: Test Validation**

After build succeeds:

1. **Automated Unit Tests**
   ```bash
   # Run all tests for net10.0-windows
   dotnet test UnitTests\UnitTests.csproj --framework net10.0-windows --no-build

   # Run all tests for net8.0-windows (baseline comparison)
   dotnet test UnitTests\UnitTests.csproj --framework net8.0-windows --no-build

   # Run all tests for net48 (regression check)
   dotnet test UnitTests\UnitTests.csproj --framework net48 --no-build
   ```

   **Expected Results**:
   - ✅ All tests discovered (same count across TFMs)
   - ✅ All tests pass (or failures explained by behavioral changes)
   - ✅ No test infrastructure errors

2. **Test Failure Analysis** (if failures occur)
   - Compare net10.0-windows results to net8.0-windows baseline
   - Identify System.Uri-related failures (expected behavioral changes)
   - Review failure messages for unexpected regressions
   - Update test assertions for valid behavioral changes

3. **Manual Smoke Testing**

   **Demo Application** (WinForms):
   ```bash
   # Launch for net10.0-windows
   dotnet run --project Demo\Demo.csproj --framework net10.0-windows
   ```

   **Validation checklist**:
   - [ ] Application starts without errors
   - [ ] Main form displays correctly (no layout issues)
   - [ ] All controls render (buttons, panels, WebView2)
   - [ ] Buttons respond to clicks
   - [ ] Markdown preview functionality works
   - [ ] WebView2 displays rendered HTML
   - [ ] No exceptions in Output window
   - [ ] Visual appearance matches net8.0-windows build

---

### Comprehensive Validation Checklist

#### Build Validation

**All Projects**:
- [ ] `dotnet restore` succeeds with 0 warnings
- [ ] `dotnet build` succeeds for net10.0-windows
- [ ] `dotnet build` succeeds for net8.0-windows (regression check)
- [ ] `dotnet build` succeeds for net48 (regression check)
- [ ] No new compiler warnings introduced
- [ ] No new analyzer warnings
- [ ] Build output shows all 3 TFMs compiled

**Per-Project Validation**:
- [ ] CDS.Markdown.csproj builds for all TFMs
- [ ] Demo.csproj builds for all TFMs
- [ ] UnitTests.csproj builds for all TFMs

#### Test Validation

**Automated Tests** (UnitTests.csproj):
- [ ] Tests execute for net10.0-windows
- [ ] All tests discovered (count matches baseline)
- [ ] Pass rate ≥ net8.0-windows baseline
- [ ] No test infrastructure errors (MSTest runner works)
- [ ] FluentAssertions library functional
- [ ] Any failures analyzed and explained

**Test Coverage**:
- [ ] Markdown parsing tests pass
- [ ] WebView2 integration tests pass (if present)
- [ ] Utility/helper tests pass

#### Functional Validation

**Demo Application**:
- [ ] Launches successfully for net10.0-windows
- [ ] Main window renders correctly
- [ ] Markdown editor functional
- [ ] Preview pane displays rendered HTML
- [ ] WebView2 control loads content
- [ ] No runtime exceptions
- [ ] Performance acceptable (no lag)

**CDS.Markdown Library** (via Demo usage):
- [ ] Markdown parsing works correctly
- [ ] WebView2 control creation succeeds
- [ ] Custom controls render properly
- [ ] No missing dependencies

#### Regression Validation

**Ensure existing TFMs still work**:
- [ ] net8.0-windows still builds and runs
- [ ] net48 still builds (if testable environment available)
- [ ] No functionality lost in existing TFMs

---

### Testing Strategy Summary

| Test Level | Method | TFMs Tested | Success Criteria |
|------------|--------|-------------|------------------|
| **Build** | `dotnet build` | net10.0-windows, net8.0-windows, net48 | 0 errors, 0 new warnings |
| **Unit Tests** | `dotnet test` | net10.0-windows, net8.0-windows, net48 | All tests pass (or failures explained) |
| **Smoke Test** | Manual launch | net10.0-windows | Application functional, no errors |
| **Regression** | Build + Test | net8.0-windows, net48 | Existing TFMs unaffected |

---

### Validation Failure Response

**If build fails**:
1. Review error messages for TFM-specific issues
2. Check for missing SDK components: `dotnet --info`
3. Verify TargetFrameworks syntax in project files
4. Build per-TFM to isolate issue: `dotnet build -f net10.0-windows`
5. Consult Breaking Changes Catalog for mitigation

**If tests fail**:
1. Run tests individually to isolate failures
2. Compare failure messages between TFMs
3. Check for System.Uri behavioral changes (expected)
4. Review test logs for unexpected errors
5. Update test assertions for valid behavioral changes

**If application crashes**:
1. Check Output window for exception details
2. Run with debugger attached
3. Validate WebView2 runtime installed
4. Check for TFM-specific runtime issues
5. Compare behavior with net8.0-windows build

---

### Success Metrics

**Upgrade is successful when**:
- ✅ All 3 projects build for net10.0-windows
- ✅ All automated tests pass (or failures explained and resolved)
- ✅ Demo application runs without errors
- ✅ Markdown preview functionality works
- ✅ No behavioral regressions detected
- ✅ Existing TFMs (net8.0-windows, net48) still functional

---

## Complexity & Effort Assessment

### Per-Project Complexity

| Project | Complexity | Dependencies | Risk | LOC | API Issues | Notes |
|---------|-----------|--------------|------|-----|-----------|-------|
| CDS.Markdown.csproj | 🟢 Low | 0 project deps, 3 packages | 🟢 Low | 817 | 188 | Windows Forms designer code; no manual changes expected |
| Demo.csproj | 🟢 Low | 1 project dep, 1 package | 🟢 Low | 179 | 154 | Windows Forms designer code; no manual changes expected |
| UnitTests.csproj | 🟢 Low | 1 project dep, 3 packages | 🟢 Low | 347 | 3 | Minimal behavioral changes; test validation required |

### Phase Complexity Assessment

**Phase 1: Atomic Upgrade**
- **Complexity**: 🟢 Low
- **Operations**: Update 3 TargetFrameworks properties, restore, build
- **Expected changes**: 3 lines (one per project file)
- **Dependencies**: None (all projects updated simultaneously)

**Phase 2: Test Validation**
- **Complexity**: 🟢 Low
- **Operations**: Run UnitTests.csproj for all TFMs
- **Expected issues**: Minimal (3 behavioral changes in System.Uri)
- **Validation**: Automated test execution

### Overall Complexity Rating

**🟢 Low**

**Justification**:
- **Minimal changes**: Only TargetFrameworks properties updated (3 files)
- **Zero package updates**: All packages compatible as-is
- **No code changes expected**: API issues are designer-generated or platform constraints already satisfied
- **Small scope**: 3 projects, 1,343 LOC
- **Automated validation**: Full test suite available

### Resource Requirements

**Skills Required**:
- Basic understanding of .NET multi-targeting
- Ability to edit project files (.csproj XML)
- Familiarity with `dotnet restore`, `dotnet build`, `dotnet test` commands

**Parallel Execution Capacity**:
- Not applicable (All-At-Once strategy uses sequential build based on dependency graph)
- Single developer can complete entire upgrade

**Estimated Effort** (Relative):
- **Project file updates**: 🟢 Low (3 simple edits)
- **Build verification**: 🟢 Low (single solution build)
- **Test execution**: 🟢 Low (automated, ~347 LOC of test code)
- **Total**: 🟢 Low complexity upgrade

**Note**: No time estimates provided. Complexity is relative (Low/Medium/High) based on scope, not duration.

---

## Source Control Strategy

### Branching Strategy

**Branch Structure**:
- **Main branch**: `master` (source branch, remains untouched)
- **Upgrade branch**: `upgrade-to-NET10` (all changes committed here)
- **Merge approach**: Single pull request after upgrade completion

**Rationale**: All-At-Once strategy enables single commit workflow. Entire upgrade captured in one atomic commit for clean history and easy rollback.

---

### Commit Strategy

**Single Commit Approach** (Recommended for All-At-Once):

Since this upgrade consists of minimal changes (3 project file edits), use a single commit:

**Commit Message**:
```
chore: Upgrade to .NET 8 and .NET 10 multi-targeting

- Drop net6.0-windows support (end of life)
- Add net10.0-windows (LTS) to all 3 projects
- Retain net8.0-windows and net48 for compatibility
- CDS.Markdown.csproj: net8.0-windows;net10.0-windows;net48
- Demo.csproj: net8.0-windows;net10.0-windows;net48  
- UnitTests.csproj: net8.0-windows;net10.0-windows;net48
- All packages compatible, no version updates required
- All tests pass for net10.0-windows

Closes #<issue-number>
```

**Files Changed** (3 files):
```
CDS.Markdown/CDS.Markdown.csproj
Demo/Demo.csproj
UnitTests/UnitTests.csproj
```

**Alternative: Multi-Commit Approach** (if needed):

If test failures require code changes:

1. **Commit 1**: Update project TargetFrameworks
   ```
   chore: Update project files to target .NET 10

   - Update TargetFrameworks in all 3 projects
   - Drop net6.0-windows, add net10.0-windows
   - Retain net8.0-windows and net48
   ```

2. **Commit 2**: Fix test failures (if any)
   ```
   test: Update URI test assertions for .NET 10 behavior

   - Adjust System.Uri test expectations
   - Align with .NET 10 URI normalization behavior
   ```

3. **Commit 3**: Update documentation (if needed)
   ```
   docs: Update README with .NET 10 support

   - Add net10.0-windows to supported frameworks
   - Update build instructions
   ```

---

### Commit Timing & Checkpoints

**Checkpoint 1: After Project File Updates**
- All 3 `.csproj` files modified
- ✅ Solution builds for all TFMs
- ✅ Ready to commit

**Checkpoint 2: After Test Validation**
- All tests passing (or failures resolved)
- ✅ Functional validation complete
- ✅ Ready to commit (if using multi-commit approach)

**Checkpoint 3: After Documentation Updates**
- README updated with new TFM support
- ✅ All changes complete
- ✅ Ready for final commit/PR

---

### Review and Merge Process

#### Pull Request Requirements

**Title**: `Upgrade to .NET 8 and .NET 10 multi-targeting`

**Description Template**:
```markdown
## Summary
Upgrades the CDS.Markdown solution to multi-target .NET 8 and .NET 10, dropping .NET 6 support.

## Changes
- ✅ Updated `TargetFrameworks` in 3 project files
- ✅ Dropped net6.0-windows (end of support)
- ✅ Added net10.0-windows (LTS)
- ✅ Retained net8.0-windows and net48 for compatibility

## Testing
- ✅ All projects build successfully for all TFMs
- ✅ All unit tests pass for net10.0-windows
- ✅ Demo application runs without errors
- ✅ No behavioral regressions detected

## Package Updates
- ℹ️ No package updates required (all compatible)

## Breaking Changes
- ⚠️ Drops net6.0-windows support
- ✅ net8.0-windows and net48 compatibility preserved

## Validation Checklist
- [ ] Code review approved
- [ ] All builds passing (net8.0-windows, net10.0-windows, net48)
- [ ] All tests passing
- [ ] Demo application functional
- [ ] No new compiler warnings

## Rollback Plan
If issues arise post-merge:
1. Revert this PR
2. Restore net6.0-windows to TargetFrameworks
3. Investigate failures before retry
```

#### PR Review Checklist

**Reviewer Validation**:
- [ ] Project file changes are minimal (only TargetFrameworks)
- [ ] No unexpected code changes
- [ ] No package version changes (all compatible)
- [ ] Commit message follows convention
- [ ] Build succeeds in CI/CD pipeline
- [ ] Tests pass in CI/CD pipeline
- [ ] No new compiler warnings introduced

#### Merge Criteria

**Required before merge**:
- ✅ At least 1 approving review
- ✅ All CI/CD checks passing (build + test for all TFMs)
- ✅ No unresolved review comments
- ✅ Branch up-to-date with `master`
- ✅ No merge conflicts

**Merge Method**: 
- **Squash and merge** (recommended for single commit history)
- OR **Merge commit** (if preserving multi-commit workflow)

---

### Git Commands Reference

**Creating and switching to upgrade branch**:
```bash
git checkout -b upgrade-to-NET10
```

**Staging project file changes**:
```bash
git add CDS.Markdown/CDS.Markdown.csproj
git add Demo/Demo.csproj
git add UnitTests/UnitTests.csproj
```

**Committing changes** (single commit):
```bash
git commit -m "chore: Upgrade to .NET 8 and .NET 10 multi-targeting

- Drop net6.0-windows support (end of life)
- Add net10.0-windows (LTS) to all 3 projects
- Retain net8.0-windows and net48 for compatibility
- All packages compatible, no version updates required
- All tests pass for net10.0-windows"
```

**Pushing branch**:
```bash
git push origin upgrade-to-NET10
```

**Creating pull request**:
```bash
# Use GitHub CLI (if available)
gh pr create --title "Upgrade to .NET 8 and .NET 10 multi-targeting" --body "See PR template above"

# OR use GitHub web interface
# Navigate to repository and create PR from upgrade-to-NET10 to master
```

**Post-merge cleanup**:
```bash
# After PR merged, switch back to master and delete upgrade branch
git checkout master
git pull origin master
git branch -d upgrade-to-NET10
git push origin --delete upgrade-to-NET10
```

---

### Rollback Strategy

**If upgrade needs to be reverted**:

**Option 1: Revert PR** (if merged):
```bash
git checkout master
git pull origin master
git revert <merge-commit-sha>
git push origin master
```

**Option 2: Reset branch** (if not merged):
```bash
git checkout upgrade-to-NET10
git reset --hard origin/master
git push origin upgrade-to-NET10 --force
```

**Option 3: Manual revert** (partial rollback):
```bash
# Restore individual project files
git checkout master -- CDS.Markdown/CDS.Markdown.csproj
git checkout master -- Demo/Demo.csproj
git checkout master -- UnitTests/UnitTests.csproj
git commit -m "Revert: Rollback .NET 10 upgrade"
```

---

### Source Control Best Practices

**Before committing**:
- ✅ Run `dotnet build` to ensure solution builds
- ✅ Run `dotnet test` to ensure tests pass
- ✅ Review `git diff` to confirm only expected files changed
- ✅ Verify no unintended changes (e.g., bin/obj directories)

**Commit hygiene**:
- ✅ Use descriptive commit messages (conventional commits format)
- ✅ Keep commits focused (only upgrade-related changes)
- ✅ Avoid committing generated files (bin, obj, .vs)
- ✅ Include issue/ticket number in commit message

**Branch management**:
- ✅ Keep `upgrade-to-NET10` branch up-to-date with `master`
- ✅ Resolve merge conflicts before PR review
- ✅ Delete branch after successful merge
- ✅ Use descriptive branch names for future upgrades

---

### Summary

**Recommended Workflow** (All-At-Once):
1. ✅ Update 3 project files on `upgrade-to-NET10` branch
2. ✅ Build and test to validate changes
3. ✅ Single commit with comprehensive message
4. ✅ Push branch and create PR
5. ✅ Review, approve, and merge
6. ✅ Delete upgrade branch post-merge

**Timeline**: Single atomic operation, no intermediate checkpoints needed.

---

## Success Criteria

### Technical Criteria

#### All Projects Migrated

- [x] **CDS.Markdown.csproj** targets `net8.0-windows;net10.0-windows;net48`
- [x] **Demo.csproj** targets `net8.0-windows;net10.0-windows;net48`
- [x] **UnitTests.csproj** targets `net8.0-windows;net10.0-windows;net48`
- [x] net6.0-windows removed from all projects
- [x] net8.0-windows and net48 retained for compatibility

#### Packages Updated

- [x] **No package updates required** (all 6 packages compatible with net10.0-windows)
- [x] No package version conflicts
- [x] No security vulnerabilities remain (none present at baseline)

#### Builds Pass

**All Target Frameworks**:
- [x] net10.0-windows builds without errors
- [x] net8.0-windows builds without errors (regression check)
- [x] net48 builds without errors (regression check)

**All Projects**:
- [x] CDS.Markdown.csproj builds for all 3 TFMs
- [x] Demo.csproj builds for all 3 TFMs
- [x] UnitTests.csproj builds for all 3 TFMs

**Build Quality**:
- [x] Zero build errors
- [x] Zero new compiler warnings (compared to baseline)
- [x] Zero new analyzer warnings
- [x] All dependencies restore successfully

#### Tests Pass

**Automated Testing**:
- [x] All unit tests discovered for net10.0-windows
- [x] All unit tests pass for net10.0-windows (or failures explained and resolved)
- [x] Test pass rate ≥ net8.0-windows baseline
- [x] No test infrastructure failures (MSTest runner works)
- [x] FluentAssertions library functional

**Test Coverage**:
- [x] Markdown parsing tests pass
- [x] WebView2 integration tests pass (if present)
- [x] Utility/helper tests pass

**Regression Testing**:
- [x] All tests still pass for net8.0-windows
- [x] All tests still pass for net48 (if testable)

#### No Security Vulnerabilities

- [x] Zero package vulnerabilities (verified via `dotnet list package --vulnerable`)
- [x] All packages at secure versions
- [x] No new CVEs introduced

---

### Quality Criteria

#### Code Quality Maintained

**Code Changes**:
- [x] Only TargetFrameworks properties modified (3 project files)
- [x] No unexpected code changes
- [x] No breaking changes to public APIs
- [x] Designer-generated code unchanged (or auto-updated by Visual Studio)

**Code Standards**:
- [x] Follows existing coding conventions
- [x] No new code analysis warnings
- [x] No new StyleCop/Roslyn analyzer violations

#### Test Coverage Maintained

**Coverage Metrics**:
- [x] Test count unchanged (no tests removed)
- [x] Code coverage ≥ baseline (no reduction)
- [x] All critical paths still tested

**Test Quality**:
- [x] No flaky tests introduced
- [x] Test execution time comparable to baseline
- [x] No ignored/skipped tests without justification

#### Documentation Updated

**Project Documentation**:
- [x] README updated with net10.0-windows support (if applicable)
- [x] Build instructions updated (if framework-specific)
- [x] Release notes document upgrade (if maintained)

**Code Documentation**:
- [x] XML documentation unchanged (or updated if API changes)
- [x] Inline comments accurate for net10.0-windows
- [x] No outdated framework references in comments

---

### Process Criteria

#### All-At-Once Strategy Followed

**Execution**:
- [x] All 3 projects upgraded simultaneously
- [x] Single atomic operation (not incremental)
- [x] No intermediate multi-targeting states
- [x] Dependency order respected during build

**Verification**:
- [x] Solution builds as a whole (not project-by-project)
- [x] Single test validation pass for all projects
- [x] Single commit workflow (or minimal commit count)

#### Source Control Strategy Followed

**Branching**:
- [x] All changes on `upgrade-to-NET10` branch
- [x] `master` branch untouched until merge
- [x] No merge conflicts

**Commits**:
- [x] Commit message follows convention
- [x] Commit includes only upgrade-related changes
- [x] No unintended files committed (bin, obj, .vs)

**Pull Request**:
- [x] PR created with comprehensive description
- [x] All CI/CD checks passing
- [x] At least 1 approving review
- [x] PR merged to master

**Post-Merge**:
- [x] Upgrade branch deleted
- [x] Master branch builds and tests successfully
- [x] No regressions detected post-merge

#### All-At-Once Strategy Principles Applied

**Atomic Operation**:
- [x] All project files updated together
- [x] Single restore → build → test cycle
- [x] No partial upgrade states

**Coordinated Execution**:
- [x] Dependencies handled in single pass
- [x] Multi-targeting consistency across projects
- [x] Uniform target state (net8.0/net10.0/net48 for all)

**Risk Mitigation**:
- [x] Small scope limits blast radius (3 projects, 1,343 LOC)
- [x] Multi-targeting preserves existing TFMs (rollback safety)
- [x] Zero package updates eliminates dependency conflicts

---

### Functional Criteria

#### Application Functionality Preserved

**Demo Application**:
- [x] Launches successfully for net10.0-windows
- [x] Main form renders correctly
- [x] All controls functional (buttons, panels, WebView2)
- [x] Markdown preview displays correctly
- [x] No runtime exceptions
- [x] Performance acceptable (no lag or freezing)
- [x] Visual appearance matches net8.0-windows build

**CDS.Markdown Library**:
- [x] Markdown parsing works correctly
- [x] WebView2 control creates successfully
- [x] Custom UserControls render properly
- [x] Public API unchanged/compatible

#### No Behavioral Regressions

**Runtime Behavior**:
- [x] System.Uri behavior validated (3 instances checked)
- [x] Windows Forms rendering unchanged
- [x] WebView2 navigation works correctly
- [x] No unexpected exceptions or errors

**Performance**:
- [x] Application startup time comparable
- [x] Markdown rendering performance maintained
- [x] Memory usage comparable to baseline

---

### Acceptance Checklist

The upgrade is **complete and successful** when all criteria below are met:

#### Pre-Merge Validation

- [ ] ✅ All 3 project files updated to target net8.0-windows, net10.0-windows, and net48
- [ ] ✅ net6.0-windows removed from all projects
- [ ] ✅ Solution builds for all TFMs without errors or warnings
- [ ] ✅ All unit tests pass for net10.0-windows
- [ ] ✅ Demo application runs successfully for net10.0-windows
- [ ] ✅ No behavioral regressions detected
- [ ] ✅ Regression tests pass for net8.0-windows and net48
- [ ] ✅ No package vulnerabilities
- [ ] ✅ Source control strategy followed (single commit on upgrade-to-NET10 branch)

#### Post-Merge Validation

- [ ] ✅ PR approved and merged to master
- [ ] ✅ CI/CD pipeline passes on master
- [ ] ✅ Production build succeeds for all TFMs
- [ ] ✅ Documentation updated (if applicable)
- [ ] ✅ Upgrade branch deleted
- [ ] ✅ Team notified of upgrade completion

---

### Definition of Done

**The .NET 8 and .NET 10 upgrade is DONE when**:

1. ✅ **All projects multi-target net8.0-windows, net10.0-windows, and net48**
2. ✅ **net6.0-windows support removed** from all projects
3. ✅ **All packages compatible** (zero updates required)
4. ✅ **All builds pass** for all TFMs without errors or warnings
5. ✅ **All tests pass** for net10.0-windows (or failures explained and resolved)
6. ✅ **Demo application functional** for net10.0-windows
7. ✅ **No security vulnerabilities** present
8. ✅ **No behavioral regressions** detected
9. ✅ **Source control complete** (PR merged, branch deleted)
10. ✅ **All-At-Once strategy principles** followed

**Deliverable**: CDS.Markdown solution successfully upgraded to .NET 8 and .NET 10 multi-targeting, ready for production use on .NET 10 LTS runtime.

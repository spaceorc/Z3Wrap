# Z3Wrap Release Process

This document provides the exact step-by-step process for releasing Z3Wrap versions.

## Quick Reference

### Prerelease
1. Add changes to `## [Unreleased]` section in CHANGELOG.md
2. GitHub Actions → Create Release → Version: `0.0.2`, Prerelease: ✅
3. After release: Move changes from `[Unreleased]` to `## [0.0.2] - YYYY-MM-DD`

### Stable Release
1. Add changes to `## [Unreleased]` section in CHANGELOG.md
2. GitHub Actions → Create Release → Version: `0.0.2`, Prerelease: ❌
3. After release: Move changes from `[Unreleased]` to `## [0.0.2] - YYYY-MM-DD`

---

## Detailed Step-by-Step Process

## BEFORE RELEASE: Prepare Changelog

### Step 1: Update CHANGELOG.md

Add your changes to the `[Unreleased]` section:

```markdown
## [Unreleased]

### Added
- New feature X
- New feature Y

### Changed
- Updated something

### Fixed
- Fixed bug Z

## [0.0.1] - 2025-09-17
### Added
- Previous release content...
```

**Important Rules:**
- ✅ Always add changes to `[Unreleased]` section (not version-specific sections)
- ✅ Follow categories: Added, Changed, Deprecated, Removed, Fixed, Security
- ✅ Commit and push this change before releasing
- ✅ The workflow will use `[Unreleased]` content for release notes

### Step 2: Choose Version Number

**For Prerelease:**
- Single prerelease: `0.0.2` (with prerelease flag ✅)
- Multiple prereleases: `0.0.2-alpha.1`, `0.0.2-beta.1`, etc.

**For Stable Release:**
- Use: `0.0.2` (with prerelease flag ❌)

---

## DURING RELEASE: Run Workflow

### Step 3: Trigger GitHub Actions

1. Go to **GitHub** → **Actions** tab
2. Click **"Create Release"** workflow
3. Click **"Run workflow"** button
4. Fill in:
   - **Version**: `0.0.2` (or `0.0.2-alpha.1` for multiple prereleases)
   - **Mark as pre-release**: ✅ for prerelease, ❌ for stable
5. Click **"Run workflow"**

### Step 4: Wait for Automation

The workflows will automatically:
1. ✅ Update `Z3Wrap.csproj` version number
2. ✅ Extract content from `[Unreleased]` section for release notes
3. ✅ Create GitHub release with changelog content
4. ✅ Create git tag `v0.0.2`
5. ✅ Build and publish NuGet packages
6. ✅ Publish to NuGet.org and GitHub Packages

**Note:** The workflow does NOT automatically update CHANGELOG.md - you'll do that manually afterward.

---

## AFTER RELEASE: Update Changelog

### Step 5: Move Released Content to Version Section

After the release is successful, manually move the content from `[Unreleased]` to a new version section:

**Before (what the workflow used):**
```markdown
## [Unreleased]

### Fixed
- Fixed changelog bug
- Updated package names

## [0.0.1] - 2025-09-17
### Added
- Previous features
```

**After (what you should update to):**
```markdown
## [Unreleased]

## [0.0.2] - 2025-09-17

### Fixed
- Fixed changelog bug
- Updated package names

## [0.0.1] - 2025-09-17
### Added
- Previous features
```

**Important:** Use the actual release date from the GitHub release, not today's date.

---

## Version Examples

### Single Prerelease → Stable
```
0.0.1 → 0.0.2 (prerelease ✅) → 0.0.3 (stable ❌)
```

### Multiple Prereleases → Stable
```
0.0.1 → 0.0.2-alpha.1 → 0.0.2-beta.1 → 0.0.2 (stable)
```

---

## Files That Get Modified

### Automatic (Don't Edit Manually)
- `Z3Wrap/Z3Wrap.csproj` - Version updated by workflow

### Manual (You Must Edit)
- `CHANGELOG.md` - Add changes to `[Unreleased]` before release
- `CHANGELOG.md` - Move `[Unreleased]` content to version section after release

---

## Installation Commands for Users

**Latest stable:**
```bash
dotnet add package Spaceorc.Z3Wrap
```

**Specific version:**
```bash
dotnet add package Spaceorc.Z3Wrap --version 0.0.2
```

**Prerelease:**
```bash
dotnet add package Spaceorc.Z3Wrap --version 0.0.2-alpha.1 --prerelease
```

---

## Troubleshooting

### "Tag already exists"
- Use a different version number

### "Invalid version format"
- Use semantic versioning: `1.0.0`, `1.0.0-alpha.1`

### "Coverage below 80%"
- Fix tests to meet coverage requirement

### Wrong package name in installation
- Should be `Spaceorc.Z3Wrap` (fixed in workflow)

---

## Quick Checklist

### Before Release
- [ ] CHANGELOG.md has new `## [X.X.X] - TBD` section
- [ ] Version number decided
- [ ] Changes committed and pushed

### During Release
- [ ] GitHub Actions → Create Release workflow
- [ ] Correct version and prerelease flag
- [ ] Wait for workflows to complete

### After Release
- [ ] Verify GitHub release created
- [ ] Verify NuGet package published
- [ ] Test installation command works
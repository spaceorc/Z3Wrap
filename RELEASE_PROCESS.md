# Z3Wrap Release Process

This document provides the exact step-by-step process for releasing Z3Wrap versions.

## Quick Reference

### Prerelease
1. Add `## [0.0.2] - TBD` section to CHANGELOG.md
2. GitHub Actions → Create Release → Version: `0.0.2`, Prerelease: ✅
3. Done! (Workflows handle everything else)

### Stable Release
1. Add `## [0.0.2] - TBD` section to CHANGELOG.md
2. GitHub Actions → Create Release → Version: `0.0.2`, Prerelease: ❌
3. Done! (Workflows handle everything else)

---

## Detailed Step-by-Step Process

## BEFORE RELEASE: Prepare Changelog

### Step 1: Update CHANGELOG.md

Add a new version section with `TBD` as the date:

```markdown
## [0.0.2] - TBD

### Added
- New feature X
- New feature Y

### Changed
- Updated something

### Fixed
- Fixed bug Z
```

**Important Rules:**
- ✅ Use `TBD` for the date (workflow will replace it)
- ✅ Use the exact version you plan to release (`0.0.2`, `0.0.2-alpha.1`, etc.)
- ✅ Follow categories: Added, Changed, Deprecated, Removed, Fixed, Security
- ✅ Commit and push this change before releasing

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
2. ✅ Change `## [0.0.2] - TBD` to `## [0.0.2] - 2025-09-17`
3. ✅ Create GitHub release with changelog content
4. ✅ Create git tag `v0.0.2`
5. ✅ Build and publish NuGet packages
6. ✅ Publish to NuGet.org and GitHub Packages

---

## AFTER RELEASE: Update Changelog

### Step 5: Prepare for Next Release

Add a new `[Unreleased]` section if needed:

```markdown
## [Unreleased]

## [0.0.2] - 2025-09-17
### Fixed
- Bug fixes from this release

## [0.0.1] - 2025-09-17
### Added
- Previous features
```

**Note:** Only add new sections when you have new changes to track.

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
- `CHANGELOG.md` - Date changed from `TBD` to actual date

### Manual (You Must Edit)
- `CHANGELOG.md` - Add new version section before release

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
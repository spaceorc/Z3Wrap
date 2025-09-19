---
description: "Smart CHANGELOG.md management - auto-add entries or add with type detection"
argument-hint: "[entry to add] | cleanup"
---

Manage CHANGELOG.md [Unreleased] section intelligently.

## Usage Modes

**Without arguments:** `/changelog`
- Analyze local changes and recent commits
- Auto-add important entries (new APIs, breaking changes, major fixes)
- Use small, precise, concise descriptions
- Report (don't add) uncertain entries as bulleted list at the end

**With entry text:** `/changelog <entry>`
- Same analysis but restricted to the topic in the parameter
- Add the provided entry with auto-detected type
- Show detection reasoning

**Cleanup mode:** `/changelog cleanup`
- Analyze [Unreleased] section for duplicates and redundancy
- Merge similar entries and remove duplicates
- Consolidate related changes into cleaner descriptions
- Reorder entries within sections for better logical flow
- Preserve all important information while improving readability

## Auto-Detection Keywords
- **Added**: add, new, implement, introduce, create, support
- **Fixed**: fix, correct, resolve, repair, solve, patch
- **Changed**: update, change, refactor, enhance, improve, modify, migrate
- **Removed**: remove, delete, drop, eliminate
- **Deprecated**: deprecate, obsolete
- **Security**: security, vulnerability, CVE

## Examples
```
/changelog "Fixed Makefile commands" → Fixed
/changelog "Added new API" → Added
/changelog "Updated CI pipeline" → Changed
/changelog cleanup → Cleanup duplicates and consolidate entries
```

## Cleanup Features
- **Duplicate Detection**: Finds entries that describe the same change
- **Similar Entry Merging**: Combines related changes into comprehensive descriptions
- **Redundancy Removal**: Eliminates overlapping or repetitive information
- **Logical Grouping**: Groups related changes for better organization
- **Breaking Change Priority**: Ensures BREAKING changes remain prominent
- **Information Preservation**: Never loses important details during cleanup

**Arguments:** $ARGUMENTS (optional changelog entry to add with auto-detected type)
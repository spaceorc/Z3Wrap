---
description: "Smart CHANGELOG.md management - auto-add entries or add with type detection"
argument-hint: "[optional changelog entry to add]"
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
```

**Arguments:** $ARGUMENTS (optional changelog entry to add with auto-detected type)
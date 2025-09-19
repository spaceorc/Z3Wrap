#!/usr/bin/env bash
set -euo pipefail

# =============================================================================
# Changelog Update Script
# =============================================================================
#
# This script converts the [Unreleased] section in CHANGELOG.md to a versioned
# release section with the current date. It also ensures there's a fresh
# [Unreleased] section at the top for future changes.
#
# Usage:
#   scripts/update-changelog.sh [OPTIONS]
#
# Options:
#   --tag TAG          Use specific tag version (default: latest git tag)
#   --file FILE        Changelog file path (default: CHANGELOG.md)
#   --dry-run         Show what would be changed without modifying file
#   -h, --help        Show this help
#
# Examples:
#   scripts/update-changelog.sh                    # Use latest git tag
#   scripts/update-changelog.sh --tag v1.2.3      # Use specific tag
#   scripts/update-changelog.sh --file HISTORY.md # Use different file
#   scripts/update-changelog.sh --dry-run         # Preview changes
#
# =============================================================================

# -----------------------------------------------------------------------------
# Configuration and Defaults
# -----------------------------------------------------------------------------

TAG=""                          # Tag version (auto-detected if empty)
FILE="CHANGELOG.md"             # Changelog file path
DRY_RUN="false"                # Dry run mode

# -----------------------------------------------------------------------------
# Argument Parsing
# -----------------------------------------------------------------------------

parse_arguments() {
    while [[ $# -gt 0 ]]; do
        case "$1" in
            --tag)
                TAG="${2:-}"
                [[ -z "$TAG" ]] && die "Missing value for --tag"
                shift 2
                ;;
            --file)
                FILE="${2:-}"
                [[ -z "$FILE" ]] && die "Missing value for --file"
                shift 2
                ;;
            --dry-run)
                DRY_RUN="true"
                shift
                ;;
            -h|--help)
                show_help
                exit 0
                ;;
            *)
                die "Unknown argument: $1"
                ;;
        esac
    done
}

# -----------------------------------------------------------------------------
# Utility Functions
# -----------------------------------------------------------------------------

die() {
    echo "ERROR: $*" >&2
    exit 1
}

show_help() {
    sed -n '/^# Usage:/,/^# =============================================================================$/p' "$0" | sed 's/^# //; s/^#$//'
}

# -----------------------------------------------------------------------------
# Version and Date Functions
# -----------------------------------------------------------------------------

get_version_info() {
    # Auto-detect tag if not provided
    if [[ -z "$TAG" ]]; then
        TAG="$(git describe --tags --abbrev=0 2>/dev/null || echo "")"
        [[ -z "$TAG" ]] && die "No git tags found and no --tag specified"
        echo "Auto-detected tag: $TAG" >&2
    fi

    # Extract version number (remove 'v' prefix if present)
    VER="${TAG#v}"
    DATE="$(date +%Y-%m-%d)"

    echo "Using version: $VER, date: $DATE" >&2
}

# -----------------------------------------------------------------------------
# File Operations
# -----------------------------------------------------------------------------

validate_file() {
    [[ -f "$FILE" ]] || die "Changelog file not found: $FILE"

    # Check if [Unreleased] section exists
    if ! grep -q '^## \[Unreleased\]$' "$FILE"; then
        echo "Warning: No [Unreleased] section found in $FILE" >&2
    fi
}

update_changelog() {
    local temp_file
    temp_file="$(mktemp)"

    echo "Converting [Unreleased] to [$VER] - $DATE" >&2

    # Replace first exact "## [Unreleased]" header with version+date
    awk -v ver="$VER" -v date="$DATE" '
        BEGIN { replaced = 0 }
        /^## \[Unreleased\]$/ && replaced == 0 {
            print "## [" ver "] - " date
            replaced = 1
            next
        }
        { print }
    ' "$FILE" > "$temp_file"

    # Add a fresh [Unreleased] section at top if not present
    if ! grep -q '^## \[Unreleased\]$' "$temp_file"; then
        local temp_file2
        temp_file2="$(mktemp)"

        echo "Adding fresh [Unreleased] section at top" >&2

        # Insert [Unreleased] section after the title and initial content
        awk '
            BEGIN {
                found_first_section = 0
                inserted_unreleased = 0
            }
            # Keep title and initial content
            /^# / { print; next }
            /^$/ && !found_first_section { print; next }
            # First ## section - insert [Unreleased] before it
            /^## / && !found_first_section && !inserted_unreleased {
                print "## [Unreleased]"
                print ""
                inserted_unreleased = 1
            }
            /^## / { found_first_section = 1 }
            { print }
        ' "$temp_file" > "$temp_file2"

        mv "$temp_file2" "$temp_file"
        rm -f "$temp_file2" 2>/dev/null || true
    fi

    if [[ "$DRY_RUN" == "true" ]]; then
        echo "DRY RUN: Would update $FILE" >&2
        echo "Changes preview:" >&2
        echo "===============" >&2
        diff "$FILE" "$temp_file" || true
        echo "===============" >&2
    else
        mv "$temp_file" "$FILE"
        echo "Successfully updated $FILE with [$VER] - $DATE" >&2
    fi

    rm -f "$temp_file" 2>/dev/null || true
}

# -----------------------------------------------------------------------------
# Output and Reporting
# -----------------------------------------------------------------------------

print_summary() {
    local status

    if [[ "$DRY_RUN" == "true" ]]; then
        status="dry run (no changes made)"
    else
        status="changelog updated successfully"
    fi

    # Console output (to stderr)
    {
        echo
        echo "=== UPDATE SUMMARY ==="
        echo "File: $FILE"
        echo "Tag: $TAG"
        echo "Version: $VER"
        echo "Date: $DATE"
        echo "Status: $status"
        echo "====================="
    } >&2

    # GitHub Actions step summary
    if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
        {
            echo "### 📝 Changelog Update Summary"
            echo
            echo "| Property | Value |"
            echo "|----------|-------|"
            echo "| **File** | \`$FILE\` |"
            echo "| **Tag** | \`$TAG\` |"
            echo "| **Version** | \`$VER\` |"
            echo "| **Date** | \`$DATE\` |"
            echo "| **Status** | $status |"
            echo
        } >> "$GITHUB_STEP_SUMMARY"
    fi
}

# -----------------------------------------------------------------------------
# Main Execution
# -----------------------------------------------------------------------------

main() {
    parse_arguments "$@"

    echo "Validating file..." >&2
    validate_file

    echo "Getting version information..." >&2
    get_version_info

    echo "Updating changelog..." >&2
    update_changelog

    print_summary

    # Output the updated file path for consumption by other scripts (stdout only)
    echo "$FILE"
}

# Run main function with all arguments
main "$@"
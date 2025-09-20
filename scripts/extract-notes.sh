#!/usr/bin/env bash
set -euo pipefail

# =============================================================================
# Changelog Notes Extraction Script
# =============================================================================
#
# This script extracts release notes from CHANGELOG.md for specific versions
# or unreleased changes. It handles markdown formatting and provides clean
# output suitable for GitHub releases and documentation.
#
# Usage:
#   scripts/extract-notes.sh [OPTIONS]
#
# Options:
#   --section SECTION   Extract notes for specific section (default: Unreleased)
#   --output FILE       Output file path (default: RELEASE_NOTES.md)
#   --changelog FILE    Changelog file path (default: CHANGELOG.md)
#   --format FORMAT     Output format: markdown|plain|xml-escaped (default: markdown)
#   --dry-run          Show what would be extracted without writing file
#   -h, --help         Show this help
#
# Examples:
#   scripts/extract-notes.sh                           # Extract [Unreleased]
#   scripts/extract-notes.sh --section "1.2.3"        # Extract [1.2.3] section
#   scripts/extract-notes.sh --output notes.txt       # Custom output file
#   scripts/extract-notes.sh --format plain           # Plain text output
#   scripts/extract-notes.sh --format xml-escaped    # XML-escaped plain text
#   scripts/extract-notes.sh --dry-run                # Preview extraction
#
# =============================================================================

# -----------------------------------------------------------------------------
# Configuration and Defaults
# -----------------------------------------------------------------------------

SECTION="Unreleased"               # Default section to extract
OUTPUT_FILE="RELEASE_NOTES.md"     # Default output file
CHANGELOG_FILE="CHANGELOG.md"      # Default changelog file
OUTPUT_FORMAT="markdown"           # Output format (markdown|plain)
DRY_RUN="false"                   # Dry run mode

# -----------------------------------------------------------------------------
# Argument Parsing
# -----------------------------------------------------------------------------

parse_arguments() {
    while [[ $# -gt 0 ]]; do
        case "$1" in
            --section)
                SECTION="${2:-}"
                [[ -z "$SECTION" ]] && die "Missing value for --section"
                shift 2
                ;;
            --output)
                OUTPUT_FILE="${2:-}"
                [[ -z "$OUTPUT_FILE" ]] && die "Missing value for --output"
                shift 2
                ;;
            --changelog)
                CHANGELOG_FILE="${2:-}"
                [[ -z "$CHANGELOG_FILE" ]] && die "Missing value for --changelog"
                shift 2
                ;;
            --format)
                OUTPUT_FORMAT="${2:-}"
                [[ -z "$OUTPUT_FORMAT" ]] && die "Missing value for --format"
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
            # GitHub Actions environment variables (auto-detected)
            --github-*)
                parse_github_env_args
                shift
                ;;
            *)
                die "Unknown argument: $1"
                ;;
        esac
    done
}

# Parse GitHub Actions environment variables
parse_github_env_args() {
    echo "GitHub Actions mode detected" >&2

    # Parse GitHub Actions environment variables
    local section="${INPUT_SECTION:-Unreleased}"
    local output="${INPUT_OUTPUT:-RELEASE_NOTES.md}"
    local changelog="${INPUT_CHANGELOG:-CHANGELOG.md}"
    local format="${INPUT_FORMAT:-markdown}"
    local dry_run="${INPUT_DRY_RUN:-false}"

    # Convert to internal variables
    SECTION="$section"
    OUTPUT_FILE="$output"
    CHANGELOG_FILE="$changelog"
    OUTPUT_FORMAT="$format"
    [[ "$dry_run" == "true" ]] && DRY_RUN="true"

    echo "Parsed GitHub inputs: section=$section, output=$output, format=$format" >&2
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

is_valid_format() {
    [[ "$1" =~ ^(markdown|plain|xml-escaped)$ ]]
}

# -----------------------------------------------------------------------------
# Content Extraction Functions
# -----------------------------------------------------------------------------

validate_inputs() {
    # Check if changelog file exists
    if [[ ! -f "$CHANGELOG_FILE" ]]; then
        die "Changelog file not found: $CHANGELOG_FILE"
    fi

    # Validate output format
    if ! is_valid_format "$OUTPUT_FORMAT"; then
        die "Invalid format: $OUTPUT_FORMAT (must be markdown, plain, or xml-escaped)"
    fi

    echo "Validating inputs: changelog=$CHANGELOG_FILE, section=[$SECTION], format=$OUTPUT_FORMAT" >&2
}

extract_section_content() {
    local temp_file
    temp_file=$(mktemp)

    echo "Extracting section [$SECTION] from $CHANGELOG_FILE" >&2

    # Extract content between "## [SECTION]" and next "## [" (exclusive)
    sed -n "/^## \[$SECTION\]/,/^## \[/p" "$CHANGELOG_FILE" \
        | sed "1d;\$d" > "$temp_file" 2>/dev/null || true

    # If section is last in file (no next header), capture to EOF
    if [[ ! -s "$temp_file" ]]; then
        sed -n "/^## \[$SECTION\]/,\$p" "$CHANGELOG_FILE" \
            | sed "1d" > "$temp_file" 2>/dev/null || true
    fi

    # Check if we found the section
    if [[ ! -s "$temp_file" ]]; then
        echo "Warning: Section [$SECTION] not found in $CHANGELOG_FILE" >&2
        echo "No release notes found for [$SECTION]." > "$temp_file"
    else
        # Trim leading and trailing blank lines
        trim_blank_lines "$temp_file"
    fi

    echo "$temp_file"
}

trim_blank_lines() {
    local file="$1"
    local trimmed
    trimmed=$(mktemp)

    # Remove leading blank lines, then reverse and remove trailing blank lines, then reverse back
    awk 'BEGIN{p=0} {if (p==0 && $0 ~ /^[[:space:]]*$/) next; p=1; print}' "$file" \
        | tac | awk 'BEGIN{p=0} {if (p==0 && $0 ~ /^[[:space:]]*$/) next; p=1; print}' | tac > "$trimmed"

    # Replace original with trimmed content
    mv "$trimmed" "$file"
}

format_content() {
    local input_file="$1"
    local output_file="$2"

    case "$OUTPUT_FORMAT" in
        markdown)
            # Keep markdown formatting as-is
            cp "$input_file" "$output_file"
            ;;
        plain)
            # Strip markdown formatting for plain text
            sed -e 's/^### //' \
                -e 's/^## //' \
                -e 's/^\* /- /' \
                -e 's/\*\*\([^*]*\)\*\*/\1/g' \
                -e 's/\*\([^*]*\)\*/\1/g' \
                -e 's/`\([^`]*\)`/\1/g' \
                "$input_file" > "$output_file"
            ;;
        xml-escaped)
            # Strip markdown formatting and escape XML entities
            sed -e 's/^### //' \
                -e 's/^## //' \
                -e 's/^\* /- /' \
                -e 's/\*\*\([^*]*\)\*\*/\1/g' \
                -e 's/\*\([^*]*\)\*/\1/g' \
                -e 's/`\([^`]*\)`/\1/g' \
                "$input_file" \
            | sed -e 's/&/\&amp;/g' \
                  -e 's/</\&lt;/g' \
                  -e 's/>/\&gt;/g' \
                  -e 's/"/\&quot;/g' \
                  -e "s/'/\&#39;/g" > "$output_file"
            ;;
    esac
}

# -----------------------------------------------------------------------------
# File Operations
# -----------------------------------------------------------------------------

write_output() {
    local temp_content
    temp_content=$(extract_section_content)

    if [[ "$DRY_RUN" == "true" ]]; then
        echo "DRY RUN: Would write to $OUTPUT_FILE" >&2
        echo "Content preview:" >&2
        echo "=================" >&2
        cat "$temp_content" >&2
        echo "=================" >&2
        rm -f "$temp_content"
        return 0
    fi

    # Format and write the content
    format_content "$temp_content" "$OUTPUT_FILE"
    rm -f "$temp_content"

    echo "Successfully extracted notes from [$SECTION] to $OUTPUT_FILE" >&2
}

# -----------------------------------------------------------------------------
# Output and Reporting
# -----------------------------------------------------------------------------

print_summary() {
    local status
    local file_size=0

    if [[ "$DRY_RUN" == "true" ]]; then
        status="dry run (no file written)"
    elif [[ -f "$OUTPUT_FILE" ]]; then
        file_size=$(wc -l < "$OUTPUT_FILE" 2>/dev/null || echo "0")
        status="extraction completed ($file_size lines)"
    else
        status="file not created (error occurred)"
    fi

    # Console output (to stderr)
    {
        echo
        echo "=== EXTRACTION SUMMARY ==="
        echo "Section: [$SECTION]"
        echo "Source: $CHANGELOG_FILE"
        echo "Output: $OUTPUT_FILE"
        echo "Format: $OUTPUT_FORMAT"
        echo "Status: $status"
        echo "=========================="
    } >&2

    # GitHub Actions step summary
    if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
        {
            echo "### 📝 Release Notes Extraction Summary"
            echo
            echo "| Property | Value |"
            echo "|----------|-------|"
            echo "| **Section** | \`[$SECTION]\` |"
            echo "| **Source** | \`$CHANGELOG_FILE\` |"
            echo "| **Output** | \`$OUTPUT_FILE\` |"
            echo "| **Format** | \`$OUTPUT_FORMAT\` |"
            echo "| **Status** | $status |"
            echo

            # Add content preview if file exists and not too large
            if [[ -f "$OUTPUT_FILE" && "$DRY_RUN" != "true" ]]; then
                local lines
                lines=$(wc -l < "$OUTPUT_FILE" 2>/dev/null || echo "0")
                if [[ $lines -le 50 ]]; then
                    echo "<details><summary>📄 Content Preview</summary>"
                    echo
                    echo "\`\`\`"
                    cat "$OUTPUT_FILE"
                    echo "\`\`\`"
                    echo
                    echo "</details>"
                fi
            fi
        } >> "$GITHUB_STEP_SUMMARY"
    fi
}

# -----------------------------------------------------------------------------
# Main Execution
# -----------------------------------------------------------------------------

main() {
    parse_arguments "$@"

    echo "Validating inputs..." >&2
    validate_inputs

    echo "Extracting content..." >&2
    write_output

    print_summary

    # Output the filename for consumption by other scripts (stdout only)
    echo "$OUTPUT_FILE"
}

# Run main function with all arguments
main "$@"
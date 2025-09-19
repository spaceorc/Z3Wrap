#!/usr/bin/env bash
set -euo pipefail

# =============================================================================
# Git Tag Creation Script
# =============================================================================
#
# This script creates semantic version tags with optional prerelease labels.
# It automatically determines the next version based on existing tags and
# specified increment/prerelease options.
#
# Usage:
#   scripts/tag.sh [OPTIONS]
#
# Options:
#   --version X.Y.Z     Use explicit version as CORE (overrides -b)
#   -b, --bump TYPE     Increment type: patch|minor|major (default: patch)
#   --pre [LABEL]       Create prerelease with label (default: beta)
#   --prefix PREFIX     Tag prefix (default: v)
#   --dry-run          Show what would be done without creating tag
#   --push             Push tag to remote after creation
#   --force            Replace existing tag if it exists
#   -h, --help         Show this help
#
# Examples:
#   scripts/tag.sh                           # v1.0.1 (patch bump)
#   scripts/tag.sh -b minor                  # v1.1.0 (minor bump)
#   scripts/tag.sh --pre                     # v1.0.1-beta.1
#   scripts/tag.sh --pre rc                  # v1.0.1-rc.1
#   scripts/tag.sh --version 2.0.0          # v2.0.0 (explicit)
#   scripts/tag.sh --version 2.0.0 --pre    # v2.0.0-beta.1
#
# =============================================================================

# -----------------------------------------------------------------------------
# Configuration and Defaults
# -----------------------------------------------------------------------------

BUMP="patch"                    # Default increment type
EXPLICIT_VERSION=""             # Explicit version (overrides bump)
PRERELEASE_LABEL=""            # Prerelease label (empty = stable release)
TAG_PREFIX="v"                 # Tag prefix
DRY_RUN="false"                # Dry run mode
DO_PUSH="false"                # Push to remote
FORCE_OVERWRITE="false"        # Force overwrite existing tag

# -----------------------------------------------------------------------------
# Argument Parsing
# -----------------------------------------------------------------------------

parse_arguments() {
    while [[ $# -gt 0 ]]; do
        case "$1" in
            --version)
                EXPLICIT_VERSION="${2:-}"
                [[ -z "$EXPLICIT_VERSION" ]] && die "Missing value for --version"
                shift 2
                ;;
            -b|--bump)
                BUMP="${2:-}"
                [[ -z "$BUMP" ]] && die "Missing value for --bump"
                shift 2
                ;;
            --pre)
                # Handle optional label: --pre [label]
                if [[ $# -gt 1 && ! "$2" =~ ^- ]]; then
                    PRERELEASE_LABEL="$2"
                    shift 2
                else
                    PRERELEASE_LABEL="beta"  # Default label
                    shift
                fi
                ;;
            --prefix)
                TAG_PREFIX="${2:-}"
                [[ -z "$TAG_PREFIX" ]] && die "Missing value for --prefix"
                shift 2
                ;;
            --dry-run)
                DRY_RUN="true"
                shift
                ;;
            --push)
                DO_PUSH="true"
                shift
                ;;
            --force)
                FORCE_OVERWRITE="true"
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

# Parse GitHub Actions environment variables and convert to script arguments
parse_github_env_args() {
    echo "GitHub Actions mode detected" >&2

    # Parse GitHub Actions environment variables
    local version="${INPUT_VERSION:-}"
    local bump="${INPUT_BUMP:-patch}"
    local prerelease="${INPUT_PRERELEASE:-false}"
    local prerelease_label="${INPUT_PRERELEASE_LABEL:-beta}"
    local prefix="${INPUT_PREFIX:-v}"
    local force="${INPUT_FORCE:-false}"
    local dry_run="${INPUT_DRY_RUN:-false}"

    # Convert to internal variables
    [[ -n "$version" ]] && EXPLICIT_VERSION="$version"
    BUMP="$bump"
    TAG_PREFIX="$prefix"

    # Handle prerelease
    if [[ "$prerelease" == "true" ]]; then
        PRERELEASE_LABEL="$prerelease_label"
    fi

    # Handle boolean flags
    [[ "$force" == "true" ]] && FORCE_OVERWRITE="true"
    [[ "$dry_run" == "true" ]] && DRY_RUN="true"
    [[ "$dry_run" != "true" ]] && DO_PUSH="true"  # Auto-push unless dry run

    echo "Parsed GitHub inputs: version=$version, bump=$bump, prerelease=$prerelease, label=$prerelease_label" >&2
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

is_valid_semver() {
    [[ "$1" =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]
}

is_valid_bump_type() {
    [[ "$1" =~ ^(patch|minor|major)$ ]]
}

# Parse version from tag: "v1.2.3" -> "1 2 3" or "v1.2.3-beta.4" -> "1 2 3 beta 4"
parse_tag() {
    local tag="$1"
    local escaped_prefix="${TAG_PREFIX//./\\.}"

    if [[ "$tag" =~ ^${escaped_prefix}([0-9]+)\.([0-9]+)\.([0-9]+)(-([A-Za-z0-9]+)\.([0-9]+))?$ ]]; then
        local major="${BASH_REMATCH[1]}"
        local minor="${BASH_REMATCH[2]}"
        local patch="${BASH_REMATCH[3]}"
        local pre_label="${BASH_REMATCH[5]:-}"
        local pre_number="${BASH_REMATCH[6]:-0}"
        echo "$major $minor $patch $pre_label $pre_number"
    else
        die "Invalid tag format: $tag"
    fi
}

# -----------------------------------------------------------------------------
# Version Discovery Functions
# -----------------------------------------------------------------------------

find_latest_stable_tag() {
    # Find latest stable tag (no prerelease suffix)
    local escaped_prefix="${TAG_PREFIX//./\\.}"
    git tag -l "${TAG_PREFIX}[0-9]*" --sort=-version:refname \
        | grep -E "^${escaped_prefix}[0-9]+\.[0-9]+\.[0-9]+$" \
        | head -n1 || true
}

find_latest_prerelease_tag() {
    local label="$1"
    # Find latest tag with specific prerelease label
    git tag -l "${TAG_PREFIX}[0-9]*-${label}.*" --sort=-version:refname \
        | head -n1 || true
}

# -----------------------------------------------------------------------------
# Version Calculation Logic
# -----------------------------------------------------------------------------

calculate_next_version() {
    local latest_stable
    latest_stable=$(find_latest_stable_tag)

    # Default to 0.0.0 if no stable tags exist
    if [[ -z "$latest_stable" ]]; then
        latest_stable="${TAG_PREFIX}0.0.0"
    fi

    # Parse latest stable version
    read -r stable_major stable_minor stable_patch _ _ <<<"$(parse_tag "$latest_stable")"

    echo "Latest stable tag: $latest_stable" >&2

    # Determine CORE version (major.minor.patch)
    if [[ -n "$EXPLICIT_VERSION" ]]; then
        # Use explicit version
        is_valid_semver "$EXPLICIT_VERSION" || die "Invalid version format: $EXPLICIT_VERSION (must be X.Y.Z)"
        IFS='.' read -r CORE_MAJOR CORE_MINOR CORE_PATCH <<<"$EXPLICIT_VERSION"
        echo "Using explicit core version: $EXPLICIT_VERSION" >&2
    else
        # Calculate based on bump type from latest stable
        is_valid_bump_type "$BUMP" || die "Invalid bump type: $BUMP (must be patch, minor, or major)"

        case "$BUMP" in
            major)
                CORE_MAJOR=$((stable_major + 1))
                CORE_MINOR=0
                CORE_PATCH=0
                ;;
            minor)
                CORE_MAJOR=$stable_major
                CORE_MINOR=$((stable_minor + 1))
                CORE_PATCH=0
                ;;
            patch)
                CORE_MAJOR=$stable_major
                CORE_MINOR=$stable_minor
                CORE_PATCH=$((stable_patch + 1))
                ;;
        esac
        echo "Calculated core version ($BUMP bump): $CORE_MAJOR.$CORE_MINOR.$CORE_PATCH" >&2
    fi

    # Handle prerelease logic
    if [[ -n "$PRERELEASE_LABEL" ]]; then
        # Look for existing prerelease with same CORE and LABEL
        local core_version="${CORE_MAJOR}.${CORE_MINOR}.${CORE_PATCH}"
        local existing_prerelease
        existing_prerelease=$(git tag -l "${TAG_PREFIX}${core_version}-${PRERELEASE_LABEL}.*" --sort=-version:refname | head -n1 || true)

        if [[ -n "$existing_prerelease" ]]; then
            # Continue existing prerelease series
            read -r _ _ _ _ pre_number <<<"$(parse_tag "$existing_prerelease")"
            PRERELEASE_NUMBER=$((pre_number + 1))
            echo "Found existing prerelease: $existing_prerelease" >&2
            echo "Incrementing to: ${PRERELEASE_LABEL}.${PRERELEASE_NUMBER}" >&2
        else
            # Start new prerelease series
            PRERELEASE_NUMBER=1
            echo "Starting new prerelease series: ${PRERELEASE_LABEL}.${PRERELEASE_NUMBER}" >&2
        fi

        NEW_TAG="${TAG_PREFIX}${CORE_MAJOR}.${CORE_MINOR}.${CORE_PATCH}-${PRERELEASE_LABEL}.${PRERELEASE_NUMBER}"
    else
        # Stable release
        NEW_TAG="${TAG_PREFIX}${CORE_MAJOR}.${CORE_MINOR}.${CORE_PATCH}"
    fi

    echo "New tag: $NEW_TAG" >&2
}

# -----------------------------------------------------------------------------
# Tag Creation and Management
# -----------------------------------------------------------------------------

check_tag_exists() {
    git rev-parse --verify "refs/tags/$NEW_TAG" >/dev/null 2>&1
}

create_tag() {
    if [[ "$DRY_RUN" == "true" ]]; then
        echo "DRY RUN: Would create tag '$NEW_TAG'" >&2
        return 0
    fi

    # Check if tag already exists
    if check_tag_exists; then
        if [[ "$FORCE_OVERWRITE" == "true" ]]; then
            echo "Tag '$NEW_TAG' already exists, removing..." >&2
            git tag -d "$NEW_TAG" >/dev/null
            git push --delete origin "$NEW_TAG" >/dev/null 2>&1 || true
        else
            die "Tag '$NEW_TAG' already exists. Use --force to overwrite."
        fi
    fi

    # Create the tag
    echo "Creating tag: $NEW_TAG" >&2
    git tag -a "$NEW_TAG" -m "Release $NEW_TAG"
}

push_tag() {
    if [[ "$DRY_RUN" == "true" ]]; then
        echo "DRY RUN: Would push tag '$NEW_TAG' to origin" >&2
        return 0
    fi

    if [[ "$DO_PUSH" == "true" ]]; then
        # Configure git user in CI environment
        if [[ -n "${CI:-}" ]]; then
            git config user.name "${GITHUB_ACTOR:-github-actions}"
            git config user.email "${GITHUB_ACTOR:-github-actions}@users.noreply.github.com"
        fi

        echo "Pushing tag to remote: $NEW_TAG" >&2
        git push origin "$NEW_TAG"
    fi
}

# -----------------------------------------------------------------------------
# Output and Reporting
# -----------------------------------------------------------------------------

print_summary() {
    local status

    if [[ "$DRY_RUN" == "true" ]]; then
        status="dry run (no changes made)"
    elif [[ "$DO_PUSH" == "true" ]]; then
        status="tag created and pushed to remote"
    else
        status="tag created locally (not pushed)"
    fi

    # Console output (to stderr so it doesn't interfere with stdout tag capture)
    {
        echo
        echo "=== TAG SUMMARY ==="
        echo "New tag: $NEW_TAG"
        echo "Core version: $CORE_MAJOR.$CORE_MINOR.$CORE_PATCH"
        [[ -n "$PRERELEASE_LABEL" ]] && echo "Prerelease: $PRERELEASE_LABEL.$PRERELEASE_NUMBER"
        echo "Status: $status"
        echo "=================="
    } >&2

    # GitHub Actions step summary
    if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
        {
            echo "### 🏷️ Tag Creation Summary"
            echo
            echo "| Property | Value |"
            echo "|----------|-------|"
            echo "| **New Tag** | \`$NEW_TAG\` |"
            echo "| **Core Version** | \`$CORE_MAJOR.$CORE_MINOR.$CORE_PATCH\` |"
            [[ -n "$PRERELEASE_LABEL" ]] && echo "| **Prerelease** | \`$PRERELEASE_LABEL.$PRERELEASE_NUMBER\` |"
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

    echo "Calculating next version..." >&2
    calculate_next_version

    echo "Creating tag..." >&2
    create_tag

    if [[ "$DO_PUSH" == "true" ]]; then
        echo "Pushing to remote..." >&2
        push_tag
    fi

    print_summary

    # Output the tag name for consumption by other scripts (stdout only)
    echo "$NEW_TAG"
}

# Run main function with all arguments
main "$@"
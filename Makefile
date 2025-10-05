# Z3Wrap Library Makefile
# Provides convenient commands for building, testing, and coverage

.PHONY: help build test clean coverage coverage-open restore format lint release all ci test-release release-notes pack publish-build dev-setup quick watch info version generate-native generate-library
.DEFAULT_GOAL := help

# Colors for output
GREEN := \033[0;32m
BLUE := \033[0;34m
YELLOW := \033[0;33m
RED := \033[0;31m
NC := \033[0m # No Color

help: ## Show this help message
	@echo "$(BLUE)Z3Wrap Library - Available Commands$(NC)"
	@echo ""
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  $(GREEN)%-15s$(NC) %s\n", $$1, $$2}' $(MAKEFILE_LIST)

# =============================================================================
# Core Build Commands
# =============================================================================

restore: ## Restore NuGet packages
	@echo "$(BLUE)Restoring packages...$(NC)"
	dotnet restore

clean: ## Clean build artifacts
	@echo "$(BLUE)Cleaning build artifacts...$(NC)"
	dotnet clean
	rm -rf Z3Wrap.Tests/TestResults coverage-reports

build: restore ## Build the library (debug mode)
	@echo "$(BLUE)Building Z3Wrap Library...$(NC)"
	dotnet build --no-restore

release-notes: ## Generate RELEASE_NOTES.md and RELEASE_NOTES_ESCAPED.txt from CHANGELOG [Unreleased]
	@echo "$(BLUE)Generating release notes from CHANGELOG...$(NC)"
	scripts/extract-notes.sh --section "Unreleased" --output "RELEASE_NOTES.md"
	@echo "$(BLUE)Generating XML-escaped release notes for NuGet...$(NC)"
	scripts/extract-notes.sh --section "Unreleased" --output "RELEASE_NOTES_ESCAPED.txt" --format "xml-escaped"

update-changelog: ## Update CHANGELOG.md: move [Unreleased] to latest git tag
	@echo "$(BLUE)Updating CHANGELOG.md with latest tag...$(NC)"
	scripts/update-changelog.sh

release: restore release-notes ## Build in release mode
	@echo "$(BLUE)Building Z3Wrap Library (Release)...$(NC)"
	dotnet build --configuration Release --no-restore

# =============================================================================
# Test Commands
# =============================================================================

test: build ## Run all tests (use TEST_FILTER=name to run specific tests)
	@echo "$(BLUE)Running tests...$(NC)"
	dotnet test --no-restore --no-build --logger:"console;verbosity=minimal" $(if $(TEST_FILTER),--filter "$(TEST_FILTER)",)

test-verbose: build ## Run tests with detailed output (use TEST_FILTER=name to run specific tests)
	@echo "$(BLUE)Running tests (verbose)...$(NC)"
	dotnet test --no-restore --no-build --logger:"console;verbosity=detailed" $(if $(TEST_FILTER),--filter "$(TEST_FILTER)",)

test-release: release ## Run tests in release mode (use TEST_FILTER=name to run specific tests)
	@echo "$(BLUE)Running tests (Release mode)...$(NC)"
	dotnet test --configuration Release --no-build --logger:"console;verbosity=minimal" $(if $(TEST_FILTER),--filter "$(TEST_FILTER)",)

watch: ## Run tests in watch mode (use TEST_FILTER=name to run specific tests)
	@echo "$(BLUE)Running tests in watch mode (Ctrl+C to stop)...$(NC)"
	dotnet test --watch $(if $(TEST_FILTER),--filter "$(TEST_FILTER)",)

# =============================================================================
# Quality & Coverage Commands
# =============================================================================

format: ## Format code (requires csharpier)
	@echo "$(BLUE)Formatting code...$(NC)"
	@if command -v csharpier >/dev/null 2>&1; then \
		csharpier format .; \
	else \
		echo "$(YELLOW)CSharpier not installed. Run: make dev-setup$(NC)"; \
	fi

lint: ## Run static analysis (format check)
	@echo "$(BLUE)Running static analysis...$(NC)"
	@if command -v csharpier >/dev/null 2>&1; then \
		csharpier check .; \
	else \
		echo "$(YELLOW)CSharpier not available for lint check$(NC)"; \
		echo "$(YELLOW)Install with: make dev-setup$(NC)"; \
		exit 1; \
	fi

coverage: ## Run tests with coverage and generate HTML report
	@echo "$(BLUE)Running test coverage analysis...$(NC)"
	@./scripts/coverage.sh

coverage-open: coverage ## Run coverage and open HTML report (macOS)
	@echo "$(GREEN)Opening coverage report...$(NC)"
	@if command -v open >/dev/null 2>&1; then \
		open coverage-reports/index.html; \
	elif command -v xdg-open >/dev/null 2>&1; then \
		xdg-open coverage-reports/index.html; \
	else \
		echo "$(YELLOW)Please open coverage-reports/index.html manually$(NC)"; \
	fi

# =============================================================================
# Publishing Commands
# =============================================================================

pack: test-release ## Create NuGet packages (includes generated release notes)
	@echo "$(BLUE)Creating NuGet packages...$(NC)"
	dotnet pack -c Release --no-build -o artifacts

# =============================================================================
# Workflow Commands
# =============================================================================

quick: build test ## Quick test (build + test without coverage)
	@echo "$(GREEN)✅ Quick validation passed!$(NC)"

all: restore build test ## Full build pipeline (restore, build, test)
	@echo "$(GREEN)✅ All tasks completed successfully!$(NC)"

ci: restore lint build test coverage ## CI pipeline (restore, lint, build, test, coverage)
	@echo "$(GREEN)✅ CI pipeline completed successfully!$(NC)"

publish-build: restore release test-release ## Build for publishing (restore, release build, release test)
	@echo "$(GREEN)✅ Publish build completed successfully!$(NC)"

# =============================================================================
# Code Generation Commands
# =============================================================================

generate-native: ## Generate NativeZ3Library partial classes from Z3 headers (VERBOSE=1, BRANCH=<name>, FORCE=1, ENUMS_ONLY=1)
	@echo "$(BLUE)Generating NativeZ3Library from Z3 headers...$(NC)"
	@python3 scripts/generate_native_library.py \
		$(if $(VERBOSE),--verbose,) \
		$(if $(BRANCH),--branch $(BRANCH),) \
		$(if $(FORCE),--force-download,) \
		$(if $(ENUMS_ONLY),--enums-only,)
	@echo "$(GREEN)✅ Generated in Z3Wrap/Core/Interop/$(NC)"
	@echo "$(BLUE)Formatting generated code...$(NC)"
	@$(MAKE) format

generate-library: ## Generate Z3Library partial classes from NativeZ3Library (ENUMS_ONLY=1)
	@echo "$(BLUE)Generating Z3Library partial classes...$(NC)"
	@python3 scripts/generate_library.py \
		$(if $(ENUMS_ONLY),--enums-only,)
	@echo "$(GREEN)✅ Generated in Z3Wrap/Core/$(NC)"
	@echo "$(BLUE)Formatting generated code...$(NC)"
	@$(MAKE) format

# =============================================================================
# Setup & Info Commands
# =============================================================================

dev-setup: ## Setup development environment
	@echo "$(BLUE)Setting up development environment...$(NC)"
	dotnet tool update --global dotnet-reportgenerator-globaltool || dotnet tool install --global dotnet-reportgenerator-globaltool || true
	dotnet tool update --global csharpier || dotnet tool install --global csharpier || true
	@echo "$(GREEN)✅ Development tools installed/updated$(NC)"

info: ## Show project information
	@echo "$(BLUE)Z3Wrap Library Project Information$(NC)"
	@echo ""
	@echo "Target Framework: .NET 9.0"
	@echo "Test Framework:   NUnit 4"
	@echo "Coverage Tool:    Coverlet + ReportGenerator"
	@echo ""
	@echo "$(GREEN)Quick Start:$(NC)"
	@echo "  make build      # Build the library"
	@echo "  make test       # Run tests"
	@echo "  make coverage   # Generate coverage report"
	@echo ""

version: ## Show .NET version
	@dotnet --version
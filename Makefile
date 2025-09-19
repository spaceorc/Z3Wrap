# Z3 Library Makefile
# Provides convenient commands for building, testing, and coverage

.PHONY: help build test clean coverage coverage-open restore format lint release debug
.DEFAULT_GOAL := help

# Colors for output
GREEN := \033[0;32m
BLUE := \033[0;34m
YELLOW := \033[0;33m
RED := \033[0;31m
NC := \033[0m # No Color

help: ## Show this help message
	@echo "$(BLUE)Z3 Library - Available Commands$(NC)"
	@echo ""
	@awk 'BEGIN {FS = ":.*?## "} /^[a-zA-Z_-]+:.*?## / {printf "  $(GREEN)%-15s$(NC) %s\n", $$1, $$2}' $(MAKEFILE_LIST)

build: ## Build the library
	@echo "$(BLUE)Building Z3 Library...$(NC)"
	dotnet build

debug: ## Build in debug mode
	@echo "$(BLUE)Building Z3 Library (Debug)...$(NC)"
	dotnet build --configuration Debug

release: ## Build in release mode
	@echo "$(BLUE)Building Z3 Library (Release)...$(NC)"
	dotnet build --configuration Release

test: ## Run all tests
	@echo "$(BLUE)Running tests...$(NC)"
	dotnet test --logger:"console;verbosity=minimal"

test-verbose: ## Run tests with detailed output
	@echo "$(BLUE)Running tests (verbose)...$(NC)"
	dotnet test --logger:"console;verbosity=detailed"

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

clean: ## Clean build artifacts
	@echo "$(BLUE)Cleaning build artifacts...$(NC)"
	dotnet clean
	rm -rf Z3Wrap.Tests/TestResults coverage-reports

restore: ## Restore NuGet packages
	@echo "$(BLUE)Restoring packages...$(NC)"
	dotnet restore

format: ## Format code (requires csharpier)
	@echo "$(BLUE)Formatting code...$(NC)"
	@if command -v csharpier >/dev/null 2>&1; then \
		csharpier format .; \
	else \
		echo "$(YELLOW)CSharpier not installed. Install with: dotnet tool install -g csharpier$(NC)"; \
	fi

lint: build ## Run static analysis (build + format check)
	@echo "$(BLUE)Running static analysis...$(NC)"
	@if command -v csharpier >/dev/null 2>&1; then \
		csharpier check .; \
	else \
		echo "$(YELLOW)CSharpier not available for lint check$(NC)"; \
	fi

all: restore build test ## Full build pipeline (restore, build, test)
	@echo "$(GREEN)✅ All tasks completed successfully!$(NC)"

ci: restore build test coverage ## CI pipeline (restore, build, test, coverage)
	@echo "$(GREEN)✅ CI pipeline completed successfully!$(NC)"

# Development workflow commands
dev-setup: ## Setup development environment
	@echo "$(BLUE)Setting up development environment...$(NC)"
	dotnet tool install --global dotnet-reportgenerator-globaltool || true
	dotnet tool install --global csharpier || true
	@echo "$(GREEN)✅ Development tools installed$(NC)"

quick: ## Quick test (build + test without coverage)  
	@echo "$(BLUE)Running quick validation...$(NC)"
	@$(MAKE) build
	@$(MAKE) test
	@echo "$(GREEN)✅ Quick validation passed!$(NC)"

watch: ## Run tests in watch mode
	@echo "$(BLUE)Running tests in watch mode (Ctrl+C to stop)...$(NC)"
	dotnet test --watch

# Info commands
info: ## Show project information
	@echo "$(BLUE)Z3 Library Project Information$(NC)"
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
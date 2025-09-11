#!/bin/bash

# Z3 Library Test Coverage Script
# Runs tests with coverage collection and generates HTML report

echo "🧪 Running tests with coverage collection..."

# Clean previous coverage data
rm -rf Z3Wrap.Tests/TestResults coverage-reports

# Run tests with code coverage collection
dotnet test --collect:"XPlat Code Coverage" --logger:"console;verbosity=minimal"

if [ $? -ne 0 ]; then
    echo "❌ Tests failed. Aborting coverage report generation."
    exit 1
fi

echo "📊 Generating coverage report..."

# Generate HTML coverage report
reportgenerator \
    -reports:"Z3Wrap.Tests/TestResults/*/coverage.cobertura.xml" \
    -targetdir:"coverage-reports" \
    -reporttypes:"Html;TextSummary;Badges" \
    -title:"Z3 Library Coverage Report"

if [ $? -eq 0 ]; then
    echo "✅ Coverage report generated successfully!"
    echo ""
    echo "📈 Coverage Summary:"
    cat coverage-reports/Summary.txt | grep -A 20 "Summary"
    echo ""
    echo "🌐 Open coverage-reports/index.html to view the detailed HTML report"
    echo "🏷️  Coverage badges generated in coverage-reports/badge_*.svg"
else
    echo "❌ Failed to generate coverage report"
    exit 1
fi
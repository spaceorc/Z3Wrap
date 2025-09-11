# Z3 Library Test Coverage Script (PowerShell)
# Runs tests with coverage collection and generates HTML report

Write-Host "🧪 Running tests with coverage collection..." -ForegroundColor Blue

# Clean previous coverage data
Remove-Item -Path "tests/TestResults" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path "coverage-reports" -Recurse -Force -ErrorAction SilentlyContinue

# Run tests with code coverage collection
dotnet test --collect:"XPlat Code Coverage" --logger:"console;verbosity=minimal"

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Tests failed. Aborting coverage report generation." -ForegroundColor Red
    exit 1
}

Write-Host "📊 Generating coverage report..." -ForegroundColor Blue

# Generate HTML coverage report
reportgenerator `
    -reports:"tests/TestResults/*/coverage.cobertura.xml" `
    -targetdir:"coverage-reports" `
    -reporttypes:"Html;TextSummary;Badges" `
    -title:"Z3 Library Coverage Report"

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Coverage report generated successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📈 Coverage Summary:" -ForegroundColor Yellow
    Get-Content "coverage-reports/Summary.txt" | Select-String -Pattern "Summary" -Context 0,20
    Write-Host ""
    Write-Host "🌐 Open coverage-reports/index.html to view the detailed HTML report" -ForegroundColor Cyan
    Write-Host "🏷️  Coverage badges generated in coverage-reports/badge_*.svg" -ForegroundColor Cyan
} else {
    Write-Host "❌ Failed to generate coverage report" -ForegroundColor Red
    exit 1
}
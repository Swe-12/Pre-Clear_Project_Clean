#!/usr/bin/env pwsh

$ErrorActionPreference = "Continue"
$WarningPreference = "SilentlyContinue"

Write-Host "Starting PreClear Backend..." -ForegroundColor Green
Write-Host "Working directory: $(Get-Location)" -ForegroundColor Cyan
Write-Host "Time: $(Get-Date)" -ForegroundColor Cyan

try {
    dotnet run --no-launch-profile 2>&1 | Tee-Object -FilePath "backend.log"
} catch {
    Write-Host "Error starting backend:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host $_.Exception.StackTrace -ForegroundColor Red
    exit 1
}

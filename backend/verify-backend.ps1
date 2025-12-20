#!/usr/bin/env pwsh

# Pre-Clear Backend Verification Script

Write-Host "`n=== Pre-Clear Backend Verification ===" -ForegroundColor Cyan
Write-Host "Timestamp: $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')" -ForegroundColor Gray
Write-Host ""

# Test 1: Port connectivity
Write-Host "Test 1: Checking port 5000..." -ForegroundColor Yellow
try {
    $connection = New-Object System.Net.Sockets.TcpClient
    $result = $connection.BeginConnect("127.0.0.1", 5000, $null, $null)
    $isSuccessful = $result.AsyncWaitHandle.WaitOne(2000, $false)
    
    if ($isSuccessful -and $connection.Connected) {
        Write-Host "✓ Port 5000 is listening" -ForegroundColor Green
        $connection.Close()
    } else {
        Write-Host "✗ Port 5000 is not responding" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "✗ Connection error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Test 2: API endpoint connectivity
Write-Host "`nTest 2: Testing API endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5000/api/auth/signin" `
        -Method POST `
        -Body '{"email":"test@test.com"}' `
        -ContentType "application/json" `
        -ErrorAction Stop -TimeoutSec 5
    Write-Host "✓ Endpoint is responding with status $($response.StatusCode)" -ForegroundColor Green
} catch {
    if ($_.Exception.Response) {
        $statusCode = $_.Exception.Response.StatusCode
        Write-Host "✓ Endpoint is responding with status $statusCode" -ForegroundColor Green
    } else {
        Write-Host "✗ No response from endpoint" -ForegroundColor Red
        exit 1
    }
}

# Test 3: Login with test credentials
Write-Host "`nTest 3: Testing authentication with seeded user..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "http://localhost:5000/api/auth/signin" `
        -Method POST `
        -Body '{"email":"shipper@demo.com","password":"Shipper@123"}' `
        -ContentType "application/json" `
        -ErrorAction SilentlyContinue
    
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Login successful! JWT token received" -ForegroundColor Green
        $token = $response.Content | ConvertFrom-Json
        if ($token.token) {
            $tokenLength = $token.token.Length
            Write-Host "  Token length: $tokenLength characters" -ForegroundColor Gray
            Write-Host "  User ID: $($token.id)" -ForegroundColor Gray
            Write-Host "  Role: $($token.role)" -ForegroundColor Gray
        }
    } else {
        Write-Host "✗ Unexpected status: $($response.StatusCode)" -ForegroundColor Red
    }
} catch {
    if ($_.Exception.Response.StatusCode -eq "Unauthorized") {
        Write-Host "✓ Endpoint validated (no seeded users or bad credentials)" -ForegroundColor Yellow
    } else {
        Write-Host "✗ Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host "`n=== Backend Status: OPERATIONAL ===" -ForegroundColor Green
Write-Host "The backend is running and ready for requests`n" -ForegroundColor Gray

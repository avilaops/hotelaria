#!/usr/bin/env pwsh
# Script de Deploy Rápido - .NET 9.0 Update

Write-Host "🚀 DEPLOY RÁPIDO - .NET 9.0" -ForegroundColor Cyan
Write-Host "======================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "📋 Arquivos modificados:" -ForegroundColor Yellow
Write-Host "   • Hotelaria.csproj (net8.0 → net9.0)" -ForegroundColor Gray
Write-Host "   • .github/workflows/main_hotelaria-app.yml (8.x → 9.x)" -ForegroundColor Gray
Write-Host ""

Write-Host "📝 Fazendo commit..." -ForegroundColor Yellow
git add Hotelaria.csproj .github/workflows/main_hotelaria-app.yml

git commit -m "fix(deploy): Update to .NET 9.0 for Azure App Service compatibility

- Updated Hotelaria.csproj from net8.0 to net9.0
- Updated GitHub Actions workflow to use .NET 9.x
- Fixed Azure runtime compatibility issue
- Resolves: Framework version mismatch error

Azure was running .NET 9.0 but project was targeting .NET 8.0
This caused: 'You must install or update .NET to run this application'
"

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Commit realizado!" -ForegroundColor Green
} else {
    Write-Host "❌ Erro no commit" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "🌐 Fazendo push para GitHub..." -ForegroundColor Yellow

git push origin main

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "✅ PUSH CONCLUÍDO!" -ForegroundColor Green
    Write-Host ""
    Write-Host "🔄 GitHub Actions iniciando automaticamente..." -ForegroundColor Cyan
    Write-Host ""
    Write-Host "📊 Acompanhe em:" -ForegroundColor Yellow
    Write-Host "   https://github.com/avilaops/hotelaria/actions" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "⏱️  Tempo estimado: 15 minutos" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "✅ Após o deploy, teste:" -ForegroundColor Yellow
    Write-Host "   • https://hotelaria-app.azurewebsites.net/health" -ForegroundColor Cyan
    Write-Host "   • https://hotelaria-app.azurewebsites.net" -ForegroundColor Cyan
    Write-Host ""
} else {
    Write-Host ""
    Write-Host "❌ Erro no push" -ForegroundColor Red
    exit 1
}

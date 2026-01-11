#!/usr/bin/env pwsh
# Script para iniciar o Sistema Hotelaria localmente

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  🏨 SISTEMA HOTELARIA" -ForegroundColor White
Write-Host "  Iniciando Modo Local" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se está no diretório correto
if (-not (Test-Path "Hotelaria.csproj")) {
    Write-Host "❌ Erro: Hotelaria.csproj não encontrado!" -ForegroundColor Red
    Write-Host "Execute este script no diretório raiz do projeto." -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ Projeto encontrado" -ForegroundColor Green
Write-Host ""

Write-Host "🔧 Configurações:" -ForegroundColor Yellow
Write-Host "   • Modo: Desenvolvimento Local" -ForegroundColor Gray
Write-Host "   • Hot Reload: Ativado (salve arquivos para recarregar)" -ForegroundColor Gray
Write-Host ""

Write-Host "🌐 URLs da aplicação:" -ForegroundColor Yellow
Write-Host "   • HTTP:  http://localhost:5000" -ForegroundColor Cyan
Write-Host "   • HTTPS: https://localhost:5001" -ForegroundColor Cyan
Write-Host ""

Write-Host "💡 Dicas:" -ForegroundColor Yellow
Write-Host "   • Pressione Ctrl+C para parar o servidor" -ForegroundColor Gray
Write-Host "   • Aguarde 'Now listening on...' antes de abrir o navegador" -ForegroundColor Gray
Write-Host "   • Alterações no código serão recarregadas automaticamente" -ForegroundColor Gray
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "⏳ Compilando e iniciando servidor..." -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Iniciar com watch para hot reload
dotnet watch run --urls "http://localhost:5000;https://localhost:5001"

#!/usr/bin/env pwsh
# Script para configurar variáveis de ambiente no Azure App Service
# Sistema Hotelaria v2.6.2

param(
    [string]$ResourceGroup = "hotelaria-app",
    [string]$AppName = "hotelaria-app"
)

Write-Host "=" -NoNewline -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "🔧 Configurar Variáveis de Ambiente no Azure" -ForegroundColor White
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se Azure CLI está instalado
$azInstalled = Get-Command az -ErrorAction SilentlyContinue
if (-not $azInstalled) {
    Write-Host "❌ Azure CLI não está instalado!" -ForegroundColor Red
    Write-Host "📥 Instale em: https://aka.ms/installazurecliwindows" -ForegroundColor Yellow
    exit 1
}

# Verificar se está logado
Write-Host "🔐 Verificando autenticação..." -ForegroundColor Yellow
$account = az account show 2>$null | ConvertFrom-Json -ErrorAction SilentlyContinue

if (-not $account) {
    Write-Host "⚠️  Não autenticado. Fazendo login..." -ForegroundColor Yellow
    az login --only-show-errors
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Falha no login" -ForegroundColor Red
        exit 1
    }
    
    $account = az account show 2>$null | ConvertFrom-Json
}

Write-Host "✅ Autenticado como: $($account.user.name)" -ForegroundColor Green
Write-Host ""

# Ler variáveis do .env
Write-Host "📄 Lendo configurações do arquivo .env..." -ForegroundColor Yellow

if (-not (Test-Path ".env")) {
    Write-Host "❌ Arquivo .env não encontrado!" -ForegroundColor Red
    Write-Host "💡 Crie um arquivo .env com as variáveis necessárias" -ForegroundColor Yellow
    exit 1
}

$envVars = @{}
$envContent = Get-Content .env

foreach ($line in $envContent) {
    # Ignorar comentários e linhas vazias
    if ($line -match '^\s*#' -or $line -match '^\s*$' -or $line -match '^Links') {
        continue
    }
    
    # Parse KEY=VALUE
    if ($line -match '^([^=]+)=(.*)$') {
        $key = $matches[1].Trim()
        $value = $matches[2].Trim()
        
        # Apenas variáveis importantes para o sistema
        $importantVars = @(
            'MONGO_ATLAS_URI',
            'MONGO_CONNECTION_STRING',
            'AIRBNB_CLIENT_KEY',
            'AIRBNB_SECRET_KEY',
            'PAYPAL_ID',
            'PAYPAL_TOKEN_API',
            'SENTRY_TOKEN_API',
            'ASPNETCORE_ENVIRONMENT'
        )
        
        if ($importantVars -contains $key -and -not [string]::IsNullOrWhiteSpace($value)) {
            $envVars[$key] = $value
        }
    }
}

Write-Host "✅ $($envVars.Count) variáveis encontradas" -ForegroundColor Green
Write-Host ""

# Configurar variáveis no Azure
Write-Host "🚀 Configurando variáveis no Azure App Service..." -ForegroundColor Yellow
Write-Host "   Resource Group: $ResourceGroup" -ForegroundColor Gray
Write-Host "   App Name: $AppName" -ForegroundColor Gray
Write-Host ""

$settings = @()

foreach ($key in $envVars.Keys) {
    $value = $envVars[$key]
    $settings += "$key=$value"
    
    # Mostrar versão mascarada
    $maskedValue = if ($value.Length -gt 8) {
        "$($value.Substring(0, 4))...$($value.Substring($value.Length - 4))"
    } else {
        "***"
    }
    
    Write-Host "   📝 $key = $maskedValue" -ForegroundColor Cyan
}

# Adicionar configurações fixas do Azure
$settings += "WEBSITES_PORT=8080"
$settings += "ASPNETCORE_ENVIRONMENT=Production"
$settings += "WEBSITE_TIME_ZONE=E. South America Standard Time"

Write-Host ""
Write-Host "⏳ Aplicando configurações..." -ForegroundColor Yellow

try {
    $result = az webapp config appsettings set `
        --resource-group $ResourceGroup `
        --name $AppName `
        --settings $settings `
        --only-show-errors 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✅ Variáveis configuradas com sucesso!" -ForegroundColor Green
        Write-Host ""
        
        # Reiniciar app
        Write-Host "🔄 Reiniciando aplicação..." -ForegroundColor Yellow
        az webapp restart --resource-group $ResourceGroup --name $AppName --only-show-errors
        
        if ($LASTEXITCODE -eq 0) {
            Write-Host "✅ Aplicação reiniciada!" -ForegroundColor Green
        }
    }
    else {
        Write-Host ""
        Write-Host "❌ Erro ao configurar variáveis:" -ForegroundColor Red
        Write-Host $result -ForegroundColor Yellow
        exit 1
    }
}
catch {
    Write-Host ""
    Write-Host "❌ Erro: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "✅ Configuração Completa!" -ForegroundColor Green
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "📊 Próximos passos:" -ForegroundColor Yellow
Write-Host "   1. Aguarde 2-3 minutos para o app reiniciar" -ForegroundColor Gray
Write-Host "   2. Teste o health check: https://$AppName.azurewebsites.net/health" -ForegroundColor Gray
Write-Host "   3. Acesse a aplicação: https://$AppName.azurewebsites.net" -ForegroundColor Gray
Write-Host ""
Write-Host "🔍 Para ver logs:" -ForegroundColor Yellow
Write-Host "   az webapp log tail --resource-group $ResourceGroup --name $AppName" -ForegroundColor Cyan
Write-Host ""

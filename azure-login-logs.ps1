#!/usr/bin/env pwsh
# Script para fazer login no Azure e buscar logs
# Sistema Hotelaria

Write-Host "=" -NoNewline -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "🔐 Azure Login e Busca de Logs" -ForegroundColor White
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

$appName = "hotelaria-app"
$resourceGroup = "hotelaria-app"

# Verificar se Azure CLI está instalado
$azInstalled = Get-Command az -ErrorAction SilentlyContinue
if (-not $azInstalled) {
    Write-Host "❌ Azure CLI não está instalado!" -ForegroundColor Red
    Write-Host ""
    Write-Host "📥 Para instalar:" -ForegroundColor Yellow
    Write-Host "   Windows: https://aka.ms/installazurecliwindows" -ForegroundColor Cyan
    Write-Host "   macOS: brew install azure-cli" -ForegroundColor Cyan
    Write-Host "   Linux: curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash" -ForegroundColor Cyan
    Write-Host ""
    exit 1
}

Write-Host "✅ Azure CLI encontrado" -ForegroundColor Green
Write-Host ""

# Fazer login
Write-Host "🔐 Fazendo login no Azure..." -ForegroundColor Yellow
Write-Host "   💡 Uma janela do navegador será aberta" -ForegroundColor Gray
Write-Host "   💡 Faça login com sua conta Microsoft/Azure" -ForegroundColor Gray
Write-Host ""

try {
    az login --only-show-errors
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "✅ Login bem-sucedido!" -ForegroundColor Green
        Write-Host ""
        
        # Mostrar conta atual
        $account = az account show 2>$null | ConvertFrom-Json
        Write-Host "👤 Conta: $($account.user.name)" -ForegroundColor Cyan
        Write-Host "📝 Subscription: $($account.name)" -ForegroundColor Cyan
        Write-Host ""
        
        # Buscar logs
        Write-Host "======================================================" -ForegroundColor Cyan
        Write-Host "📋 BUSCANDO LOGS DO APP SERVICE" -ForegroundColor White
        Write-Host "======================================================" -ForegroundColor Cyan
        Write-Host ""
        
        Write-Host "🔍 Verificando status do App Service..." -ForegroundColor Yellow
        $appStatus = az webapp show --name $appName --resource-group $resourceGroup --query "state" -o tsv 2>$null
        
        if ($appStatus) {
            if ($appStatus -eq "Running") {
                Write-Host "   ✅ Status: $appStatus" -ForegroundColor Green
            }
            else {
                Write-Host "   ⚠️  Status: $appStatus" -ForegroundColor Yellow
            }
        }
        Write-Host ""
        
        Write-Host "📄 Últimos logs do aplicativo (últimas 100 linhas):" -ForegroundColor Yellow
        Write-Host "======================================================" -ForegroundColor Gray
        Write-Host ""
        
        # Tentar múltiplas fontes de log
        Write-Host "🔍 Tentando buscar logs do application..." -ForegroundColor Gray
        $logs = az webapp log download --name $appName --resource-group $resourceGroup --log-file "webapp-logs.zip" 2>&1
        
        if (Test-Path "webapp-logs.zip") {
            Write-Host "   ✅ Logs baixados para: webapp-logs.zip" -ForegroundColor Green
            Write-Host "   💡 Extraia o arquivo para ver os logs completos" -ForegroundColor Cyan
        }
        
        Write-Host ""
        Write-Host "🔍 Buscando logs via tail..." -ForegroundColor Gray
        Write-Host ""
        
        # Tail logs (tempo real)
        Write-Host "======================================================" -ForegroundColor Cyan
        Write-Host "📡 LOGS EM TEMPO REAL (Ctrl+C para parar)" -ForegroundColor White
        Write-Host "======================================================" -ForegroundColor Cyan
        Write-Host ""
        
        az webapp log tail --name $appName --resource-group $resourceGroup 2>&1 | ForEach-Object {
            $line = $_
            
            # Colorir baseado no conteúdo
            if ($line -match "error|exception|fail|crash") {
                Write-Host $line -ForegroundColor Red
            }
            elseif ($line -match "warning|warn") {
                Write-Host $line -ForegroundColor Yellow
            }
            elseif ($line -match "success|started|listening|running") {
                Write-Host $line -ForegroundColor Green
            }
            else {
                Write-Host $line -ForegroundColor Gray
            }
        }
    }
    else {
        Write-Host ""
        Write-Host "❌ Falha no login" -ForegroundColor Red
        exit 1
    }
}
catch {
    Write-Host ""
    Write-Host "❌ Erro: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

#!/usr/bin/env pwsh
# Script de Deploy Manual de Emergência para Azure
# Sistema Hotelaria v2.6.2
# Use apenas se GitHub Actions falhar

param(
    [string]$ResourceGroup = "hotelaria-app",
    [string]$AppName = "hotelaria-app",
    [switch]$SkipBuild,
    [switch]$Force
)

Write-Host "=" -NoNewline -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "🚨 DEPLOY MANUAL DE EMERGÊNCIA" -ForegroundColor Red
Write-Host "  Sistema Hotelaria v2.6.2" -ForegroundColor Yellow
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "⚠️  ATENÇÃO: Este é um deploy manual de emergência!" -ForegroundColor Yellow
Write-Host "   Use apenas se o GitHub Actions não estiver funcionando." -ForegroundColor Yellow
Write-Host ""

if (-not $Force) {
    Write-Host "💡 Para confirmar, execute novamente com -Force" -ForegroundColor Cyan
    Write-Host "   Exemplo: .\deploy-manual-emergencia.ps1 -Force" -ForegroundColor Gray
    Write-Host ""
    exit 0
}

$ErrorActionPreference = "Stop"

# ==========================================
# 1. VERIFICAR AZURE CLI
# ==========================================
Write-Host "🔍 Fase 1: Verificando pré-requisitos..." -ForegroundColor Yellow
Write-Host ""

$azInstalled = Get-Command az -ErrorAction SilentlyContinue
if (-not $azInstalled) {
    Write-Host "❌ Azure CLI não está instalado!" -ForegroundColor Red
    Write-Host "📥 Instale em: https://aka.ms/installazurecliwindows" -ForegroundColor Yellow
    exit 1
}

Write-Host "   ✅ Azure CLI encontrado" -ForegroundColor Green

# Verificar autenticação
Write-Host "   🔐 Verificando autenticação..." -ForegroundColor Gray
$account = az account show 2>$null | ConvertFrom-Json -ErrorAction SilentlyContinue

if (-not $account) {
    Write-Host "   ⚠️  Não autenticado. Fazendo login..." -ForegroundColor Yellow
    az login --only-show-errors
    
    if ($LASTEXITCODE -ne 0) {
        Write-Host "   ❌ Falha no login" -ForegroundColor Red
        exit 1
    }
    
    $account = az account show 2>$null | ConvertFrom-Json
}

Write-Host "   ✅ Autenticado como: $($account.user.name)" -ForegroundColor Green
Write-Host ""

# ==========================================
# 2. BUILD DO PROJETO
# ==========================================
if (-not $SkipBuild) {
    Write-Host "🔨 Fase 2: Build do Projeto..." -ForegroundColor Yellow
    Write-Host ""

    Write-Host "   Limpando projeto..." -ForegroundColor Gray
    dotnet clean --verbosity quiet

    Write-Host "   Restaurando dependências..." -ForegroundColor Gray
    dotnet restore --verbosity quiet

    Write-Host "   Compilando Release..." -ForegroundColor Gray
    dotnet build --configuration Release --no-restore

    if ($LASTEXITCODE -ne 0) {
        Write-Host ""
        Write-Host "   ❌ Build falhou!" -ForegroundColor Red
        exit 1
    }

    Write-Host "   ✅ Build concluído" -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "⏭️  Fase 2: Build pulado (--SkipBuild)" -ForegroundColor Gray
    Write-Host ""
}

# ==========================================
# 3. PUBLISH
# ==========================================
Write-Host "📦 Fase 3: Publish..." -ForegroundColor Yellow
Write-Host ""

$publishDir = ".\deploy-temp"
Write-Host "   Diretório de publish: $publishDir" -ForegroundColor Gray

if (Test-Path $publishDir) {
    Write-Host "   Removendo publish anterior..." -ForegroundColor Gray
    Remove-Item -Path $publishDir -Recurse -Force
}

Write-Host "   Executando publish..." -ForegroundColor Gray
dotnet publish -c Release -o $publishDir --no-build

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "   ❌ Publish falhou!" -ForegroundColor Red
    exit 1
}

$publishSize = (Get-ChildItem -Path $publishDir -Recurse | Measure-Object -Property Length -Sum).Sum
$publishSizeMB = [math]::Round($publishSize / 1MB, 2)

Write-Host "   ✅ Publish concluído ($publishSizeMB MB)" -ForegroundColor Green
Write-Host ""

# ==========================================
# 4. CRIAR ZIP
# ==========================================
Write-Host "📦 Fase 4: Criando pacote ZIP..." -ForegroundColor Yellow
Write-Host ""

$zipFile = ".\hotelaria-deploy-$(Get-Date -Format 'yyyyMMdd-HHmmss').zip"
Write-Host "   Arquivo: $zipFile" -ForegroundColor Gray

if (Test-Path $zipFile) {
    Remove-Item $zipFile -Force
}

Write-Host "   Compactando..." -ForegroundColor Gray
Compress-Archive -Path "$publishDir\*" -DestinationPath $zipFile -Force

if (-not (Test-Path $zipFile)) {
    Write-Host ""
    Write-Host "   ❌ Falha ao criar ZIP!" -ForegroundColor Red
    exit 1
}

$zipSize = (Get-Item $zipFile).Length
$zipSizeMB = [math]::Round($zipSize / 1MB, 2)

Write-Host "   ✅ ZIP criado ($zipSizeMB MB)" -ForegroundColor Green
Write-Host ""

# ==========================================
# 5. DEPLOY PARA AZURE
# ==========================================
Write-Host "🚀 Fase 5: Deploy para Azure..." -ForegroundColor Yellow
Write-Host "   Resource Group: $ResourceGroup" -ForegroundColor Gray
Write-Host "   App Name: $AppName" -ForegroundColor Gray
Write-Host ""

Write-Host "   Parando aplicação..." -ForegroundColor Gray
az webapp stop --resource-group $ResourceGroup --name $AppName --only-show-errors 2>$null

Write-Host "   Fazendo upload do ZIP..." -ForegroundColor Gray
az webapp deployment source config-zip `
    --resource-group $ResourceGroup `
    --name $AppName `
    --src $zipFile `
    --only-show-errors

if ($LASTEXITCODE -ne 0) {
    Write-Host ""
    Write-Host "   ❌ Deploy falhou!" -ForegroundColor Red
    Write-Host ""
    Write-Host "   💡 Possíveis causas:" -ForegroundColor Yellow
    Write-Host "      • App Service não existe ou foi deletado" -ForegroundColor Gray
    Write-Host "      • Permissões insuficientes" -ForegroundColor Gray
    Write-Host "      • Região do Azure indisponível" -ForegroundColor Gray
    Write-Host ""
    Write-Host "   🔍 Tente verificar no Portal Azure:" -ForegroundColor Yellow
    Write-Host "      https://portal.azure.com" -ForegroundColor Cyan
    Write-Host ""
    exit 1
}

Write-Host "   ✅ Upload concluído" -ForegroundColor Green
Write-Host ""

Write-Host "   Iniciando aplicação..." -ForegroundColor Gray
az webapp start --resource-group $ResourceGroup --name $AppName --only-show-errors 2>$null

Write-Host "   ✅ Aplicação iniciada" -ForegroundColor Green
Write-Host ""

# ==========================================
# 6. AGUARDAR STARTUP
# ==========================================
Write-Host "⏳ Fase 6: Aguardando startup..." -ForegroundColor Yellow
Write-Host ""

$maxAttempts = 30
$attempt = 0
$appUrl = "https://$AppName.azurewebsites.net"

Write-Host "   Testando: $appUrl/health" -ForegroundColor Gray
Write-Host ""

while ($attempt -lt $maxAttempts) {
    $attempt++
    Write-Host "   Tentativa $attempt/$maxAttempts..." -NoNewline -ForegroundColor Gray
    
    try {
        $response = Invoke-WebRequest -Uri "$appUrl/health" -Method Get -TimeoutSec 5 -ErrorAction Stop
        
        if ($response.StatusCode -eq 200) {
            Write-Host " ✅" -ForegroundColor Green
            Write-Host ""
            Write-Host "   ✅ Aplicação está respondendo!" -ForegroundColor Green
            break
        }
    }
    catch {
        Write-Host " ⏳" -ForegroundColor Yellow
    }
    
    Start-Sleep -Seconds 2
}

if ($attempt -eq $maxAttempts) {
    Write-Host ""
    Write-Host "   ⚠️  Aplicação não respondeu no tempo esperado" -ForegroundColor Yellow
    Write-Host "   💡 Isso não significa que falhou, pode só estar demorando mais" -ForegroundColor Cyan
    Write-Host ""
}

Write-Host ""

# ==========================================
# 7. VALIDAÇÃO
# ==========================================
Write-Host "✅ Fase 7: Validação..." -ForegroundColor Yellow
Write-Host ""

Write-Host "   🔍 Testando endpoints..." -ForegroundColor Gray
Write-Host ""

$endpoints = @(
    @{ Url = "$appUrl/health"; Name = "Health Check" },
    @{ Url = "$appUrl/api/status"; Name = "Status API" },
    @{ Url = "$appUrl"; Name = "Aplicação Principal" }
)

foreach ($endpoint in $endpoints) {
    Write-Host "      $($endpoint.Name): " -NoNewline -ForegroundColor Cyan
    
    try {
        $response = Invoke-WebRequest -Uri $endpoint.Url -Method Get -TimeoutSec 10 -ErrorAction Stop
        
        if ($response.StatusCode -eq 200) {
            Write-Host "✅ OK (HTTP $($response.StatusCode))" -ForegroundColor Green
        } else {
            Write-Host "⚠️  HTTP $($response.StatusCode)" -ForegroundColor Yellow
        }
    }
    catch {
        Write-Host "❌ $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""

# ==========================================
# 8. LIMPEZA
# ==========================================
Write-Host "🧹 Fase 8: Limpeza..." -ForegroundColor Yellow
Write-Host ""

if (Test-Path $publishDir) {
    Write-Host "   Removendo diretório temporário..." -ForegroundColor Gray
    Remove-Item -Path $publishDir -Recurse -Force
    Write-Host "   ✅ Removido: $publishDir" -ForegroundColor Green
}

Write-Host "   Mantendo ZIP: $zipFile" -ForegroundColor Gray
Write-Host ""

# ==========================================
# RESUMO
# ==========================================
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "✅ DEPLOY MANUAL CONCLUÍDO!" -ForegroundColor Green
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "📊 Informações do Deploy:" -ForegroundColor Yellow
Write-Host "   • Versão: 2.6.2" -ForegroundColor Gray
Write-Host "   • Tamanho: $zipSizeMB MB" -ForegroundColor Gray
Write-Host "   • Timestamp: $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')" -ForegroundColor Gray
Write-Host "   • ZIP: $zipFile" -ForegroundColor Gray
Write-Host ""

Write-Host "🌐 URLs:" -ForegroundColor Yellow
Write-Host "   • Aplicação: $appUrl" -ForegroundColor Cyan
Write-Host "   • Health: $appUrl/health" -ForegroundColor Cyan
Write-Host "   • Status: $appUrl/api/status" -ForegroundColor Cyan
Write-Host ""

Write-Host "🔍 Ver logs:" -ForegroundColor Yellow
Write-Host "   az webapp log tail --resource-group $ResourceGroup --name $AppName" -ForegroundColor Cyan
Write-Host ""

Write-Host "💡 Próximos passos:" -ForegroundColor Yellow
Write-Host "   1. Acesse: $appUrl" -ForegroundColor Gray
Write-Host "   2. Faça login com: admin / admin123" -ForegroundColor Gray
Write-Host "   3. Teste as funcionalidades principais" -ForegroundColor Gray
Write-Host ""

Write-Host "⚠️  LEMBRETE: Este foi um deploy manual de emergência!" -ForegroundColor Yellow
Write-Host "   Prefira usar GitHub Actions para deploys futuros." -ForegroundColor Yellow
Write-Host ""

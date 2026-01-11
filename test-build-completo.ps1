#!/usr/bin/env pwsh
# Script para testar build completo do projeto
# Sistema Hotelaria v2.6.2

Write-Host "=" -NoNewline -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "🔨 Teste de Build Completo" -ForegroundColor White
Write-Host "  Sistema Hotelaria v2.6.2" -ForegroundColor Yellow
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

$ErrorActionPreference = "Continue"
$BuildSuccess = $true

# ==========================================
# 1. LIMPAR PROJETO
# ==========================================
Write-Host "🧹 Fase 1: Limpeza..." -ForegroundColor Yellow
Write-Host ""

Write-Host "   Removendo bin/..." -ForegroundColor Gray
Remove-Item -Path ".\bin" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "   Removendo obj/..." -ForegroundColor Gray
Remove-Item -Path ".\obj" -Recurse -Force -ErrorAction SilentlyContinue

Write-Host "   Executando dotnet clean..." -ForegroundColor Gray
dotnet clean --verbosity quiet

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✅ Limpeza concluída" -ForegroundColor Green
} else {
    Write-Host "   ⚠️  Limpeza teve avisos (continuando...)" -ForegroundColor Yellow
}

Write-Host ""

# ==========================================
# 2. RESTORE DE DEPENDÊNCIAS
# ==========================================
Write-Host "📦 Fase 2: Restore de Dependências..." -ForegroundColor Yellow
Write-Host ""

dotnet restore --verbosity minimal

if ($LASTEXITCODE -eq 0) {
    Write-Host "   ✅ Restore concluído com sucesso" -ForegroundColor Green
} else {
    Write-Host "   ❌ Restore falhou!" -ForegroundColor Red
    $BuildSuccess = $false
}

Write-Host ""

# ==========================================
# 3. BUILD DEBUG
# ==========================================
if ($BuildSuccess) {
    Write-Host "🔨 Fase 3: Build Debug..." -ForegroundColor Yellow
    Write-Host ""

    dotnet build --configuration Debug --no-restore

    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Build Debug concluído" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Build Debug falhou!" -ForegroundColor Red
        $BuildSuccess = $false
    }

    Write-Host ""
}

# ==========================================
# 4. BUILD RELEASE
# ==========================================
if ($BuildSuccess) {
    Write-Host "🔨 Fase 4: Build Release..." -ForegroundColor Yellow
    Write-Host ""

    dotnet build --configuration Release --no-restore

    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Build Release concluído" -ForegroundColor Green
    } else {
        Write-Host "   ❌ Build Release falhou!" -ForegroundColor Red
        $BuildSuccess = $false
    }

    Write-Host ""
}

# ==========================================
# 5. PUBLISH
# ==========================================
if ($BuildSuccess) {
    Write-Host "📦 Fase 5: Publish..." -ForegroundColor Yellow
    Write-Host ""

    $publishDir = ".\publish"
    Remove-Item -Path $publishDir -Recurse -Force -ErrorAction SilentlyContinue

    dotnet publish -c Release -o $publishDir --no-build

    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Publish concluído" -ForegroundColor Green
        
        # Verificar arquivos essenciais
        $essentialFiles = @(
            "Hotelaria.dll",
            "Hotelaria.pdb",
            "web.config",
            "wwwroot"
        )
        
        Write-Host ""
        Write-Host "   📋 Verificando arquivos essenciais..." -ForegroundColor Cyan
        
        foreach ($file in $essentialFiles) {
            $path = Join-Path $publishDir $file
            if (Test-Path $path) {
                Write-Host "      ✅ $file" -ForegroundColor Green
            } else {
                Write-Host "      ❌ $file (FALTANDO)" -ForegroundColor Red
                $BuildSuccess = $false
            }
        }
        
        # Tamanho do publish
        $publishSize = (Get-ChildItem -Path $publishDir -Recurse | Measure-Object -Property Length -Sum).Sum
        $publishSizeMB = [math]::Round($publishSize / 1MB, 2)
        Write-Host ""
        Write-Host "   📊 Tamanho do publish: $publishSizeMB MB" -ForegroundColor Cyan
        
    } else {
        Write-Host "   ❌ Publish falhou!" -ForegroundColor Red
        $BuildSuccess = $false
    }

    Write-Host ""
}

# ==========================================
# 6. VERIFICAR ERROS
# ==========================================
Write-Host "🔍 Fase 6: Verificação de Erros..." -ForegroundColor Yellow
Write-Host ""

# Verificar se há erros de compilação
$errorLog = ".\obj\project.assets.json"
if (Test-Path $errorLog) {
    Write-Host "   ✅ Arquivo de assets encontrado" -ForegroundColor Green
} else {
    Write-Host "   ⚠️  Arquivo de assets não encontrado" -ForegroundColor Yellow
}

Write-Host ""

# ==========================================
# RESUMO
# ==========================================
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "📊 RESUMO DO BUILD" -ForegroundColor White
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

if ($BuildSuccess) {
    Write-Host "✅ BUILD COMPLETO COM SUCESSO!" -ForegroundColor Green
    Write-Host ""
    Write-Host "📁 Arquivos de publish disponíveis em: .\publish\" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "🚀 Próximos passos:" -ForegroundColor Yellow
    Write-Host "   1. Testar localmente:" -ForegroundColor Gray
    Write-Host "      dotnet run --configuration Release" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "   2. Testar publish:" -ForegroundColor Gray
    Write-Host "      cd publish" -ForegroundColor Cyan
    Write-Host "      dotnet Hotelaria.dll" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "   3. Fazer deploy:" -ForegroundColor Gray
    Write-Host "      git add ." -ForegroundColor Cyan
    Write-Host "      git commit -m 'fix: Complete Azure deployment fixes'" -ForegroundColor Cyan
    Write-Host "      git push origin main" -ForegroundColor Cyan
    Write-Host ""
    
    exit 0
} else {
    Write-Host "❌ BUILD FALHOU!" -ForegroundColor Red
    Write-Host ""
    Write-Host "🔍 Verifique os erros acima e corrija antes de continuar." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "💡 Dicas:" -ForegroundColor Yellow
    Write-Host "   • Verifique se todas as dependências estão instaladas" -ForegroundColor Gray
    Write-Host "   • Verifique se há erros de sintaxe no código" -ForegroundColor Gray
    Write-Host "   • Tente executar: dotnet build --verbosity detailed" -ForegroundColor Gray
    Write-Host ""
    
    exit 1
}

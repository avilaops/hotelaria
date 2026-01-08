# 🧪 Script de Validação Local para CI/CD (Windows)
# Este script simula os checks que o GitHub Actions fará

Write-Host "🚀 Iniciando validação local do projeto Hotelaria..." -ForegroundColor Cyan
Write-Host ""

$ErrorCount = 0

# ============================================
# 1. Verificar .NET SDK
# ============================================
Write-Host "📋 [1/6] Verificando .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = dotnet --version
    Write-Host "✅ .NET SDK encontrado: v$dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "❌ .NET SDK não encontrado!" -ForegroundColor Red
    $ErrorCount++
}
Write-Host ""

# ============================================
# 2. Restore Dependencies
# ============================================
Write-Host "📦 [2/6] Restaurando dependências..." -ForegroundColor Yellow
$restoreResult = dotnet restore
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Dependências restauradas com sucesso" -ForegroundColor Green
} else {
    Write-Host "❌ Falha ao restaurar dependências" -ForegroundColor Red
    $ErrorCount++
}
Write-Host ""

# ============================================
# 3. Build
# ============================================
Write-Host "🔨 [3/6] Compilando projeto..." -ForegroundColor Yellow
$buildResult = dotnet build --no-restore --configuration Release /p:TreatWarningsAsErrors=false
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Build concluído com sucesso" -ForegroundColor Green
} else {
    Write-Host "❌ Falha no build" -ForegroundColor Red
    $ErrorCount++
}
Write-Host ""

# ============================================
# 4. Run Tests
# ============================================
Write-Host "🧪 [4/6] Executando testes..." -ForegroundColor Yellow
$testResult = dotnet test --no-build --configuration Release --verbosity minimal
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Todos os testes passaram" -ForegroundColor Green
} else {
    Write-Host "❌ Alguns testes falharam" -ForegroundColor Red
    $ErrorCount++
}
Write-Host ""

# ============================================
# 5. Verify Publish
# ============================================
Write-Host "📤 [5/6] Testando publicação..." -ForegroundColor Yellow
$publishResult = dotnet publish --no-build --configuration Release --output ./publish-test
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Publicação bem-sucedida" -ForegroundColor Green
    # Cleanup
    Remove-Item -Path ./publish-test -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "❌ Falha na publicação" -ForegroundColor Red
    $ErrorCount++
}
Write-Host ""

# ============================================
# 6. Check Git Status
# ============================================
Write-Host "📂 [6/6] Verificando status do Git..." -ForegroundColor Yellow
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Host "⚠️  Existem arquivos não commitados:" -ForegroundColor Yellow
    git status --short
} else {
    Write-Host "✅ Working tree limpo" -ForegroundColor Green
}
Write-Host ""

# ============================================
# Summary
# ============================================
Write-Host "==================================" -ForegroundColor Cyan
Write-Host "📊 RESUMO DA VALIDAÇÃO" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan

if ($ErrorCount -eq 0) {
    Write-Host "✅ Todas as verificações passaram!" -ForegroundColor Green
    Write-Host "🚀 Você pode fazer push com segurança" -ForegroundColor Green
    exit 0
} else {
    Write-Host "❌ $ErrorCount verificação(ões) falharam" -ForegroundColor Red
    Write-Host "🛑 Corrija os erros antes de fazer push" -ForegroundColor Red
    exit 1
}

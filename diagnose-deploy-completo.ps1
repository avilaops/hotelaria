#!/usr/bin/env pwsh
# Script para diagnosticar problemas de deploy no Azure
# Sistema Hotelaria - Diagnóstico Completo

Write-Host "=" -NoNewline -ForegroundColor Cyan
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "🏨 Sistema Hotelaria - Diagnóstico de Deploy" -ForegroundColor White
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

$appName = "hotelaria-app"
$resourceGroup = "hotelaria-app"
$url = "https://hotelaria-app.azurewebsites.net"

# ============================================
# 1. VERIFICAR CONECTIVIDADE COM A APLICAÇÃO
# ============================================
Write-Host "📡 1. Testando conectividade com a aplicação..." -ForegroundColor Yellow
Write-Host ""

try {
    $response = Invoke-WebRequest -Uri $url -Method Get -TimeoutSec 10 -ErrorAction Stop
    Write-Host "   ✅ Status Code: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   ✅ Aplicação respondendo!" -ForegroundColor Green
}
catch {
    Write-Host "   ❌ Erro ao acessar: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   ⚠️  Status Code: $($_.Exception.Response.StatusCode.value__)" -ForegroundColor Yellow
}

Write-Host ""

# ============================================
# 2. VERIFICAR SE AZURE CLI ESTÁ INSTALADO
# ============================================
Write-Host "🔧 2. Verificando Azure CLI..." -ForegroundColor Yellow
Write-Host ""

$azInstalled = Get-Command az -ErrorAction SilentlyContinue
if ($azInstalled) {
    $azVersion = az version --query '"azure-cli"' -o tsv 2>$null
    Write-Host "   ✅ Azure CLI instalado: $azVersion" -ForegroundColor Green
    
    # Verificar login
    Write-Host "   🔐 Verificando autenticação..." -ForegroundColor Gray
    $account = az account show 2>$null | ConvertFrom-Json -ErrorAction SilentlyContinue
    
    if ($account) {
        Write-Host "   ✅ Autenticado como: $($account.user.name)" -ForegroundColor Green
        Write-Host "   📝 Subscription: $($account.name)" -ForegroundColor Gray
    }
    else {
        Write-Host "   ⚠️  Não autenticado no Azure" -ForegroundColor Yellow
        Write-Host "   💡 Execute: az login" -ForegroundColor Cyan
    }
}
else {
    Write-Host "   ❌ Azure CLI não instalado" -ForegroundColor Red
    Write-Host "   💡 Instale em: https://aka.ms/installazurecliwindows" -ForegroundColor Cyan
}

Write-Host ""

# ============================================
# 3. VERIFICAR LOGS DO AZURE (SE AUTENTICADO)
# ============================================
if ($azInstalled -and $account) {
    Write-Host "📋 3. Buscando logs do Azure Web App..." -ForegroundColor Yellow
    Write-Host ""
    
    try {
        Write-Host "   🔍 Últimas 50 linhas de log..." -ForegroundColor Gray
        Write-Host ""
        
        $logs = az webapp log tail --name $appName --resource-group $resourceGroup --only-show-errors 2>&1
        
        if ($logs) {
            $logs | Select-Object -Last 50 | ForEach-Object {
                if ($_ -match "error|exception|fail") {
                    Write-Host "   ❌ $_" -ForegroundColor Red
                }
                elseif ($_ -match "warning|warn") {
                    Write-Host "   ⚠️  $_" -ForegroundColor Yellow
                }
                else {
                    Write-Host "   $_" -ForegroundColor Gray
                }
            }
        }
    }
    catch {
        Write-Host "   ⚠️  Não foi possível obter logs: $($_.Exception.Message)" -ForegroundColor Yellow
    }
    
    Write-Host ""
}

# ============================================
# 4. VERIFICAR STATUS DO GITHUB ACTIONS
# ============================================
Write-Host "🔄 4. Verificando GitHub Actions..." -ForegroundColor Yellow
Write-Host ""

$ghInstalled = Get-Command gh -ErrorAction SilentlyContinue
if ($ghInstalled) {
    try {
        Write-Host "   🔍 Últimas execuções do workflow..." -ForegroundColor Gray
        $workflows = gh run list --limit 5 --json status,conclusion,name,createdAt,url 2>$null | ConvertFrom-Json
        
        if ($workflows) {
            foreach ($run in $workflows) {
                $status = $run.status
                $conclusion = $run.conclusion
                $name = $run.name
                $date = ([DateTime]$run.createdAt).ToString("dd/MM/yyyy HH:mm")
                
                $statusIcon = switch ($conclusion) {
                    "success" { "✅" }
                    "failure" { "❌" }
                    "cancelled" { "⚠️" }
                    default { "⏳" }
                }
                
                $color = switch ($conclusion) {
                    "success" { "Green" }
                    "failure" { "Red" }
                    "cancelled" { "Yellow" }
                    default { "Gray" }
                }
                
                Write-Host "   $statusIcon " -NoNewline -ForegroundColor $color
                Write-Host "$name - $date" -ForegroundColor $color
                Write-Host "      📎 $($run.url)" -ForegroundColor Gray
            }
        }
    }
    catch {
        Write-Host "   ⚠️  Erro ao buscar workflows: $($_.Exception.Message)" -ForegroundColor Yellow
    }
}
else {
    Write-Host "   ⚠️  GitHub CLI não instalado" -ForegroundColor Yellow
    Write-Host "   💡 Instale em: https://cli.github.com/" -ForegroundColor Cyan
    Write-Host "   📱 Ou verifique em: https://github.com/avilaops/hotelaria/actions" -ForegroundColor Cyan
}

Write-Host ""

# ============================================
# 5. VERIFICAR ARQUIVOS LOCAIS
# ============================================
Write-Host "📁 5. Verificando arquivos locais..." -ForegroundColor Yellow
Write-Host ""

$criticalFiles = @(
    "Hotelaria.csproj",
    "Program.cs",
    "App.razor",
    "Pages/Login.razor",
    "Dockerfile",
    ".env"
)

foreach ($file in $criticalFiles) {
    if (Test-Path $file) {
        Write-Host "   ✅ $file" -ForegroundColor Green
    }
    else {
        Write-Host "   ❌ $file (FALTANDO)" -ForegroundColor Red
    }
}

Write-Host ""

# ============================================
# 6. VERIFICAR VARIÁVEIS DE AMBIENTE
# ============================================
Write-Host "🔐 6. Verificando variáveis de ambiente (.env)..." -ForegroundColor Yellow
Write-Host ""

if (Test-Path ".env") {
    $envContent = Get-Content .env
    $requiredVars = @(
        "MONGO_CONNECTION_STRING",
        "AIRBNB_API_KEY",
        "PAYPAL_CLIENT_ID",
        "SENTRY_DSN"
    )
    
    foreach ($var in $requiredVars) {
        $found = $envContent | Where-Object { $_ -match "^$var=" }
        if ($found) {
            $value = ($found -split "=", 2)[1]
            if ($value -and $value -ne "your_*" -and $value.Length -gt 5) {
                Write-Host "   ✅ $var" -ForegroundColor Green
            }
            else {
                Write-Host "   ⚠️  $var (vazio ou placeholder)" -ForegroundColor Yellow
            }
        }
        else {
            Write-Host "   ❌ $var (não encontrado)" -ForegroundColor Red
        }
    }
}
else {
    Write-Host "   ❌ Arquivo .env não encontrado" -ForegroundColor Red
}

Write-Host ""

# ============================================
# 7. VERIFICAR BUILD LOCAL
# ============================================
Write-Host "🔨 7. Testando build local..." -ForegroundColor Yellow
Write-Host ""

try {
    Write-Host "   🔍 Executando dotnet build..." -ForegroundColor Gray
    $buildResult = dotnet build --configuration Release 2>&1
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ Build local bem-sucedido!" -ForegroundColor Green
    }
    else {
        Write-Host "   ❌ Build local falhou!" -ForegroundColor Red
        Write-Host ""
        Write-Host "   Erros:" -ForegroundColor Yellow
        $buildResult | Where-Object { $_ -match "error" } | ForEach-Object {
            Write-Host "   $_" -ForegroundColor Red
        }
    }
}
catch {
    Write-Host "   ❌ Erro ao executar build: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""

# ============================================
# 8. RESUMO E RECOMENDAÇÕES
# ============================================
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "📊 RESUMO DO DIAGNÓSTICO" -ForegroundColor White
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "🔗 Links Úteis:" -ForegroundColor Yellow
Write-Host "   • Azure Portal: https://portal.azure.com" -ForegroundColor Cyan
Write-Host "   • App no Azure: https://portal.azure.com/#@/resource/subscriptions/.../resourceGroups/$resourceGroup/providers/Microsoft.Web/sites/$appName" -ForegroundColor Cyan
Write-Host "   • GitHub Actions: https://github.com/avilaops/hotelaria/actions" -ForegroundColor Cyan
Write-Host "   • Aplicação: $url" -ForegroundColor Cyan
Write-Host ""

Write-Host "💡 Próximos Passos Sugeridos:" -ForegroundColor Yellow
Write-Host ""
Write-Host "   1️⃣  Verificar logs no Azure Portal:" -ForegroundColor White
Write-Host "      - Acesse o portal: https://portal.azure.com" -ForegroundColor Gray
Write-Host "      - Navegue até App Service > $appName" -ForegroundColor Gray
Write-Host "      - Vá em 'Log stream' ou 'Diagnose and solve problems'" -ForegroundColor Gray
Write-Host ""
Write-Host "   2️⃣  Verificar GitHub Actions:" -ForegroundColor White
Write-Host "      - Acesse: https://github.com/avilaops/hotelaria/actions" -ForegroundColor Gray
Write-Host "      - Verifique o último workflow executado" -ForegroundColor Gray
Write-Host "      - Veja os logs detalhados de cada step" -ForegroundColor Gray
Write-Host ""
Write-Host "   3️⃣  Se o deploy falhou:" -ForegroundColor White
Write-Host "      - Commite e push uma pequena alteração" -ForegroundColor Gray
Write-Host "      - Isso vai triggerar um novo deploy" -ForegroundColor Gray
Write-Host "      - Monitore o processo no GitHub Actions" -ForegroundColor Gray
Write-Host ""
Write-Host "   4️⃣  Se estiver com HTTP 503:" -ForegroundColor White
Write-Host "      - Verifique se o App Service está rodando (não stopped)" -ForegroundColor Gray
Write-Host "      - Verifique configurações de HTTPS only" -ForegroundColor Gray
Write-Host "      - Verifique se há erros de startup no Log stream" -ForegroundColor Gray
Write-Host ""

Write-Host "======================================================" -ForegroundColor Cyan
Write-Host "✅ Diagnóstico completo!" -ForegroundColor Green
Write-Host "======================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "💬 Se precisar de mais ajuda, compartilhe:" -ForegroundColor Yellow
Write-Host "   • Screenshots dos logs do Azure Portal" -ForegroundColor Gray
Write-Host "   • URL da execução do GitHub Actions" -ForegroundColor Gray
Write-Host "   • Mensagem de erro específica que está vendo" -ForegroundColor Gray
Write-Host ""

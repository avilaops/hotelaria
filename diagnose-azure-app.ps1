# 🔍 Azure App Diagnostics - Complete Check
# Diagnóstico completo da aplicação no Azure

param(
    [string]$AppName = "hotelaria-app",
    [string]$ResourceGroup = "hotelaria-rg"
)

Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║  AZURE APP DIAGNOSTICS                ║" -ForegroundColor Cyan
Write-Host "║  hotelaria-app                        ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# 1. Check if app exists
Write-Host "1️⃣  Verificando se app existe..." -ForegroundColor Yellow
try {
    $app = az webapp show --name $AppName --resource-group $ResourceGroup 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "   ✅ App encontrado!" -ForegroundColor Green
    } else {
        Write-Host "   ❌ App não encontrado!" -ForegroundColor Red
        exit 1
    }
} catch {
    Write-Host "   ❌ Erro ao verificar app: $_" -ForegroundColor Red
    exit 1
}
Write-Host ""

# 2. Check app state
Write-Host "2️⃣  Verificando estado da aplicação..." -ForegroundColor Yellow
$state = az webapp show --name $AppName --resource-group $ResourceGroup --query state -o tsv
Write-Host "   Estado: $state" -ForegroundColor $(if ($state -eq "Running") { "Green" } else { "Red" })
Write-Host ""

# 3. Check authentication settings
Write-Host "3️⃣  Verificando autenticação..." -ForegroundColor Yellow
try {
    $authEnabled = az webapp auth show --name $AppName --resource-group $ResourceGroup --query enabled -o tsv 2>$null
    if ($authEnabled -eq "true") {
        Write-Host "   ⚠️  Azure AD Auth HABILITADO (pode causar problemas!)" -ForegroundColor Red
        Write-Host "   Desabilitando..." -ForegroundColor Yellow
        az webapp auth update --name $AppName --resource-group $ResourceGroup --enabled false --action AllowAnonymous | Out-Null
        Write-Host "   ✅ Autenticação desabilitada!" -ForegroundColor Green
    } else {
        Write-Host "   ✅ Azure AD Auth desabilitado" -ForegroundColor Green
    }
} catch {
    Write-Host "   ⚠️  Não foi possível verificar autenticação" -ForegroundColor Yellow
}
Write-Host ""

# 4. Check environment variables
Write-Host "4️⃣  Verificando variáveis de ambiente..." -ForegroundColor Yellow
$appSettings = az webapp config appsettings list --name $AppName --resource-group $ResourceGroup -o json | ConvertFrom-Json

$requiredVars = @(
    "ASPNETCORE_ENVIRONMENT",
    "WEBSITE_RUN_FROM_PACKAGE",
    "SCM_DO_BUILD_DURING_DEPLOYMENT"
)

$missingVars = @()
foreach ($var in $requiredVars) {
    $exists = $appSettings | Where-Object { $_.name -eq $var }
    if ($exists) {
        Write-Host "   ✅ $var = $($exists.value)" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  $var - FALTANDO!" -ForegroundColor Red
        $missingVars += $var
    }
}
Write-Host ""

# 5. Get recent logs
Write-Host "5️⃣  Buscando logs recentes..." -ForegroundColor Yellow
Write-Host "   (Últimas 50 linhas)" -ForegroundColor Gray
Write-Host "   ----------------------------------------" -ForegroundColor Gray

try {
    $logs = az webapp log tail --name $AppName --resource-group $ResourceGroup --timeout 10 2>&1
    if ($logs) {
        $logs | Select-Object -Last 50 | ForEach-Object {
            if ($_ -match "error|fail|exception") {
                Write-Host "   🔴 $_" -ForegroundColor Red
            } elseif ($_ -match "warn") {
                Write-Host "   🟡 $_" -ForegroundColor Yellow
            } else {
                Write-Host "   $_" -ForegroundColor White
            }
        }
    }
} catch {
    Write-Host "   ⚠️  Não foi possível buscar logs" -ForegroundColor Yellow
}
Write-Host ""

# 6. Check deployment status
Write-Host "6️⃣  Verificando último deploy..." -ForegroundColor Yellow
try {
    $deployments = az webapp deployment list --name $AppName --resource-group $ResourceGroup -o json | ConvertFrom-Json
    if ($deployments) {
        $lastDeploy = $deployments | Select-Object -First 1
        Write-Host "   Data: $($lastDeploy.received_time)" -ForegroundColor White
        Write-Host "   Status: $($lastDeploy.status)" -ForegroundColor $(if ($lastDeploy.status -eq "Success") { "Green" } else { "Red" })
        Write-Host "   Autor: $($lastDeploy.author)" -ForegroundColor White
    } else {
        Write-Host "   ⚠️  Nenhum deployment encontrado" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   ⚠️  Não foi possível verificar deployments" -ForegroundColor Yellow
}
Write-Host ""

# 7. Check runtime stack
Write-Host "7️⃣  Verificando runtime..." -ForegroundColor Yellow
$runtime = az webapp config show --name $AppName --resource-group $ResourceGroup --query linuxFxVersion -o tsv
Write-Host "   Runtime: $runtime" -ForegroundColor White
Write-Host ""

# 8. Restart app
Write-Host "8️⃣  Reiniciando aplicação..." -ForegroundColor Yellow
az webapp restart --name $AppName --resource-group $ResourceGroup | Out-Null
Write-Host "   ✅ App reiniciado!" -ForegroundColor Green
Write-Host ""

# 9. Summary
Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║  RESUMO DO DIAGNÓSTICO                ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "🌐 URL: https://$AppName.azurewebsites.net" -ForegroundColor Cyan
Write-Host "🌐 URL: https://hotelaria.avila.inc" -ForegroundColor Cyan
Write-Host ""

if ($missingVars.Count -gt 0) {
    Write-Host "⚠️  ATENÇÃO: Variáveis faltando:" -ForegroundColor Red
    $missingVars | ForEach-Object { Write-Host "   - $_" -ForegroundColor Red }
    Write-Host ""
    Write-Host "📝 Para adicionar variáveis:" -ForegroundColor Yellow
    Write-Host "   az webapp config appsettings set --name $AppName --resource-group $ResourceGroup --settings VAR=VALUE" -ForegroundColor Gray
    Write-Host ""
}

Write-Host "📊 Próximos passos:" -ForegroundColor Yellow
Write-Host "   1. Aguardar 30 segundos" -ForegroundColor White
Write-Host "   2. Acessar: https://hotelaria.avila.inc" -ForegroundColor White
Write-Host "   3. Se persistir erro, executar:" -ForegroundColor White
Write-Host "      .\fix-azure-complete.ps1" -ForegroundColor Cyan
Write-Host ""
Write-Host "✅ Diagnóstico concluído!" -ForegroundColor Green

# 🔍 Diagnóstico de HTTP 503 - Azure App Service
# Análise completa de falhas de disponibilidade

param(
    [string]$AppName = "hotelaria-app",
    [string]$ResourceGroup = "hotelaria-rg"
)

Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Red
Write-Host "║  DIAGNÓSTICO HTTP 503                 ║" -ForegroundColor Red
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Red
Write-Host ""

# Verificar se está logado
Write-Host "🔐 Verificando autenticação Azure..." -ForegroundColor Yellow
try {
    $account = az account show 2>&1 | ConvertFrom-Json
    Write-Host "   ✅ Logado como: $($account.user.name)" -ForegroundColor Green
} catch {
    Write-Host "   ❌ Não autenticado!" -ForegroundColor Red
    Write-Host "   Execute: az login" -ForegroundColor Yellow
    exit 1
}
Write-Host ""

# 1. Estado do App
Write-Host "1️⃣  Estado do App Service..." -ForegroundColor Yellow
$appState = az webapp show --name $AppName --resource-group $ResourceGroup --query "{state:state,availabilityState:availabilityState,enabled:enabled}" -o json | ConvertFrom-Json
Write-Host "   Estado: $($appState.state)" -ForegroundColor $(if ($appState.state -eq "Running") { "Green" } else { "Red" })
Write-Host "   Disponibilidade: $($appState.availabilityState)" -ForegroundColor $(if ($appState.availabilityState -eq "Normal") { "Green" } else { "Red" })
Write-Host "   Habilitado: $($appState.enabled)" -ForegroundColor $(if ($appState.enabled -eq $true) { "Green" } else { "Red" })
Write-Host ""

# 2. Uso de Recursos
Write-Host "2️⃣  Métricas de Recursos (última hora)..." -ForegroundColor Yellow
$endTime = (Get-Date).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
$startTime = (Get-Date).AddHours(-1).ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")

# CPU
Write-Host "   📊 CPU Usage:" -ForegroundColor Cyan
$cpuMetrics = az monitor metrics list `
    --resource "/subscriptions/3b49f371-dd88-46c7-ba30-aeb54bd5c2f6/resourceGroups/$ResourceGroup/providers/Microsoft.Web/sites/$AppName" `
    --metric "CpuPercentage" `
    --start-time $startTime `
    --end-time $endTime `
    --interval PT1M `
    --aggregation Average `
    -o json 2>$null | ConvertFrom-Json

if ($cpuMetrics -and $cpuMetrics.value) {
    $latestCpu = $cpuMetrics.value[0].timeseries[0].data | Select-Object -Last 1
    if ($latestCpu.average) {
        $cpuValue = [math]::Round($latestCpu.average, 2)
        Write-Host "      Atual: $cpuValue%" -ForegroundColor $(if ($cpuValue -gt 80) { "Red" } elseif ($cpuValue -gt 60) { "Yellow" } else { "Green" })
    }
}

# Memory
Write-Host "   💾 Memory Usage:" -ForegroundColor Cyan
$memMetrics = az monitor metrics list `
    --resource "/subscriptions/3b49f371-dd88-46c7-ba30-aeb54bd5c2f6/resourceGroups/$ResourceGroup/providers/Microsoft.Web/sites/$AppName" `
    --metric "MemoryPercentage" `
    --start-time $startTime `
    --end-time $endTime `
    --interval PT1M `
    --aggregation Average `
    -o json 2>$null | ConvertFrom-Json

if ($memMetrics -and $memMetrics.value) {
    $latestMem = $memMetrics.value[0].timeseries[0].data | Select-Object -Last 1
    if ($latestMem.average) {
        $memValue = [math]::Round($latestMem.average, 2)
        Write-Host "      Atual: $memValue%" -ForegroundColor $(if ($memValue -gt 80) { "Red" } elseif ($memValue -gt 60) { "Yellow" } else { "Green" })
    }
}

# Requests
Write-Host "   📥 HTTP Requests (últimos 5 min):" -ForegroundColor Cyan
$reqMetrics = az monitor metrics list `
    --resource "/subscriptions/3b49f371-dd88-46c7-ba30-aeb54bd5c2f6/resourceGroups/$ResourceGroup/providers/Microsoft.Web/sites/$AppName" `
    --metric "Requests" `
    --start-time $startTime `
    --end-time $endTime `
    --interval PT5M `
    --aggregation Total `
    -o json 2>$null | ConvertFrom-Json

if ($reqMetrics -and $reqMetrics.value) {
    $latestReq = $reqMetrics.value[0].timeseries[0].data | Select-Object -Last 1
    if ($latestReq.total) {
        Write-Host "      Total: $($latestReq.total)" -ForegroundColor White
    }
}

# HTTP 5xx
Write-Host "   ❌ HTTP 5xx Errors:" -ForegroundColor Cyan
$http5xxMetrics = az monitor metrics list `
    --resource "/subscriptions/3b49f371-dd88-46c7-ba30-aeb54bd5c2f6/resourceGroups/$ResourceGroup/providers/Microsoft.Web/sites/$AppName" `
    --metric "Http5xx" `
    --start-time $startTime `
    --end-time $endTime `
    --interval PT5M `
    --aggregation Total `
    -o json 2>$null | ConvertFrom-Json

if ($http5xxMetrics -and $http5xxMetrics.value) {
    $latest5xx = $http5xxMetrics.value[0].timeseries[0].data | Select-Object -Last 1
    if ($latest5xx.total) {
        Write-Host "      Total: $($latest5xx.total)" -ForegroundColor $(if ($latest5xx.total -gt 0) { "Red" } else { "Green" })
    }
}
Write-Host ""

# 3. Logs Recentes
Write-Host "3️⃣  Últimos Logs (últimas 50 linhas)..." -ForegroundColor Yellow
Write-Host "   ----------------------------------------" -ForegroundColor Gray
try {
    $logs = az webapp log tail --name $AppName --resource-group $ResourceGroup --timeout 10 2>&1
    if ($logs) {
        $logs | Select-Object -Last 50 | ForEach-Object {
            $line = $_
            if ($line -match "error|fail|exception|503") {
                Write-Host "   🔴 $line" -ForegroundColor Red
            } elseif ($line -match "warn|timeout") {
                Write-Host "   🟡 $line" -ForegroundColor Yellow
            } elseif ($line -match "starting|started|listening") {
                Write-Host "   🟢 $line" -ForegroundColor Green
            } else {
                Write-Host "   $line" -ForegroundColor White
            }
        }
    }
} catch {
    Write-Host "   ⚠️  Não foi possível buscar logs" -ForegroundColor Yellow
}
Write-Host ""

# 4. Teste de Conectividade
Write-Host "4️⃣  Testando conectividade..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://$AppName.azurewebsites.net" -Method GET -TimeoutSec 10
    Write-Host "   ✅ Status Code: $($response.StatusCode)" -ForegroundColor Green
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    Write-Host "   ❌ Status Code: $statusCode" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# 5. Configurações do App
Write-Host "5️⃣  Verificando configurações críticas..." -ForegroundColor Yellow
$config = az webapp config show --name $AppName --resource-group $ResourceGroup -o json | ConvertFrom-Json
Write-Host "   Always On: $($config.alwaysOn)" -ForegroundColor $(if ($config.alwaysOn) { "Green" } else { "Red" })
Write-Host "   HTTP 2.0: $($config.http20Enabled)" -ForegroundColor $(if ($config.http20Enabled) { "Green" } else { "Yellow" })
Write-Host "   Minimum Instances: $($config.minimumElasticInstanceCount)" -ForegroundColor White
Write-Host ""

# Resumo
Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║  DIAGNÓSTICO COMPLETO                 ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

Write-Host "📊 Próximas ações recomendadas:" -ForegroundColor Yellow
Write-Host "   1. Se CPU/Memory > 80%: Considerar upgrade do plano" -ForegroundColor White
Write-Host "   2. Se HTTP 5xx persistir: Verificar logs de aplicação" -ForegroundColor White
Write-Host "   3. Se app não responde: Restart manual" -ForegroundColor White
Write-Host "   4. Verificar Application Insights para detalhes" -ForegroundColor White
Write-Host ""

Write-Host "🌐 URLs para verificação manual:" -ForegroundColor Yellow
Write-Host "   App: https://$AppName.azurewebsites.net" -ForegroundColor Cyan
Write-Host "   Kudu: https://$AppName.scm.azurewebsites.net" -ForegroundColor Cyan
Write-Host "   Portal: https://portal.azure.com" -ForegroundColor Cyan
Write-Host ""

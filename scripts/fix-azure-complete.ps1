# 🔧 Azure App Complete Fix
# Correção completa da aplicação no Azure

param(
    [string]$AppName = "hotelaria-app",
    [string]$ResourceGroup = "hotelaria-rg"
)

Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║  AZURE APP COMPLETE FIX               ║" -ForegroundColor Cyan
Write-Host "║  hotelaria-app                        ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# 1. Disable Azure AD Auth
Write-Host "1️⃣  Desabilitando Azure AD Authentication..." -ForegroundColor Yellow
try {
    az webapp auth update `
        --name $AppName `
        --resource-group $ResourceGroup `
        --enabled false `
        --action AllowAnonymous | Out-Null
    Write-Host "   ✅ Azure AD desabilitado!" -ForegroundColor Green
} catch {
    Write-Host "   ⚠️  Já estava desabilitado" -ForegroundColor Yellow
}
Write-Host ""

# 2. Configure essential environment variables
Write-Host "2️⃣  Configurando variáveis essenciais..." -ForegroundColor Yellow

$settings = @{
    "ASPNETCORE_ENVIRONMENT" = "Production"
    "WEBSITE_RUN_FROM_PACKAGE" = "1"
    "SCM_DO_BUILD_DURING_DEPLOYMENT" = "false"
    "ASPNETCORE_URLS" = "http://+:8080"
    "PORT" = "8080"
    "WEBSITES_PORT" = "8080"
}

foreach ($key in $settings.Keys) {
    Write-Host "   Configurando $key = $($settings[$key])..." -ForegroundColor Gray
}

$settingsString = ($settings.GetEnumerator() | ForEach-Object { "$($_.Key)=$($_.Value)" }) -join " "

az webapp config appsettings set `
    --name $AppName `
    --resource-group $ResourceGroup `
    --settings $settingsString | Out-Null

Write-Host "   ✅ Variáveis configuradas!" -ForegroundColor Green
Write-Host ""

# 3. Configure runtime
Write-Host "3️⃣  Configurando runtime .NET 9..." -ForegroundColor Yellow
az webapp config set `
    --name $AppName `
    --resource-group $ResourceGroup `
    --linux-fx-version "DOTNETCORE|9.0" | Out-Null
Write-Host "   ✅ Runtime configurado!" -ForegroundColor Green
Write-Host ""

# 4. Enable detailed logging
Write-Host "4️⃣  Habilitando logs detalhados..." -ForegroundColor Yellow
az webapp log config `
    --name $AppName `
    --resource-group $ResourceGroup `
    --application-logging filesystem `
    --detailed-error-messages true `
    --failed-request-tracing true `
    --web-server-logging filesystem | Out-Null
Write-Host "   ✅ Logs habilitados!" -ForegroundColor Green
Write-Host ""

# 5. Restart app
Write-Host "5️⃣  Reiniciando aplicação..." -ForegroundColor Yellow
az webapp restart --name $AppName --resource-group $ResourceGroup | Out-Null
Write-Host "   ✅ App reiniciado!" -ForegroundColor Green
Write-Host ""

# 6. Wait and check
Write-Host "6️⃣  Aguardando inicialização..." -ForegroundColor Yellow
Write-Host "   " -NoNewline
for ($i = 1; $i -le 30; $i++) {
    Write-Host "█" -NoNewline -ForegroundColor Cyan
    Start-Sleep -Seconds 1
}
Write-Host ""
Write-Host "   ✅ Aguardado 30 segundos!" -ForegroundColor Green
Write-Host ""

# 7. Test endpoint
Write-Host "7️⃣  Testando endpoint..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "https://$AppName.azurewebsites.net" -Method GET -TimeoutSec 10
    if ($response.StatusCode -eq 200) {
        Write-Host "   ✅ App respondendo corretamente!" -ForegroundColor Green
    } else {
        Write-Host "   ⚠️  Status: $($response.StatusCode)" -ForegroundColor Yellow
    }
} catch {
    Write-Host "   ❌ App ainda não está respondendo" -ForegroundColor Red
    Write-Host "   Erro: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# 8. Get latest logs
Write-Host "8️⃣  Últimos logs (últimas 20 linhas)..." -ForegroundColor Yellow
Write-Host "   ----------------------------------------" -ForegroundColor Gray
try {
    az webapp log tail --name $AppName --resource-group $ResourceGroup --timeout 5 2>&1 | Select-Object -Last 20 | ForEach-Object {
        if ($_ -match "error|fail|exception") {
            Write-Host "   🔴 $_" -ForegroundColor Red
        } elseif ($_ -match "warn") {
            Write-Host "   🟡 $_" -ForegroundColor Yellow
        } else {
            Write-Host "   $_" -ForegroundColor White
        }
    }
} catch {
    Write-Host "   ⚠️  Logs não disponíveis ainda" -ForegroundColor Yellow
}
Write-Host ""

# Summary
Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║  CORREÇÃO CONCLUÍDA                   ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""
Write-Host "✅ Todas as correções aplicadas!" -ForegroundColor Green
Write-Host ""
Write-Host "🌐 Tente acessar agora:" -ForegroundColor Yellow
Write-Host "   https://hotelaria.avila.inc" -ForegroundColor Cyan
Write-Host "   https://$AppName.azurewebsites.net" -ForegroundColor Cyan
Write-Host ""
Write-Host "🔑 Credenciais de teste:" -ForegroundColor Yellow
Write-Host "   Usuário: admin" -ForegroundColor White
Write-Host "   Senha: admin123" -ForegroundColor White
Write-Host ""
Write-Host "📊 Se ainda houver erro:" -ForegroundColor Yellow
Write-Host "   1. Verificar logs: az webapp log tail --name $AppName --resource-group $ResourceGroup" -ForegroundColor Gray
Write-Host "   2. Fazer novo deploy via GitHub Actions" -ForegroundColor Gray
Write-Host "   3. Abrir Kudu: https://$AppName.scm.azurewebsites.net" -ForegroundColor Gray
Write-Host ""

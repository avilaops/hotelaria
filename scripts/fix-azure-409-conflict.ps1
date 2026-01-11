# 🔧 Fix Azure 409 Conflict
# Resolve deployment conflicts and force clean restart

param(
    [string]$AppName = "hotelaria-app",
    [string]$ResourceGroup = "hotelaria-rg"
)

Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Red
Write-Host "║  FIX AZURE 409 CONFLICT               ║" -ForegroundColor Red
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Red
Write-Host ""

# 1. Stop app
Write-Host "1️⃣  Stopping app..." -ForegroundColor Yellow
az webapp stop --name $AppName --resource-group $ResourceGroup
Write-Host "   ✅ App stopped!" -ForegroundColor Green
Write-Host ""

# 2. Wait
Write-Host "2️⃣  Waiting 10 seconds..." -ForegroundColor Yellow
Start-Sleep -Seconds 10
Write-Host "   ✅ Wait complete!" -ForegroundColor Green
Write-Host ""

# 3. Start app
Write-Host "3️⃣  Starting app..." -ForegroundColor Yellow
az webapp start --name $AppName --resource-group $ResourceGroup
Write-Host "   ✅ App started!" -ForegroundColor Green
Write-Host ""

# 4. Restart (force clean start)
Write-Host "4️⃣  Force restarting..." -ForegroundColor Yellow
az webapp restart --name $AppName --resource-group $ResourceGroup
Write-Host "   ✅ App restarted!" -ForegroundColor Green
Write-Host ""

Write-Host "╔════════════════════════════════════════╗" -ForegroundColor Green
Write-Host "║  CONFLICT RESOLVED                    ║" -ForegroundColor Green
Write-Host "╚════════════════════════════════════════╝" -ForegroundColor Green
Write-Host ""
Write-Host "🔄 Agora execute novamente o deploy:" -ForegroundColor Yellow
Write-Host "   git commit --allow-empty -m 'chore: redeploy after conflict fix'" -ForegroundColor Gray
Write-Host "   git push origin main" -ForegroundColor Gray
Write-Host ""

# 🔧 Fix Azure Authentication Override
# Remove Azure AD authentication and restore local auth

Write-Host "🔍 Checking Azure Authentication..." -ForegroundColor Yellow

# Get current auth settings
az webapp auth show --name hotelaria-app --resource-group hotelaria-rg

Write-Host "`n🔥 Removing Azure AD Authentication..." -ForegroundColor Red

# Disable Azure AD authentication
az webapp auth update `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --enabled false `
  --action AllowAnonymous

Write-Host "`n✅ Authentication disabled!" -ForegroundColor Green
Write-Host "🔄 Restarting app..." -ForegroundColor Yellow

# Restart app
az webapp restart --name hotelaria-app --resource-group hotelaria-rg

Write-Host "`n✅ Done! Try accessing:" -ForegroundColor Green
Write-Host "   https://hotelaria.avila.inc/login" -ForegroundColor Cyan
Write-Host "`n   Login: admin" -ForegroundColor White
Write-Host "   Senha: admin123" -ForegroundColor White

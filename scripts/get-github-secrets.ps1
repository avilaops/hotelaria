# 🔐 Script para Obter Secrets do Azure para GitHub Environment
# Data: 09/01/2026
# Uso: .\get-github-secrets.ps1

Write-Host "============================================" -ForegroundColor Cyan
Write-Host "OBTER SECRETS DO AZURE PARA GITHUB" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

# Verificar se está logado no Azure
Write-Host "🔍 Verificando login no Azure..." -ForegroundColor Yellow
$account = az account show 2>$null | ConvertFrom-Json

if (-not $account) {
    Write-Host "❌ Você não está logado no Azure!" -ForegroundColor Red
    Write-Host "Execute: az login" -ForegroundColor Yellow
    exit 1
}

Write-Host "✅ Logado como: $($account.user.name)" -ForegroundColor Green
Write-Host ""

# 1. Obter Subscription ID
Write-Host "📋 Obtendo Subscription ID..." -ForegroundColor Yellow
$subscriptionId = az account show --query id -o tsv

if (-not $subscriptionId) {
    Write-Host "❌ Erro ao obter Subscription ID" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Subscription ID obtido" -ForegroundColor Green
Write-Host ""

# 2. Obter Tenant ID
Write-Host "📋 Obtendo Tenant ID..." -ForegroundColor Yellow
$tenantId = az account show --query tenantId -o tsv

if (-not $tenantId) {
    Write-Host "❌ Erro ao obter Tenant ID" -ForegroundColor Red
    exit 1
}

Write-Host "✅ Tenant ID obtido" -ForegroundColor Green
Write-Host ""

# 3. Obter Client ID
Write-Host "📋 Obtendo Client ID do Service Principal..." -ForegroundColor Yellow
$clientId = az ad sp list --display-name hotelaria-app --query "[0].appId" -o tsv

if (-not $clientId) {
    Write-Host "⚠️  Service Principal 'hotelaria-app' não encontrado" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Tentando encontrar por outro nome..." -ForegroundColor Yellow
    
    # Listar todos os Service Principals do usuário
    $allSPs = az ad sp list --show-mine --query "[].{name:displayName, id:appId}" -o json | ConvertFrom-Json
    
    if ($allSPs.Count -gt 0) {
        Write-Host ""
        Write-Host "Service Principals encontrados:" -ForegroundColor Cyan
        foreach ($sp in $allSPs) {
            Write-Host "  - $($sp.name) (ID: $($sp.id))" -ForegroundColor White
        }
        Write-Host ""
        Write-Host "Digite o nome exato do Service Principal que deseja usar:" -ForegroundColor Yellow
        $spName = Read-Host
        
        $clientId = az ad sp list --display-name $spName --query "[0].appId" -o tsv
    }
    
    if (-not $clientId) {
        Write-Host ""
        Write-Host "❌ Nenhum Service Principal encontrado!" -ForegroundColor Red
        Write-Host ""
        Write-Host "Deseja criar um novo Service Principal? (S/N)" -ForegroundColor Yellow
        $create = Read-Host
        
        if ($create -eq "S" -or $create -eq "s") {
            Write-Host ""
            Write-Host "📝 Criando novo Service Principal..." -ForegroundColor Yellow
            
            $sp = az ad sp create-for-rbac `
                --name "hotelaria-app-sp" `
                --role contributor `
                --scopes "/subscriptions/$subscriptionId/resourceGroups/hotelaria-rg" `
                --sdk-auth | ConvertFrom-Json
            
            if ($sp) {
                $clientId = $sp.clientId
                Write-Host "✅ Service Principal criado com sucesso!" -ForegroundColor Green
                Write-Host ""
                Write-Host "⚠️  IMPORTANTE: Anote o Client Secret abaixo (não será exibido novamente):" -ForegroundColor Red
                Write-Host "   Client Secret: $($sp.clientSecret)" -ForegroundColor White
                Write-Host ""
            } else {
                Write-Host "❌ Erro ao criar Service Principal" -ForegroundColor Red
                exit 1
            }
        } else {
            exit 1
        }
    }
}

Write-Host "✅ Client ID obtido" -ForegroundColor Green
Write-Host ""

# Exibir resumo
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "✅ TODOS OS SECRETS OBTIDOS COM SUCESSO!" -ForegroundColor Green
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""

Write-Host "📋 COPIE E COLE NO GITHUB ENVIRONMENT:" -ForegroundColor Yellow
Write-Host "   (Settings → Environments → Production → Add environment secret)" -ForegroundColor Gray
Write-Host ""

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray

Write-Host ""
Write-Host "1️⃣  SECRET #1" -ForegroundColor Yellow
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host "Name:" -ForegroundColor Cyan
Write-Host "AZUREAPPSERVICE_CLIENTID_856727333C674C08A008A6D80815BE73" -ForegroundColor White
Write-Host ""
Write-Host "Secret:" -ForegroundColor Cyan
Write-Host $clientId -ForegroundColor White
Write-Host ""

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray

Write-Host ""
Write-Host "2️⃣  SECRET #2" -ForegroundColor Yellow
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host "Name:" -ForegroundColor Cyan
Write-Host "AZUREAPPSERVICE_TENANTID_E98A3D91C0BE4E9D9C73AEDAC8E060A8" -ForegroundColor White
Write-Host ""
Write-Host "Secret:" -ForegroundColor Cyan
Write-Host $tenantId -ForegroundColor White
Write-Host ""

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray

Write-Host ""
Write-Host "3️⃣  SECRET #3" -ForegroundColor Yellow
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host "Name:" -ForegroundColor Cyan
Write-Host "AZUREAPPSERVICE_SUBSCRIPTIONID_D8775514D3A74A7B87470C2515F3D1A1" -ForegroundColor White
Write-Host ""
Write-Host "Secret:" -ForegroundColor Cyan
Write-Host $subscriptionId -ForegroundColor White
Write-Host ""

Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor DarkGray
Write-Host ""

# Copiar para clipboard se disponível
try {
    $secretsText = @"
SECRET 1:
Name: AZUREAPPSERVICE_CLIENTID_856727333C674C08A008A6D80815BE73
Value: $clientId

SECRET 2:
Name: AZUREAPPSERVICE_TENANTID_E98A3D91C0BE4E9D9C73AEDAC8E060A8
Value: $tenantId

SECRET 3:
Name: AZUREAPPSERVICE_SUBSCRIPTIONID_D8775514D3A74A7B87470C2515F3D1A1
Value: $subscriptionId
"@

    Set-Clipboard -Value $secretsText
    Write-Host "📋 Secrets copiados para a área de transferência!" -ForegroundColor Green
} catch {
    Write-Host "⚠️  Não foi possível copiar automaticamente" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "🔗 PRÓXIMOS PASSOS:" -ForegroundColor Yellow
Write-Host "   1. Acesse: https://github.com/avilaops/hotelaria/settings/environments" -ForegroundColor White
Write-Host "   2. Clique em 'Production'" -ForegroundColor White
Write-Host "   3. Clique em 'Add environment secret'" -ForegroundColor White
Write-Host "   4. Adicione os 3 secrets acima" -ForegroundColor White
Write-Host "   5. Marque 'Allow administrators to bypass'" -ForegroundColor White
Write-Host "   6. Clique em 'Save protection rules'" -ForegroundColor White
Write-Host ""
Write-Host "📖 Guia completo: docs/GITHUB-ENVIRONMENT-PRODUCTION.md" -ForegroundColor Cyan
Write-Host ""
Write-Host "✅ PRONTO! Agora pode fazer deploy." -ForegroundColor Green

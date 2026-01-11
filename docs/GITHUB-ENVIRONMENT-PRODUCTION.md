# 🚀 Configurar GitHub Environment Production

**Data:** 09/01/2026  
**Objetivo:** Configurar ambiente Production no GitHub com secrets do Azure  
**Tempo estimado:** 10 minutos  
**Status:** Seu workflow usa **Service Principal** (OIDC)

---

## 🔍 Diagnóstico do Seu Workflow

Analisando `.github/workflows/main_hotelaria-app.yml`:

```yaml
deploy:
  environment:
    name: 'Production'  # ← Environment necessário
  
  steps:
    - name: Login to Azure
      uses: azure/login@v2
      with:
        client-id: ${{ secrets.AZUREAPPSERVICE_CLIENTID_856727333C674C08A008A6D80815BE73 }}
        tenant-id: ${{ secrets.AZUREAPPSERVICE_TENANTID_E98A3D91C0BE4E9D9C73AEDAC8E060A8 }}
        subscription-id: ${{ secrets.AZUREAPPSERVICE_SUBSCRIPTIONID_D8775514D3A74A7B87470C2515F3D1A1 }}
```

**❌ Problema:** O job `deploy` não declara `environment: Production`, mas os secrets estão esperando ser do environment.

---

## ✅ Solução: 3 Secrets Necessários

Você precisa adicionar **exatamente estes 3 secrets** no environment **Production**:

### 📋 Lista de Secrets

| # | Nome do Secret | Valor | Status |
|---|----------------|-------|--------|
| 1 | `AZUREAPPSERVICE_CLIENTID_856727333C674C08A008A6D80815BE73` | Client ID do Service Principal | ❌ Faltando |
| 2 | `AZUREAPPSERVICE_TENANTID_E98A3D91C0BE4E9D9C73AEDAC8E060A8` | Tenant ID do Azure AD | ❌ Faltando |
| 3 | `AZUREAPPSERVICE_SUBSCRIPTIONID_D8775514D3A74A7B87470C2515F3D1A1` | Subscription ID | ❌ Faltando |

---

## 🔐 Passo 1: Obter os Valores dos Secrets

### Método 1: Via Azure Portal (Mais Fácil)

1. **Acessar Azure Portal:**
   - URL: https://portal.azure.com

2. **Obter Subscription ID:**
   ```
   Portal → Subscriptions → Copiar "Subscription ID"
   ```
   **Exemplo:** `12345678-1234-1234-1234-123456789012`

3. **Obter Tenant ID:**
   ```
   Portal → Microsoft Entra ID → Overview → "Tenant ID"
   ```
   **Exemplo:** `87654321-4321-4321-4321-210987654321`

4. **Obter Client ID:**
   ```
   Portal → Microsoft Entra ID → App registrations → "hotelaria-app" → Application (client) ID
   ```
   **Exemplo:** `11111111-2222-3333-4444-555555555555`

### Método 2: Via Azure CLI

```powershell
# 1. Subscription ID
az account show --query id -o tsv

# 2. Tenant ID
az account show --query tenantId -o tsv

# 3. Client ID (precisa do nome do Service Principal)
az ad sp list --display-name hotelaria-app --query "[0].appId" -o tsv
```

---

## 🌍 Passo 2: Adicionar Secrets no GitHub

### Passo a Passo Visual

#### 1. Acessar Environment

```
GitHub → https://github.com/avilaops/hotelaria
        ↓
Settings (engrenagem) → Environments
        ↓
Production → "Add environment secret"
```

#### 2. Adicionar Secret #1

```
┌─────────────────────────────────────────────────────────┐
│ Add secret                                              │
├─────────────────────────────────────────────────────────┤
│ Name:                                                   │
│ [AZUREAPPSERVICE_CLIENTID_856727333C674C08A008A6D80815BE73] │
│                                                         │
│ Secret:                                                 │
│ ┌─────────────────────────────────────────────────────┐ │
│ │ 11111111-2222-3333-4444-555555555555                │ │
│ └─────────────────────────────────────────────────────┘ │
│                                                         │
│ [Add secret]                                            │
└─────────────────────────────────────────────────────────┘
```

**IMPORTANTE:** Copie o nome EXATAMENTE como está acima (com o sufixo `_856727...`)

#### 3. Adicionar Secret #2

```
Name: AZUREAPPSERVICE_TENANTID_E98A3D91C0BE4E9D9C73AEDAC8E060A8
Secret: 87654321-4321-4321-4321-210987654321
```

#### 4. Adicionar Secret #3

```
Name: AZUREAPPSERVICE_SUBSCRIPTIONID_D8775514D3A74A7B87470C2515F3D1A1
Secret: 12345678-1234-1234-1234-123456789012
```

---

## ⚙️ Passo 3: Configurar Deployment Protection

Na tela do environment **Production**:

### Configurações Recomendadas

```
Deployment protection rules:
┌────────────────────────────────────────────┐
│ ☐ Required reviewers                      │
│   (Deixe desmarcado para auto-deploy)     │
│                                            │
│ ☐ Wait timer                               │
│   (Deixe desmarcado para deploy imediato) │
│                                            │
│ ☑️ Allow administrators to bypass         │
│   configured protection rules             │
│   (MARQUE ESTA! Importante!)              │
└────────────────────────────────────────────┘

[Save protection rules]
```

### Deployment branches and tags:

```
No restriction ▼

(Permite qualquer branch fazer deploy)
```

---

## 📝 Passo 4: Atualizar Workflow (Necessário!)

O workflow atual **não declara** o environment. Precisa adicionar:

```yaml
deploy:
  runs-on: ubuntu-latest
  needs: build
  environment:  # ← ADICIONAR ESTA LINHA
    name: 'Production'  # ← E ESTA LINHA
  permissions:
    id-token: write
    contents: read
```

---

## 🧪 Passo 5: Testar Deployment

### Fazer um Commit de Teste

```bash
# Fazer uma mudança pequena
echo "# Deploy test" >> README.md

# Commit
git add README.md
git commit -m "test: trigger production deployment"

# Push para main
git push origin main
```

### Verificar Execução

```
GitHub → Actions → Último workflow
```

**Esperado:**
```
✅ build (completed)
✅ deploy (completed) ← Deve aparecer "Production"
```

---

## 🔧 Troubleshooting

### Erro: "Secret not found"

**Causa:** Nome do secret está errado ou não está no environment.

**Solução:**
1. Verificar se os 3 secrets existem em **Production** (não em Repository secrets)
2. Verificar se os nomes são **exatamente** iguais (incluindo sufixos)
3. Re-adicionar os secrets se necessário

### Erro: "AADSTS700016: Application not found"

**Causa:** Client ID incorreto ou Service Principal não existe.

**Solução:**
```powershell
# Verificar se Service Principal existe
az ad sp show --id <CLIENT_ID>

# Se não existir, criar novo
az ad sp create-for-rbac `
  --name hotelaria-app-sp `
  --role contributor `
  --scopes /subscriptions/<SUBSCRIPTION_ID>/resourceGroups/hotelaria-rg
```

### Erro: "Deployment protection rules"

**Causa:** Proteções configuradas estão bloqueando.

**Solução:**
1. Ir em Settings → Environments → Production
2. Marcar "Allow administrators to bypass"
3. Salvar

---

## 📋 Checklist Final

Antes de fazer push, verifique:

- [ ] ✅ 3 secrets adicionados no environment **Production**
- [ ] ✅ Nomes dos secrets conferidos (com sufixos corretos)
- [ ] ✅ "Allow administrators to bypass" marcado
- [ ] ✅ Workflow atualizado com `environment: Production`
- [ ] ✅ Service Principal existe e tem permissões
- [ ] ✅ Commit de teste pronto

---

## 🎯 Resumo Visual

```
┌─────────────────────────────────────────────┐
│ GitHub Repository: avilaops/hotelaria       │
├─────────────────────────────────────────────┤
│ Settings → Environments → Production        │
│                                             │
│ Environment secrets (3):                    │
│ ✅ AZUREAPPSERVICE_CLIENTID_856727...       │
│ ✅ AZUREAPPSERVICE_TENANTID_E98A3D...       │
│ ✅ AZUREAPPSERVICE_SUBSCRIPTIONID_D877...   │
│                                             │
│ Protection rules:                           │
│ ☑️ Allow administrators to bypass          │
│                                             │
│ Deployment branches:                        │
│ No restriction                              │
└─────────────────────────────────────────────┘
         ↓
┌─────────────────────────────────────────────┐
│ Workflow: main_hotelaria-app.yml            │
├─────────────────────────────────────────────┤
│ deploy:                                     │
│   environment:                              │
│     name: 'Production'  ← NECESSÁRIO        │
│   steps:                                    │
│     - Login to Azure                        │
│       with:                                 │
│         client-id: ${{ secrets.AZURE... }} │
└─────────────────────────────────────────────┘
         ↓
┌─────────────────────────────────────────────┐
│ Azure App Service: hotelaria-app            │
├─────────────────────────────────────────────┤
│ Status: Running                             │
│ URL: https://hotelaria-app.azurewebsites.net│
└─────────────────────────────────────────────┘
```

---

## 🚀 Comando Rápido

Se preferir fazer tudo via script PowerShell:

```powershell
# configure-github-environment.ps1

# 1. Obter valores
$subscriptionId = az account show --query id -o tsv
$tenantId = az account show --query tenantId -o tsv
$clientId = az ad sp list --display-name hotelaria-app --query "[0].appId" -o tsv

# 2. Exibir instruções
Write-Host "============================================" -ForegroundColor Cyan
Write-Host "SECRETS PARA ADICIONAR NO GITHUB" -ForegroundColor Cyan
Write-Host "============================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "1. AZUREAPPSERVICE_CLIENTID_856727333C674C08A008A6D80815BE73" -ForegroundColor Yellow
Write-Host "   Valor: $clientId" -ForegroundColor White
Write-Host ""
Write-Host "2. AZUREAPPSERVICE_TENANTID_E98A3D91C0BE4E9D9C73AEDAC8E060A8" -ForegroundColor Yellow
Write-Host "   Valor: $tenantId" -ForegroundColor White
Write-Host ""
Write-Host "3. AZUREAPPSERVICE_SUBSCRIPTIONID_D8775514D3A74A7B87470C2515F3D1A1" -ForegroundColor Yellow
Write-Host "   Valor: $subscriptionId" -ForegroundColor White
Write-Host ""
Write-Host "Adicione estes secrets em:" -ForegroundColor Green
Write-Host "https://github.com/avilaops/hotelaria/settings/environments" -ForegroundColor Green
```

Salve como `configure-github-environment.ps1` e execute.

---

## 📞 Suporte

Se ainda tiver erros:

1. **Verificar logs do workflow:**
   - GitHub → Actions → Último run → Ver detalhes

2. **Verificar Azure:**
   ```powershell
   az webapp show --name hotelaria-app --resource-group hotelaria-rg
   ```

3. **Recriar Service Principal:**
   ```powershell
   # Deletar antigo
   az ad sp delete --id <CLIENT_ID>
   
   # Criar novo
   az ad sp create-for-rbac --name hotelaria-app-sp --role contributor
   ```

---

**✅ Depois de configurar, o deploy deve funcionar automaticamente!**

*Versão: 2.6.4*  
*Data: 09/01/2026*  
*Guia Completo de Configuração GitHub Environment*

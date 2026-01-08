# 🔐 Configuração de Deploy Azure com Service Principal

**Data:** 08/01/2026  
**Versão:** v2.6.3  
**Autor:** Nicolas Rosa

---

## ⚠️ **Problema Identificado:**

```
##[error]Deployment Failed, Error: Publish profile is invalid
```

**Causa:** Azure desabilitou autenticação básica (Basic Auth) por padrão por motivos de segurança.

**Solução:** Usar **Azure Service Principal** com autenticação moderna (OAuth 2.0).

---

## ✅ **Solução Completa:**

### **Passo 1: Criar Service Principal no Azure**

#### **Opção A: Via Azure CLI (Recomendado)**

Abra o PowerShell ou Azure Cloud Shell:

```powershell
# 1. Fazer login no Azure
az login

# 2. Listar suas subscriptions
az account list --output table

# 3. Definir a subscription ativa (copie o ID da tabela)
az account set --subscription "xxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx"

# 4. Listar resource groups
az group list --output table

# 5. Criar Service Principal com permissões
az ad sp create-for-rbac \
  --name "github-actions-hotelaria" \
  --role contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group-name} \
  --sdk-auth
```

**📋 Substitua:**
- `{subscription-id}` → ID da subscription (passo 2)
- `{resource-group-name}` → Nome do resource group (passo 4)

**Exemplo:**
```powershell
az ad sp create-for-rbac \
  --name "github-actions-hotelaria" \
  --role contributor \
  --scopes /subscriptions/12345678-1234-1234-1234-123456789012/resourceGroups/hotelaria-rg \
  --sdk-auth
```

---

#### **Opção B: Via Portal Azure**

Se não tem Azure CLI instalado:

1. **Portal Azure** → **Azure Active Directory**
2. **App registrations** → **New registration**
3. **Name:** `github-actions-hotelaria`
4. **Supported account types:** Single tenant
5. **Register**
6. **Certificates & secrets** → **New client secret**
7. **Description:** `GitHub Actions Deploy`
8. **Expires:** 24 months
9. **Add** → **📋 COPIE O SECRET** (só aparece uma vez!)

Depois:

10. **IAM (Access Control)** no Resource Group
11. **Add role assignment**
12. **Role:** Contributor
13. **Assign access to:** User, group, or service principal
14. **Select:** `github-actions-hotelaria`
15. **Save**

---

### **Passo 2: Formato do JSON (Output do Comando)**

O comando `az ad sp create-for-rbac` vai retornar:

```json
{
  "clientId": "xxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx",
  "clientSecret": "xxxxx~xxxxxxxxxxxxxxx",
  "subscriptionId": "xxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx",
  "tenantId": "xxxxx-xxxx-xxxx-xxxx-xxxxxxxxxx",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

**📋 Copie TODO esse JSON!**

---

### **Passo 3: Adicionar Secret no GitHub**

1. Vá em: https://github.com/avilaops/hotelaria/settings/secrets/actions

2. Clique em **"New repository secret"**

3. **Name:** `AZURE_CREDENTIALS`

4. **Secret:** Cole o JSON completo

5. Clique **"Add secret"**

---

### **Passo 4: Remover Secret Antigo (Opcional)**

Se já tinha `AZURE_WEBAPP_PUBLISH_PROFILE`:

1. https://github.com/avilaops/hotelaria/settings/secrets/actions
2. Clique em `AZURE_WEBAPP_PUBLISH_PROFILE`
3. **Remove** (não é mais necessário)

---

### **Passo 5: Testar Deploy**

Após adicionar o secret `AZURE_CREDENTIALS`:

```powershell
# Trigger novo deploy
git commit --allow-empty -m "🔐 Deploy with Service Principal"
git push origin main
```

Ou manualmente no GitHub:
1. **Actions** → **Build & Deploy (.NET)**
2. **Run workflow** → **Run workflow**

---

## 🔍 **Verificação:**

### **Secrets Necessários:**

| Secret Name | Descrição | Status |
|-------------|-----------|--------|
| `AZURE_CREDENTIALS` | JSON do Service Principal | ✅ Obrigatório |
| ~~`AZURE_WEBAPP_PUBLISH_PROFILE`~~ | Publish Profile (deprecated) | ❌ Não usar mais |

---

## 📊 **Comparação: Publish Profile vs Service Principal**

| Feature | Publish Profile | Service Principal |
|---------|----------------|-------------------|
| **Segurança** | ❌ Basic Auth | ✅ OAuth 2.0 |
| **Suportado no Azure** | ⚠️ Sendo descontinuado | ✅ Recomendado |
| **Expira?** | ⚠️ Pode expirar | ✅ Renovável |
| **Permissões** | ⚠️ Acesso total ao app | ✅ RBAC granular |
| **Múltiplos ambientes** | ❌ Um profile por app | ✅ Um SP para tudo |

---

## 🛠️ **Troubleshooting:**

### **Erro: "Failed to get app runtime OS"**
```
##[warning]Failed to set resource details: Failed to get app runtime OS
```

**Solução:**
- Isso é apenas um warning, não afeta o deploy
- Causado por limitação da action `azure/webapps-deploy@v2`
- Pode ser ignorado se o deploy funcionar

---

### **Erro: "The client does not have authorization"**
```
##[error]The client '...' with object id '...' does not have authorization
```

**Solução:**
1. Verificar se o Service Principal tem role **Contributor**
2. Verificar scope (deve incluir o resource group)
3. Re-criar Service Principal:

```powershell
# Deletar antigo
az ad sp delete --id {app-id}

# Criar novo
az ad sp create-for-rbac --name "github-actions-hotelaria" --role contributor --scopes /subscriptions/{sub-id}/resourceGroups/{rg-name} --sdk-auth
```

---

### **Erro: "App not found"**
```
##[error]Error: Failed to fetch App 'hotelaria-app' details
```

**Solução:**
1. Verificar se o app existe no Azure:
```powershell
az webapp list --output table
```

2. Verificar nome do app no workflow:
```yaml
env:
  AZURE_WEBAPP_NAME: 'hotelaria-app'  # ← Deve ser o nome exato
```

3. Verificar resource group:
```powershell
az webapp show --name hotelaria-app --resource-group {seu-rg}
```

---

## 🔐 **Segurança:**

### **Boas Práticas:**

✅ **Usar Service Principal** em vez de Publish Profile  
✅ **Rotacionar secrets** a cada 6-12 meses  
✅ **Permissões mínimas** (Contributor apenas no RG necessário)  
✅ **Ambientes separados** (Production/Staging)  
✅ **Revisar acessos** periodicamente  

❌ **Nunca commitar** secrets no código  
❌ **Nunca compartilhar** Service Principal credentials  
❌ **Nunca usar** mesma SP para prod/dev  

---

## 📚 **Comandos Úteis:**

```powershell
# Listar Service Principals
az ad sp list --display-name "github-actions-hotelaria" --output table

# Verificar permissões
az role assignment list --assignee {service-principal-id} --output table

# Renovar client secret (se expirar)
az ad sp credential reset --name {service-principal-id}

# Deletar Service Principal
az ad sp delete --id {service-principal-id}

# Verificar status do app
az webapp show --name hotelaria-app --resource-group {rg} --query state
```

---

## 🎯 **Checklist Final:**

- [ ] Azure CLI instalado (`az --version`)
- [ ] Login no Azure (`az login`)
- [ ] Service Principal criado
- [ ] JSON copiado
- [ ] Secret `AZURE_CREDENTIALS` adicionado no GitHub
- [ ] Workflow atualizado
- [ ] Deploy testado
- [ ] App funcionando

---

## 📞 **Suporte:**

Se ainda tiver problemas:

1. **Verificar logs do GitHub Actions**
2. **Verificar logs do Azure App Service** (Portal → App Service → Log stream)
3. **Verificar kudu** (https://hotelaria-app.scm.azurewebsites.net)

---

**Versão:** v2.6.3  
**Data:** 08/01/2026  
**Status:** ✅ Service Principal Configurado

---

**Ávila Inc. - Desenvolvido com ❤️ em Portugal**

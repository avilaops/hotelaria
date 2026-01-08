# 🔐 Guia Completo: Publish Profile no Azure (2026)

**Data:** 08/01/2026  
**Versão:** v2.6.4  
**Autor:** Nicolas Rosa

---

## ⚠️ PROBLEMA: "Autenticação Básica Desabilitada"

Quando você tenta baixar o Publish Profile, aparece:

```
❌ Baixar o perfil de publicação
   A autenticação básica está desabilitada
```

**Causa:** Azure desabilitou Basic Auth por padrão desde 2024 por segurança.

---

## ✅ SOLUÇÃO COMPLETA

### **Opção 1: Habilitar Temporariamente Basic Auth (Mais Rápido)**

> ⚠️ **Atenção:** Não recomendado para produção de longo prazo.

#### Passo 1: Habilitar Basic Auth

1. **No Azure Portal**, acesse seu Web App: `hotelaria-app`
2. Menu lateral esquerdo → **"Configuração"** (ou **"Configuration"**)
3. Aba **"Configurações gerais"** (ou **"General settings"**)
4. **SCM Basic Auth Publishing Credentials** → Alterar para **ON** ✅
5. **FTP Basic Auth Publishing Credentials** → Alterar para **ON** ✅ (opcional)
6. Clicar em **"Salvar"** no topo
7. Confirmar a mudança

#### Passo 2: Baixar Publish Profile

1. Voltar para **"Visão geral"** (ou **"Overview"**)
2. Clicar em **"Baixar perfil de publicação"** (ou **"Get publish profile"**)
3. Arquivo `.publishsettings` será baixado
4. **IMPORTANTE:** Abrir o arquivo e copiar **TODO** o conteúdo XML

#### Passo 3: Adicionar no GitHub Secrets

1. Ir para: https://github.com/avilaops/hotelaria/settings/secrets/actions
2. Clicar em **"New repository secret"**
3. **Name:** `AZURE_WEBAPP_PUBLISH_PROFILE`
4. **Secret:** Colar o XML completo
5. Clicar **"Add secret"**

#### Passo 4: Testar Deploy

```bash
# Fazer commit vazio para trigger
git commit --allow-empty -m "🚀 Deploy: Test Publish Profile"
git push origin main
```

Ir para: https://github.com/avilaops/hotelaria/actions e verificar o deploy.

---

### **Opção 2: Usar Service Principal (Recomendado para Produção)**

> ✅ **Recomendado:** Método moderno e seguro, sem precisar habilitar Basic Auth.

#### Passo 1: Criar Service Principal

**Via Azure CLI (Recomendado):**

```powershell
# 1. Login no Azure
az login

# 2. Listar subscriptions
az account list --output table

# 3. Definir subscription ativa (copie o ID da tabela)
az account set --subscription "SEU-SUBSCRIPTION-ID"

# 4. Criar Service Principal
az ad sp create-for-rbac `
  --name "github-actions-hotelaria" `
  --role contributor `
  --scopes /subscriptions/SEU-SUBSCRIPTION-ID/resourceGroups/SEU-RESOURCE-GROUP `
  --sdk-auth
```

**Substitua:**
- `SEU-SUBSCRIPTION-ID` → ID da sua subscription (passo 2)
- `SEU-RESOURCE-GROUP` → Nome do resource group (ex: `hotelaria-rg`)

**Exemplo real:**
```powershell
az ad sp create-for-rbac `
  --name "github-actions-hotelaria" `
  --role contributor `
  --scopes /subscriptions/3b49f371-dd88-46cf-ba99-2b0da3bbc4f4/resourceGroups/hotelaria-rg `
  --sdk-auth
```

#### Passo 2: Copiar JSON Output

O comando retornará algo assim:

```json
{
  "clientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "clientSecret": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
  "subscriptionId": "3b49f371-dd88-46cf-ba99-2b0da3bbc4f4",
  "tenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "activeDirectoryEndpointUrl": "https://login.microsoftonline.com",
  "resourceManagerEndpointUrl": "https://management.azure.com/",
  "activeDirectoryGraphResourceId": "https://graph.windows.net/",
  "sqlManagementEndpointUrl": "https://management.core.windows.net:8443/",
  "galleryEndpointUrl": "https://gallery.azure.com/",
  "managementEndpointUrl": "https://management.core.windows.net/"
}
```

**📋 Copie TODO esse JSON!**

#### Passo 3: Adicionar Secret no GitHub

1. Ir para: https://github.com/avilaops/hotelaria/settings/secrets/actions
2. Clicar em **"New repository secret"**
3. **Name:** `AZURE_CREDENTIALS`
4. **Secret:** Colar o JSON completo
5. Clicar **"Add secret"**

#### Passo 4: Atualizar Workflow

Editar `.github/workflows/dotnet.yml`:

**Localizar:**
```yaml
- name: 🚀 Deploy to Azure Web App
  id: deploy-to-webapp
  uses: azure/webapps-deploy@v2
  with:
    app-name: ${{ env.AZURE_WEBAPP_NAME }}
    publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
    package: ${{ env.AZURE_WEBAPP_PACKAGE_PATH }}
```

**Substituir por:**
```yaml
- name: 🔐 Azure Login
  uses: azure/login@v1
  with:
    creds: ${{ secrets.AZURE_CREDENTIALS }}

- name: 🚀 Deploy to Azure Web App
  id: deploy-to-webapp
  uses: azure/webapps-deploy@v2
  with:
    app-name: ${{ env.AZURE_WEBAPP_NAME }}
    package: ${{ env.AZURE_WEBAPP_PACKAGE_PATH }}
```

#### Passo 5: Testar Deploy

```bash
git add .github/workflows/dotnet.yml
git commit -m "🔐 feat: Migrar para Service Principal"
git push origin main
```

---

## 🔍 Comparação dos Métodos

| Feature | Publish Profile | Service Principal |
|---------|----------------|-------------------|
| **Configuração** | ⚠️ Requer habilitar Basic Auth | ✅ Sem Basic Auth |
| **Segurança** | ⚠️ Menos seguro | ✅ Mais seguro (OAuth 2.0) |
| **Suporte** | ⚠️ Sendo descontinuado | ✅ Método recomendado |
| **Expiração** | ⚠️ Pode expirar | ✅ Renovável facilmente |
| **Múltiplos Apps** | ❌ Um profile por app | ✅ Um SP para vários apps |
| **Permissões** | ⚠️ Acesso total ao app | ✅ Granular (RBAC) |
| **Facilidade** | ✅ Mais fácil inicialmente | ⚠️ Requer Azure CLI |

---

## 📝 Passo a Passo Visual (Publish Profile)

### 1. Habilitar Basic Auth

```
Portal Azure
    └─ hotelaria-app
        └─ Configuração (menu lateral)
            └─ Configurações gerais (aba)
                └─ SCM Basic Auth: OFF → ON ✅
                └─ Salvar (topo)
```

### 2. Baixar Profile

```
Portal Azure
    └─ hotelaria-app
        └─ Visão geral
            └─ Baixar perfil de publicação (topo)
                └─ hotelaria-app-xxxxx.PublishSettings (download)
```

### 3. Conteúdo do Arquivo

```xml
<publishData>
  <publishProfile 
    profileName="hotelaria-app - Web Deploy"
    publishMethod="MSDeploy"
    publishUrl="hotelaria-app.scm.azurewebsites.net:443"
    msdeploySite="hotelaria-app"
    userName="$hotelaria-app"
    userPWD="SENHA-MUITO-LONGA-AQUI"
    ...
  />
  ...
</publishData>
```

**📋 Copiar TUDO (incluindo `<publishData>` até `</publishData>`)**

---

## 🚨 Troubleshooting

### ❌ Erro: "Basic Auth is disabled"

**Solução:**
1. Configuração → Configurações gerais
2. **SCM Basic Auth** → ON
3. Salvar
4. Aguardar 1-2 minutos
5. Tentar baixar novamente

---

### ❌ Erro: "Failed to authenticate"

**Possíveis causas:**
1. Basic Auth ainda desabilitado
2. Publish Profile expirado
3. Secret no GitHub incorreto

**Solução:**
1. Re-habilitar Basic Auth
2. Baixar novo Publish Profile
3. Atualizar secret no GitHub
4. Tentar deploy novamente

---

### ❌ Erro: "The client does not have authorization"

**Causa:** Service Principal sem permissões.

**Solução:**
```powershell
# Verificar permissões
az role assignment list --assignee SEU-SERVICE-PRINCIPAL-ID --output table

# Re-criar Service Principal
az ad sp delete --id SEU-SERVICE-PRINCIPAL-ID
az ad sp create-for-rbac --name "github-actions-hotelaria" --role contributor --scopes /subscriptions/SEU-SUB-ID/resourceGroups/SEU-RG --sdk-auth
```

---

### ❌ Erro: "App not found"

**Solução:**
```powershell
# Verificar se o app existe
az webapp list --output table

# Verificar nome no workflow
# .github/workflows/dotnet.yml
env:
  AZURE_WEBAPP_NAME: 'hotelaria-app'  # ← Deve ser o nome EXATO
```

---

## 🔐 Segurança: Boas Práticas

### ✅ Fazer

1. **Usar Service Principal em produção**
2. **Rotacionar secrets a cada 90 dias**
3. **Usar RBAC com permissões mínimas**
4. **Desabilitar Basic Auth após obter profile**
5. **Nunca commitar secrets no código**

### ❌ Não Fazer

1. **Deixar Basic Auth habilitado permanentemente**
2. **Compartilhar Publish Profile**
3. **Usar mesmo Service Principal para dev/prod**
4. **Expor secrets em logs**

---

## 📊 Checklist de Implementação

### Opção 1: Publish Profile

- [ ] Habilitar SCM Basic Auth no Azure
- [ ] Baixar Publish Profile
- [ ] Copiar XML completo
- [ ] Adicionar secret `AZURE_WEBAPP_PUBLISH_PROFILE` no GitHub
- [ ] Fazer push para trigger deploy
- [ ] Verificar deploy no Actions
- [ ] (Opcional) Desabilitar Basic Auth após sucesso

### Opção 2: Service Principal

- [ ] Azure CLI instalado (`az --version`)
- [ ] Login no Azure (`az login`)
- [ ] Criar Service Principal
- [ ] Copiar JSON output
- [ ] Adicionar secret `AZURE_CREDENTIALS` no GitHub
- [ ] Atualizar workflow para usar `azure/login@v1`
- [ ] Remover referência a `publish-profile`
- [ ] Fazer push para trigger deploy
- [ ] Verificar deploy no Actions

---

## 🎯 URLs Importantes

### Azure Portal
```
App Service: https://portal.azure.com/#@/resource/subscriptions/3b49f371-dd88-46cf-ba99-2b0da3bbc4f4/resourceGroups/SEU-RG/providers/Microsoft.Web/sites/hotelaria-app/appServices

Configuração: https://portal.azure.com/#@/resource/.../configuration

Kudu: https://hotelaria-app.scm.azurewebsites.net
```

### GitHub
```
Secrets: https://github.com/avilaops/hotelaria/settings/secrets/actions

Actions: https://github.com/avilaops/hotelaria/actions

Workflow: https://github.com/avilaops/hotelaria/blob/main/.github/workflows/dotnet.yml
```

---

## 📚 Comandos Úteis

### Azure CLI

```powershell
# Login
az login

# Listar subscriptions
az account list --output table

# Listar resource groups
az group list --output table

# Listar apps
az webapp list --output table

# Verificar app específico
az webapp show --name hotelaria-app --resource-group hotelaria-rg

# Verificar status
az webapp show --name hotelaria-app --resource-group hotelaria-rg --query state

# Ver logs em tempo real
az webapp log tail --name hotelaria-app --resource-group hotelaria-rg

# Restart app
az webapp restart --name hotelaria-app --resource-group hotelaria-rg
```

### Service Principal

```powershell
# Listar Service Principals
az ad sp list --display-name "github-actions-hotelaria" --output table

# Verificar permissões
az role assignment list --assignee SEU-SP-ID --output table

# Deletar Service Principal
az ad sp delete --id SEU-SP-ID

# Renovar client secret (se expirar)
az ad sp credential reset --name SEU-SP-ID
```

---

## 🎉 Resultado Esperado

### Após Configuração Bem-Sucedida

```
╔════════════════════════════════════════╗
║  DEPLOY AZURE VIA GITHUB ACTIONS      ║
╠════════════════════════════════════════╣
║  Workflow:              ✅ Configurado ║
║  Secret GitHub:         ✅ Adicionado  ║
║  Azure App:             ✅ Rodando     ║
║  Deploy Automático:     ✅ Funcionando ║
╠════════════════════════════════════════╣
║  Status: Pronto para Push!            ║
╚════════════════════════════════════════╝
```

### No GitHub Actions

```
✅ Build & Test
✅ Deploy to Azure
✅ Health Check

🎉 Deployment successful!
🌐 https://hotelaria-app.azurewebsites.net
```

---

## 📞 Suporte Adicional

### Documentação Oficial
- [Azure Web Apps](https://docs.microsoft.com/azure/app-service/)
- [GitHub Actions Azure](https://docs.microsoft.com/azure/app-service/deploy-github-actions)
- [Service Principal](https://docs.microsoft.com/azure/developer/github/connect-from-azure)

### Documentos Relacionados
- `docs/AZURE-SERVICE-PRINCIPAL.md` - Guia detalhado de Service Principal
- `docs/AZURE-DEPLOY.md` - Deploy completo no Azure
- `docs/GITHUB-ACTIONS-SETUP.md` - Configuração do CI/CD

---

## ✅ Conclusão

Você tem **duas opções**:

### Opção Rápida (5 minutos):
1. Habilitar Basic Auth
2. Baixar Publish Profile
3. Adicionar secret no GitHub
4. Deploy!

### Opção Segura (10 minutos):
1. Instalar Azure CLI
2. Criar Service Principal
3. Atualizar workflow
4. Deploy!

**Recomendação:** Comece com Publish Profile para testar, depois migre para Service Principal.

---

**Versão:** v2.6.4  
**Data:** 08/01/2026  
**Status:** ✅ Guia Completo

---

**Ávila Inc. - Desenvolvido com ❤️ em Portugal**

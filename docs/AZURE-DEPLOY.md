# ☁️ Deploy no Azure - Sistema de Hotelaria

## 🎯 Visão Geral

Este guia mostra como fazer deploy do sistema de Hotelaria no **Azure App Service** com configuração completa e automação via GitHub Actions.

---

## 📋 Pré-requisitos

### 1. **Conta Azure**
- Conta Azure ativa (pode ser gratuita)
- Créditos disponíveis ou plano pago
- Acesso ao Portal Azure: https://portal.azure.com

### 2. **Ferramentas Locais**
- Azure CLI instalado
- Git configurado
- .NET 8.0 SDK

### 3. **Repositório GitHub**
- Repositório: `https://github.com/avilaops/hotelaria`
- Acesso de admin para configurar secrets

---

## 🚀 Método 1: Deploy via Portal Azure (Recomendado para Iniciantes)

### **Passo 1: Criar App Service**

1. Acesse o [Portal Azure](https://portal.azure.com)
2. Clique em **"Criar um recurso"**
3. Busque por **"Web App"**
4. Clique em **"Criar"**

### **Passo 2: Configurar Básico**

```
Detalhes do Projeto:
├── Assinatura: [Sua assinatura]
├── Grupo de Recursos: hotelaria-rg (criar novo)
│
Detalhes da Instância:
├── Nome: hotelaria-app (ou nome único)
├── Publicar: Código
├── Pilha de runtime: .NET 8 (LTS)
├── Sistema Operacional: Linux
├── Região: Brazil South (São Paulo)
│
Plano do Serviço de Aplicativo:
├── Nome: hotelaria-plan
├── SKU: B1 (Básico) - R$ ~55/mês
└── OU F1 (Gratuito) - Para testes
```

### **Passo 3: Configurar Deployment**

Na aba **"Implantação"**:

1. **Habilitar CI/CD:** Sim
2. **Conta GitHub:** Conectar sua conta
3. **Organização:** avilaops
4. **Repositório:** hotelaria
5. **Branch:** main

### **Passo 4: Revisar e Criar**

1. Clique em **"Revisar + criar"**
2. Aguarde a validação
3. Clique em **"Criar"**
4. Aguarde a criação (2-3 minutos)

### **Passo 5: Configurar Variáveis de Ambiente**

Após criação, vá para o App Service:

1. No menu lateral, clique em **"Configuração"**
2. Na aba **"Configurações do aplicativo"**, adicione:

```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:8080

# PayPal (opcional)
PAYPAL_CLIENT_ID = seu_client_id
PAYPAL_CLIENT_SECRET = seu_secret

# MongoDB (opcional)
MONGODB_CONNECTION_STRING = sua_connection_string
MONGODB_DATABASE_NAME = hotelaria

# Airbnb (opcional)
AIRBNB_CLIENT_KEY = seu_client_key
AIRBNB_SECRET_KEY = seu_secret_key

# Sentry (opcional)
SENTRY_TOKEN_API = seu_token
```

3. Clique em **"Salvar"**
4. Aguarde o reinício do aplicativo

---

## 🔧 Método 2: Deploy via Azure CLI (Avançado)

### **Passo 1: Instalar Azure CLI**

#### Windows:
```powershell
winget install Microsoft.AzureCLI
```

#### macOS:
```bash
brew install azure-cli
```

#### Linux:
```bash
curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
```

### **Passo 2: Login no Azure**

```bash
az login
```

Isso abrirá o navegador para autenticação.

### **Passo 3: Criar Recursos**

```bash
# Definir variáveis
RESOURCE_GROUP="hotelaria-rg"
APP_NAME="hotelaria-app"
PLAN_NAME="hotelaria-plan"
LOCATION="brazilsouth"

# Criar grupo de recursos
az group create \
  --name $RESOURCE_GROUP \
  --location $LOCATION

# Criar plano do App Service
az appservice plan create \
  --name $PLAN_NAME \
  --resource-group $RESOURCE_GROUP \
  --location $LOCATION \
  --sku B1 \
  --is-linux

# Criar Web App
az webapp create \
  --name $APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --plan $PLAN_NAME \
  --runtime "DOTNETCORE:8.0"
```

### **Passo 4: Configurar Deployment do GitHub**

```bash
# Obter credenciais de publicação
az webapp deployment list-publishing-credentials \
  --name $APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --query "{username:publishingUserName, password:publishingPassword}" \
  --output json
```

Copie o `username` e `password` para usar no GitHub Actions.

### **Passo 5: Configurar Variáveis de Ambiente**

```bash
# Configurar variáveis
az webapp config appsettings set \
  --name $APP_NAME \
  --resource-group $RESOURCE_GROUP \
  --settings \
    ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080
```

---

## 🔄 Método 3: Deploy Automatizado via GitHub Actions (Recomendado)

### **Passo 1: Obter Publish Profile**

1. No Portal Azure, vá para o App Service
2. Clique em **"Obter perfil de publicação"**
3. Salve o arquivo `.publishsettings`
4. Abra o arquivo e copie todo o conteúdo XML

### **Passo 2: Adicionar Secret no GitHub**

1. Vá para: `https://github.com/avilaops/hotelaria/settings/secrets/actions`
2. Clique em **"New repository secret"**
3. Nome: `AZURE_WEBAPP_PUBLISH_PROFILE`
4. Valor: Cole o conteúdo XML do publish profile
5. Clique em **"Add secret"**

### **Passo 3: Criar Workflow do GitHub Actions**

Crie o arquivo `.github/workflows/azure-deploy.yml`:

```yaml
name: Deploy to Azure

on:
  push:
    branches: [ main ]
  workflow_dispatch:

env:
  AZURE_WEBAPP_NAME: hotelaria-app
  DOTNET_VERSION: '8.0.x'

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - name: Checkout code
      uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: ${{ env.DOTNET_VERSION }}
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --configuration Release --no-restore
    
    - name: Test
      run: dotnet test --no-restore --verbosity normal
    
    - name: Publish
      run: dotnet publish -c Release -o ./publish
    
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: ${{ env.AZURE_WEBAPP_NAME }}
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish
```

### **Passo 4: Fazer Push e Verificar Deploy**

```bash
git add .github/workflows/azure-deploy.yml
git commit -m "ci: adicionar workflow de deploy no Azure"
git push origin main
```

Vá para: `https://github.com/avilaops/hotelaria/actions` e acompanhe o deploy.

---

## 🌐 Configurar Domínio Personalizado (Opcional)

### **Passo 1: Adicionar Domínio**

1. No App Service, clique em **"Domínios personalizados"**
2. Clique em **"Adicionar domínio personalizado"**
3. Digite seu domínio: `hotelaria.seudominio.com`

### **Passo 2: Configurar DNS**

No seu provedor de DNS, adicione:

```
Tipo: CNAME
Nome: hotelaria (ou @)
Valor: hotelaria-app.azurewebsites.net
TTL: 3600
```

### **Passo 3: Habilitar HTTPS**

1. No App Service, clique em **"Certificados TLS/SSL"**
2. Aba **"Domínios personalizados"**
3. Clique em **"Adicionar associação"**
4. Selecione seu domínio
5. Tipo de certificado: **"Certificado gerenciado pelo Serviço de Aplicativo"** (gratuito)
6. Clique em **"Adicionar"**

---

## 📊 Monitoramento e Logs

### **Visualizar Logs em Tempo Real**

#### Via Portal:
1. App Service → **"Log stream"**
2. Selecione: **"Application logs"**

#### Via CLI:
```bash
az webapp log tail \
  --name hotelaria-app \
  --resource-group hotelaria-rg
```

### **Configurar Application Insights**

1. No App Service, clique em **"Application Insights"**
2. Clique em **"Ativar Application Insights"**
3. Criar novo recurso: `hotelaria-insights`
4. Clique em **"Aplicar"**

Isso fornecerá:
- Monitoramento de performance
- Rastreamento de erros
- Métricas de usuário
- Dashboards personalizados

---

## 🔒 Segurança e Boas Práticas

### **1. Configurar CORS**

```bash
az webapp cors add \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --allowed-origins "https://hotelaria.seudominio.com"
```

### **2. Configurar Autenticação do Azure AD (Opcional)**

1. App Service → **"Autenticação"**
2. Clique em **"Adicionar provedor de identidade"**
3. Selecione: **"Microsoft"**
4. Configure conforme necessário

### **3. Habilitar Always On**

```bash
az webapp config set \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --always-on true
```

### **4. Configurar Health Check**

```bash
az webapp config set \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --health-check-path "/"
```

---

## 💰 Custos Estimados

### **Plano Gratuito (F1)**
- **Custo:** R$ 0,00/mês
- **Limitações:**
  - 60 minutos de CPU/dia
  - 1 GB de RAM
  - 1 GB de armazenamento
  - Sem custom domain
  - Sem Always On
- **Ideal para:** Testes e desenvolvimento

### **Plano Básico (B1)**
- **Custo:** R$ 55-65/mês
- **Recursos:**
  - CPU dedicado
  - 1.75 GB de RAM
  - 10 GB de armazenamento
  - Custom domain com SSL
  - Always On
- **Ideal para:** Produção pequena/média

### **Plano Standard (S1)**
- **Custo:** R$ 190-220/mês
- **Recursos:**
  - CPU dedicado
  - 1.75 GB de RAM
  - 50 GB de armazenamento
  - Auto-scaling
  - Staging slots
  - Backup automático
- **Ideal para:** Produção com alta disponibilidade

---

## 🧪 Testar o Deploy

### **1. Verificar Status**

```bash
az webapp show \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --query "{name:name, state:state, defaultHostName:defaultHostName}" \
  --output table
```

### **2. Abrir no Navegador**

```bash
az webapp browse \
  --name hotelaria-app \
  --resource-group hotelaria-rg
```

Ou acesse diretamente:
```
https://hotelaria-app.azurewebsites.net
```

### **3. Testar Endpoints**

```bash
# Health check
curl https://hotelaria-app.azurewebsites.net/

# Login
curl https://hotelaria-app.azurewebsites.net/login
```

---

## 🔄 Atualizações e Rollback

### **Deploy Manual de Nova Versão**

```bash
# Build e publicar localmente
dotnet publish -c Release -o ./publish

# Fazer deploy
az webapp deployment source config-zip \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --src ./publish.zip
```

### **Rollback para Versão Anterior**

1. Portal Azure → App Service
2. **"Slots de implantação"** (se configurado)
3. **"Trocar"** para voltar à versão anterior

Ou via CLI:
```bash
az webapp deployment list \
  --name hotelaria-app \
  --resource-group hotelaria-rg

# Escolha o ID do deployment anterior
az webapp deployment source sync \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --deployment-id <deployment-id>
```

---

## 🐛 Troubleshooting

### **Problema: App não inicia**

**Verificar logs:**
```bash
az webapp log tail --name hotelaria-app --resource-group hotelaria-rg
```

**Soluções comuns:**
1. Verificar variável `ASPNETCORE_URLS = http://+:8080`
2. Verificar runtime stack: `.NET 8`
3. Verificar arquivo `Hotelaria.csproj` tem `<TargetFramework>net8.0</TargetFramework>`

### **Problema: 502 Bad Gateway**

**Causas:**
- App travou ou crashou
- Porta incorreta
- Timeout de inicialização

**Solução:**
```bash
# Reiniciar app
az webapp restart \
  --name hotelaria-app \
  --resource-group hotelaria-rg

# Verificar configuração
az webapp config show \
  --name hotelaria-app \
  --resource-group hotelaria-rg
```

### **Problema: Variáveis de ambiente não carregadas**

**Solução:**
```bash
# Listar variáveis atuais
az webapp config appsettings list \
  --name hotelaria-app \
  --resource-group hotelaria-rg

# Adicionar variável faltante
az webapp config appsettings set \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --settings NOVA_VARIAVEL=valor
```

---

## 📦 Backup e Recuperação

### **Configurar Backup Automático**

1. Portal Azure → App Service
2. **"Backups"**
3. **"Configurar"**
4. Configurar:
   - Storage Account
   - Frequência: Diária
   - Retenção: 30 dias

### **Restaurar de Backup**

```bash
az webapp config backup restore \
  --backup-name <backup-name> \
  --resource-group hotelaria-rg \
  --webapp-name hotelaria-app \
  --overwrite
```

---

## 🎯 Checklist de Deploy

Antes de ir para produção:

- [ ] App Service criado e configurado
- [ ] GitHub Actions funcionando
- [ ] Variáveis de ambiente configuradas
- [ ] HTTPS habilitado
- [ ] Domínio personalizado configurado (se aplicável)
- [ ] Application Insights ativado
- [ ] Logs configurados
- [ ] Backup automático configurado
- [ ] Health check testado
- [ ] Credenciais sensíveis no Azure Key Vault ou App Settings
- [ ] .env NÃO commitado no Git
- [ ] Testes realizados no ambiente de produção

---

## 🚀 Resumo Rápido

### **Deploy Rápido (5 minutos):**

1. **Portal Azure** → Criar Web App
2. **Configuração:**
   - Nome: `hotelaria-app`
   - Runtime: `.NET 8`
   - Plano: `B1` (ou F1 para testes)
   - Região: `Brazil South`
3. **Deployment:** Conectar GitHub (avilaops/hotelaria)
4. **Variáveis:** Adicionar configurações do aplicativo
5. **Acessar:** `https://hotelaria-app.azurewebsites.net`

---

## 📞 Suporte

### **Documentação Oficial:**
- Azure App Service: https://learn.microsoft.com/azure/app-service/
- .NET no Azure: https://learn.microsoft.com/azure/app-service/quickstart-dotnetcore

### **Comunidade:**
- Azure Forum: https://learn.microsoft.com/answers/
- Stack Overflow: Tag `azure-app-service`

---

**🎉 Deploy no Azure Completo!**

*Sistema de Hotelaria - v2.6.0*  
*Última atualização: Janeiro 2026*

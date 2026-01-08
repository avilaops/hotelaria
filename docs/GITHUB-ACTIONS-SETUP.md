# 🚀 Configuração do GitHub Actions & Azure Deploy

Este guia explica como configurar o pipeline CI/CD completo para o projeto Hotelaria.

## 📋 Índice

1. [Estrutura do Pipeline](#estrutura-do-pipeline)
2. [Configuração do Azure](#configuração-do-azure)
3. [Secrets do GitHub](#secrets-do-github)
4. [Ambientes](#ambientes)
5. [Workflows Disponíveis](#workflows-disponíveis)

---

## 🏗️ Estrutura do Pipeline

O pipeline é dividido em **5 jobs principais**:

### 1️⃣ **Build & Test** 🏗️
- Compila o projeto
- Executa testes unitários
- Gera relatórios de cobertura
- Cache de dependências NuGet

### 2️⃣ **Code Quality & Security** 🔍
- Análise estática de código (CodeQL)
- Detecção de vulnerabilidades
- Verificação de qualidade

### 3️⃣ **Publish Artifact** 📦
- Publica artefatos para deploy
- Retenção de 7 dias
- Upload para GitHub Actions

### 4️⃣ **Deploy to Azure** 🚀
- Deploy automático para Azure
- Ambiente: Production
- Rollback automático em caso de falha

### 5️⃣ **Health Check** 🏥
- Verificação de saúde da aplicação
- Testes de disponibilidade
- Notificações de status

---

## ☁️ Configuração do Azure

### Passo 1: Criar Web App no Azure

```bash
# Login no Azure
az login

# Criar Resource Group
az group create --name hotelaria-rg --location "East US"

# Criar App Service Plan
az appservice plan create \
  --name hotelaria-plan \
  --resource-group hotelaria-rg \
  --sku B1 \
  --is-linux

# Criar Web App
az webapp create \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --plan hotelaria-plan \
  --runtime "DOTNET|8.0"
```

### Passo 2: Obter Publish Profile

**Opção 1: Via Portal Azure**
1. Acesse o Azure Portal
2. Vá para o seu Web App
3. Clique em **"Get publish profile"**
4. Salve o arquivo XML

**Opção 2: Via CLI**
```bash
az webapp deployment list-publishing-profiles \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --xml
```

---

## 🔐 Secrets do GitHub

Configure os seguintes secrets no GitHub:

### Como adicionar secrets:
1. Vá para o repositório no GitHub
2. **Settings** → **Secrets and variables** → **Actions**
3. Clique em **"New repository secret"**

### Secrets necessários:

#### **AZURE_WEBAPP_PUBLISH_PROFILE** (Production)
```
Conteúdo: Cole o XML do publish profile do ambiente de produção
Usado em: Deploy to Production
```

#### **AZURE_WEBAPP_PUBLISH_PROFILE_STAGING** (Staging)
```
Conteúdo: Cole o XML do publish profile do ambiente de staging
Usado em: Deploy to Staging
```

---

## 🌍 Ambientes

Configure os ambientes no GitHub para controle de deploy:

### Production
1. **Settings** → **Environments** → **New environment**
2. Nome: `Production`
3. Configurações recomendadas:
   - ✅ Required reviewers (1-2 pessoas)
   - ✅ Wait timer: 5 minutes
   - ✅ Deployment branches: `main` apenas

### Staging
1. Nome: `Staging`
2. Configurações recomendadas:
   - ✅ Deployment branches: `develop` apenas
   - ❌ Required reviewers (deploy automático)

---

## 📝 Workflows Disponíveis

### 1. **CI/CD Pipeline** (`.github/workflows/dotnet.yml`)
**Trigger:** Push/PR em `main` e `develop`

**Fluxo:**
```
Push → Build → Test → Code Analysis → Publish → Deploy → Health Check
```

**Jobs:**
- ✅ Build & Test
- ✅ Code Quality & Security
- ✅ Publish Artifact
- ✅ Deploy to Azure (apenas main)
- ✅ Health Check

### 2. **Staging Deployment** (`.github/workflows/staging.yml`)
**Trigger:** Push em `develop`

**Fluxo:**
```
Push develop → Build → Test → Deploy to Staging
```

### 3. **Dependabot** (`.github/dependabot.yml`)
**Trigger:** Automático (semanal)

**Atualiza:**
- ✅ GitHub Actions versions
- ✅ NuGet packages
- ⚠️ Ignora major updates

---

## 🎯 Variáveis de Ambiente

Edite em `.github/workflows/dotnet.yml`:

```yaml
env:
  DOTNET_VERSION: '8.0.x'                    # Versão do .NET
  BUILD_CONFIGURATION: 'Release'             # Configuração do build
  AZURE_WEBAPP_NAME: 'hotelaria-app'         # Nome do Web App
  AZURE_WEBAPP_PACKAGE_PATH: './publish'     # Caminho de publicação
```

---

## 🚦 Status Badges

Adicione ao README.md:

```markdown
![Build Status](https://github.com/avilaops/hotelaria/actions/workflows/dotnet.yml/badge.svg)
![Staging](https://github.com/avilaops/hotelaria/actions/workflows/staging.yml/badge.svg)
```

---

## ✅ Checklist de Configuração

- [ ] Web App criado no Azure (Production)
- [ ] Web App criado no Azure (Staging) - opcional
- [ ] Publish Profile baixado para ambos ambientes
- [ ] Secrets configurados no GitHub
- [ ] Ambientes `Production` e `Staging` criados
- [ ] Protection rules configuradas em Production
- [ ] Teste de push em branch `develop`
- [ ] Teste de push em branch `main`
- [ ] Verificação de deploy bem-sucedido

---

## 🔧 Troubleshooting

### ❌ Erro: "Azure Web App not found"
**Solução:** Verifique o nome do Web App em `AZURE_WEBAPP_NAME`

### ❌ Erro: "Invalid publish profile"
**Solução:** Regere o publish profile no Azure e atualize o secret

### ❌ Erro: "Tests failed"
**Solução:** Execute localmente: `dotnet test --verbosity detailed`

### ❌ Erro: "Artifact not found"
**Solução:** Verifique se o job `publish-artifact` foi executado com sucesso

---

## 📊 Monitoramento

### GitHub Actions Dashboard
```
https://github.com/avilaops/hotelaria/actions
```

### Azure Portal
```
https://portal.azure.com → hotelaria-app → Monitoring
```

### Logs em Tempo Real
```bash
az webapp log tail --name hotelaria-app --resource-group hotelaria-rg
```

---

## 🎓 Recursos Adicionais

- [GitHub Actions Docs](https://docs.github.com/actions)
- [Azure Web Apps](https://docs.microsoft.com/azure/app-service/)
- [.NET CI/CD Best Practices](https://docs.microsoft.com/dotnet/devops/)

---

## 👤 Autor

**Projeto Hotelaria** - Sistema de Gerenciamento de Hotel
- GitHub: [@avilaops](https://github.com/avilaops)
- Repositório: [hotelaria](https://github.com/avilaops/hotelaria)

---

**Última atualização:** $(date +%Y-%m-%d)

# ✅ Checklist de Validação - CI/CD Completo

Use esta checklist para garantir que tudo está configurado corretamente.

## 📦 Arquivos Criados

Verifique se todos os arquivos foram criados:

### GitHub Actions & CI/CD
- [x] `.github/workflows/dotnet.yml` - Pipeline principal
- [x] `.github/workflows/staging.yml` - Deploy staging
- [x] `.github/dependabot.yml` - Atualizações automáticas
- [x] `.github/pull_request_template.md` - Template de PR

### Scripts
- [x] `scripts/validate-ci.ps1` - Validação Windows
- [x] `scripts/validate-ci.sh` - Validação Linux/macOS

### Documentação
- [x] `docs/GITHUB-ACTIONS-SETUP.md` - Guia completo de setup
- [x] `docs/CI-CD-SUMMARY.md` - Resumo executivo
- [x] `docs/QUICK-COMMANDS.md` - Comandos rápidos

### Configuração
- [x] `.gitattributes` - Normalização de arquivos
- [x] `README.md` - Atualizado com badges e CI/CD info

---

## 🔧 Configuração Local

### Pré-requisitos
- [ ] .NET 8.0 SDK instalado
- [ ] Git instalado
- [ ] Azure CLI instalado (para deploy)
- [ ] GitHub CLI instalado (opcional)

### Testes Locais
- [ ] `dotnet restore` funciona
- [ ] `dotnet build` compila sem erros críticos
- [ ] `dotnet test` (se tiver testes)
- [ ] `dotnet publish` gera artefatos
- [ ] Scripts de validação executam (`validate-ci.ps1` ou `.sh`)

---

## ☁️ Configuração Azure

### Web Apps
- [ ] Web App de **Production** criado
  - Nome: `hotelaria-app` (ou personalizado)
  - Runtime: `.NET 8`
  - Region: Escolhida
  - Plan: Adequado ao uso

- [ ] Web App de **Staging** criado (opcional)
  - Nome: `hotelaria-app-staging`
  - Runtime: `.NET 8`
  - Region: Mesma da production
  - Plan: Pode ser menor/compartilhado

### Publish Profiles
- [ ] Publish Profile da **Production** baixado
- [ ] Publish Profile da **Staging** baixado (se aplicável)
- [ ] Arquivos XML salvos localmente

---

## 🔐 Configuração GitHub

### Repository Settings

#### Secrets
Vá para: **Settings → Secrets and variables → Actions**

- [ ] `AZURE_WEBAPP_PUBLISH_PROFILE` criado (Production)
- [ ] `AZURE_WEBAPP_PUBLISH_PROFILE_STAGING` criado (Staging)
- [ ] Secrets testados (não vazaram informações)

#### Environments
Vá para: **Settings → Environments**

##### Production Environment
- [ ] Nome: `Production`
- [ ] Protection rules configuradas:
  - [ ] Required reviewers (1-2 pessoas)
  - [ ] Wait timer (opcional, ex: 5 min)
  - [ ] Deployment branches: `main` apenas
- [ ] Environment secrets (se necessário)

##### Staging Environment (opcional)
- [ ] Nome: `Staging`
- [ ] Deployment branches: `develop` apenas
- [ ] Sem required reviewers (deploy automático)

#### Branch Protection
Vá para: **Settings → Branches**

##### Main Branch
- [ ] Require pull request reviews
- [ ] Require status checks (Build & Test)
- [ ] Require branches up to date
- [ ] Include administrators (opcional)

##### Develop Branch (opcional)
- [ ] Require status checks
- [ ] Menos restritivo que main

---

## 📝 Atualizar Variáveis

### Arquivo `.github/workflows/dotnet.yml`
```yaml
env:
  AZURE_WEBAPP_NAME: 'hotelaria-app'  # ⚠️ Seu nome aqui
```
- [ ] `AZURE_WEBAPP_NAME` atualizado com o nome correto

### Arquivo `.github/workflows/staging.yml`
```yaml
env:
  AZURE_WEBAPP_NAME: 'hotelaria-app-staging'  # ⚠️ Seu nome aqui
```
- [ ] `AZURE_WEBAPP_NAME` atualizado com o nome correto

### README.md
```markdown
[![Build Status](https://github.com/avilaops/hotelaria/actions/workflows/dotnet.yml/badge.svg)]
```
- [ ] URL do badge atualizada com seu usuário/repo

---

## 🧪 Testes de Validação

### Teste 1: Validação Local
```bash
.\scripts\validate-ci.ps1  # Windows
./scripts/validate-ci.sh   # Linux/macOS
```
- [ ] Todos os checks passaram (✅)
- [ ] Nenhum erro crítico

### Teste 2: Push para Develop (Staging)
```bash
git checkout -b test-staging
git push origin test-staging
```
- [ ] Workflow `staging.yml` executou
- [ ] Jobs completaram com sucesso
- [ ] Deploy para Staging OK
- [ ] App acessível em staging

### Teste 3: Pull Request
```bash
# Criar PR de test-staging para main
```
- [ ] PR template apareceu
- [ ] CI executou automaticamente
- [ ] Build passou
- [ ] Testes passaram
- [ ] Code Quality passou

### Teste 4: Deploy Production
```bash
git checkout main
git merge test-staging
git push origin main
```
- [ ] Workflow `dotnet.yml` executou
- [ ] Todos os 5 jobs completaram:
  - [ ] Build & Test
  - [ ] Code Quality & Security
  - [ ] Publish Artifact
  - [ ] Deploy Production
  - [ ] Health Check
- [ ] Deploy para Production OK
- [ ] App acessível em production

---

## 🔍 Validações Pós-Deploy

### Application Health
- [ ] URL production responde: `https://hotelaria-app.azurewebsites.net`
- [ ] URL staging responde (se aplicável)
- [ ] Sem erros 500
- [ ] Interface carrega corretamente

### Azure Portal
- [ ] Web App mostra status "Running"
- [ ] Logs não mostram erros críticos
- [ ] Métricas normais (CPU, Memory)

### GitHub
- [ ] Actions Dashboard mostra workflows com sucesso
- [ ] Badges no README mostram "passing"
- [ ] Artifacts armazenados corretamente

---

## 📊 Monitoramento Contínuo

### Diário
- [ ] Verificar GitHub Actions Dashboard
- [ ] Verificar erros no Azure Portal
- [ ] Revisar PRs pendentes

### Semanal
- [ ] Revisar Dependabot PRs
- [ ] Analisar métricas de performance
- [ ] Verificar logs de erros

### Mensal
- [ ] Revisar custos Azure
- [ ] Analisar tempo de build
- [ ] Atualizar documentação se necessário

---

## 🐛 Troubleshooting

Se algo der errado, verifique:

### Workflow Falhou
- [ ] Ver logs detalhados no GitHub Actions
- [ ] Verificar se secrets estão corretos
- [ ] Confirmar nomes de Azure Web Apps
- [ ] Re-run do workflow

### Deploy Falhou
- [ ] Publish profile válido e não expirado
- [ ] Web App existe e está running
- [ ] Logs do Azure para detalhes
- [ ] Restart do Web App

### App Não Responde
- [ ] Verificar status no Azure Portal
- [ ] Ver logs de aplicação
- [ ] Confirmar configurações corretas
- [ ] Testar deploy manual

---

## 🎯 Próximas Melhorias

Após tudo funcionando, considere:

- [ ] Adicionar testes de integração
- [ ] Configurar Application Insights
- [ ] Implementar notificações (Slack/Teams)
- [ ] Adicionar smoke tests
- [ ] Configurar Blue-Green deployment
- [ ] Implementar rollback automático
- [ ] Adicionar code coverage reports
- [ ] Configurar cache de build mais agressivo

---

## ✅ Status Final

**Data de Conclusão:** ___/___/______

**Configurado por:** _________________

**Notas:**
```
[Espaço para observações]
```

---

## 🆘 Suporte

- 📖 [Documentação Completa](docs/GITHUB-ACTIONS-SETUP.md)
- 📝 [Resumo Executivo](docs/CI-CD-SUMMARY.md)
- ⚡ [Comandos Rápidos](docs/QUICK-COMMANDS.md)
- 🐛 [Issues no GitHub](https://github.com/avilaops/hotelaria/issues)

---

**🎉 Parabéns! Se todos os itens estão marcados, seu CI/CD está 100% funcional!**

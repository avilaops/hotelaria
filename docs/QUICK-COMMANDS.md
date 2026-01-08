# ⚡ Comandos Rápidos - CI/CD

## 🚀 Desenvolvimento Local

### Validar antes de push
```bash
# Windows
.\scripts\validate-ci.ps1

# Linux/macOS
./scripts/validate-ci.sh
```

### Build e Teste
```bash
# Restore
dotnet restore

# Build
dotnet build

# Testes
dotnet test

# Build Release
dotnet build -c Release

# Publicar
dotnet publish -c Release -o ./publish
```

---

## 🔄 Git Workflow

### Feature Branch
```bash
# Criar e trocar para nova branch
git checkout -b feature/nome-da-feature

# Fazer commit
git add .
git commit -m "feat: descrição da feature"

# Push
git push origin feature/nome-da-feature
```

### Deploy Staging (develop)
```bash
git checkout develop
git merge feature/nome-da-feature
git push origin develop
# 🚀 Deploy automático para Staging
```

### Deploy Production (main)
```bash
git checkout main
git merge develop
git push origin main
# 🚀 Deploy automático para Production
```

---

## ☁️ Azure CLI

### Login
```bash
az login
```

### Listar Web Apps
```bash
az webapp list --output table
```

### Ver logs em tempo real
```bash
az webapp log tail --name hotelaria-app --resource-group hotelaria-rg
```

### Restart Web App
```bash
az webapp restart --name hotelaria-app --resource-group hotelaria-rg
```

### Obter Publish Profile
```bash
az webapp deployment list-publishing-profiles \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --xml > publishprofile.xml
```

### Ver configurações
```bash
az webapp config appsettings list \
  --name hotelaria-app \
  --resource-group hotelaria-rg
```

---

## 🐛 Debug

### Ver status do Web App
```bash
az webapp show --name hotelaria-app --resource-group hotelaria-rg
```

### Download de logs
```bash
az webapp log download \
  --name hotelaria-app \
  --resource-group hotelaria-rg \
  --log-file logs.zip
```

### SSH para o container
```bash
az webapp ssh --name hotelaria-app --resource-group hotelaria-rg
```

---

## 🔍 GitHub Actions

### Ver workflows
```bash
# Lista workflows
gh workflow list

# Ver runs de um workflow
gh run list --workflow=dotnet.yml

# Ver logs de um run
gh run view <run-id> --log
```

### Re-run workflow
```bash
# Re-run do último
gh run rerun

# Re-run de um específico
gh run rerun <run-id>
```

### Trigger manual
```bash
gh workflow run dotnet.yml
```

---

## 📦 Docker (Local)

### Build
```bash
docker build -t hotelaria:latest .
```

### Run
```bash
docker run -d -p 8080:80 --name hotelaria hotelaria:latest
```

### Logs
```bash
docker logs -f hotelaria
```

### Stop e Remove
```bash
docker stop hotelaria
docker rm hotelaria
```

---

## 🧪 Testes Avançados

### Com cobertura
```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Verbose
```bash
dotnet test --verbosity detailed
```

### Específico
```bash
dotnet test --filter FullyQualifiedName~NomeDoTeste
```

### Report HTML
```bash
dotnet test --logger "html;LogFileName=test-results.html"
```

---

## 🔐 Secrets Management

### Adicionar secret (GitHub CLI)
```bash
gh secret set SECRET_NAME < secret_file.txt
```

### Listar secrets
```bash
gh secret list
```

### Deletar secret
```bash
gh secret delete SECRET_NAME
```

---

## 📊 Monitoramento

### GitHub Actions Dashboard
```
https://github.com/avilaops/hotelaria/actions
```

### Azure Portal
```
https://portal.azure.com
```

### Application Insights (se configurado)
```bash
az monitor app-insights component show \
  --app hotelaria-app \
  --resource-group hotelaria-rg
```

---

## 🚨 Rollback

### Via Azure Portal
1. App Service → Deployment Center → Logs
2. Selecione versão anterior
3. Clique em "Redeploy"

### Via GitHub
1. Actions → Selecione workflow com sucesso anterior
2. "Re-run all jobs"

### Via Git (revert)
```bash
git revert HEAD
git push origin main
# Pipeline executa automaticamente
```

---

## 🔧 Troubleshooting Rápido

### Pipeline falhou?
```bash
# Ver logs
gh run view --log

# Re-run
gh run rerun
```

### Deploy falhou?
```bash
# Ver logs Azure
az webapp log tail --name hotelaria-app --resource-group hotelaria-rg

# Restart app
az webapp restart --name hotelaria-app --resource-group hotelaria-rg
```

### App não responde?
```bash
# Health check
curl https://hotelaria-app.azurewebsites.net/

# Ver métricas
az webapp show --name hotelaria-app --resource-group hotelaria-rg --query state
```

---

## 📝 Aliases Úteis (Opcional)

Adicione ao seu `.bashrc` ou `.bash_profile`:

```bash
# Git
alias gco='git checkout'
alias gst='git status'
alias gp='git push'
alias gl='git pull'

# .NET
alias dr='dotnet run'
alias dt='dotnet test'
alias db='dotnet build'
alias dp='dotnet publish -c Release'

# CI
alias validate='.\scripts\validate-ci.ps1'  # Windows
# alias validate='./scripts/validate-ci.sh'  # Linux

# Azure
alias az-logs='az webapp log tail --name hotelaria-app --resource-group hotelaria-rg'
alias az-restart='az webapp restart --name hotelaria-app --resource-group hotelaria-rg'
```

---

**💡 Dica:** Salve este arquivo como referência rápida!

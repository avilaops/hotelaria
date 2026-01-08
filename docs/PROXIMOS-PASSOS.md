# 🚀 Próximos Passos - Deploy e CodeQL

## ✅ O Que Foi Configurado

### 1. **Publish Profile** (Azure Deploy)
- ✅ Basic Auth habilitado no Azure
- ✅ Workflow `.github/workflows/dotnet.yml` pronto
- ⏳ **FALTA:** Baixar Publish Profile e adicionar no GitHub

### 2. **CodeQL** (Security Analysis)
- ✅ Workflow `.github/workflows/codeql.yml` criado
- ✅ Configuração `.github/codeql/codeql-config.yml` criada
- ✅ Documentação `docs/CODEQL-SECURITY.md` criada
- ✅ Badge adicionado no README.md
- ⏳ **FALTA:** Fazer commit e push

---

## 📋 Passo a Passo

### 1️⃣ Baixar Publish Profile (Azure)

**No Azure Portal:**

```
1. hotelaria-app → Visão geral
2. Clicar: "Baixar perfil de publicação"
3. Abrir: hotelaria-app-xxxxx.PublishSettings
4. Copiar: TODO o conteúdo XML (Ctrl+A, Ctrl+C)
```

**No GitHub:**

```
1. Ir para: https://github.com/avilaops/hotelaria/settings/secrets/actions
2. New repository secret
3. Name: AZURE_WEBAPP_PUBLISH_PROFILE
4. Secret: [Colar XML]
5. Add secret
```

---

### 2️⃣ Commit e Push (CodeQL)

**No terminal:**

```bash
# Verificar status
git status

# Adicionar novos arquivos
git add .github/workflows/codeql.yml
git add .github/codeql/codeql-config.yml
git add docs/CODEQL-SECURITY.md
git add docs/AZURE-PUBLISH-PROFILE-GUIA.md
git add docs/AZURE-QUICK-FIX.md
git add README.md

# Commit
git commit -m "🔍 feat: Adicionar CodeQL Security Analysis

- Workflow de análise de segurança
- Suporte para C#, JavaScript e Actions
- Análise automática em push/PR
- Agendamento semanal (sábados 23:32 UTC)
- Documentação completa

Refs #security"

# Push
git push origin main
```

---

### 3️⃣ Verificar Execução

#### GitHub Actions

```
1. Ir para: https://github.com/avilaops/hotelaria/actions
2. Ver workflows:
   - ✅ Build & Deploy (.NET)
   - 🔍 CodeQL Security Analysis
3. Aguardar conclusão (~5-10 min)
```

#### Security Tab

```
1. Ir para: https://github.com/avilaops/hotelaria/security
2. Code scanning → Ver alertas
3. Analisar vulnerabilidades encontradas
```

---

## 🎯 Resultado Esperado

### Após Commit e Push

```
╔════════════════════════════════════════╗
║  GITHUB ACTIONS                       ║
╠════════════════════════════════════════╣
║  ✅ Build & Deploy (.NET)            ║
║     └─ Build: ✅                      ║
║     └─ Test: ✅                       ║
║     └─ Publish: ✅                    ║
║     └─ Deploy: ⏳ (aguardando secret) ║
║                                        ║
║  🔍 CodeQL Security Analysis          ║
║     └─ C#: 🔄 Running...              ║
║     └─ JavaScript: 🔄 Running...      ║
║     └─ Actions: 🔄 Running...         ║
╚════════════════════════════════════════╝
```

### Após Adicionar Publish Profile Secret

```
╔════════════════════════════════════════╗
║  AZURE DEPLOY                         ║
╠════════════════════════════════════════╣
║  ✅ Build successful                  ║
║  ✅ Artifact uploaded                 ║
║  ✅ Deploy to Azure                   ║
║  ✅ Health check passed               ║
╠════════════════════════════════════════╣
║  🌐 App: hotelaria-app.azurewebsites.net║
╚════════════════════════════════════════╝
```

### Após CodeQL Analysis

```
╔════════════════════════════════════════╗
║  SECURITY ANALYSIS                    ║
╠════════════════════════════════════════╣
║  Languages: 3 (C#, JS, Actions)       ║
║  Files: 50+                           ║
║  Queries: 200+                        ║
║  Vulnerabilities: X found             ║
╠════════════════════════════════════════╣
║  View: Security → Code scanning       ║
╚════════════════════════════════════════╝
```

---

## 📚 Documentação Criada

### Azure Deploy
- 📄 `docs/AZURE-PUBLISH-PROFILE-GUIA.md` - Guia completo
- 📄 `docs/AZURE-QUICK-FIX.md` - Solução rápida (3 passos)
- 📄 `docs/AZURE-SERVICE-PRINCIPAL.md` - Service Principal (alternativa)

### CodeQL Security
- 📄 `docs/CODEQL-SECURITY.md` - Documentação completa
- 📄 `.github/workflows/codeql.yml` - Workflow
- 📄 `.github/codeql/codeql-config.yml` - Configuração

---

## 🔗 Links Importantes

### GitHub
```
Repository: https://github.com/avilaops/hotelaria
Actions: https://github.com/avilaops/hotelaria/actions
Security: https://github.com/avilaops/hotelaria/security
Secrets: https://github.com/avilaops/hotelaria/settings/secrets/actions
```

### Azure
```
Portal: https://portal.azure.com
App: hotelaria-app
Configuração: Settings → Configuration → General settings
```

---

## ✅ Checklist Final

### Deploy Azure
- [x] Basic Auth habilitado
- [ ] Publish Profile baixado
- [ ] Secret `AZURE_WEBAPP_PUBLISH_PROFILE` adicionado
- [ ] Push para testar deploy
- [ ] App funcionando em hotelaria-app.azurewebsites.net

### CodeQL
- [x] Workflow criado
- [x] Configuração criada
- [x] Documentação criada
- [x] Badge adicionado
- [ ] Commit e push
- [ ] Primeira análise executada
- [ ] Alertas revisados

---

## 🚨 Comandos Úteis

### Git

```bash
# Ver status
git status

# Adicionar tudo
git add .

# Commit com mensagem
git commit -m "🔍 feat: Adicionar CodeQL + Azure docs"

# Push
git push origin main

# Ver histórico
git log --oneline -10

# Ver diferenças
git diff
```

### GitHub CLI (opcional)

```bash
# Ver workflows
gh workflow list

# Ver execuções
gh run list

# Ver logs
gh run view

# Ver secrets
gh secret list
```

---

## 📞 Precisa de Ajuda?

### Leia os Guias

```bash
# Deploy Azure (completo)
cat docs/AZURE-PUBLISH-PROFILE-GUIA.md

# Deploy Azure (rápido)
cat docs/AZURE-QUICK-FIX.md

# CodeQL Security
cat docs/CODEQL-SECURITY.md
```

### Verificar Logs

```bash
# GitHub Actions
https://github.com/avilaops/hotelaria/actions

# Azure Portal
https://portal.azure.com → hotelaria-app → Log stream
```

---

## 🎉 Conclusão

Após seguir todos os passos:

```
✅ CodeQL configurado e analisando código
✅ Azure Deploy pronto (após adicionar secret)
✅ CI/CD completo funcionando
✅ Segurança automatizada
✅ Documentação completa
```

---

**Versão:** v2.6.4  
**Data:** 08/01/2026  
**Status:** ⏳ Aguardando commit e secret

---

**🚀 Próximo passo: Commit e Push!**

*Ávila Inc. - Desenvolvido com ❤️ em Portugal*

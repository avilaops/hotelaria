# 🚨 Application Error - Solução Rápida

**Erro:** `Application Error` no https://hotelaria.avila.inc

---

## 🎯 Solução em 2 Passos

### Passo 1: Diagnóstico

```powershell
.\diagnose-azure-app.ps1
```

**O que faz:**
- ✅ Verifica se app existe
- ✅ Checa estado da aplicação
- ✅ Desabilita Azure AD (se habilitado)
- ✅ Verifica variáveis de ambiente
- ✅ Mostra logs recentes
- ✅ Reinicia a aplicação

---

### Passo 2: Correção Completa (se ainda houver erro)

```powershell
.\fix-azure-complete.ps1
```

**O que faz:**
- 🔧 Desabilita Azure AD Authentication
- 🔧 Configura variáveis essenciais (`PORT`, `ASPNETCORE_URLS`, etc.)
- 🔧 Define runtime .NET 9
- 🔧 Habilita logs detalhados
- 🔧 Reinicia e testa endpoint
- 🔧 Mostra logs em tempo real

---

## 📋 Checklist de Execução

- [ ] **Pré-requisito:** Azure CLI instalado
  ```powershell
  az --version
  # Se não instalado: https://aka.ms/installazurecliwindows
  ```

- [ ] **Login no Azure**
  ```powershell
  az login
  ```

- [ ] **Executar diagnóstico**
  ```powershell
  .\diagnose-azure-app.ps1
  ```

- [ ] **Se erro persistir, executar correção**
  ```powershell
  .\fix-azure-complete.ps1
  ```

- [ ] **Testar acesso**
  ```
  https://hotelaria.avila.inc
  Usuário: admin
  Senha: admin123
  ```

---

## 🔍 Causas Comuns do "Application Error"

### 1. **Azure AD Authentication Habilitado** ⚠️
**Sintoma:** Redirecionamento para `login.microsoftonline.com`

**Solução:**
```powershell
az webapp auth update `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --enabled false `
  --action AllowAnonymous
```

---

### 2. **Porta Incorreta** ⚠️
**Sintoma:** App não responde ou timeout

**Solução:** Configurar variáveis de porta
```powershell
az webapp config appsettings set `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --settings `
    PORT=8080 `
    WEBSITES_PORT=8080 `
    ASPNETCORE_URLS=http://+:8080
```

---

### 3. **Runtime Incorreto** ⚠️
**Sintoma:** "The specified framework 'Microsoft.NETCore.App' was not found"

**Solução:** Definir .NET 9
```powershell
az webapp config set `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --linux-fx-version "DOTNETCORE|9.0"
```

---

### 4. **Deploy Falhou** ⚠️
**Sintoma:** Pacote corrompido ou incompleto

**Solução:** Re-deploy via GitHub Actions
```bash
git commit --allow-empty -m "🔧 fix: Re-deploy to Azure"
git push origin main
```

---

### 5. **Variáveis de Ambiente Faltando** ⚠️
**Sintoma:** App inicia mas falha ao acessar recursos

**Solução:** Configurar variáveis essenciais
```powershell
az webapp config appsettings set `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --settings `
    ASPNETCORE_ENVIRONMENT=Production `
    WEBSITE_RUN_FROM_PACKAGE=1 `
    SCM_DO_BUILD_DURING_DEPLOYMENT=false
```

---

## 🛠️ Comandos Úteis

### Ver logs em tempo real
```powershell
az webapp log tail --name hotelaria-app --resource-group hotelaria-rg
```

### Reiniciar app
```powershell
az webapp restart --name hotelaria-app --resource-group hotelaria-rg
```

### Ver estado do app
```powershell
az webapp show `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --query state
```

### Abrir Kudu (diagnóstico avançado)
```
https://hotelaria-app.scm.azurewebsites.net
```

### Ver variáveis configuradas
```powershell
az webapp config appsettings list `
  --name hotelaria-app `
  --resource-group hotelaria-rg
```

---

## 📊 Fluxo de Resolução

```
┌─────────────────────────────────┐
│  "Application Error"            │
└───────────┬─────────────────────┘
            │
            ▼
┌─────────────────────────────────┐
│  1. Executar diagnóstico        │
│     .\diagnose-azure-app.ps1    │
└───────────┬─────────────────────┘
            │
            ▼
      ┌─────┴─────┐
      │  Erro     │
      │ resolvido?│
      └─────┬─────┘
            │
     ┌──────┴──────┐
     │             │
    Sim           Não
     │             │
     ▼             ▼
┌─────────┐  ┌──────────────────────┐
│  Testar │  │ 2. Executar correção │
│   app   │  │  .\fix-azure-complete│
└─────────┘  └──────────┬───────────┘
                        │
                        ▼
                  ┌─────────────┐
                  │ Ainda erro? │
                  └──────┬──────┘
                         │
                    ┌────┴────┐
                    │         │
                   Sim       Não
                    │         │
                    ▼         ▼
            ┌───────────┐  ┌────────┐
            │ Re-deploy │  │ Testar │
            │  GitHub   │  │  app   │
            │  Actions  │  └────────┘
            └───────────┘
```

---

## 🎯 Resultado Esperado

Após executar os scripts:

```
╔════════════════════════════════════════╗
║  APP FUNCIONANDO                      ║
╠════════════════════════════════════════╣
║  Status:              ✅ Running       ║
║  URL:                 ✅ Respondendo   ║
║  Authentication:      ✅ Local         ║
║  Logs:                ✅ Sem erros     ║
╠════════════════════════════════════════╣
║  Acesso: https://hotelaria.avila.inc  ║
╚════════════════════════════════════════╝
```

---

## 📞 Se Nada Funcionar

1. **Verificar GitHub Actions**
   - https://github.com/avilaops/hotelaria/actions
   - Ver se último deploy teve sucesso

2. **Verificar Kudu**
   - https://hotelaria-app.scm.azurewebsites.net
   - Processos → Ver se app está rodando

3. **Fazer deploy manual**
   ```bash
   git commit --allow-empty -m "🔧 fix: Manual redeploy"
   git push origin main
   ```

4. **Deletar e recriar app** (último recurso)
   ```powershell
   # CUIDADO: Isso apaga o app!
   az webapp delete --name hotelaria-app --resource-group hotelaria-rg
   # Depois recriar via GitHub Actions
   ```

---

## ✅ Próximos Passos (após resolver)

1. [ ] Atualizar GitHub Secrets (se usar Publish Profile)
2. [ ] Migrar para Service Principal (mais seguro)
3. [ ] Configurar custom domain corretamente
4. [ ] Habilitar SSL/HTTPS
5. [ ] Configurar Application Insights (monitoramento)

---

**Data:** 09/01/2026  
**Status:** 🚨 Guia de Resolução de "Application Error"

---

**Ávila Inc. - Troubleshooting Guide**

# 🚨 HTTP 503 Service Unavailable - Guia de Resolução

**Data:** 09/01/2026  
**App:** hotelaria-app  
**Erro:** HTTP 503 (Service Unavailable)

---

## 🔴 Problema Detectado

```
HTTP Status: 503
SubStatus: 0
Erros: 2
Descrição: Servidor indisponível

Failed Requests:
- GET / → 503
- POST /_blazor/negotiate → 503

Período: 2026-01-09 12:35 - 12:55
```

---

## 💡 Causas Comuns de HTTP 503

### 1. **App Reiniciando** ⏳
- Após deploy
- Após mudança de configuração
- Azure forçou restart

### 2. **Recursos Esgotados** 💻
- CPU > 90%
- Memory > 90%
- Threads esgotados

### 3. **App Crash** 💥
- Exceção não tratada
- Erro de startup
- Dependência faltando

### 4. **Cold Start** ❄️
- App desligado por inatividade (Always On = OFF)
- Primeira requisição após idle

### 5. **Timeout de Inicialização** ⏱️
- App leva muito tempo para iniciar
- Dependências externas lentas

---

## 🔍 Diagnóstico Rápido

### Opção 1: Script Automático (Recomendado)

```powershell
# Executar diagnóstico completo
.\diagnose-http-503.ps1
```

**O que verifica:**
- ✅ Estado do App Service
- ✅ Uso de CPU/Memory
- ✅ Contagem de HTTP 5xx
- ✅ Logs recentes
- ✅ Teste de conectividade
- ✅ Configurações críticas

---

### Opção 2: Diagnóstico Manual

#### 1. **Verificar Estado do App**
```powershell
az webapp show `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --query "{state:state,availabilityState:availabilityState}"
```

**Esperado:**
```json
{
  "state": "Running",
  "availabilityState": "Normal"
}
```

---

#### 2. **Ver Logs em Tempo Real**
```powershell
az webapp log tail `
  --name hotelaria-app `
  --resource-group hotelaria-rg
```

**Procure por:**
- ❌ `error`, `exception`, `fail`
- ⚠️ `timeout`, `warn`
- ✅ `Application started`, `listening`

---

#### 3. **Verificar Métricas de Recurso**

**Via Azure Portal:**
```
hotelaria-app → Monitoring → Metrics

Métricas importantes:
- CPU Percentage
- Memory Percentage
- Requests
- Http 5xx
- Average Response Time
```

**Via CLI:**
```powershell
# CPU
az monitor metrics list `
  --resource "/subscriptions/.../hotelaria-app" `
  --metric "CpuPercentage" `
  --aggregation Average

# Memory
az monitor metrics list `
  --resource "/subscriptions/.../hotelaria-app" `
  --metric "MemoryPercentage" `
  --aggregation Average
```

---

## ✅ Soluções por Causa

### Solução 1: App Reiniciando ⏳

**Se logs mostram:**
```
Application is starting
Waiting for app to be ready
```

**Ação:** Aguardar 1-2 minutos

**Verificação:**
```powershell
# Testar após 2 minutos
Invoke-WebRequest -Uri "https://hotelaria-app.azurewebsites.net"
```

---

### Solução 2: Recursos Esgotados 💻

**Se CPU/Memory > 80%:**

#### Opção A: Restart Rápido
```powershell
az webapp restart `
  --name hotelaria-app `
  --resource-group hotelaria-rg
```

#### Opção B: Upgrade do Plano (se persistir)
```powershell
# De Basic B1 para B2
az appservice plan update `
  --name hotelaria-plan `
  --resource-group hotelaria-rg `
  --sku B2
```

**Comparação de Planos:**

| Plano | CPU | RAM | Preço/mês |
|-------|-----|-----|-----------|
| **B1** | 1 core | 1.75 GB | ~$55 |
| **B2** | 2 cores | 3.5 GB | ~$110 |
| **S1** | 1 core | 1.75 GB | ~$70 (+ features) |

---

### Solução 3: App Crash 💥

**Se logs mostram exceções:**

#### 1. Ver Stack Trace Completo
```powershell
# Baixar logs
az webapp log download `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --log-file logs.zip

# Extrair e ler
Expand-Archive logs.zip -DestinationPath ./logs
Get-Content ./logs/*.log -Tail 100
```

#### 2. Verificar Dependências
```
Kudu Console: https://hotelaria-app.scm.azurewebsites.net

Verificar:
- /home/site/wwwroot/Hotelaria.dll existe?
- Todas as dependências estão presentes?
- .NET 9 está instalado?
```

#### 3. Re-deploy
```bash
# Forçar novo deploy via GitHub Actions
git commit --allow-empty -m "fix: Redeploy após HTTP 503"
git push origin main
```

---

### Solução 4: Cold Start ❄️

**Se Always On = OFF:**

#### Habilitar Always On
```powershell
az webapp config set `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --always-on true
```

**Ou via Portal:**
```
hotelaria-app → Configuration → General settings
→ Always On: ON
→ Save
```

**Benefícios:**
- ✅ Sem cold starts
- ✅ Resposta instantânea
- ✅ Conexões mantidas

**Custo:**
- ⚠️ Plano Basic ou superior necessário

---

### Solução 5: Timeout de Inicialização ⏱️

**Se app demora muito para iniciar:**

#### Aumentar Timeout
```powershell
az webapp config appsettings set `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --settings WEBSITE_TIME_ZONE="E. South America Standard Time" `
             WEBSITES_CONTAINER_START_TIME_LIMIT=1800
```

**Otimizar Startup:**

1. **Remover dependências pesadas do startup**
2. **Usar lazy loading**
3. **Cachear configurações**

---

## 🔧 Correções Imediatas

### Fix 1: Restart Forçado
```powershell
# Stop
az webapp stop --name hotelaria-app --resource-group hotelaria-rg

# Wait
Start-Sleep -Seconds 10

# Start
az webapp start --name hotelaria-app --resource-group hotelaria-rg

# Restart
az webapp restart --name hotelaria-app --resource-group hotelaria-rg
```

---

### Fix 2: Limpar Cache
```powershell
# Via Kudu
Invoke-WebRequest `
  -Uri "https://hotelaria-app.scm.azurewebsites.net/api/command" `
  -Method POST `
  -Headers @{"Authorization"="Basic $base64Creds"} `
  -Body '{"command":"rm -rf /home/site/wwwroot/.nuget","dir":"/"}'
```

---

### Fix 3: Redeployment Completo
```bash
# Forçar build limpo no GitHub Actions
git commit --allow-empty -m "fix: Clean redeploy"
git push origin main
```

---

## 📊 Checklist de Verificação

Após aplicar correções:

- [ ] App Status = "Running"?
- [ ] Availability State = "Normal"?
- [ ] CPU < 80%?
- [ ] Memory < 80%?
- [ ] HTTP 5xx = 0?
- [ ] Logs sem erros?
- [ ] Site acessível?
- [ ] Login funcionando?

---

## 🎯 Fluxo de Resolução

```
┌─────────────────────────────────┐
│  HTTP 503 Detectado             │
└───────────┬─────────────────────┘
            │
            ▼
┌─────────────────────────────────┐
│  1. Executar diagnóstico        │
│     .\diagnose-http-503.ps1     │
└───────────┬─────────────────────┘
            │
            ▼
      ┌─────┴─────┐
      │  Causa    │
      │identificada│
      └─────┬─────┘
            │
     ┌──────┴──────────────┐
     │                     │
  Recursos            App Crash
  Esgotados               │
     │                     │
     ▼                     ▼
 Restart           Re-deploy
     │                     │
     └──────┬──────────────┘
            │
            ▼
    ┌───────────────┐
    │ Aguardar 2min │
    └───────┬───────┘
            │
            ▼
      ┌─────────┐
      │ Testar  │
      └─────┬───┘
            │
       ┌────┴────┐
       │         │
      OK      Ainda 503
       │         │
       ▼         ▼
   ┌────────┐ ┌─────────────┐
   │ Feito! │ │ Upgrade     │
   └────────┘ │ Plano ou    │
              │ Support     │
              └─────────────┘
```

---

## 🚨 Se Nada Funcionar

### Opção 1: Criar Novo Slot
```powershell
# Criar slot de staging
az webapp deployment slot create `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --slot staging

# Deploy para staging
# Testar
# Swap com produção
az webapp deployment slot swap `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --slot staging
```

---

### Opção 2: Abrir Ticket de Suporte Azure
```
Azure Portal → Help + Support → New Support Request

Severity: B (Production down)
Problem Type: Technical
Service: App Service
```

---

### Opção 3: Rollback
```bash
# Reverter para commit anterior que funcionava
git log --oneline -10
git revert <commit-hash>
git push origin main
```

---

## 📝 Prevenção Futura

### 1. **Application Insights**
```powershell
# Habilitar monitoring avançado
az monitor app-insights component create `
  --app hotelaria-insights `
  --location brazilsouth `
  --resource-group hotelaria-rg `
  --application-type web
```

### 2. **Health Checks**
Adicionar endpoint de health:

```csharp
// Program.cs
app.MapHealthChecks("/health");
```

Configurar no Azure:
```
Configuration → Health check
Path: /health
```

### 3. **Auto-scaling** (Plano S1+)
```
hotelaria-plan → Scale out → Rules
- CPU > 70% → +1 instância
- CPU < 30% → -1 instância
```

---

**Data:** 09/01/2026  
**Status:** 🚨 Guia de Resolução HTTP 503

---

**Ávila Inc. - Troubleshooting Guide**

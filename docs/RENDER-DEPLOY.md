# 🚀 Deploy no Render - Guia Completo

## 📋 Visão Geral

Este guia mostra como fazer deploy do **Hotelaria v2.5.1** no Render usando Docker.

---

## ✅ Pré-requisitos

1. **Conta no Render**
   - Criar conta em: https://render.com
   - Plan Free disponível

2. **Repositório GitHub**
   - Projeto deve estar no GitHub
   - Repositório: https://github.com/avilaops/hotelaria

3. **Arquivos Necessários**
   - ✅ `Dockerfile` (já criado)
   - ✅ `render.yaml` (já criado)
   - ✅ `.dockerignore` (recomendado)

---

## 🎯 Passo a Passo - Deploy Manual

### 1️⃣ Acessar Render Dashboard

1. Fazer login em: https://dashboard.render.com
2. Clicar em **"New +"**
3. Selecionar **"Web Service"**

### 2️⃣ Conectar Repositório

1. **Connect GitHub**
   - Autorizar Render a acessar GitHub
   - Selecionar repositório: `avilaops/hotelaria`

2. **Ou usar URL**
   ```
   https://github.com/avilaops/hotelaria
   ```

### 3️⃣ Configurar Web Service

#### Configurações Básicas
```
Name:            hotelaria
Language:        Docker
Branch:          main
Root Directory:  (deixar vazio)
```

#### Configurações do Docker
```
Dockerfile Path: ./Dockerfile
Docker Context:  .
Docker Command:  (deixar vazio - usa ENTRYPOINT)
```

#### Plan
```
Plan: Free
```

#### Region
```
Region: Oregon (US West) - Recomendado
Ou:     Frankfurt (Europe) - Mais próximo de Portugal
```

### 4️⃣ Variáveis de Ambiente

Adicionar as seguintes variáveis:

| Key | Value |
|-----|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ASPNETCORE_URLS` | `http://+:$PORT` |

**Nota:** A variável `$PORT` é fornecida automaticamente pelo Render.

### 5️⃣ Health Check

```
Health Check Path: /
```

### 6️⃣ Auto Deploy

```
☑️ Auto-Deploy: Yes
```

Isso fará deploy automaticamente quando você fizer push para `main`.

### 7️⃣ Criar Web Service

1. Clicar em **"Create Web Service"**
2. Aguardar o build (5-10 minutos)
3. Ver logs em tempo real

---

## 🔄 Deploy Automático com render.yaml

### Método Alternativo (Recomendado)

1. **Arquivo `render.yaml` já está criado** ✅

2. **No Render Dashboard:**
   - Clicar em **"New +"**
   - Selecionar **"Blueprint"**
   - Conectar repositório GitHub
   - Render detectará `render.yaml` automaticamente
   - Clicar em **"Apply"**

3. **Benefícios:**
   - Configuração como código
   - Fácil replicação
   - Versionamento

---

## 📝 Configuração Detalhada

### Dockerfile

```dockerfile
# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Hotelaria.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:$PORT
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE $PORT
ENTRYPOINT ["dotnet", "Hotelaria.dll"]
```

**Características:**
- ✅ Multi-stage build (reduz tamanho)
- ✅ Usa porta dinâmica `$PORT`
- ✅ Otimizado para produção
- ✅ Health check incluído

### render.yaml

```yaml
services:
  - type: web
    name: hotelaria
    runtime: docker
    repo: https://github.com/avilaops/hotelaria
    branch: main
    dockerfilePath: ./Dockerfile
    dockerContext: .
    plan: free
    region: oregon
    healthCheckPath: /
    envVars:
      - key: ASPNETCORE_ENVIRONMENT
        value: Production
      - key: ASPNETCORE_URLS
        value: http://+:$PORT
    autoDeploy: true
```

---

## 🌐 URLs e Acesso

### URL do Deploy

Após o deploy, você receberá uma URL como:

```
https://hotelaria.onrender.com
```

### URL Customizada (Opcional)

1. No Dashboard do Render
2. Settings → Custom Domain
3. Adicionar domínio próprio
4. Configurar DNS

---

## 🔍 Monitoramento

### Logs

1. **Acessar Logs:**
   - Dashboard → Seu serviço
   - Tab "Logs"
   - Ver em tempo real

2. **Tipos de Logs:**
   ```
   Build Logs:   Durante compilação
   Deploy Logs:  Durante deploy
   Runtime Logs: Aplicação rodando
   ```

### Metrics

```
CPU Usage
Memory Usage
Request Count
Response Time
```

### Health Checks

```
Endpoint:  /
Interval:  30 segundos
Timeout:   3 segundos
Retries:   3
```

---

## ⚠️ Limitações do Plan Free

### Recursos
```
RAM:           512 MB
CPU:           0.1 CPU
Build Time:    Ilimitado
Bandwidth:     100 GB/mês
```

### Sleep Mode
```
Após 15 min de inatividade:
- Serviço entra em sleep
- Primeira request: ~30s para acordar
- Solução: Usar plano pago ou keep-alive ping
```

### Limitações
```
❌ Não persiste dados (in-memory reset)
❌ Não suporta WebSockets permanentes
❌ IP pode mudar
✅ SSL/TLS automático
✅ CDN global
```

---

## 🔧 Troubleshooting

### Erro: Build Failed

**Problema:** Falha na compilação

**Solução:**
```bash
# Verificar localmente
docker build -t hotelaria .
docker run -p 5000:5000 -e PORT=5000 hotelaria
```

### Erro: Application Failed to Start

**Problema:** App não inicia

**Verificar:**
1. Variável `$PORT` está configurada?
2. `ASPNETCORE_URLS` está correto?
3. Logs mostram erros?

**Solução:**
```bash
# No Dockerfile, garantir:
ENV ASPNETCORE_URLS=http://+:$PORT
```

### Erro: Health Check Failing

**Problema:** Health check falhando

**Verificar:**
1. Rota `/` está acessível?
2. App está ouvindo na porta correta?

**Solução:**
```yaml
# render.yaml
healthCheckPath: /
```

### Erro: Out of Memory

**Problema:** 512 MB não é suficiente

**Solução:**
1. Otimizar aplicação
2. Ou fazer upgrade de plan

---

## 🚀 Comandos Úteis

### Build Local
```bash
# Build da imagem
docker build -t hotelaria .

# Rodar localmente
docker run -p 5000:5000 -e PORT=5000 hotelaria

# Ver logs
docker logs <container-id>
```

### Deploy Manual
```bash
# Fazer push para GitHub
git add .
git commit -m "feat: atualização"
git push origin main

# Render faz deploy automaticamente
```

### Rollback
```bash
# No Render Dashboard:
1. Ir para "Events"
2. Encontrar deploy anterior
3. Clicar em "Redeploy"
```

---

## 📊 Checklist de Deploy

### Antes do Deploy
- [ ] Código funciona localmente
- [ ] Docker build sem erros
- [ ] Variáveis de ambiente definidas
- [ ] Health check testado
- [ ] Documentação atualizada

### Durante o Deploy
- [ ] Build iniciou corretamente
- [ ] Logs sem erros críticos
- [ ] Health check passou
- [ ] URL acessível

### Depois do Deploy
- [ ] Aplicação carrega
- [ ] Login funciona
- [ ] Credenciais de teste OK
- [ ] Páginas principais funcionam
- [ ] Performance aceitável

---

## 🔐 Segurança

### HTTPS

✅ **Automático!** Render fornece SSL/TLS grátis

```
http://hotelaria.onrender.com  → redireciona para
https://hotelaria.onrender.com ← SSL automático
```

### Credenciais

**⚠️ IMPORTANTE:** Trocar senhas padrão em produção!

```csharp
// AuthService.cs - linha ~20
// Alterar senhas dos usuários de exemplo
AdicionarUsuario(new Usuario
{
    Username = "admin",
    SenhaHash = HashSenha("NOVA_SENHA_FORTE"), // ← Mudar aqui
    ...
});
```

### Variáveis Sensíveis

Se adicionar banco de dados ou APIs:
```
# Usar Environment Variables no Render
DATABASE_URL
API_KEY
JWT_SECRET
```

---

## 🎯 Melhorias Futuras

### Persistência de Dados

**Problema:** Dados resetam a cada deploy

**Solução:**
1. Adicionar PostgreSQL (Render oferece grátis)
2. Ou usar Redis para cache
3. Ou integrar com Supabase

### Keep-Alive

**Problema:** Sleep após 15 min

**Solução:**
```javascript
// Adicionar cron job ou ping service
// UptimeRobot (grátis)
// https://uptimerobot.com
```

### Monitoramento

**Adicionar:**
- Application Insights
- New Relic
- Sentry para errors

---

## 📚 Recursos Úteis

### Documentação
- [Render Docs](https://render.com/docs)
- [Docker Docs](https://docs.docker.com)
- [ASP.NET Core Docs](https://docs.microsoft.com/aspnet/core)

### Suporte
- [Render Community](https://community.render.com)
- [GitHub Issues](https://github.com/avilaops/hotelaria/issues)

### Status
- [Render Status](https://status.render.com)

---

## 🎉 Resultado Esperado

Após seguir este guia, você terá:

✅ **URL Pública**
```
https://hotelaria.onrender.com
```

✅ **SSL Automático**
```
🔒 Certificado válido
```

✅ **Deploy Automático**
```
Push → Build → Deploy
```

✅ **Sistema Funcionando**
```
Login
Dashboard
Gestão de Reservas
Relatórios
etc.
```

---

## 📞 Suporte

Problemas no deploy?

1. **Verificar Logs** no Render Dashboard
2. **Testar Localmente** com Docker
3. **Abrir Issue** no GitHub
4. **Contatar** avilaops

---

## 🏆 Status

```
╔═══════════════════════════════════╗
║  DEPLOY NO RENDER - CONFIGURADO  ║
╠═══════════════════════════════════╣
║  Dockerfile:     ✅ Otimizado    ║
║  render.yaml:    ✅ Criado       ║
║  Documentação:   ✅ Completa     ║
║  Pronto para:    ✅ Deploy       ║
╚═══════════════════════════════════╝
```

---

**🚀 Pronto para fazer deploy!**

*Siga os passos acima e em 10 minutos seu sistema estará online!*

---

**Versão:** 2.5.1  
**Data:** 07/01/2026  
**Platform:** Render  
**Status:** ✅ Ready to Deploy

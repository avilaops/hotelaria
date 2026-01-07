# 🚀 Guia Rápido - Deploy Render (5 Minutos)

## ✅ Passo 1: Preencher Formulário

### Na tela que você mostrou, preencha:

```
┌─────────────────────────────────────────┐
│ Source Code                             │
│ ✅ avilaops / hotelaria (já detectado) │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│ Name                                    │
│ hotelaria                               │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│ Language                                │
│ Docker ← (já selecionado)               │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│ Branch                                  │
│ main ← (já selecionado)                 │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│ Region                                  │
│ Oregon (US West) ← (recomendado)        │
│ ou Frankfurt (EU Central)               │
└─────────────────────────────────────────┘

┌─────────────────────────────────────────┐
│ Root Directory                          │
│ (deixar vazio)                          │
└─────────────────────────────────────────┘
```

---

## ✅ Passo 2: Scroll Página Baixo

### Adicionar Environment Variables

Role a página para baixo até encontrar **"Environment Variables"**

### Clicar em "+ Add Environment Variable"

#### Variável 1
```
Key:   ASPNETCORE_ENVIRONMENT
Value: Production
```
Clicar em **[Add]**

#### Variável 2
```
Key:   ASPNETCORE_URLS
Value: http://+:$PORT
```
Clicar em **[Add]**

**⚠️ IMPORTANTE:** Escreva exatamente `$PORT` com cifrão!

---

## ✅ Passo 3: Configurar Health Check

```
┌─────────────────────────────────────────┐
│ Health Check Path                       │
│ /                                       │
└─────────────────────────────────────────┘
```

---

## ✅ Passo 4: Habilitar Auto-Deploy

```
☑️ Auto-Deploy: Yes
```

---

## ✅ Passo 5: Selecionar Plan

```
Plan: Free
```

---

## ✅ Passo 6: Criar Web Service

Clicar no botão verde:

```
┌─────────────────────────────────┐
│   [ Create Web Service ]        │
└─────────────────────────────────┘
```

---

## ⏱️ Aguardar Build (5-10 min)

### Você verá tela de logs:

```
Building...
├── Cloning repository
├── Building Docker image
│   ├── [1/12] FROM mcr.microsoft.com/dotnet/sdk:8.0
│   ├── [2/12] WORKDIR /src
│   ├── [3/12] COPY Hotelaria.csproj
│   ├── [4/12] RUN dotnet restore
│   ├── [5/12] COPY . .
│   ├── [6/12] RUN dotnet build
│   ├── [7/12] RUN dotnet publish
│   ├── [8/12] FROM mcr.microsoft.com/dotnet/aspnet:8.0
│   ├── [9/12] WORKDIR /app
│   ├── [10/12] COPY --from=build
│   ├── [11/12] ENV ASPNETCORE_URLS
│   └── [12/12] ENTRYPOINT ["dotnet", "Hotelaria.dll"]
└── Build complete!

Deploying...
├── Starting container
├── Running health checks
└── ✅ Your service is live!
```

---

## 🎉 Pronto!

### URL Gerada

Após completar, você terá uma URL como:

```
https://hotelaria.onrender.com
```

### Testar

1. **Acessar URL**
2. **Será redirecionado para `/login`**
3. **Fazer login:**
   - Usuário: `admin`
   - Senha: `admin123`

---

## 🔍 Se Algo Der Errado

### Ver Logs

```
Dashboard → Seu serviço → Logs
```

### Erros Comuns

#### Build Failed
```
Solução: Verificar se Dockerfile existe no repo
```

#### Port Error
```
Solução: Verificar se ASPNETCORE_URLS = http://+:$PORT
```

#### 500 Error
```
Solução: Ver logs para detalhes do erro
```

---

## 📋 Checklist Final

Antes de clicar "Create Web Service":

- [ ] ✅ Name: `hotelaria`
- [ ] ✅ Language: `Docker`
- [ ] ✅ Branch: `main`
- [ ] ✅ Region escolhida
- [ ] ✅ Root Directory vazio
- [ ] ✅ `ASPNETCORE_ENVIRONMENT` = `Production`
- [ ] ✅ `ASPNETCORE_URLS` = `http://+:$PORT`
- [ ] ✅ Health Check Path: `/`
- [ ] ✅ Auto-Deploy: Yes
- [ ] ✅ Plan: Free

---

## 🎯 Resumo Visual

```
┌─────────────────────────────────────┐
│  1. Preencher Name                  │
│  2. Confirmar Language = Docker     │
│  3. Confirmar Branch = main         │
│  4. Escolher Region                 │
│  5. Root Directory = vazio          │
│  ↓                                  │
│  6. Adicionar ENV vars:             │
│     - ASPNETCORE_ENVIRONMENT        │
│     - ASPNETCORE_URLS               │
│  ↓                                  │
│  7. Health Check = /                │
│  8. Auto-Deploy = Yes               │
│  9. Plan = Free                     │
│  ↓                                  │
│  10. [Create Web Service]           │
│  ↓                                  │
│  11. Aguardar build (~10 min)       │
│  ↓                                  │
│  12. ✅ Aplicação Live!             │
└─────────────────────────────────────┘
```

---

## 💡 Dicas

### Durante Build
- ✅ Pode fechar aba - continua em background
- ✅ Receberá email quando finalizar
- ✅ Pode ver progresso em Dashboard

### Depois do Deploy
- ✅ Sleep após 15 min inatividade
- ✅ Primeira request após sleep = ~30s
- ✅ SSL/TLS automático
- ✅ CDN global incluso

### Se Quiser Atualizar
```
1. Fazer push no GitHub
2. Render detecta automaticamente
3. Faz build e deploy sozinho
```

---

## 🚀 Pode Começar!

**Tudo pronto para criar o Web Service!**

Tempo estimado: **5-10 minutos**

---

**✨ Boa sorte com o deploy!**

*Qualquer problema, veja os logs!*

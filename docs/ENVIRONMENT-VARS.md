# ⚙️ Variáveis de Ambiente - GitHub e Render

## 🎯 Configuração no Render

### 📋 Variáveis Obrigatórias

Durante a criação do Web Service no Render, adicione estas variáveis:

#### 1. Variáveis do ASP.NET Core

| Key | Value | Descrição |
|-----|-------|-----------|
| `ASPNETCORE_ENVIRONMENT` | `Production` | Define ambiente de produção |
| `ASPNETCORE_URLS` | `http://+:$PORT` | Configura porta dinâmica |

#### 2. Variáveis Opcionais (Futuro)

Se você adicionar banco de dados ou outras integrações:

| Key | Exemplo | Descrição |
|-----|---------|-----------|
| `DATABASE_URL` | `postgresql://...` | String de conexão do DB |
| `REDIS_URL` | `redis://...` | URL do Redis (cache) |
| `JWT_SECRET` | `sua-chave-secreta-aqui` | Chave para tokens JWT |

---

## 🔧 Como Adicionar no Render

### Durante a Criação

1. **Na tela de criação do Web Service**
2. **Scroll até "Environment Variables"**
3. **Clicar em "Add Environment Variable"**

### Passo a Passo

#### Variável 1: ASPNETCORE_ENVIRONMENT
```
1. Clicar em "+ Add Environment Variable"
2. Key:   ASPNETCORE_ENVIRONMENT
3. Value: Production
4. Clicar em "Add"
```

#### Variável 2: ASPNETCORE_URLS
```
1. Clicar em "+ Add Environment Variable"
2. Key:   ASPNETCORE_URLS
3. Value: http://+:$PORT
4. Clicar em "Add"
```

**⚠️ IMPORTANTE:** Use exatamente `$PORT` (com cifrão) - Render substitui automaticamente.

---

## 🔐 Variáveis Secretas (GitHub Secrets)

Se você quiser guardar secrets no GitHub para CI/CD:

### No GitHub Repository

1. **Ir para Settings**
   ```
   Repository → Settings
   ```

2. **Secrets and Variables**
   ```
   Settings → Secrets and variables → Actions
   ```

3. **Adicionar Secrets**
   ```
   Clicar em "New repository secret"
   ```

### Secrets Recomendados

| Name | Description | Exemplo |
|------|-------------|---------|
| `RENDER_API_KEY` | API key do Render | `rnd_...` |
| `DOCKER_USERNAME` | Username DockerHub | `avilaops` |
| `DOCKER_TOKEN` | Token DockerHub | `dckr_pat_...` |

---

## 📝 Template de Variáveis

### Para Copiar e Colar no Render

```bash
# ASP.NET Core
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:$PORT

# Database (se adicionar no futuro)
# DATABASE_URL=postgresql://user:pass@host:5432/dbname

# Cache (se adicionar Redis)
# REDIS_URL=redis://default:pass@host:6379

# Secrets (se adicionar)
# JWT_SECRET=sua-chave-super-secreta-aqui
# ENCRYPTION_KEY=outra-chave-secreta
```

---

## 🎯 Configuração Completa no Render

### Tela de Environment Variables

Deve ficar assim:

```
┌────────────────────────────────────────────┐
│ Environment Variables                       │
├────────────────────────────────────────────┤
│                                            │
│ ASPNETCORE_ENVIRONMENT                     │
│ Production                          [Edit] │
│                                            │
│ ASPNETCORE_URLS                            │
│ http://+:$PORT                     [Edit] │
│                                            │
│ [+ Add Environment Variable]               │
└────────────────────────────────────────────┘
```

---

## 🔍 Verificar Variáveis

### Depois do Deploy

1. **No Render Dashboard**
   ```
   Seu serviço → Settings → Environment
   ```

2. **Ver Variáveis**
   ```
   Lista de todas as variáveis configuradas
   ```

3. **Editar Variáveis**
   ```
   Clicar em Edit → Modificar → Save Changes
   ```

**⚠️ Nota:** Após salvar mudanças, o serviço faz redeploy automático.

---

## 🧪 Testar Variáveis

### Verificar se Estão Funcionando

Após o deploy, você pode testar:

1. **Acessar aplicação**
   ```
   https://hotelaria.onrender.com
   ```

2. **Verificar resposta**
   - Se carregar corretamente = ✅ Variáveis OK
   - Se erro 500 = ❌ Verificar logs

3. **Ver logs**
   ```
   Dashboard → Logs
   Procurar por: "ASPNETCORE_ENVIRONMENT"
   ```

---

## 🚨 Erros Comuns

### Erro 1: Porta Incorreta

**Sintoma:**
```
Application failed to bind to $PORT
```

**Solução:**
```bash
# Verificar ASPNETCORE_URLS
ASPNETCORE_URLS=http://+:$PORT  # ✅ Correto
ASPNETCORE_URLS=http://+:5000   # ❌ Errado
```

### Erro 2: Ambiente Errado

**Sintoma:**
```
Development mode in production
```

**Solução:**
```bash
ASPNETCORE_ENVIRONMENT=Production  # ✅ Correto
ASPNETCORE_ENVIRONMENT=Development # ❌ Errado
```

### Erro 3: Variável Não Definida

**Sintoma:**
```
Variable $PORT is not defined
```

**Solução:**
- Render define `$PORT` automaticamente
- Não precisa criar manualmente
- Apenas referenciá-la em `ASPNETCORE_URLS`

---

## 📊 Variáveis Padrão do Render

O Render já fornece automaticamente:

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `PORT` | Porta HTTP | `10000` |
| `RENDER_SERVICE_ID` | ID do serviço | `srv-xxxxx` |
| `RENDER_SERVICE_NAME` | Nome do serviço | `hotelaria` |
| `RENDER_EXTERNAL_URL` | URL externa | `https://hotelaria.onrender.com` |
| `RENDER_REGION` | Região | `oregon` |

**Uso:**
```csharp
// No código C#
var port = Environment.GetEnvironmentVariable("PORT");
var serviceId = Environment.GetEnvironmentVariable("RENDER_SERVICE_ID");
```

---

## 🔄 Atualizar Variáveis

### Modificar Variáveis Existentes

1. **Render Dashboard**
   ```
   Serviço → Settings → Environment
   ```

2. **Clicar em Edit** ao lado da variável

3. **Modificar valor**

4. **Save Changes**

5. **Aguardar Redeploy** (automático)

### Adicionar Nova Variável

1. **Settings → Environment**

2. **Add Environment Variable**

3. **Preencher Key e Value**

4. **Save**

---

## 🎯 Checklist de Variáveis

Antes de fazer deploy:

- [ ] ✅ `ASPNETCORE_ENVIRONMENT` = `Production`
- [ ] ✅ `ASPNETCORE_URLS` = `http://+:$PORT`
- [ ] ⏳ Database URL (se aplicável)
- [ ] ⏳ Redis URL (se aplicável)
- [ ] ⏳ JWT Secret (se aplicável)
- [ ] ⏳ API Keys (se aplicável)

---

## 🔐 Boas Práticas

### Segurança

1. **Nunca commitar secrets**
   ```bash
   # .gitignore
   *.env
   appsettings.Production.json
   secrets.json
   ```

2. **Usar variáveis de ambiente**
   ```csharp
   // ✅ Correto
   var secret = Environment.GetEnvironmentVariable("JWT_SECRET");
   
   // ❌ Errado
   var secret = "minha-senha-123";
   ```

3. **Rotacionar secrets regularmente**
   - Trocar senhas a cada 90 dias
   - Trocar tokens comprometidos imediatamente

### Organização

1. **Nomear consistentemente**
   ```
   APP_NAME_FEATURE_TYPE
   Ex: HOTELARIA_DB_PASSWORD
   ```

2. **Documentar variáveis**
   ```
   # Lista todas as variáveis necessárias
   # em docs/ENVIRONMENT.md
   ```

3. **Versionar configuração**
   ```yaml
   # render.yaml
   envVars:
     - key: ASPNETCORE_ENVIRONMENT
       value: Production
   ```

---

## 🎉 Resultado Final

Após configurar tudo corretamente:

```
╔═══════════════════════════════════════╗
║  VARIÁVEIS DE AMBIENTE CONFIGURADAS  ║
╠═══════════════════════════════════════╣
║  ASPNETCORE_ENVIRONMENT:  ✅         ║
║  ASPNETCORE_URLS:         ✅         ║
║  $PORT (automático):      ✅         ║
╠═══════════════════════════════════════╣
║  Status: Pronto para Deploy          ║
╚═══════════════════════════════════════╝
```

---

## 📚 Referências

### Documentação
- [Render Environment Variables](https://render.com/docs/environment-variables)
- [ASP.NET Core Configuration](https://docs.microsoft.com/aspnet/core/fundamentals/configuration)
- [Docker Environment Variables](https://docs.docker.com/compose/environment-variables/)

### Exemplos
```bash
# Exemplo de uso no código
var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
var dbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
```

---

## ✅ Pronto!

Agora você pode:

1. ✅ Adicionar variáveis no Render
2. ✅ Configurar secrets no GitHub
3. ✅ Fazer deploy com confiança
4. ✅ Aplicação rodará corretamente

---

**🔧 Variáveis configuradas = Deploy pronto!**

*Versão: 2.5.1*  
*Data: 07/01/2026*

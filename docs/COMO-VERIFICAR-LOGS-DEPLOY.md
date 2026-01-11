# 🔍 Guia: Como Verificar Logs do Deploy - Sistema Hotelaria

**Data:** 09/01/2026  
**Problema:** Aplicação não abre após deploy

---

## 🚨 DIAGNÓSTICO ATUAL

### ❌ Problemas Identificados:

1. **Aplicação não responde** (Timeout)
2. **Azure CLI não autenticado** (não consigo puxar logs automaticamente)
3. **GitHub CLI não encontrado** (não consigo verificar workflows)
4. **Variáveis de ambiente faltando** no `.env`

### ✅ O que está OK:

- ✅ Build local funciona
- ✅ Todos os arquivos estão presentes
- ✅ Estrutura do projeto correta

---

## 📋 PASSO A PASSO PARA DIAGNOSTICAR

### Opção 1: Verificar GitHub Actions (RECOMENDADO) 🟢

**Por que começar aqui?** O deploy é feito via GitHub Actions. Se falhou lá, nem chegou ao Azure.

#### 1. Acessar GitHub Actions

```
🔗 https://github.com/avilaops/hotelaria/actions
```

#### 2. O que verificar:

- [ ] ✅ **Status do último workflow**: Verde (sucesso) ou Vermelho (falha)?
- [ ] 📅 **Data/hora**: É recente? (última alteração que você fez)
- [ ] 🔴 **Se vermelho**: Clique no workflow e veja qual step falhou
- [ ] 📋 **Logs**: Clique no step que falhou e leia a mensagem de erro

#### 3. Possíveis erros no GitHub Actions:

| Erro | Causa | Solução |
|------|-------|---------|
| ❌ Build failed | Erro de compilação | Ver logs, corrigir código |
| ❌ Deploy failed | Credenciais Azure incorretas | Verificar secrets do GitHub |
| ⚠️ Tests failed | Testes falhando | Corrigir testes ou desabilitar |
| 🔒 Authentication failed | Service Principal expirado | Reconfigurar Azure credentials |

#### 4. Screenshot do que procurar:

```
GitHub Actions → Seu repositório → Tab "Actions"

✅ Verde = Deploy OK (mas app pode não estar iniciando)
❌ Vermelho = Deploy falhou (nem chegou ao Azure)
🟡 Amarelo = Workflow rodando
```

---

### Opção 2: Verificar Azure Portal 🔵

**Quando usar?** Se o GitHub Actions está verde, mas o app não abre.

#### 1. Fazer Login no Azure

```
🔗 https://portal.azure.com
```

**Credenciais:** Use sua conta Microsoft/Azure

#### 2. Encontrar seu App Service

```
Portal Azure → Pesquisar "hotelaria-app" → Clicar no App Service
```

#### 3. Verificar Status

**Localização:** Painel principal do App Service

- [ ] ✅ Status: **Running** (verde)
- [ ] ❌ Status: **Stopped** (vermelho)

**Se stopped:**
```
Clique no botão "Start" no topo da página
```

#### 4. Ver Logs em Tempo Real

**Caminho:** `App Service → Monitoring → Log stream`

```
1. Clique em "Log stream" no menu esquerdo
2. Aguarde conexão
3. Veja os logs aparecendo em tempo real
4. Procure por:
   ❌ "error"
   ❌ "exception"
   ❌ "failed"
   ⚠️ "warning"
```

#### 5. Diagnosticar Problemas

**Caminho:** `App Service → Diagnose and solve problems`

```
1. Clique em "Diagnose and solve problems"
2. Escolha categoria:
   - "Availability and Performance"
   - "Configuration and Management"
3. Veja os problemas detectados automaticamente
```

#### 6. Verificar Configurações

**Caminho:** `App Service → Settings → Configuration`

- [ ] **Application settings**: Verificar se variáveis de ambiente estão configuradas
- [ ] **General settings**: Verificar se HTTPS Only está configurado corretamente
- [ ] **Platform**: Verificar se está .NET 8

---

## 🛠️ AÇÕES IMEDIATAS

### 1. Verificar GitHub Actions (5 minutos)

```bash
# Acesse:
https://github.com/avilaops/hotelaria/actions

# Veja:
- Último workflow está verde ou vermelho?
- Se vermelho, qual step falhou?
- Copie a mensagem de erro
```

### 2. Se GitHub Actions está VERDE mas app não abre:

Vá para Azure Portal e:

```
1. Verificar se App Service está "Running"
2. Ver "Log stream" para erros de startup
3. Verificar "Configuration" > "Application settings"
```

### 3. Se GitHub Actions está VERMELHO:

```
1. Veja qual step falhou
2. Leia a mensagem de erro
3. Possíveis soluções:
   - Build: Corrigir erro de código
   - Deploy: Verificar credentials do Azure
   - Tests: Corrigir testes
```

---

## 🔄 FORÇAR NOVO DEPLOY

Se quiser forçar um novo deploy agora:

### Método 1: Fazer uma alteração mínima

```bash
# No terminal (PowerShell)
git add .
git commit -m "Trigger deploy"
git push origin main
```

Isso vai triggerar o GitHub Actions automaticamente.

### Método 2: Executar workflow manualmente

```
1. GitHub → Actions
2. Selecione o workflow "Build and deploy..."
3. Clique em "Run workflow"
4. Escolha branch "main"
5. Clique em "Run workflow" (botão verde)
```

---

## 📸 O QUE EU PRECISO VER

Para te ajudar melhor, compartilhe screenshots de:

### 1. GitHub Actions
```
🔗 https://github.com/avilaops/hotelaria/actions

Screenshot mostrando:
- Lista de workflows
- Status (verde/vermelho)
- Data/hora
```

### 2. Se workflow falhou
```
Screenshot do log do step que falhou
(clique no workflow → clique no step vermelho → copie o erro)
```

### 3. Azure Portal (se conseguir acessar)
```
Screenshot de:
- Status do App Service (Running/Stopped)
- Log stream (se tiver erros)
```

### 4. Erro no Browser
```
Screenshot do que aparece quando você acessa:
https://hotelaria-app.azurewebsites.net
```

---

## 🎯 CHECKLIST DE VERIFICAÇÃO

Use esta checklist para diagnosticar:

### GitHub Actions
- [ ] Acessei https://github.com/avilaops/hotelaria/actions
- [ ] Vi o status do último workflow
- [ ] Se vermelho, identifiquei qual step falhou
- [ ] Copiei a mensagem de erro (se houver)

### Azure Portal
- [ ] Fiz login em https://portal.azure.com
- [ ] Encontrei o App Service "hotelaria-app"
- [ ] Verifiquei o status (Running/Stopped)
- [ ] Vi o Log stream
- [ ] Copiei erros do log (se houver)

### Teste de Acesso
- [ ] Tentei acessar https://hotelaria-app.azurewebsites.net
- [ ] Anotei o erro exato que aparece
- [ ] Tirei screenshot do erro

---

## 💡 DICAS IMPORTANTES

### ⏰ Tempo de Deploy
Após fazer push, aguarde:
- 5-10 minutos para o workflow completar
- Mais 2-3 minutos para o app iniciar no Azure

### 🔍 Onde Procurar Primeiro
1. **GitHub Actions** (mais fácil de diagnosticar)
2. **Azure Log Stream** (se GitHub Actions está OK)
3. **Browser DevTools** (F12) para erros de frontend

### 🚫 Erros Comuns

| Sintoma | Causa Provável | Onde Verificar |
|---------|----------------|----------------|
| Timeout | App não iniciou | Azure Log Stream |
| 503 Service Unavailable | App stopped ou crashou | Azure Status + Logs |
| 404 Not Found | Deploy não completou | GitHub Actions |
| 500 Internal Server Error | Erro na aplicação | Azure Log Stream |

---

## 📞 PRÓXIMOS PASSOS

1. **Agora:** Acesse GitHub Actions e veja o status
2. **Me diga:**
   - Workflow está verde ou vermelho?
   - Se vermelho, qual mensagem de erro?
   - Se verde, qual erro aparece no browser?
3. **Compartilhe:**
   - Screenshot do GitHub Actions
   - Screenshot do erro no browser

Com essas informações, consigo te ajudar a resolver o problema específico! 🎯

---

## 🔗 Links Rápidos

| Recurso | Link |
|---------|------|
| **GitHub Actions** | https://github.com/avilaops/hotelaria/actions |
| **Azure Portal** | https://portal.azure.com |
| **Sua Aplicação** | https://hotelaria-app.azurewebsites.net |
| **App no Azure** | https://portal.azure.com/#view/WebsitesExtension/WebsiteMenuBlade/~/overview/resourceId/%2Fsubscriptions%2F%3Csubscription-id%3E%2FresourceGroups%2Fhotelaria-app%2Fproviders%2FMicrosoft.Web%2Fsites%2Fhotelaria-app |

---

**Importante:** Não tenho acesso direto aos seus logs. Preciso que você acesse e compartilhe as informações! 🙏

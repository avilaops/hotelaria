# 🚀 SOLUÇÃO RÁPIDA: Habilitar Publish Profile

## ⚠️ Problema
```
❌ A autenticação básica está desabilitada
```

## ✅ Solução em 3 Passos

### 📍 Passo 1: Habilitar Basic Auth (2 minutos)

1. **Azure Portal** → Seu Web App: `hotelaria-app`
2. **Menu lateral** → Clique em **"Configuração"**
3. **Aba** → **"Configurações gerais"**
4. **Procure:**
   ```
   SCM Basic Auth Publishing Credentials
   ```
5. **Altere de** `OFF` **para** `ON` ✅
6. **Clique** em **"Salvar"** (topo da página)
7. **Aguarde** 30 segundos

---

### 📍 Passo 2: Baixar Publish Profile (1 minuto)

1. **Volte** para **"Visão geral"** (menu lateral)
2. **Clique** em **"Baixar perfil de publicação"** (topo)
   - Ou: **"Get publish profile"**
3. **Arquivo baixado:** `hotelaria-app-xxxxx.PublishSettings`
4. **Abra o arquivo** com Notepad/VSCode
5. **Copie TODO o conteúdo** (Ctrl+A, Ctrl+C)

---

### 📍 Passo 3: Adicionar no GitHub (2 minutos)

1. **Ir para:** https://github.com/avilaops/hotelaria/settings/secrets/actions

2. **Clicar** em **"New repository secret"**

3. **Preencher:**
   ```
   Name:   AZURE_WEBAPP_PUBLISH_PROFILE
   Secret: [Colar o XML completo aqui]
   ```

4. **Clicar** em **"Add secret"**

5. **Fazer push** para testar:
   ```bash
   git commit --allow-empty -m "🚀 Test Azure deploy"
   git push origin main
   ```

6. **Verificar deploy:** https://github.com/avilaops/hotelaria/actions

---

## 🎯 Resultado

Após ~5 minutos:

```
✅ Workflow executado
✅ Deploy no Azure completo
✅ App disponível em: https://hotelaria-app.azurewebsites.net
```

---

## 📸 Referência Visual

### Tela de Configuração

```
┌─────────────────────────────────────────┐
│ hotelaria-app - Configuração           │
├─────────────────────────────────────────┤
│                                         │
│ ┌─ Configurações gerais ─────────────┐ │
│ │                                     │ │
│ │ SCM Basic Auth Publishing          │ │
│ │ Credentials                         │ │
│ │                                     │ │
│ │ [ OFF ]  → [ ON ] ✅               │ │
│ │                                     │ │
│ │ FTP Basic Auth Publishing          │ │
│ │ Credentials                         │ │
│ │                                     │ │
│ │ [ OFF ]  → [ ON ] ✅ (opcional)    │ │
│ │                                     │ │
│ └─────────────────────────────────────┘ │
│                                         │
│ [💾 Salvar]  [❌ Descartar]            │
└─────────────────────────────────────────┘
```

### Conteúdo do Publish Profile

```xml
<publishData>
  <publishProfile 
    profileName="hotelaria-app - Web Deploy"
    publishMethod="MSDeploy"
    publishUrl="hotelaria-app.scm.azurewebsites.net:443"
    msdeploySite="hotelaria-app"
    userName="$hotelaria-app"
    userPWD="SENHA_LONGA_AQUI_ABC123..."
    destinationAppUrl="http://hotelaria-app.azurewebsites.net"
    SQLServerDBConnectionString=""
    mySQLDBConnectionString=""
    hostingProviderForumLink=""
    controlPanelLink="http://windows.azure.com"
    webSystem="WebSites">
    ...
  </publishProfile>
  ...
</publishData>
```

**⚠️ COPIAR TUDO!**

---

## 🔒 Segurança

Após confirmar que o deploy funciona:

1. **Desabilitar Basic Auth** (opcional mas recomendado):
   - Configuração → Configurações gerais
   - SCM Basic Auth → OFF
   - Salvar

2. **O secret no GitHub** continuará funcionando mesmo com Basic Auth OFF!

---

## ❓ Perguntas Frequentes

### Q: Por que preciso habilitar Basic Auth?

**R:** Azure desabilitou por padrão em 2024. Precisamos habilitar apenas para baixar o profile, depois pode desabilitar.

### Q: É seguro?

**R:** Para desenvolvimento/teste: Sim. Para produção: Prefira Service Principal (ver `docs/AZURE-PUBLISH-PROFILE-GUIA.md`).

### Q: O profile expira?

**R:** Raramente, mas pode acontecer. Se o deploy falhar, basta gerar um novo profile.

### Q: Posso usar para múltiplos apps?

**R:** Não, cada app precisa do próprio profile. Para múltiplos apps, use Service Principal.

---

## 🚨 Troubleshooting Rápido

### ❌ Ainda mostra "Basic Auth desabilitado"

**Solução:**
- Aguarde 1-2 minutos após salvar
- Faça refresh da página (F5)
- Tente baixar novamente

### ❌ Arquivo XML está corrompido

**Solução:**
- Delete o arquivo baixado
- Limpe o cache do navegador
- Baixe novamente

### ❌ Deploy falha no GitHub Actions

**Solução:**
```bash
# Verificar se o secret está correto
# GitHub → Settings → Secrets → AZURE_WEBAPP_PUBLISH_PROFILE

# Verificar se copiou TODO o XML
# Deve começar com: <publishData>
# Deve terminar com: </publishData>

# Tentar novamente
git commit --allow-empty -m "🔄 Retry deploy"
git push origin main
```

---

## 📞 Precisa de Ajuda?

### Guia Completo
- **Leia:** `docs/AZURE-PUBLISH-PROFILE-GUIA.md`
- **Service Principal:** `docs/AZURE-SERVICE-PRINCIPAL.md`

### GitHub Actions
- **Ver logs:** https://github.com/avilaops/hotelaria/actions
- **Workflow:** `.github/workflows/dotnet.yml`

---

**✅ Pronto! Agora você pode fazer deploy no Azure via GitHub Actions!**

*Última atualização: 08/01/2026*

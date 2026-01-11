# 🔐 Guia de Autenticação Azure CLI

## 🎯 O Problema

Para configurar variáveis de ambiente e fazer deploy no Azure, preciso de autenticação, mas não consigo fazer login interativo automaticamente.

## ✅ Solução: Você Fazer o Login

### Passo 1: Abrir PowerShell como Administrador

1. Pressione `Win + X`
2. Escolha "Windows PowerShell (Admin)" ou "Terminal (Admin)"

### Passo 2: Fazer Login no Azure

```powershell
az login
```

**O que vai acontecer:**
- Uma janela do navegador vai abrir
- Faça login com sua conta Microsoft/Azure
- Após login, o terminal vai mostrar suas subscriptions

### Passo 3: Confirmar Autenticação

```powershell
az account show
```

**Deve mostrar algo como:**
```json
{
  "environmentName": "AzureCloud",
  "homeTenantId": "...",
  "id": "...",
  "name": "...",
  "user": {
    "name": "seu@email.com",
    "type": "user"
  }
}
```

---

## 🚀 Depois de Autenticado

### Opção 1: Executar Script de Configuração

```powershell
cd D:\Hotelaria
.\configure-azure-env.ps1
```

### Opção 2: Deploy Manual Completo

```powershell
.\deploy-manual-emergencia.ps1 -Force
```

---

## 🔑 Alternativa: Service Principal (Para Automação Total)

Se você quiser que eu tenha acesso automático sem precisar fazer login toda vez, você pode criar um **Service Principal**:

### Como Criar Service Principal:

```powershell
# 1. Fazer login
az login

# 2. Criar Service Principal
az ad sp create-for-rbac --name "hotelaria-deploy-sp" --role contributor \
    --scopes /subscriptions/{subscription-id}/resourceGroups/hotelaria-app

# Vai retornar algo como:
# {
#   "appId": "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX",
#   "displayName": "hotelaria-deploy-sp",
#   "password": "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX",
#   "tenant": "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"
# }
```

### Configurar no GitHub Secrets:

As credenciais que aparecem devem ser adicionadas nos Secrets do GitHub:
- `AZURE_CLIENT_ID` = appId
- `AZURE_CLIENT_SECRET` = password
- `AZURE_TENANT_ID` = tenant
- `AZURE_SUBSCRIPTION_ID` = sua subscription

Mas isso já está configurado no seu GitHub Actions! ✅

---

## 💡 Resumo: O Que Você Precisa Fazer AGORA

### Passo a Passo Simples:

1. **Abrir PowerShell como Admin**
2. **Executar:** `az login`
3. **Fazer login no navegador**
4. **Voltar ao PowerShell**
5. **Executar:** `cd D:\Hotelaria`
6. **Executar:** `.\configure-azure-env.ps1`

Ou, mais simples ainda:

```powershell
# Tudo em um comando (após az login):
cd D:\Hotelaria ; .\configure-azure-env.ps1
```

---

## 🎯 Depois Disso

Com as variáveis configuradas, você pode fazer o deploy:

### Via GitHub Actions (RECOMENDADO):
```bash
git add .
git commit -m "fix: Update to .NET 9.0 for Azure compatibility"
git push origin main
```

### Via Script Manual:
```powershell
.\deploy-manual-emergencia.ps1 -Force
```

---

## ❓ Por Que Não Consigo Fazer Login Por Você?

Por limitações de segurança e automação:
- ❌ Não tenho acesso ao navegador interativo
- ❌ Não posso armazenar suas credenciais
- ❌ Comandos interativos (como `az login`) não funcionam via terminal remoto

Mas depois que você fizer o login uma vez, o token fica salvo e eu posso executar os scripts! ✅

---

## ✅ Checklist

- [ ] PowerShell aberto como Admin
- [ ] `az login` executado
- [ ] Login feito no navegador
- [ ] `az account show` confirmou autenticação
- [ ] `.\configure-azure-env.ps1` executado
- [ ] Deploy via GitHub Actions ou manual

---

**Próxima ação:** Abra o PowerShell e execute `az login` 🚀

# ⚡ Quick Start - Deploy no Azure

## 🎯 Deploy em 5 Minutos

### **Pré-requisitos**
- ✅ Conta Azure ativa
- ✅ Repositório no GitHub: `avilaops/hotelaria`

---

## 📋 Passo a Passo

### **1. Criar Web App no Azure Portal**

```
1. Acesse: https://portal.azure.com
2. Clique em "Criar um recurso"
3. Busque: "Web App"
4. Clique em "Criar"
```

### **2. Configurar Básico**

```
Nome: hotelaria-app
Publicar: Código
Pilha de runtime: .NET 8 (LTS)
Sistema Operacional: Linux
Região: Brazil South
Plano: B1 (Básico) - R$ ~55/mês
```

### **3. Configurar Deployment**

```
Habilitar CI/CD: Sim
Conta GitHub: Conectar
Repositório: avilaops/hotelaria
Branch: main
```

### **4. Criar**

```
Clique em "Revisar + criar"
Aguarde 2-3 minutos
```

### **5. Configurar Variáveis**

```
App Service → Configuração → Configurações do aplicativo

Adicionar:
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:8080
```

### **6. Acessar**

```
URL: https://hotelaria-app.azurewebsites.net
```

---

## 🔄 Deploy Automático via GitHub Actions

### **Configurar Secret no GitHub:**

```bash
# 1. No Azure Portal:
App Service → Obter perfil de publicação
Salvar arquivo .publishsettings

# 2. No GitHub:
https://github.com/avilaops/hotelaria/settings/secrets/actions
New repository secret
Nome: AZURE_WEBAPP_PUBLISH_PROFILE
Valor: [colar conteúdo XML do arquivo]
```

### **Workflow já está configurado:**

```
.github/workflows/azure-deploy.yml
```

Basta fazer push e o deploy acontece automaticamente! 🚀

---

## 🧪 Verificar Deploy

```bash
# Via navegador
https://hotelaria-app.azurewebsites.net

# Via CLI
az webapp browse --name hotelaria-app --resource-group hotelaria-rg
```

---

## 📊 Monitorar

```
Portal Azure → App Service → Log stream
```

---

## 💰 Custos

| Plano | Custo/mês | Ideal para |
|-------|-----------|------------|
| F1 (Free) | R$ 0 | Testes |
| B1 (Basic) | R$ 55-65 | Produção pequena |
| S1 (Standard) | R$ 190-220 | Produção alta disponibilidade |

---

## 🔒 Segurança

### **Variáveis Sensíveis:**

❌ **NÃO adicionar no código:**
```csharp
// NÃO FAZER ISSO!
var apiKey = "sk-123456789";
```

✅ **Usar App Settings do Azure:**
```
Portal Azure → Configuração → Configurações do aplicativo
```

---

## 📦 Estrutura de Deployment

```
Hotelaria/
├── .github/
│   └── workflows/
│       ├── dotnet.yml              (Build e testes)
│       └── azure-deploy.yml        (Deploy no Azure) 🆕
├── docs/
│   ├── AZURE-DEPLOY.md            (Guia completo) 🆕
│   └── AZURE-QUICKSTART.md        (Este arquivo) 🆕
└── [outros arquivos do projeto]
```

---

## 🆘 Problemas Comuns

### **App não inicia (502 Bad Gateway)**

**Solução:**
```
Verificar: Configurações do aplicativo
ASPNETCORE_URLS = http://+:8080
```

### **Variáveis não carregadas**

**Solução:**
```
App Service → Reiniciar
Aguardar 1-2 minutos
```

### **Deploy falha no GitHub Actions**

**Solução:**
```
Verificar secret: AZURE_WEBAPP_PUBLISH_PROFILE
Re-download do perfil de publicação se necessário
```

---

## 📞 Ajuda

- **Guia Completo:** `docs/AZURE-DEPLOY.md`
- **Documentação Azure:** https://learn.microsoft.com/azure/app-service/
- **Issues GitHub:** https://github.com/avilaops/hotelaria/issues

---

**✨ Deploy no Azure em 5 minutos!** 🚀

*Sistema de Hotelaria - v2.6.0*

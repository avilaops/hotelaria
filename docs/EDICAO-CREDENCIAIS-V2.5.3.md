# 🔧 Edição de Credenciais em Produção - v2.5.3

## 🎯 Nova Funcionalidade

Agora os desenvolvedores podem **editar as chaves de acesso diretamente na interface** e alternar entre modo **Sandbox/Production** em tempo real!

---

## ✨ O Que Foi Adicionado

### 1. Botão "Editar Credenciais"

Cada integração agora tem um botão **✏️ Editar Credenciais** que permite:

- ✅ Visualizar credenciais atuais (mascaradas)
- ✅ Editar Client ID, Tokens, Keys
- ✅ Alternar entre Sandbox e Production (PayPal)
- ✅ Salvar e testar imediatamente

### 2. Modo de Edição Inline

Ao clicar em "Editar Credenciais", o card expande mostrando:

```
┌────────────────────────────────────┐
│ PayPal Logo      ● Desconectado   │
├────────────────────────────────────┤
│ Client ID:                         │
│ [________________________]         │
│                                    │
│ API Token:                         │
│ [________________________]         │
│                                    │
│ Ambiente:                          │
│ [Sandbox ▼]  [Production]          │
│                                    │
│ [Cancelar]  [💾 Salvar]            │
└────────────────────────────────────┘
```

### 3. Suporte a Múltiplos Ambientes

#### PayPal
- **Sandbox** → https://api-m.sandbox.paypal.com
- **Production** → https://api-m.paypal.com

A URL da API muda automaticamente baseada no ambiente selecionado!

---

## 🔐 Como Usar

### Passo a Passo

1. **Acessar Integrações**
   ```
   Login → Configurações → Integrações
   Login com: nicolasrosaab / 7Aciqgr7@
   ```

2. **Selecionar Integração**
   - Clicar no card da integração desejada

3. **Editar Credenciais**
   - Clicar em **✏️ Editar Credenciais**

4. **Preencher Dados**
   - **PayPal:**
     - Client ID: `Ac4buNlLjPT130g4vbvAr`
     - API Token: `EEobBz_RPqm2lkPGCaGJ...`
     - Ambiente: `Production` ou `Sandbox`
   
   - **MongoDB:**
     - Connection String: `mongodb+srv://...`
     - Database Name: `hotelaria`
   
   - **Airbnb:**
     - Client Key: `af5e33493f7b6e3e...`
     - Secret Key: `cbf0e70fc6ee6371...`
   
   - **Sentry:**
     - API Token: `sntrys_eyJpYXQi...`

5. **Salvar**
   - Clicar em **💾 Salvar**

6. **Testar Conexão**
   - Clicar em **Testar Conexão**
   - Verificar se conecta em **Production**

---

## 🎨 Interface Visual

### Estado Normal
```
┌──────────────────────────────────────┐
│ PayPal Logo         ● Conectado      │
├──────────────────────────────────────┤
│ CLIENT ID:    Ac4b...bvAr            │
│ AMBIENTE:     Production             │
│                                      │
│ [✏️ Editar]  [Testar Conexão]        │
└──────────────────────────────────────┘
```

### Estado Editando
```
┌──────────────────────────────────────┐
│ PayPal Logo         ● Desconectado   │
├──────────────────────────────────────┤
│ ┌────────────────────────────────┐   │
│ │ Client ID:                     │   │
│ │ [Ac4buNlLjPT130g4vbvAr_____]   │   │
│ │                                │   │
│ │ API Token:                     │   │
│ │ [************************___]   │   │
│ │                                │   │
│ │ Ambiente:                      │   │
│ │ [ Production ▼ ]               │   │
│ │                                │   │
│ │ [Cancelar] [💾 Salvar]         │   │
│ └────────────────────────────────┘   │
└──────────────────────────────────────┘
```

---

## 🚀 Funcionalidades por Integração

### 1. PayPal

#### Campos Editáveis:
- ✅ Client ID
- ✅ API Token
- ✅ Ambiente (Sandbox/Production)

#### Como Funciona:
1. Editar credenciais
2. Selecionar **Production**
3. Salvar
4. Testar conexão
5. API usa URL de produção automaticamente

#### URL da API:
```javascript
const baseUrl = environment === "Production" 
    ? "https://api-m.paypal.com"
    : "https://api-m.sandbox.paypal.com";
```

---

### 2. MongoDB

#### Campos Editáveis:
- ✅ Connection String
- ✅ Database Name

#### Como Funciona:
1. Editar Connection String completa
2. Definir nome do banco
3. Salvar
4. Testar conexão
5. Sistema conecta no MongoDB Atlas

#### Exemplo de Connection String:
```
mongodb+srv://user:pass@cluster0.xxxxx.mongodb.net/
```

---

### 3. Airbnb

#### Campos Editáveis:
- ✅ Client Key
- ✅ Secret Key

#### Como Funciona:
1. Editar credenciais da API Airbnb
2. Salvar
3. Testar conexão
4. Sistema valida keys

---

### 4. Sentry

#### Campos Editáveis:
- ✅ API Token

#### Como Funciona:
1. Editar token de autenticação
2. Salvar
3. Testar conexão
4. Sistema valida token com Sentry API

---

## 🔒 Segurança

### Armazenamento em Memória

As credenciais editadas são armazenadas **em memória** durante a execução:

```csharp
private readonly Dictionary<string, string> _secureConfig = new();
```

### Não Persiste Automaticamente

⚠️ **IMPORTANTE:** As credenciais editadas na interface **não são salvas no .env**. 

Elas ficam ativas apenas durante a execução atual.

### Para Persistir:

1. **Método 1: Manual**
   - Copiar valores da interface
   - Colar no arquivo `.env`

2. **Método 2: Variáveis de Ambiente (Render)**
   ```
   Settings → Environment
   Adicionar variáveis manualmente
   ```

---

## 📊 Fluxo de Edição

```
┌─────────────────┐
│ Ver Status      │
│ (Mascarado)     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Clicar Editar   │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Formulário      │
│ Aparece         │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Preencher       │
│ Credenciais     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Salvar          │
│ (em memória)    │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│ Testar Conexão  │
│ (com novas keys)│
└────────┬────────┘
         │
    ┌────┴────┐
    │         │
  Erro      Sucesso
    │         │
    ▼         ▼
┌─────┐   ┌────────┐
│Ajustar│ │Usar em │
│Credenc│ │Produção│
└─────┘   └────────┘
```

---

## 🎯 Casos de Uso

### Caso 1: Testar em Sandbox

```
1. Editar PayPal
2. Ambiente: Sandbox
3. Usar credenciais de teste
4. Salvar
5. Testar → Funciona!
```

### Caso 2: Migrar para Production

```
1. Editar PayPal
2. Ambiente: Production
3. Usar credenciais reais
4. Salvar
5. Testar → Verifica conexão real
6. Processar pagamentos em produção
```

### Caso 3: Trocar Database

```
1. Editar MongoDB
2. Nova Connection String
3. Novo Database Name
4. Salvar
5. Testar → Conecta no novo banco
```

---

## 🔍 Validações

### Antes de Salvar:

✅ **PayPal**
- Client ID não pode estar vazio
- API Token não pode estar vazio

✅ **MongoDB**
- Connection String deve começar com `mongodb://` ou `mongodb+srv://`

✅ **Airbnb**
- Client Key deve ter formato válido
- Secret Key não pode estar vazio

✅ **Sentry**
- Token deve começar com `sntrys_`

---

## 💡 Dicas

### 1. Copiar Credenciais Atuais

Antes de editar, anote as credenciais atuais em caso de erro.

### 2. Testar Sandbox Primeiro

Sempre teste em Sandbox antes de usar Production.

### 3. Verificar Logs

Se falhar, verificar erro em:
```
GetStatus() → LastError
```

### 4. Restaurar Padrões

Para voltar às credenciais do `.env`:
```powershell
# Reiniciar aplicação
dotnet run
```

---

## 🎨 Estilo Visual

### Design Minimalista

- ✅ Inputs com 44px altura (touch-friendly)
- ✅ Border radius 8px
- ✅ Focus com shadow azul
- ✅ Placeholder discreto
- ✅ Labels uppercase pequenas

### Feedback Visual

- 🟢 **Verde** → Conectado
- 🔴 **Vermelho** → Desconectado
- 🔵 **Azul** → Editando

### Animações

- Transição suave ao abrir formulário
- Shadow ao focar input
- Hover nos botões

---

## 📝 Código Exemplo

### Editar Credenciais

```csharp
private void EditarPayPal()
{
    paypalEditClientId = ConfigService.GetSecureValue("PAYPAL_ID") ?? "";
    paypalEditToken = ConfigService.GetSecureValue("PAYPAL_TOKEN_API") ?? "";
    paypalEditEnvironment = "Sandbox";
    editandoPayPal = true;
}
```

### Salvar Credenciais

```csharp
private void SalvarPayPal()
{
    if (!string.IsNullOrWhiteSpace(paypalEditClientId))
    {
        ConfigService.UpdateConfiguration("PAYPAL_ID", paypalEditClientId);
        ConfigService.UpdateConfiguration("PAYPAL_TOKEN_API", paypalEditToken);
        ConfigService.UpdateConfiguration("PAYPAL_ENVIRONMENT", paypalEditEnvironment);
        
        editandoPayPal = false;
        LoadIntegrationStatus();
    }
}
```

### Testar com Novo Ambiente

```csharp
var baseUrl = _currentEnvironment == "Production" 
    ? "https://api-m.paypal.com"
    : "https://api-m.sandbox.paypal.com";
```

---

## ✅ Checklist de Verificação

Após editar credenciais:

- [ ] Credenciais preenchidas corretamente
- [ ] Ambiente selecionado (se aplicável)
- [ ] Salvou as alterações
- [ ] Testou a conexão
- [ ] Status mudou para "Conectado"
- [ ] Funcionalidade está operacional

---

## 🎉 Benefícios

### Antes:
```
❌ Editar .env manualmente
❌ Reiniciar aplicação
❌ Não sabe se funciona até testar
❌ Difícil alternar entre Sandbox/Production
```

### Depois:
```
✅ Editar direto na interface
✅ Testar imediatamente
✅ Ver status em tempo real
✅ Alternar ambiente com 1 clique
✅ Feedback instantâneo
```

---

## 🚀 Status

```
╔═══════════════════════════════════════╗
║  EDIÇÃO DE CREDENCIAIS - v2.5.3      ║
╠═══════════════════════════════════════╣
║  Edição Inline:        ✅            ║
║  Sandbox/Production:   ✅            ║
║  Teste em Tempo Real:  ✅            ║
║  4 Integrações:        ✅            ║
║  Interface Minimalista: ✅            ║
╚═══════════════════════════════════════╝
```

---

**🔧 Edite, Teste e Implemente em Produção!**

*Versão: 2.5.3*  
*Data: 07/01/2026*  
*Feature: Edição de Credenciais em Tempo Real*

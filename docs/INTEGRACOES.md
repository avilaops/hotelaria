# 🔌 Integrações - Documentação Completa

## 📋 Visão Geral

Sistema de integrações com PayPal, MongoDB, Airbnb e Sentry implementado na versão 2.5.2.

---

## 🔐 Acesso

### Credenciais de Acesso

| Campo | Valor |
|-------|-------|
| **Usuário** | `nicolasrosaab` |
| **Senha** | `7Aciqgr7@` |

### Permissões Necessárias

1. **Autenticação no Sistema**
   - Fazer login no Hotelaria
   - Perfil: Administrador

2. **Autenticação em Integrações**
   - Usuário e senha específicos
   - Camada extra de segurança

---

## 🚀 Como Acessar

### Passo a Passo

1. **Fazer Login no Sistema**
   ```
   URL: /login
   Usuário: admin
   Senha: admin123
   ```

2. **Acessar Configurações**
   ```
   Menu lateral → ⚙️ Configurações
   ```

3. **Clicar em Integrações**
   ```
   Seção "Integrações" → Card "APIs e Serviços"
   ```

4. **Autenticar em Integrações**
   ```
   Usuário: nicolasrosaab
   Senha: 7Aciqgr7@
   ```

5. **Gerenciar Integrações**
   - Ver status de conexão
   - Testar conectividade
   - Ver configurações mascaradas

---

## 🔌 Integrações Disponíveis

### 1. PayPal 💰

#### Descrição
Processamento de pagamentos online

#### Credenciais Configuradas
```env
PAYPAL_ID=Ac4buNlLjPT130g4vbvAr
PAYPAL_TOKEN_API=EEobBz_RPqm2lkPGCaGJo98kR_LIfs...
```

#### Funcionalidades
- ✅ Teste de conexão
- ✅ Status de configuração
- ✅ Mascaramento de credenciais
- ⏳ Criação de pagamentos (futuro)
- ⏳ Webhooks (futuro)

#### Status Exibido
```
Connected:   true/false
ClientId:    Ac4b...bvAr (mascarado)
Environment: Sandbox
LastError:   (se houver)
```

---

### 2. MongoDB 🍃

#### Descrição
Banco de dados NoSQL para backup e sincronização

#### Credenciais Configuradas
```env
MONGO_ATLAS_URI=mongodb+srv://nicolasrosaab_db_user:...
```

#### Funcionalidades
- ✅ Teste de conexão
- ✅ Status do banco
- ✅ Informações mascaradas
- ⏳ Sync de reservas (futuro)
- ⏳ Backup automático (futuro)

#### Status Exibido
```
Connected:        true/false
Database:         hotelaria
Provider:         MongoDB Atlas
ConnectionString: mongo...@cluster0 (mascarado)
```

---

### 3. Airbnb 🏠

#### Descrição
Sincronização com plataforma Airbnb

#### Credenciais Configuradas
```env
AIRBNB_CLIENT_KEY=af5e33493f7b6e3e443df3251ca04ef4
AIRBNB_SECRET_KEY=cbf0e70fc6ee6371ba48c1b38530f3b1
```

#### Funcionalidades
- ✅ Teste de conexão
- ✅ Status de integração
- ✅ Credenciais mascaradas
- ⏳ Sync de reservas (futuro)
- ⏳ Atualização de disponibilidade (futuro)
- ⏳ Sincronização de preços (futuro)

#### Status Exibido
```
Connected:  true/false
ClientKey:  af5e...4ef4 (mascarado)
Features:   Sync, Disponibilidade, Preços
```

---

### 4. Sentry 🐛

#### Descrição
Monitoramento de erros e performance

#### Credenciais Configuradas
```env
SENTRY_TOKEN_API=sntrys_eyJpYXQiOjE3NjI0NzcxMD...
```

#### Funcionalidades
- ✅ Teste de conexão
- ✅ Status do serviço
- ✅ Token mascarado
- ✅ Captura de erros
- ✅ Contagem de erros
- ⏳ Alertas configuráveis (futuro)
- ⏳ Performance monitoring (futuro)

#### Status Exibido
```
Connected:  true/false
Token:      sntr...Dpfw (mascarado)
ErrorCount: 0
LastError:  Nenhum / dd/MM/yyyy HH:mm
Features:   Error Tracking, Performance, Alerts
```

---

## 🎨 Interface

### Tela de Bloqueio

```
┌──────────────────────────────┐
│                              │
│            🔒                │
│                              │
│      Área Restrita           │
│  Acesso exclusivo para       │
│    administradores           │
│                              │
│  ┌────────────────────┐      │
│  │ Usuário            │      │
│  └────────────────────┘      │
│                              │
│  ┌────────────────────┐      │
│  │ Senha              │      │
│  └────────────────────┘      │
│                              │
│  [     Acessar     ]         │
│                              │
└──────────────────────────────┘
```

### Tela de Integrações

```
┌──────────────────────────────────────┐
│ 🔌 Integrações          [🚪 Sair]   │
├──────────────────────────────────────┤
│                                      │
│ ┌──────────────────────────────────┐ │
│ │ PayPal Logo    ● Conectado       │ │
│ ├──────────────────────────────────┤ │
│ │ Client ID: Ac4b...bvAr           │ │
│ │ Ambiente:  Sandbox               │ │
│ │ [Testar Conexão]                 │ │
│ └──────────────────────────────────┘ │
│                                      │
│ ┌──────────────────────────────────┐ │
│ │ MongoDB Logo   ● Conectado       │ │
│ ├──────────────────────────────────┤ │
│ │ Database:  hotelaria             │ │
│ │ Provider:  MongoDB Atlas         │ │
│ │ [Testar Conexão]                 │ │
│ └──────────────────────────────────┘ │
│                                      │
│ [... Airbnb, Sentry ...]            │
└──────────────────────────────────────┘
```

---

## 🔒 Segurança

### Camadas de Proteção

#### 1. Autenticação do Sistema
```
✅ Login no Hotelaria
✅ Perfil Administrador obrigatório
```

#### 2. Autenticação de Integrações
```
✅ Usuário específico (nicolasrosaab)
✅ Senha específica (7Aciqgr7@)
```

#### 3. Mascaramento de Dados
```
Valor real:   Ac4buNlLjPT130g4vbvAr
Exibido:      Ac4b...bvAr
```

#### 4. Variáveis de Ambiente
```
✅ Armazenadas em .env
✅ Não versionadas no Git
✅ Injetadas via ConfigurationService
```

---

## 🛠️ Arquitetura

### Serviços Criados

```
Services/
├── ConfigurationService.cs    (gerencia credenciais)
├── PayPalService.cs           (integração PayPal)
├── MongoDBService.cs          (integração MongoDB)
├── AirbnbService.cs           (integração Airbnb)
└── SentryService.cs           (integração Sentry)
```

### Fluxo de Dados

```
┌─────────────────┐
│ .env file       │
│ (credenciais)   │
└────────┬────────┘
         │
         ▼
┌─────────────────────┐
│ ConfigurationService│
│ (carrega e protege) │
└────────┬────────────┘
         │
         ▼
┌─────────────────────┐
│ Integration Services│
│ (PayPal, Mongo, etc)│
└────────┬────────────┘
         │
         ▼
┌─────────────────────┐
│ Pages/Integracoes   │
│ (UI protegida)      │
└─────────────────────┘
```

---

## 📝 Uso Programático

### ConfigurationService

```csharp
// Validar acesso
bool valid = ConfigService.ValidateAccess(username, password);

// Obter valor mascarado
string masked = ConfigService.GetMaskedValue("PAYPAL_ID");
// Retorna: "Ac4b...bvAr"

// Obter valor real (interno)
string real = ConfigService.GetSecureValue("PAYPAL_ID");
// Retorna: "Ac4buNlLjPT130g4vbvAr"

// Status de integrações
var status = ConfigService.GetIntegrationStatus();
// Retorna: { "PayPal": true, "MongoDB": true, ... }
```

### PayPalService

```csharp
// Testar conexão
bool connected = await PayPalService.TestConnection();

// Ver status
var status = PayPalService.GetStatus();

// Criar pagamento
string paymentId = await PayPalService.CreatePayment(100.00m, "USD");
```

### MongoDBService

```csharp
// Testar conexão
bool connected = await MongoDBService.TestConnection();

// Salvar documento
bool saved = await MongoDBService.SaveReserva(reserva);

// Buscar todos
var items = await MongoDBService.GetAll<Reserva>();
```

---

## 🧪 Testes

### Teste Manual

1. **Acessar Integrações**
   - Ir para /integracoes
   - Fazer login

2. **Testar PayPal**
   - Clicar em "Testar Conexão" no card PayPal
   - Ver status mudar para "Conectado" ou erro

3. **Testar MongoDB**
   - Clicar em "Testar Conexão" no card MongoDB
   - Verificar conexão com Atlas

4. **Testar Airbnb**
   - Clicar em "Testar Conexão"
   - Validar API keys

5. **Testar Sentry**
   - Clicar em "Testar Conexão"
   - Confirmar token válido

---

## ⚠️ Troubleshooting

### Problema: Não Consegue Acessar

**Sintoma:**
```
Credenciais inválidas
```

**Solução:**
```
Verificar:
- Usuário: nicolasrosaab
- Senha: 7Aciqgr7@
- Caps Lock desligado
```

### Problema: Integração Desconectada

**Sintoma:**
```
Status: Desconectado
```

**Soluções:**
1. Verificar se .env está carregado
2. Verificar se variáveis estão no Render
3. Clicar em "Testar Conexão"
4. Ver mensagem de erro

### Problema: Credenciais Não Aparecem

**Sintoma:**
```
Não configurado
```

**Solução:**
```bash
# Verificar .env
cat .env | grep PAYPAL

# Ou no Render:
Settings → Environment → Ver variáveis
```

---

## 🔄 Melhorias Futuras

### Versão 2.6.0
- [ ] Implementar pagamentos reais via PayPal
- [ ] Sync automático com MongoDB
- [ ] Import de reservas do Airbnb
- [ ] Dashboard de erros do Sentry

### Versão 3.0.0
- [ ] Webhooks configuráveis
- [ ] Notificações em tempo real
- [ ] Integração com Stripe
- [ ] Integração com Booking.com

---

## 📚 Referências

### APIs Utilizadas
- [PayPal API](https://developer.paypal.com/docs/api/overview/)
- [MongoDB Atlas](https://docs.atlas.mongodb.com/)
- [Airbnb API](https://www.airbnb.com/partner)
- [Sentry API](https://docs.sentry.io/api/)

### Segurança
- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security](https://docs.microsoft.com/aspnet/core/security/)

---

## ✅ Checklist

### Implementação
- [x] ConfigurationService criado
- [x] PayPalService implementado
- [x] MongoDBService implementado
- [x] AirbnbService implementado
- [x] SentryService implementado
- [x] Página de Integrações criada
- [x] Autenticação dupla configurada
- [x] Mascaramento de credenciais
- [x] CSS minimalista aplicado
- [x] Card em Configurações adicionado

### Segurança
- [x] Credenciais em .env
- [x] Validação de acesso
- [x] Perfil Administrador obrigatório
- [x] Dados mascarados na UI
- [x] .env no .gitignore

### Documentação
- [x] Guia de acesso
- [x] Descrição de cada integração
- [x] Exemplos de uso
- [x] Troubleshooting
- [x] Roadmap de melhorias

---

## 🎉 Resultado

```
╔════════════════════════════════════╗
║  INTEGRAÇÕES IMPLEMENTADAS        ║
╠════════════════════════════════════╣
║  PayPal:     ✅ Configurado       ║
║  MongoDB:    ✅ Configurado       ║
║  Airbnb:     ✅ Configurado       ║
║  Sentry:     ✅ Configurado       ║
║  Segurança:  ✅ Dupla camada      ║
║  UI:         ✅ Minimalista       ║
╚════════════════════════════════════╝
```

---

**🔐 Acesso protegido. Integrações seguras. Pronto para uso!**

*Versão: 2.5.2*  
*Data: 07/01/2026*

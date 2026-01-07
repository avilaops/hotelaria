# 🔐 Sistema de Autenticação e Gestão de Usuários

## 🎯 Visão Geral

Sistema completo de autenticação com login/senha, gestão de usuários, perfis de acesso e proteção de rotas implementado na versão 2.5.0.

---

## 🆕 Novidades da v2.5.0

### Sistema de Login
- ✅ Página de login moderna e responsiva
- ✅ Validação de credenciais
- ✅ Hash de senhas (SHA256)
- ✅ Sessão de usuário
- ✅ Botão de logout
- ✅ Credenciais de teste exibidas

### Gestão de Usuários
- ✅ CRUD completo de usuários
- ✅ Perfis de acesso (Administrador, Gerente, Recepcionista, Visualizador)
- ✅ Ativar/desativar usuários
- ✅ Redefinição de senha
- ✅ Busca e filtros
- ✅ Interface em cards com avatares

### Proteção de Rotas
- ✅ Redirect automático para login
- ✅ Verificação de autenticação
- ✅ Controle de permissões por perfil
- ✅ Exibição de usuário logado

---

## 👥 Perfis de Usuário

### Hierarquia de Permissões
```
Administrador (Nível 1)
    ↓
Gerente (Nível 2)
    ↓
Recepcionista (Nível 3)
    ↓
Visualizador (Nível 4)
```

### Descrição dos Perfis

#### 1. Administrador 👑
**Permissões:**
- ✅ Acesso total ao sistema
- ✅ Gestão de usuários
- ✅ Configurações do sistema
- ✅ Todas as funcionalidades de níveis inferiores

**Características:**
- Pode criar, editar e excluir usuários
- Pode alterar perfis de outros usuários
- Não pode excluir a si mesmo
- Não pode remover o último administrador

#### 2. Gerente 📊
**Permissões:**
- ✅ Gestão de usuários (criar/editar)
- ✅ Relatórios completos
- ✅ Gestão financeira
- ✅ Todas as funcionalidades de Recepcionista

**Características:**
- Pode gerenciar usuários de nível inferior
- Acesso a relatórios financeiros
- Não pode excluir administradores

#### 3. Recepcionista 📋
**Permissões:**
- ✅ Gestão de reservas (CRUD)
- ✅ Gestão de hóspedes (CRUD)
- ✅ Calendário de ocupação
- ✅ Check-in/Check-out

**Características:**
- Operações do dia-a-dia
- Não acessa configurações sensíveis
- Não gerencia usuários

#### 4. Visualizador 👁️
**Permissões:**
- ✅ Visualização de dashboard
- ✅ Visualização de reservas
- ✅ Visualização de calendário
- ❌ Sem permissão de edição

**Características:**
- Apenas leitura
- Ideal para auditoria
- Não pode criar/editar/excluir

---

## 🔑 Credenciais de Acesso

### Usuários Pré-cadastrados

| Usuário | Senha | Perfil | Descrição |
|---------|-------|--------|-----------|
| `admin` | `admin123` | Administrador | Acesso completo |
| `maria` | `maria123` | Gerente | Gestão e relatórios |
| `joao` | `joao123` | Recepcionista | Operações diárias |

---

## 🚀 Como Usar

### 1. Fazer Login

#### Passo a Passo
```
1. Acessar http://localhost:5000/login
2. Digitar usuário (ex: admin)
3. Digitar senha (ex: admin123)
4. Clicar em "🔑 Entrar" ou pressionar Enter
5. Sistema redireciona para dashboard
```

#### Interface de Login
```
┌─────────────────────────────┐
│         🏨                  │
│      Hotelaria              │
│  Sistema de Gestão          │
├─────────────────────────────┤
│ 👤 Usuário:                 │
│ [______________]            │
│                             │
│ 🔒 Senha:                   │
│ [______________]            │
│                             │
│ □ Lembrar-me                │
│                             │
│ [  🔑 Entrar  ]             │
├─────────────────────────────┤
│ Credenciais de Teste:       │
│ Admin: admin / admin123     │
│ Gerente: maria / maria123   │
│ Recep: joao / joao123       │
└─────────────────────────────┘
```

### 2. Gerenciar Usuários

#### Acessar Gestão
```
1. Fazer login como Administrador ou Gerente
2. Clicar em ⚙️ Configurações (rodapé do menu)
3. Seção "Sistema"
4. Clicar em "👤 Usuários e Permissões"
5. Página de gestão abre
```

#### Criar Novo Usuário
```
1. Clicar em "➕ Novo Usuário"
2. Preencher:
   - Nome Completo
   - Nome de Usuário (único)
   - Email
   - Senha (mínimo 6 caracteres)
   - Confirmar Senha
   - Perfil (Administrador/Gerente/Recepcionista/Visualizador)
   - Status (Ativo/Inativo)
3. Clicar em "💾 Salvar"
4. Usuário criado e aparece na lista
```

#### Editar Usuário
```
1. Na lista de usuários
2. Clicar em "✏️ Editar" no card do usuário
3. Modificar dados desejados
4. (Opcional) Clicar em "🔒 Redefinir Senha"
5. Digitar nova senha
6. Clicar em "💾 Salvar"
```

#### Excluir Usuário
```
1. Na lista de usuários
2. Clicar em "🗑️ Excluir" no card
3. Confirmar exclusão
4. Usuário removido

Restrições:
- Não pode excluir a si mesmo
- Não pode remover o último administrador
```

### 3. Fazer Logout

#### Opções de Logout
```
Opção 1:
1. No canto superior direito
2. Clicar em "🚪 Sair"
3. Sistema desloga e volta para tela de login

Opção 2:
1. Fechar navegador (sem "Lembrar-me")
2. Sessão expira automaticamente
```

---

## 🔒 Segurança

### Hash de Senhas
```csharp
// Algoritmo: SHA256
// Processo:
1. Senha em texto plano
2. Encoding UTF-8
3. Hash SHA256
4. Base64 encoding
5. Armazenamento do hash

// Exemplo:
Senha: "admin123"
Hash: "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI="
```

### Validações de Senha
- ✅ Mínimo 6 caracteres
- ✅ Confirmação obrigatória
- ✅ Hash antes de armazenar
- ✅ Nunca armazenar em texto plano

### Sessão de Usuário
```csharp
public class SessaoUsuario
{
    public Usuario? Usuario { get; set; }
    public DateTime DataLogin { get; set; }
    public bool EstaAutenticado => Usuario != null;
}
```

### Proteção de Rotas
```csharp
// MainLayout verifica autenticação
protected override void OnInitialized()
{
    if (!AuthService.EstaAutenticado() && !IsLoginPage)
    {
        NavigationManager.NavigateTo("/login", forceLoad: true);
    }
}
```

---

## 📊 Interface de Gestão de Usuários

### Cards de Usuário

```
┌─────────────────────────────────┐
│                                 │
│            [👤]                 │
│          MS Avatar              │
│                                 │
│       Maria Silva               │
│       @maria                    │
│   maria@hotelaria.com           │
│                                 │
│  [Gerente] [Ativo] [Você]       │
│                                 │
│  📅 Criado: 07/01/2026          │
│  🕒 Último: 07/01/2026 10:30    │
│                                 │
│  [✏️ Editar] [🗑️ Excluir]       │
└─────────────────────────────────┘
```

### Badges de Status

| Badge | Cor | Significado |
|-------|-----|-------------|
| **Administrador** | Azul | Perfil admin |
| **Gerente** | Laranja | Perfil gerente |
| **Recepcionista** | Verde | Perfil recepcionista |
| **Visualizador** | Roxo | Perfil visualização |
| **Ativo** | - | Usuário ativo |
| **Inativo** | Vermelho | Usuário desativado |
| **Você** | Gradiente | Usuário logado |

### Filtros e Busca

```
┌─────────────────────────────────┐
│ Buscar: [___________________]  │
│ Perfil: [Todos ▼]              │
└─────────────────────────────────┘
```

**Busca por:**
- Nome
- Username
- Email

**Filtro por Perfil:**
- Todos
- Administrador
- Gerente
- Recepcionista
- Visualizador

---

## 🎨 Design e UX

### Página de Login

#### Características
- ✅ Background com gradiente
- ✅ Card centralizado
- ✅ Logo e título
- ✅ Campos de input estilizados
- ✅ Botão com loading spinner
- ✅ Credenciais de teste visíveis
- ✅ Animação de entrada (slideIn)
- ✅ Suporte a Enter para login

#### Cores
```css
Background: linear-gradient(135deg, #667eea 0%, #764ba2 100%)
Card: #ffffff
Primary: #003580
Input Focus: rgba(102, 126, 234, 0.1)
Error: #ffebee / #c62828
```

### Página de Usuários

#### Layout
- Grid responsivo (3 colunas desktop → 1 coluna mobile)
- Cards com hover effect (elevação + borda)
- Avatar circular com iniciais
- Informações organizadas
- Botões de ação no rodapé

#### Estados
```
Normal:     Border #e0e0e0
Hover:      Border #003580 + Shadow
Inativo:    Opacity 60%
```

---

## 🔧 API do AuthService

### Métodos de Autenticação

```csharp
// Login
bool Login(string username, string senha)
// Retorna true se sucesso

// Logout
void Logout()
// Limpa sessão

// Verificar Autenticação
bool EstaAutenticado()
// Retorna true se logado

// Obter Usuário Atual
Usuario? ObterUsuarioAtual()
// Retorna usuário logado ou null

// Verificar Permissão
bool TemPermissao(PerfilUsuario perfilMinimo)
// Retorna true se tem permissão
```

### Métodos de Gestão

```csharp
// Listar Usuários
List<Usuario> ObterTodos()
Usuario? ObterPorId(int id)
Usuario? ObterPorUsername(string username)

// CRUD
bool AdicionarUsuario(Usuario usuario)
bool AtualizarUsuario(Usuario usuario)
bool RemoverUsuario(int id)

// Senha
bool AlterarSenha(int usuarioId, string senhaAtual, string novaSenha)
void RedefinirSenha(int usuarioId, string novaSenha)
static string HashSenha(string senha)

// Filtros
List<Usuario> FiltrarPorPerfil(PerfilUsuario? perfil)
List<Usuario> BuscarUsuarios(string termo)

// Utilitários
string ObterNomePerfil(PerfilUsuario perfil)
```

---

## 📱 Responsividade

### Desktop (1920px)
- Grid de 3 colunas
- Cards amplos
- Todas as informações visíveis
- Sidebar completa

### Tablet (768px)
- Grid de 2 colunas
- Cards médios
- Sidebar colapsada
- Touch-friendly

### Mobile (375px)
- Grid de 1 coluna
- Cards compactos
- Menu hamburger
- Botões grandes

---

## 🧪 Casos de Teste

### TC-001: Login com Credenciais Válidas
```
Pré-condições: Usuário existe e está ativo
Passos:
1. Acessar /login
2. Digitar username: admin
3. Digitar senha: admin123
4. Clicar em Entrar
Resultado: Login bem-sucedido, redirect para /
```

### TC-002: Login com Senha Incorreta
```
Pré-condições: Usuário existe
Passos:
1. Acessar /login
2. Digitar username: admin
3. Digitar senha: senhaerrada
4. Clicar em Entrar
Resultado: Mensagem "Usuário ou senha incorretos"
```

### TC-003: Criar Novo Usuário
```
Pré-condições: Logado como Administrador
Passos:
1. Acessar /usuarios
2. Clicar em "Novo Usuário"
3. Preencher formulário
4. Clicar em Salvar
Resultado: Usuário criado e aparece na lista
```

### TC-004: Tentar Excluir Próprio Usuário
```
Pré-condições: Logado
Passos:
1. Acessar /usuarios
2. Localizar card com badge "Você"
3. Verificar botão "Excluir"
Resultado: Botão "Excluir" não aparece
```

### TC-005: Logout
```
Pré-condições: Usuário logado
Passos:
1. Clicar em "Sair" no header
2. Verificar redirect
Resultado: Desloga e volta para /login
```

---

## 🚨 Limitações Conhecidas

### Sessão
- ⚠️ Sessão armazenada em memória (perdida ao reiniciar)
- ⚠️ Não persiste entre recargas de página
- ⚠️ "Lembrar-me" não implementado ainda

### Segurança
- ⚠️ SHA256 simples (produção deveria usar PBKDF2 ou bcrypt)
- ⚠️ Sem salt nas senhas
- ⚠️ Sem rate limiting
- ⚠️ Sem 2FA

### Funcionalidades
- ⚠️ Sem recuperação de senha
- ⚠️ Sem histórico de login
- ⚠️ Sem bloqueio por tentativas
- ⚠️ Sem auditoria de ações

---

## 🔄 Melhorias Futuras

### Versão 2.6.0 (Curto Prazo)
- [ ] Persistência de sessão (cookies/localStorage)
- [ ] "Lembrar-me" funcional
- [ ] Recuperação de senha por email
- [ ] Timeout de sessão configurável
- [ ] Histórico de últimos logins

### Versão 3.0.0 (Médio Prazo)
- [ ] Autenticação 2FA (Google Authenticator)
- [ ] Login social (Google, Facebook)
- [ ] PBKDF2 ou bcrypt para hash
- [ ] Salt individual por usuário
- [ ] Rate limiting (anti-brute force)
- [ ] Auditoria completa de ações
- [ ] Permissões granulares por módulo

### Versão 4.0.0 (Longo Prazo)
- [ ] SSO (Single Sign-On)
- [ ] LDAP/Active Directory
- [ ] Biometria
- [ ] Token JWT para API
- [ ] Refresh tokens
- [ ] OAuth 2.0
- [ ] Compliance (LGPD, GDPR)

---

## 📚 Documentos Relacionados

- `docs/CONFIGURACAO.md` - Módulo de Configuração
- `docs/TESTES-V2.4.0.md` - Testes da versão anterior
- `CHANGELOG.md` - Histórico de versões
- `README.md` - Visão geral do projeto

---

## ✅ Checklist de Implementação

### Modelos
- [x] Model Usuario
- [x] Enum PerfilUsuario
- [x] Model LoginModel
- [x] Model SessaoUsuario

### Serviços
- [x] AuthService criado
- [x] Hash de senhas (SHA256)
- [x] Gestão de sessão
- [x] CRUD de usuários
- [x] Verificação de permissões
- [x] Usuários de exemplo

### Páginas
- [x] Login.razor
- [x] Usuarios.razor
- [x] Configuracao.razor atualizada

### Componentes
- [x] MainLayout com menu de usuário
- [x] Botão de logout
- [x] Proteção de rotas
- [x] Exibição de usuário logado

### Estilos
- [x] CSS página de login
- [x] CSS gestão de usuários
- [x] CSS menu de usuário
- [x] Responsividade completa

### Testes
- [x] Login funcional
- [x] Logout funcional
- [x] Criação de usuários
- [x] Edição de usuários
- [x] Exclusão de usuários
- [x] Filtros e buscas
- [x] Proteção de rotas

---

## 🎉 Conclusão

O Sistema de Autenticação e Gestão de Usuários v2.5.0 oferece:

- ✅ **Segurança** - Hash de senhas e proteção de rotas
- ✅ **Controle** - 4 perfis de acesso hierárquicos
- ✅ **Gestão** - CRUD completo de usuários
- ✅ **Usabilidade** - Interface moderna e intuitiva
- ✅ **Flexibilidade** - Fácil adicionar novos perfis
- ✅ **Documentação** - Completa e detalhada

**🔐 Sistema de Autenticação Completo e Funcional!**

*Versão: 2.5.0 - Janeiro 2026*  
*Segurança e Controle de Acesso*

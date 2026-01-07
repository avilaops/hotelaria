# Changelog

Todas as mudanças notáveis neste projeto serão documentadas neste arquivo.

O formato é baseado em [Keep a Changelog](https://keepachangelog.com/pt-BR/1.0.0/),
e este projeto adere ao [Semantic Versioning](https://semver.org/lang/pt-BR/).

## [2.5.1] - 2026-01-07

### 🎨 Design
- **Redesign Completo da Página de Login**
  - Design minimalista inspirado em Apple/Steve Jobs
  - Paleta de cores reduzida e elegante
  - Tipografia San Francisco (Apple System Font)
  - Ícone SVG vetorial ao invés de emoji
  - Espaçamento generoso e respirável
  - Remoção de gradientes pesados
  - Foco no essencial

### ✨ Interface
- **Novo Layout Minimalista**
  - Card branco flutuante (18px border-radius)
  - Fundo cinza claro (#f5f5f7)
  - Sombra sutil (0 4px 24px rgba(0,0,0,0.06))
  - Inputs com altura de 44px (touch-friendly)
  - Botão com elevação no hover
  - Credenciais de teste discretas em card separado

### 🎭 Animações
- **Transições Suaves**
  - Fade in + scale no card (0.4s cubic-bezier)
  - Lift no hover do botão
  - Focus ring nos inputs (4px shadow)
  - Spinner minimalista no loading
  - Todas com easing natural

### ♿ Acessibilidade
- **WCAG AAA Compliant**
  - Contraste de cores otimizado
  - Focus visível em todos os elementos
  - Suporte a `prefers-reduced-motion`
  - Touch targets mínimos de 44px
  - Labels semânticos

### 🌙 Preparação Futura
- **Dark Mode Ready**
  - Media query `prefers-color-scheme` preparada
  - Paleta de cores dark definida
  - Suporte futuro completo

### 📐 Tipografia
- **Apple System Font Stack**
  - -apple-system, BlinkMacSystemFont
  - Segoe UI, Roboto, Helvetica
  - Letter-spacing otimizado
  - Hierarquia clara de tamanhos

### 📱 Responsividade
- **Mobile Optimized**
  - Layout adaptativo para < 480px
  - Credenciais em stack vertical
  - Padding reduzido em mobile
  - Fontes escaláveis

### 🎨 Cores
- **Paleta Reduzida**
  - Branco: #FFFFFF
  - Fundo: #F5F5F7
  - Cinza Claro: #D2D2D7
  - Cinza Médio: #86868B
  - Cinza Escuro: #1D1D1F
  - Azul: #003580
  - Azul Hover: #002D6B

### 📚 Documentação
- Novo guia `docs/DESIGN-MINIMALISTA.md`
  - Filosofia de design
  - Comparação antes/depois
  - Princípios aplicados
  - Código destacado
  - Inspirações (Apple, Material, Fluent)

---

## [2.5.0] - 2026-01-07

### 🔐 Adicionado
- **Sistema Completo de Autenticação**
  - Página de login moderna e responsiva
  - Validação de credenciais
  - Hash de senhas (SHA256)
  - Sessão de usuário
  - Botão de logout no header
  - Redirect automático para login
  - Suporte a Enter na tela de login
  - Credenciais de teste exibidas

- **Gestão de Usuários**
  - CRUD completo de usuários
  - Interface em cards com avatares
  - Página `/usuarios` acessível via Configuração
  - Card "Usuários e Permissões" ativo
  - Busca por nome, username e email
  - Filtro por perfil
  - Criação com validação completa
  - Edição de dados do usuário
  - Redefinição de senha
  - Ativação/desativação de usuários
  - Exclusão com restrições de segurança

- **Sistema de Perfis de Acesso**
  - 4 perfis hierárquicos:
    - Administrador (acesso total)
    - Gerente (gestão e relatórios)
    - Recepcionista (operações diárias)
    - Visualizador (apenas leitura)
  - Controle de permissões por perfil
  - Verificação de nível de acesso

- **Usuários Pré-cadastrados**
  - Admin (admin/admin123) - Administrador
  - Maria (maria/maria123) - Gerente
  - João (joao/joao123) - Recepcionista

### ✨ Melhorado
- **MainLayout**
  - Exibição de usuário logado no header
  - Nome do usuário e perfil visíveis
  - Botão "Sair" funcional
  - Proteção de rotas implementada
  - Redirect automático se não autenticado

- **Página de Configuração**
  - Card "Usuários e Permissões" ativo
  - Versão atualizada para 2.5.0
  - Link para gestão de usuários

### 🎨 Interface
- **Página de Login**
  - Background com gradiente roxo
  - Card centralizado branco
  - Logo e título do sistema
  - Campos estilizados com ícones
  - Checkbox "Lembrar-me"
  - Botão com loading spinner
  - Alert de erro vermelho
  - Credenciais de teste destacadas
  - Rodapé com versão
  - Animação de entrada (slideIn)
  - Responsivo (desktop/tablet/mobile)

- **Página de Usuários**
  - Grid responsivo de cards
  - Avatar circular com iniciais
  - Badges coloridos por perfil
  - Meta informações (criação, último acesso)
  - Botões de ação (editar, excluir)
  - Cards inativos com opacidade reduzida
  - Badge "Você" no usuário logado
  - Hover effects com elevação

- **Modal de Usuário**
  - Criação e edição no mesmo modal
  - Formulário completo
  - Campos de senha condicionais
  - Botão "Redefinir Senha"
  - Validação em tempo real
  - Mensagens de erro

- **Menu de Usuário**
  - Header com nome e perfil
  - Botão de logout estilizado
  - Responsivo

### 🔒 Segurança
- Hash de senhas com SHA256
- Validação de senha mínima (6 caracteres)
- Confirmação de senha obrigatória
- Proteção contra exclusão do próprio usuário
- Proteção do último administrador
- Verificação de username único
- Sessão com evento de mudança de estado

### 🛠️ Técnico
- Novo serviço `AuthService`
- Modelo `Usuario` com 9 campos
- Modelo `LoginModel` para autenticação
- Modelo `SessaoUsuario` para gerenciar sessão
- Enum `PerfilUsuario` com 4 níveis
- Event `OnAuthStateChanged` para reatividade
- Métodos de hash de senha estáticos
- CRUD completo no AuthService
- Filtros e buscas implementados
- Registro do serviço como Singleton

### 📚 Documentação
- Guia completo de autenticação (`docs/AUTENTICACAO-V2.5.0.md`)
- Descrição detalhada dos perfis
- Credenciais de teste documentadas
- Casos de uso explicados
- API do AuthService documentada
- Limitações conhecidas listadas
- Roadmap de melhorias

---

## [2.4.0] - 2026-01-07

### ⚙️ Adicionado
- **Módulo de Configuração Centralizado**
  - Página de configuração em `/configuracao`
  - Botão fixo no rodapé do menu lateral (⚙️)
  - 5 seções organizadas (Gestão de Dados, Sistema, Integrações, Dados e Backup, Informações)
  - 13 cards de funcionalidades (2 ativos, 11 futuros)
  - Navegação integrada para Quartos e Importar Dados
  - Badges "Em breve" para funcionalidades futuras
  - Informações do sistema (versão, ambiente, tecnologia)

### ✨ Melhorado
- **Menu Lateral**
  - Removido link direto de "Quartos" (movido para Configuração)
  - Removido link direto de "Importar Dados" (movido para Configuração)
  - Adicionado botão "Configurações" no rodapé
  - Menu mais limpo e focado

- **Organização de Funcionalidades**
  - Gestão de quartos acessível via Configuração → Gestão de Dados → Quartos
  - Importação de dados acessível via Configuração → Dados e Backup → Importar
  - Estrutura preparada para novas funcionalidades

### 🎨 Interface
- **Design do Módulo**
  - Cards interativos com hover effects
  - Ícones grandes com gradientes
  - Animações de entrada (fadeInUp)
  - Layout responsivo em grid
  - Cores e estilos consistentes
  
- **Botão de Configuração**
  - Posição fixa no rodapé
  - Ícone ⚙️ + texto
  - Hover com translação
  - Active state diferenciado

### 📚 Documentação
- Guia completo do módulo (`docs/CONFIGURACAO.md`)
- Estrutura de seções explicada
- Casos de uso detalhados
- Screenshots da interface
- Roadmap de funcionalidades futuras

### 🔧 Técnico
- Navegação programática com `NavigationManager`
- Cards desabilitados com classe CSS
- Grid responsivo com auto-fill
- Animações CSS com keyframes
- Estrutura modular para expansão

---

## [2.3.0] - 2026-01-07

### 🛏️ Adicionado
- **Sistema de Vagas no Calendário de Ocupação**
  - Propriedade `NumeroVagas` no modelo Quarto
  - Visualização de múltiplas linhas por quarto (uma linha por vaga)
  - Criação rápida de reserva clicando em célula disponível
  - Edição de reserva clicando em célula ocupada
  - Modal completo para criar/editar reservas
  - Alteração de datas de check-in/checkout pelo calendário
  - Exclusão de reservas pelo calendário
  - Busca de vagas disponíveis com contador por quarto
  - Estatísticas atualizadas (vagas disponíveis vs total)
  - Botão "➕ Nova Reserva" no cabeçalho

### ✨ Melhorado
- **Modelo Quarto**
  - Campo `NumeroVagas` para definir quantidade de camas/vagas
  - Dados de exemplo atualizados (Quarto 1: 8 vagas, Quarto 2: 6 vagas, etc.)
  - Total de 23 vagas no sistema exemplo
  
- **Calendário de Ocupação**
  - Exibição de linha por vaga ao invés de linha por quarto
  - Rowspan no nome do quarto para agrupar visualmente
  - Tooltips informativos com número da vaga
  - Células clicáveis para interação
  - Hover effect diferenciado para ação
  
- **Gestão de Reservas**
  - Reservas associadas a vagas específicas via campo Observacoes
  - Verificação de disponibilidade por vaga
  - Cálculo de vagas disponíveis por período
  - Prevenção de double booking na mesma vaga

### 🎨 Interface
- Modal de reserva com seleção de vaga
- Contador de vagas disponíveis vs ocupadas
- Visual otimizado para hostels e dormitórios
- Informação de vagas nos cards de busca
- Campo de vaga no formulário de reserva

### 📚 Documentação
- Guia completo do sistema de vagas (`docs/SISTEMA-VAGAS.md`)
- Explicação de múltiplas linhas por quarto
- Casos de uso detalhados
- Comparação antes vs depois
- Melhorias futuras sugeridas

### 🔧 Técnico
- Método `ObterReservaDiaVaga()` para buscar por vaga específica
- Método `GetVagasDisponiveisQuarto()` para contagem
- Lógica de associação vaga-reserva
- Validação de número de vaga no formulário

---

## [2.2.0] - 2026-01-07

### 📊 Adicionado
- **Sistema de Relatórios Detalhados**
  - Página de relatórios com 21 colunas de informações completas
  - Tabela detalhada com todos os dados financeiros e operacionais
  - Filtros por data (início/fim), status e número de reserva
  - Painel de estatísticas com 8 métricas calculadas
  - Exportação para CSV com encoding UTF-8
  - Design moderno com scroll horizontal e vertical
  - Badges coloridos para status e formas de pagamento
  - Integração completa com sistema de importação

### ✨ Melhorado
- **Modelo de Reserva Expandido**
  - Campos financeiros detalhados (taxas, comissões, valores líquidos)
  - Formas de pagamento específicas (Dinheiro, Cartão, PIX, MBWay, etc.)
  - Informações redundantes de hóspede para relatórios
  - Campos de documento e nascimento
  - Informações de quarto e tipo de cama
  - Flag de pagamento online
  - Data de pagamento

- **Serviço de Importação**
  - Preenche automaticamente todos os novos campos
  - Calcula valores derivados (diária livre taxa, líquido)
  - Mapeia inteligentemente formas de pagamento
  - Detecta pagamentos online
  - Armazena informações redundantes para performance

### 🎨 Interface
- Menu lateral atualizado com ícone de relatórios (📊)
- Cabeçalho com gradiente azul consistente
- Tabela responsiva com colunas fixas
- Scrollbar customizado
- Animações de carregamento
- Layout adaptado para mobile/tablet

### 📚 Documentação
- Guia completo de relatórios (`docs/RELATORIOS.md`)
- Descrição de todos os 21 campos
- Instruções de uso e exportação
- Exemplos de casos de uso
- Comparação com outras páginas

### 🛠️ Técnico
- Novo serviço `RelatorioService`
- Modelo `ReservaDetalhada` para exibição
- Modelo `RelatorioEstatisticas` para métricas
- Enum `FormaPagamento` com 8 opções
- JavaScript para download de arquivos
- Registro de serviço no DI container

---

## [2.1.0] - 2026-01-07

### 🎨 Adicionado
- **Calendário de Ocupação Redesenhado (Estilo Beds24)**
  - Design visual moderno com gradientes e cores profissionais
  - Badges visuais de check-in/out (🔑 IN, 🚪 OUT)
  - Sistema de cores inteligente por status (disponível, ocupado, limpeza, manutenção)
  - Coluna de quartos fixa (sticky) com informações detalhadas
  - Cabeçalho de dias fixo ao rolar verticalmente
  - Destaque visual para dia atual e fins de semana
  - Painel de estatísticas do mês (taxa de ocupação, disponibilidade, check-ins/outs)
  - Padrões de listras para status de limpeza e manutenção
  - Modal de busca de disponibilidade melhorado
  - Modal de detalhes da reserva aprimorado

### ✨ Melhorado
- **Interatividade do Calendário**
  - Hover effects nas células com destaque visual
  - Tooltips informativos detalhados
  - Click nas células para abrir detalhes da reserva
  - Navegação intuitiva entre meses (◄ ► botões + botão "Hoje")
  
- **Visual e UX**
  - Grid de calendário otimizado (50x70px por célula)
  - Scroll horizontal suave com scrollbar customizado
  - Informações de reserva visíveis (nome hóspede + duração)
  - Legenda integrada no cabeçalho
  - Gradientes em check-in (verde) e check-out (laranja)
  
- **Responsividade**
  - Layout adaptado para desktop, tablet e mobile
  - Estatísticas em grid responsivo
  - Fontes e tamanhos otimizados por dispositivo

### 📚 Documentação
- Guia completo de melhorias do calendário (`docs/CALENDARIO-MELHORIAS.md`)
- Comparação antes vs depois
- Referências e inspirações de design
- Sugestões de melhorias futuras

### 🐛 Corrigido
- Warning CS8602 em `Disponibilidade.razor` (verificação de nulo para `reserva.Hospede`)

---

## [2.0.0] - 2026-01-07

### 🎉 Adicionado
- **Sistema de Importação de Dados Completo**
  - Importação de reservas em massa via CSV/TSV/TXT
  - Detecção automática de separador (vírgula, tabulação, ponto-e-vírgula)
  - Preview completo dos dados antes de importar
  - Validação inteligente com erros e avisos diferenciados
  - Criação automática de hóspedes e quartos
  - Suporte a múltiplos formatos de data e valores monetários
  - Estatísticas detalhadas de processamento
  - Interface visual moderna com drag & drop
  - Feedback visual com animações e spinners
  - Documentação completa de uso

### ✨ Melhorado
- **Validações de Importação**
  - Diferenciação entre erros críticos (❌) e avisos (⚠️)
  - Validação de cabeçalho do arquivo
  - Validação de datas dentro de período razoável (±2 anos)
  - Extração inteligente de número do quarto (múltiplos padrões)
  - Mensagens de erro mais claras e acionáveis

- **Interface de Importação**
  - Upload com área de arrastar e soltar
  - Cards coloridos para estatísticas
  - Tabela de preview formatada
  - Badges visuais para status
  - Animações suaves (bounce, spin)
  - Layout responsivo
  - Botões com estados de loading

- **Processamento de Dados**
  - Suporte a CSV com campos entre aspas
  - Limpeza automática de textos
  - Parsing robusto de valores monetários
  - Múltiplos formatos de data suportados
  - Tratamento de erros aprimorado

### 📚 Documentação
- Guia completo de importação (`docs/IMPORTACAO.md`)
- Resumo de melhorias (`docs/IMPORT-IMPROVEMENTS.md`)
- Exemplos de arquivos CSV e TSV
- Checklist de importação
- Solução de problemas detalhada

### 🛠️ Técnico
- Código refatorado com métodos focados
- Validações centralizadas
- Regex para parsing de campos complexos
- Limite de tamanho de arquivo (10MB)
- Performance otimizada

---

## [1.0.0] - 2026-01-07

### Adicionado
- Dashboard com estatísticas em tempo real
- Sistema completo de gestão de reservas
  - Criação, edição e cancelamento de reservas
  - Filtros por data, status e busca
  - Check-in e check-out rápidos
- Gestão de quartos
  - Cadastro e edição de quartos
  - Filtros por tipo e status
  - Alteração rápida de status
- Gestão de hóspedes
  - Cadastro completo de hóspedes
  - Histórico de reservas
  - Busca por múltiplos critérios
- Calendário de disponibilidade
  - Visualização mensal de ocupação
  - Busca de quartos disponíveis
- Relatórios financeiros
  - Receitas e comissões
  - Taxa de ocupação e RevPAR
  - Top hóspedes
  - Estatísticas detalhadas
- Interface moderna inspirada no Booking.com
- Dados de exemplo para demonstração
- Sistema de navegação lateral
- Badges de status coloridos
- Modais para criação/edição
- Layout responsivo

### Tecnologias
- ASP.NET Core 8.0
- Blazor Server
- C# 12
- CSS3 customizado

### Documentação
- README.md completo
- CONTRIBUTING.md
- LICENSE (MIT)
- Dockerfile e docker-compose.yml
- GitHub Actions workflow
- Configurações do VS Code

[2.5.1]: https://github.com/avilaops/hotelaria/releases/tag/v2.5.1
[2.5.0]: https://github.com/avilaops/hotelaria/releases/tag/v2.5.0
[2.4.0]: https://github.com/avilaops/hotelaria/releases/tag/v2.4.0
[2.3.0]: https://github.com/avilaops/hotelaria/releases/tag/v2.3.0
[2.2.0]: https://github.com/avilaops/hotelaria/releases/tag/v2.2.0
[2.1.0]: https://github.com/avilaops/hotelaria/releases/tag/v2.1.0
[2.0.0]: https://github.com/avilaops/hotelaria/releases/tag/v2.0.0
[1.0.0]: https://github.com/avilaops/hotelaria/releases/tag/v1.0.0

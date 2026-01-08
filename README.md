# 🏨 Hotelaria - Sistema de Gestão Hoteleira

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)
[![Build Status](https://github.com/avilaops/hotelaria/actions/workflows/dotnet.yml/badge.svg)](https://github.com/avilaops/hotelaria/actions)
[![Staging](https://github.com/avilaops/hotelaria/actions/workflows/staging.yml/badge.svg)](https://github.com/avilaops/hotelaria/actions)

Sistema completo de gestão hoteleira desenvolvido com ASP.NET Core Blazor Server, similar ao painel de controle do Booking.com. Gerencie reservas, quartos, hóspedes e finanças de forma moderna e eficiente.

## ✨ Funcionalidades

### 📊 Dashboard
- Estatísticas em tempo real (reservas, check-ins, pendências)
- Resumo de ocupação de quartos
- Próximos check-ins e check-outs do dia
- Indicadores financeiros e taxa de ocupação

### 📋 Gestão de Reservas
- Listagem completa com filtros avançados
- Busca por nome de hóspede ou número de reserva
- Filtros por data de check-in/check-out e status
- Ações rápidas de check-in e check-out
- Cálculo automático de valores e comissões
- Histórico completo de reservas

### 🛏️ Gestão de Quartos
- Visualização em cards com informações detalhadas
- Filtros por status (disponível, ocupado, limpeza, manutenção)
- Filtros por tipo (standard, deluxe, suíte, presidential)
- Alteração rápida de status
- Cadastro de comodidades e descrições

### 👥 Gestão de Hóspedes
- Cadastro completo de hóspedes
- Histórico de reservas por hóspede
- Badges VIP para hóspedes frequentes
- Busca por nome, email ou documento
- Estatísticas individuais (total gasto, número de reservas)

### 📅 Disponibilidade
- Busca de quartos disponíveis por período
- Calendário visual de ocupação mensal
- Legenda de status em cores
- Cálculo de preços por período

### 💰 Financeiro
- Resumo de receitas e comissões
- Receitas por tipo de pagamento
- Receitas por tipo de quarto
- Taxa de ocupação e RevPAR
- Top 5 hóspedes
- Lista de transações recentes
- Estatísticas detalhadas (diária média, tempo médio de estadia)

## 🚀 Tecnologias

- **Framework**: ASP.NET Core 8.0
- **UI**: Blazor Server
- **Linguagem**: C# 12
- **Estilo**: CSS3 customizado (similar ao Booking.com)
- **Armazenamento**: In-Memory (pode ser facilmente adaptado para Entity Framework)
- **CI/CD**: GitHub Actions com deploy automático para Azure

## 📋 Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior
- IDE: Visual Studio 2022, VS Code ou Rider
- Navegador moderno (Chrome, Firefox, Edge, Safari)

> 📖 Para instruções detalhadas de instalação em diferentes sistemas operacionais, consulte o [Guia de Instalação](docs/INSTALLATION.md).

## 🔧 Instalação

### Início Rápido

**Windows:**
```bash
git clone https://github.com/avilaops/hotelaria.git
cd hotelaria
start.bat
```

**Linux/macOS:**
```bash
git clone https://github.com/avilaops/hotelaria.git
cd hotelaria
chmod +x start.sh
./start.sh
```

### 1. Clone o repositório

```bash
git clone https://github.com/avilaops/hotelaria.git
cd hotelaria
```

### 2. Restaurar dependências

```bash
dotnet restore
```

### 3. Compilar o projeto

```bash
dotnet build
```

### 4. Executar a aplicação

```bash
dotnet run
```

A aplicação estará disponível em:
- **HTTPS**: https://localhost:5001
- **HTTP**: http://localhost:5000

## 📁 Estrutura do Projeto

```
Hotelaria/
├── Models/              # Modelos de dados
│   ├── Hospede.cs      # Modelo de hóspede
│   ├── Quarto.cs       # Modelo de quarto e enums
│   └── Reserva.cs      # Modelo de reserva e enums
├── Services/           # Serviços de negócio
│   ├── HospedeService.cs
│   ├── QuartoService.cs
│   └── ReservaService.cs
├── Pages/              # Páginas Blazor
│   ├── Index.razor           # Dashboard
│   ├── Reservas.razor        # Gestão de reservas
│   ├── Quartos.razor         # Gestão de quartos
│   ├── Hospedes.razor        # Gestão de hóspedes
│   ├── Disponibilidade.razor # Calendário
│   ├── Financeiro.razor      # Relatórios
│   └── _Host.cshtml          # Host page
├── Shared/             # Componentes compartilhados
│   └── MainLayout.razor      # Layout principal
├── wwwroot/            # Arquivos estáticos
│   └── css/
│       └── site.css          # Estilos customizados
├── App.razor           # Componente raiz
├── Program.cs          # Ponto de entrada
└── _Imports.razor      # Imports globais
```

## 🎨 Design

O design da aplicação foi inspirado no painel de controle do Booking.com, com:
- Interface limpa e moderna
- Paleta de cores profissional (azul #003580 como cor principal)
- Layout responsivo
- Componentes reutilizáveis
- Feedback visual claro (badges, cores de status)
- Navegação intuitiva

## 🗄️ Modelos de Dados

### Hóspede
- ID, Nome, Email, Telefone
- Documento, País
- Data de cadastro
- Lista de reservas

### Quarto
- ID, Número
- Tipo (Standard, Deluxe, Suíte, Presidential)
- Capacidade, Preço por noite
- Status (Disponível, Ocupado, Limpeza, Manutenção)
- Descrição e Comodidades

### Reserva
- ID, Número de reserva
- Hóspede e Quarto (relacionamentos)
- Check-in, Check-out, Data da reserva
- Status (Pendente, Confirmada, Check-in realizado, etc)
- Valor total, Comissão
- Tipo de pagamento
- Número de adultos/crianças
- Observações

## 📊 Dados de Exemplo

A aplicação vem pré-configurada com:
- 6 hóspedes de diferentes países
- 7 quartos de diversos tipos
- 6 reservas de exemplo
- Dados financeiros para demonstração

## 🚢 Deploy

### 🔄 CI/CD Automático (Recomendado)

O projeto possui pipeline completo de CI/CD com GitHub Actions:

**Pipeline Completo:**
```
Push → Build → Test → Code Analysis → Publish → Deploy → Health Check
```

**Features:**
- ✅ Build e testes automáticos
- ✅ Análise de código (CodeQL)
- ✅ Verificação de vulnerabilidades
- ✅ Deploy automático para Azure
- ✅ Health checks pós-deploy
- ✅ Ambientes Production e Staging
- ✅ Atualizações automáticas de dependências (Dependabot)

**Setup:**
1. Configure os secrets do GitHub (publish profiles)
2. Configure os ambientes Production e Staging
3. Faça push para `main` (production) ou `develop` (staging)
4. O deploy acontece automaticamente! 🚀

📖 **[Guia Completo de CI/CD](docs/GITHUB-ACTIONS-SETUP.md)** - Configuração detalhada do GitHub Actions

**Validação Local:**
Antes de fazer push, valide localmente:

```bash
# Windows
.\scripts\validate-ci.ps1

# Linux/macOS
chmod +x scripts/validate-ci.sh
./scripts/validate-ci.sh
```

### Docker

Criar `Dockerfile`:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Hotelaria.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hotelaria.dll"]
```

Executar:
```bash
docker build -t hotelaria .
docker run -d -p 8080:80 hotelaria
```

### Azure App Service

**Deploy Rápido (5 minutos):**

1. **Portal Azure** → Criar Web App
2. **Configuração:**
   - Nome: `hotelaria-app`
   - Runtime: `.NET 8`
   - Plano: `B1` (R$ ~55/mês)
   - Região: `Brazil South`
3. **Deployment:** Conectar GitHub (avilaops/hotelaria)
4. **Acessar:** `https://hotelaria-app.azurewebsites.net`

📖 **Guias de Deploy:**
- [Guia Completo Azure](docs/AZURE-DEPLOY.md) - Todos os métodos e configurações
- [Quick Start Azure](docs/AZURE-QUICKSTART.md) - Deploy em 5 minutos
- [Deploy via CLI](docs/AZURE-DEPLOY.md#método-2-deploy-via-azure-cli-avançado)
- [Deploy Automático via GitHub Actions](docs/AZURE-DEPLOY.md#método-3-deploy-automatizado-via-github-actions-recomendado)

### Render

📖 Ver [Guia de Deploy no Render](docs/RENDER-DEPLOY.md) para instruções detalhadas.

### Outras Plataformas

- **Heroku**: Suporte via Dockerfile
- **AWS Elastic Beanstalk**: Suporte para .NET 8
- **Google Cloud Run**: Container pronto para deploy

## 🧪 Testes

Execute os testes localmente:

```bash
# Todos os testes
dotnet test

# Com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Verboso
dotnet test --verbosity detailed
```

## 🔒 Segurança

- ✅ Análise automática de código (CodeQL)
- ✅ Scan de vulnerabilidades
- ✅ Atualizações automáticas de dependências
- ✅ HTTPS por padrão

## 🤝 Contribuindo

Contribuições são bem-vindas! Para contribuir:

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

**Workflow de Contribuição:**
1. Fork → Branch → Código → Commit
2. Execute `validate-ci.ps1` localmente
3. Push → Pull Request
4. CI automático valida seu código
5. Review → Merge → Deploy automático! 🎉

> 📖 Leia nosso [Guia de Contribuição](CONTRIBUTING.md) para mais detalhes sobre o processo e padrões de código.

## 📊 Status do Projeto

| Branch | Status | Deploy |
|--------|--------|--------|
| `main` | ![Build](https://github.com/avilaops/hotelaria/actions/workflows/dotnet.yml/badge.svg?branch=main) | Production |
| `develop` | ![Build](https://github.com/avilaops/hotelaria/actions/workflows/staging.yml/badge.svg?branch=develop) | Staging |

## 📝 Roadmap

- [ ] Autenticação e autorização
- [ ] Integração com Entity Framework Core
- [ ] Relatórios em PDF/Excel
- [ ] Notificações por email
- [ ] API REST
- [ ] App mobile (Blazor Hybrid)
- [ ] Multi-idioma (i18n)

> 📖 Veja o [CHANGELOG](CHANGELOG.md) para histórico de versões.

### 6️⃣ **Relatórios Financeiros**
✅ Dashboard completo:
- Receita total, comissões, líquida
- Taxa de ocupação e RevPAR
- Receitas por tipo de pagamento
- Receitas por tipo de quarto
- Top 5 hóspedes
- Transações recentes
- Estatísticas detalhadas (diária média, tempo médio de estadia)

### 7️⃣ **Importação de Dados**
✅ **Novo!** Sistema completo de importação:
- Upload de arquivos CSV/TSV (Excel)
- Validação automática de dados
- Preview antes de importar
- Criação automática de hóspedes e quartos
- Tratamento de erros linha por linha
- Suporte para múltiplos formatos de data e moeda
- [Ver Guia Completo](docs/IMPORTACAO.md)

---

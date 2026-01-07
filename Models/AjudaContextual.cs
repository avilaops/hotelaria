namespace Hotelaria.Models
{
    public class AjudaContextual
    {
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<string> TopicosAjuda { get; set; } = new();
        public List<AtalhoTeclado> Atalhos { get; set; } = new();
        public string VideoUrl { get; set; } = string.Empty;
    }

    public class AtalhoTeclado
    {
        public string Tecla { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string Funcao { get; set; } = string.Empty;
    }

    public static class AjudaModulos
    {
        public static Dictionary<string, AjudaContextual> Modulos = new()
        {
            ["principal"] = new AjudaContextual
            {
                Titulo = "📊 Página Principal - Ajuda",
                Descricao = "Visão geral do sistema com estatísticas em tempo real",
                TopicosAjuda = new List<string>
                {
                    "Visualize total de reservas, confirmadas, check-ins e pendentes",
                    "Acompanhe status dos quartos (disponíveis, ocupados, limpeza, manutenção)",
                    "Veja próximos check-ins e check-outs do dia",
                    "Monitore resumo financeiro mensal e taxa de ocupação"
                },
                Atalhos = new List<AtalhoTeclado>
                {
                    new() { Tecla = "F1", Descricao = "Abrir esta ajuda", Funcao = "Ajuda contextual" },
                    new() { Tecla = "F5", Descricao = "Atualizar dados", Funcao = "Refresh da página" },
                    new() { Tecla = "Ctrl + R", Descricao = "Ir para Reservas", Funcao = "Navegação rápida" }
                }
            },
            ["reservas"] = new AjudaContextual
            {
                Titulo = "📋 Reservas - Ajuda",
                Descricao = "Gerencie todas as reservas do hotel",
                TopicosAjuda = new List<string>
                {
                    "Clique em '➕ Nova Reserva' para criar uma reserva",
                    "Use filtros por data, status e busca por nome/número",
                    "Realize check-in/check-out rápido com os botões de ação",
                    "Edite reservas clicando no botão ✏️",
                    "Cadastre hóspedes rapidamente com o botão ➕ no formulário"
                },
                Atalhos = new List<AtalhoTeclado>
                {
                    new() { Tecla = "F2", Descricao = "Nova Reserva", Funcao = "Abrir modal de criação" },
                    new() { Tecla = "F3", Descricao = "Buscar", Funcao = "Focar campo de busca" },
                    new() { Tecla = "F5", Descricao = "Atualizar lista", Funcao = "Recarregar dados" },
                    new() { Tecla = "Esc", Descricao = "Fechar modal", Funcao = "Cancelar ação" }
                }
            },
            ["quartos"] = new AjudaContextual
            {
                Titulo = "🛏️ Quartos - Ajuda",
                Descricao = "Gerencie quartos e seus status",
                TopicosAjuda = new List<string>
                {
                    "Clique no botão ➕ circular para adicionar novo quarto",
                    "Filtre por status (Disponível, Ocupado, Limpeza, Manutenção)",
                    "Filtre por tipo (Standard, Deluxe, Suíte, Presidential)",
                    "Altere status rapidamente com o botão '🔄 Status'",
                    "Edite ou exclua quartos com os botões de ação"
                },
                Atalhos = new List<AtalhoTeclado>
                {
                    new() { Tecla = "F2", Descricao = "Novo Quarto", Funcao = "Abrir formulário" },
                    new() { Tecla = "F3", Descricao = "Filtrar", Funcao = "Focar filtros" },
                    new() { Tecla = "F5", Descricao = "Atualizar", Funcao = "Recarregar lista" }
                }
            },
            ["hospedes"] = new AjudaContextual
            {
                Titulo = "👥 Hóspedes - Ajuda",
                Descricao = "Gerencie cadastro de hóspedes",
                TopicosAjuda = new List<string>
                {
                    "Visualize todos os hóspedes cadastrados",
                    "Veja histórico completo de reservas por hóspede",
                    "Badge VIP é atribuído automaticamente para hóspedes frequentes",
                    "Use busca por nome, email ou documento",
                    "Acompanhe estatísticas individuais (total gasto, número de reservas)"
                },
                Atalhos = new List<AtalhoTeclado>
                {
                    new() { Tecla = "F2", Descricao = "Novo Hóspede", Funcao = "Cadastrar" },
                    new() { Tecla = "F3", Descricao = "Buscar", Funcao = "Focar busca" },
                    new() { Tecla = "F5", Descricao = "Atualizar", Funcao = "Recarregar" }
                }
            },
            ["disponibilidade"] = new AjudaContextual
            {
                Titulo = "📅 Disponibilidade - Ajuda",
                Descricao = "Consulte disponibilidade e calendário de ocupação",
                TopicosAjuda = new List<string>
                {
                    "Use a busca para encontrar quartos disponíveis por período",
                    "Calendário mostra ocupação mensal de todos os quartos",
                    "Clique nas reservas (células vermelhas) para editar dados do hóspede",
                    "Verde = Disponível, Vermelho = Reservado, Amarelo = Limpeza, Azul = Manutenção",
                    "Use os botões para navegar entre meses ou voltar para hoje"
                },
                Atalhos = new List<AtalhoTeclado>
                {
                    new() { Tecla = "F3", Descricao = "Buscar quartos", Funcao = "Focar busca" },
                    new() { Tecla = "F5", Descricao = "Hoje", Funcao = "Voltar ao mês atual" },
                    new() { Tecla = "←", Descricao = "Mês anterior", Funcao = "Navegar calendário" },
                    new() { Tecla = "→", Descricao = "Próximo mês", Funcao = "Navegar calendário" }
                }
            },
            ["financeiro"] = new AjudaContextual
            {
                Titulo = "💰 Financeiro - Ajuda",
                Descricao = "Relatórios e análises financeiras",
                TopicosAjuda = new List<string>
                {
                    "Visualize resumo completo: receitas, comissões e lucro líquido",
                    "Analise receitas por tipo de pagamento",
                    "Veja receitas por tipo de quarto",
                    "Acompanhe taxa de ocupação e RevPAR",
                    "Consulte Top 5 hóspedes que mais gastaram",
                    "Revise transações recentes com todos os detalhes"
                },
                Atalhos = new List<AtalhoTeclado>
                {
                    new() { Tecla = "F5", Descricao = "Atualizar dados", Funcao = "Recarregar relatório" },
                    new() { Tecla = "Ctrl + P", Descricao = "Imprimir", Funcao = "Imprimir relatório" }
                }
            },
            ["importar"] = new AjudaContextual
            {
                Titulo = "📥 Importar Dados - Ajuda",
                Descricao = "Importe múltiplas reservas de uma vez",
                TopicosAjuda = new List<string>
                {
                    "Exporte sua planilha Excel como TSV (Texto Separado por Tabulações)",
                    "Selecione o arquivo e aguarde o processamento automático",
                    "Revise erros e dados válidos antes de confirmar",
                    "Sistema cria automaticamente hóspedes e quartos novos",
                    "Consulte o guia completo em docs/IMPORTACAO.md"
                },
                Atalhos = new List<AtalhoTeclado>
                {
                    new() { Tecla = "F2", Descricao = "Selecionar arquivo", Funcao = "Abrir seletor" },
                    new() { Tecla = "F5", Descricao = "Reprocessar", Funcao = "Processar novamente" },
                    new() { Tecla = "Enter", Descricao = "Confirmar importação", Funcao = "Importar dados" }
                }
            }
        };
    }
}

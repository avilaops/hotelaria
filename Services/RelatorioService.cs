using Hotelaria.Models;

namespace Hotelaria.Services
{
    public class RelatorioService
    {
        private readonly ReservaService _reservaService;
        private readonly HospedeService _hospedeService;
        private readonly QuartoService _quartoService;

        public RelatorioService(
            ReservaService reservaService,
            HospedeService hospedeService,
            QuartoService quartoService)
        {
            _reservaService = reservaService;
            _hospedeService = hospedeService;
            _quartoService = quartoService;
        }

        // Obter todas as reservas com informações completas
        public List<ReservaDetalhada> ObterReservasDetalhadas(
            DateTime? dataInicio = null,
            DateTime? dataFim = null,
            StatusReserva? status = null,
            string? numeroReserva = null)
        {
            var reservas = _reservaService.ObterTodas();

            // Aplicar filtros
            if (dataInicio.HasValue)
                reservas = reservas.Where(r => r.CheckIn >= dataInicio.Value).ToList();

            if (dataFim.HasValue)
                reservas = reservas.Where(r => r.CheckOut <= dataFim.Value).ToList();

            if (status.HasValue)
                reservas = reservas.Where(r => r.Status == status.Value).ToList();

            if (!string.IsNullOrWhiteSpace(numeroReserva))
                reservas = reservas.Where(r => r.NumeroReserva.Contains(numeroReserva, StringComparison.OrdinalIgnoreCase)).ToList();

            // Mapear para modelo detalhado
            return reservas.Select(r => new ReservaDetalhada
            {
                Id = r.Id,
                NumeroReserva = r.NumeroReserva,
                NomeHospede = r.Hospede?.Nome ?? "N/A",
                EmailHospede = r.Hospede?.Email ?? "",
                TelefoneHospede = r.Hospede?.Telefone ?? "",
                DocumentoHospede = r.Hospede?.Documento ?? "",
                PaisHospede = r.Hospede?.Pais ?? "",
                NumeroQuarto = r.Quarto?.Numero ?? "",
                TipoQuarto = r.Quarto?.Tipo.ToString() ?? "",
                Cama = r.TipoCama ?? $"Q {r.Quarto?.Numero}",
                CheckIn = r.CheckIn,
                CheckOut = r.CheckOut,
                Dias = r.DiasHospedagem,
                Pessoas = r.TotalPessoas,
                Valor = r.ValorTotal,
                ValorComissao = r.Comissao,
                PagoOnline = r.PagoOnline,
                TaxaTurismo = r.TaxaTurismo,
                NumeroPessoas = r.TotalPessoas,
                DiariaLivreTaxa = r.DiariaLivreTaxa,
                Total = r.ValorComissaoMaisTaxa > 0 ? r.ValorComissaoMaisTaxa : r.ValorTotal,
                LivreTx = r.LivreTx,
                DiariaForaPaga = r.DiariaForaPaga,
                FormaPagamento = GetFormaPagamentoTexto(r.FormaPagamento),
                DataPagamento = r.DataPagamento,
                Status = r.Status,
                QuartoNumero = r.Quarto?.Numero ?? r.NumeroQuarto ?? "N/A",
                NumeroDocumento = r.NumeroDocumentoHospede ?? r.Hospede?.Documento ?? "",
                Pais = r.PaisHospede ?? r.Hospede?.Pais ?? "",
                TipoDocumento = r.TipoDocumentoHospede ?? "",
                DataNascimento = r.DataNascimentoHospede
            }).OrderByDescending(r => r.CheckIn).ToList();
        }

        // Estatísticas gerais
        public RelatorioEstatisticas ObterEstatisticas(DateTime? dataInicio = null, DateTime? dataFim = null)
        {
            var reservas = ObterReservasDetalhadas(dataInicio, dataFim);

            return new RelatorioEstatisticas
            {
                TotalReservas = reservas.Count,
                TotalHospedes = reservas.Select(r => r.NumeroDocumento).Distinct().Count(),
                TotalDiarias = reservas.Sum(r => r.Dias),
                TotalPessoas = reservas.Sum(r => r.Pessoas),
                ReceitaTotal = reservas.Sum(r => r.Valor),
                ComissaoTotal = reservas.Sum(r => r.ValorComissao),
                TaxaTurismoTotal = reservas.Sum(r => r.TaxaTurismo),
                ReceitaLiquida = reservas.Sum(r => r.LivreTx),
                MediaDiaria = reservas.Any() ? reservas.Average(r => r.DiariaLivreTaxa) : 0,
                MediaEstadia = reservas.Any() ? reservas.Average(r => r.Dias) : 0,
                PagamentosOnline = reservas.Count(r => r.PagoOnline),
                PagamentosPresencial = reservas.Count(r => !r.PagoOnline)
            };
        }

        // Exportar para CSV
        public string ExportarParaCSV(List<ReservaDetalhada> reservas)
        {
            var csv = new System.Text.StringBuilder();
            
            // Cabeçalho
            csv.AppendLine("Nome;Nascimento;Nº Documento;País;Tipo Documento;Cama;Check-in;Check-out;Dias;Pessoas;Valor;Comissão;Pago Online;Taxa Turismo;Nº Reserva;Diária Livre Taxa;Total;Livre Tx;Diária Fora Paga;Forma Pagamento;Data Pagamento");

            // Dados
            foreach (var r in reservas)
            {
                csv.AppendLine($"{r.NomeHospede};{r.DataNascimento?.ToString("dd/MM/yyyy")};{r.NumeroDocumento};{r.Pais};{r.TipoDocumento};{r.Cama};{r.CheckIn:dd/MM/yyyy};{r.CheckOut:dd/MM/yyyy};{r.Dias};{r.Pessoas};{r.Valor:F2};{r.ValorComissao:F2};{(r.PagoOnline ? "Sim" : "Não")};{r.TaxaTurismo:F2};{r.NumeroReserva};{r.DiariaLivreTaxa:F2};{r.Total:F2};{r.LivreTx:F2};{r.DiariaForaPaga:F2};{r.FormaPagamento};{r.DataPagamento?.ToString("dd/MM/yyyy")}");
            }

            return csv.ToString();
        }

        private string GetFormaPagamentoTexto(FormaPagamento forma)
        {
            return forma switch
            {
                FormaPagamento.Dinheiro => "Dinheiro",
                FormaPagamento.CartaoCredito => "Cartão de Crédito",
                FormaPagamento.CartaoDebito => "Cartão de Débito",
                FormaPagamento.TransferenciaBancaria => "Transferência Bancária",
                FormaPagamento.PIX => "PIX",
                FormaPagamento.Online => "Online",
                FormaPagamento.MBWay => "MB Way",
                FormaPagamento.Multibanco => "Multibanco",
                _ => "Não Definido"
            };
        }
    }

    // Modelo para exibição detalhada
    public class ReservaDetalhada
    {
        public int Id { get; set; }
        public string NumeroReserva { get; set; } = string.Empty;
        public string NomeHospede { get; set; } = string.Empty;
        public string EmailHospede { get; set; } = string.Empty;
        public string TelefoneHospede { get; set; } = string.Empty;
        public string DocumentoHospede { get; set; } = string.Empty;
        public string PaisHospede { get; set; } = string.Empty;
        public string NumeroQuarto { get; set; } = string.Empty;
        public string TipoQuarto { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string Cama { get; set; } = string.Empty;
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int Dias { get; set; }
        public int Pessoas { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorComissao { get; set; }
        public bool PagoOnline { get; set; }
        public decimal TaxaTurismo { get; set; }
        public int NumeroPessoas { get; set; }
        public decimal DiariaLivreTaxa { get; set; }
        public decimal Total { get; set; }
        public decimal LivreTx { get; set; }
        public decimal DiariaForaPaga { get; set; }
        public string FormaPagamento { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
        public StatusReserva Status { get; set; }
        public string QuartoNumero { get; set; } = string.Empty;
    }

    // Modelo para estatísticas
    public class RelatorioEstatisticas
    {
        public int TotalReservas { get; set; }
        public int TotalHospedes { get; set; }
        public int TotalDiarias { get; set; }
        public int TotalPessoas { get; set; }
        public decimal ReceitaTotal { get; set; }
        public decimal ComissaoTotal { get; set; }
        public decimal TaxaTurismoTotal { get; set; }
        public decimal ReceitaLiquida { get; set; }
        public decimal MediaDiaria { get; set; }
        public double MediaEstadia { get; set; }
        public int PagamentosOnline { get; set; }
        public int PagamentosPresencial { get; set; }
    }
}

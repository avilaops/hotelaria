using Hotelaria.Models;
using System.Globalization;

namespace Hotelaria.Services
{
    public class ImportacaoService
    {
        private readonly HospedeService _hospedeService;
        private readonly QuartoService _quartoService;
        private readonly ReservaService _reservaService;

        public ImportacaoService(
            HospedeService hospedeService,
            QuartoService quartoService,
            ReservaService reservaService)
        {
            _hospedeService = hospedeService;
            _quartoService = quartoService;
            _reservaService = reservaService;
        }

        public ImportacaoResultado ProcessarCSV(string conteudoCSV)
        {
            var resultado = new ImportacaoResultado();
            var linhas = conteudoCSV.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (linhas.Length == 0)
            {
                resultado.Erros.Add("Arquivo vazio");
                return resultado;
            }

            resultado.TotalLinhas = linhas.Length - 1; // Excluir cabeçalho

            for (int i = 1; i < linhas.Length; i++)
            {
                try
                {
                    var dados = ProcessarLinha(linhas[i], i + 1);
                    resultado.DadosProcessados.Add(dados);
                    
                    if (dados.IsValid)
                    {
                        resultado.LinhasImportadas++;
                    }
                    else
                    {
                        resultado.LinhasComErro++;
                        resultado.Erros.AddRange(dados.Erros.Select(e => $"Linha {i + 1}: {e}"));
                    }
                }
                catch (Exception ex)
                {
                    resultado.LinhasComErro++;
                    resultado.Erros.Add($"Linha {i + 1}: Erro ao processar - {ex.Message}");
                }
            }

            return resultado;
        }

        private ReservaImport ProcessarLinha(string linha, int numeroLinha)
        {
            var colunas = linha.Split('\t'); // Assumindo TSV (Tab Separated Values)
            var dados = new ReservaImport();

            try
            {
                // Mapear colunas baseado na planilha
                if (colunas.Length >= 20)
                {
                    dados.Nome = colunas[0]?.Trim() ?? "";
                    dados.DataNascimento = ParseData(colunas[1]);
                    dados.NumeroDocumento = colunas[2]?.Trim() ?? "";
                    dados.Pais = colunas[3]?.Trim() ?? "";
                    dados.TipoDocumento = colunas[4]?.Trim() ?? "";
                    dados.Cama = colunas[5]?.Trim() ?? "";
                    dados.CheckIn = ParseData(colunas[6]);
                    dados.CheckOut = ParseData(colunas[7]);
                    dados.DiasPessoas = ParseInt(colunas[8]);
                    dados.ValorPagamento = ParseDecimal(colunas[9]);
                    dados.TipoPagamento = colunas[10]?.Trim() ?? "";
                    dados.TaxaBooking = ParseDecimal(colunas[11]);
                    dados.TaxaPagamento = ParseDecimal(colunas[12]);
                    dados.NumeroReserva = colunas[13]?.Trim() ?? "";
                    dados.Diaria = ParseDecimal(colunas[14]);
                    dados.Total = ParseDecimal(colunas[15]);
                    dados.FormaPagamento = colunas[18]?.Trim() ?? "";
                    dados.DataPagamento = ParseData(colunas[19]);

                    // Extrair número do quarto da coluna "Cama"
                    ExtractQuartoEPessoas(dados);

                    // Validar dados essenciais
                    ValidarDados(dados);
                }
                else
                {
                    dados.IsValid = false;
                    dados.Erros.Add($"Número insuficiente de colunas ({colunas.Length})");
                }
            }
            catch (Exception ex)
            {
                dados.IsValid = false;
                dados.Erros.Add($"Erro ao processar: {ex.Message}");
            }

            return dados;
        }

        private void ExtractQuartoEPessoas(ReservaImport dados)
        {
            // Extrair número do quarto (ex: "Q 3 - Cama 01" -> 3)
            var cama = dados.Cama;
            if (cama.Contains("Q "))
            {
                var partes = cama.Split(new[] { ' ', '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (partes.Length >= 2 && int.TryParse(partes[1], out int numeroQuarto))
                {
                    dados.NumeroQuarto = numeroQuarto;
                }
            }

            // Extrair número de pessoas dos "Dias Pessoas" (assumindo formato)
            dados.NumeroAdultos = dados.DiasPessoas > 0 ? 1 : 0;
            dados.NumeroCriancas = 0;
        }

        private void ValidarDados(ReservaImport dados)
        {
            if (string.IsNullOrWhiteSpace(dados.Nome))
            {
                dados.IsValid = false;
                dados.Erros.Add("Nome é obrigatório");
            }

            if (!dados.CheckIn.HasValue)
            {
                dados.IsValid = false;
                dados.Erros.Add("Data de Check-in inválida");
            }

            if (!dados.CheckOut.HasValue)
            {
                dados.IsValid = false;
                dados.Erros.Add("Data de Check-out inválida");
            }

            if (dados.CheckIn >= dados.CheckOut)
            {
                dados.IsValid = false;
                dados.Erros.Add("Check-out deve ser posterior ao Check-in");
            }

            if (string.IsNullOrWhiteSpace(dados.NumeroReserva))
            {
                dados.IsValid = false;
                dados.Erros.Add("Número de reserva é obrigatório");
            }
        }

        public void ImportarParaSistema(List<ReservaImport> dados)
        {
            foreach (var item in dados.Where(d => d.IsValid))
            {
                try
                {
                    // Criar ou encontrar hóspede
                    var hospede = _hospedeService.ObterTodos()
                        .FirstOrDefault(h => h.Documento == item.NumeroDocumento);

                    if (hospede == null)
                    {
                        hospede = new Hospede
                        {
                            Nome = item.Nome,
                            Email = $"{item.Nome.Replace(" ", "").ToLower()}@importado.com",
                            Telefone = "",
                            Documento = item.NumeroDocumento,
                            Pais = item.Pais,
                            DataCadastro = DateTime.Now
                        };
                        _hospedeService.AdicionarHospede(hospede);
                    }

                    // Encontrar ou criar quarto
                    var quarto = _quartoService.ObterTodos()
                        .FirstOrDefault(q => q.Numero == item.NumeroQuarto.ToString());

                    if (quarto == null)
                    {
                        // Criar quarto se não existir
                        quarto = new Quarto
                        {
                            Numero = item.NumeroQuarto.ToString(),
                            Tipo = TipoQuarto.Standard,
                            Capacidade = 2,
                            PrecoPorNoite = item.Diaria,
                            Status = StatusQuarto.Disponivel,
                            Descricao = "Quarto importado",
                            Comodidades = new List<string> { "Wi-Fi", "TV", "Ar condicionado" }
                        };
                        _quartoService.AdicionarQuarto(quarto);
                    }

                    // Criar reserva
                    var reserva = new Reserva
                    {
                        NumeroReserva = item.NumeroReserva,
                        HospedeId = hospede.Id,
                        QuartoId = quarto.Id,
                        CheckIn = item.CheckIn!.Value,
                        CheckOut = item.CheckOut!.Value,
                        DataReserva = DateTime.Now,
                        Status = DateTime.Now >= item.CheckIn ? StatusReserva.CheckInRealizado : StatusReserva.Confirmada,
                        ValorTotal = item.Total,
                        Comissao = item.TaxaBooking,
                        TipoPagamento = MapearTipoPagamento(item.TipoPagamento),
                        NumeroAdultos = item.NumeroAdultos > 0 ? item.NumeroAdultos : 1,
                        NumeroCriancas = item.NumeroCriancas,
                        Observacoes = $"Importado - {item.FormaPagamento}"
                    };

                    _reservaService.AdicionarReserva(reserva);
                }
                catch (Exception ex)
                {
                    // Log erro mas continua processamento
                    Console.WriteLine($"Erro ao importar reserva {item.NumeroReserva}: {ex.Message}");
                }
            }
        }

        private DateTime? ParseData(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return null;

            // Tentar vários formatos
            string[] formatos = { 
                "dd/MM/yyyy", 
                "dd-MM-yyyy",
                "yyyy-MM-dd",
                "MM/dd/yyyy" 
            };

            foreach (var formato in formatos)
            {
                if (DateTime.TryParseExact(valor.Trim(), formato, 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime data))
                {
                    return data;
                }
            }

            return null;
        }

        private int ParseInt(string valor)
        {
            if (int.TryParse(valor?.Trim(), out int resultado))
                return resultado;
            return 0;
        }

        private decimal ParseDecimal(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor)) return 0;
            
            // Remover símbolos de moeda e espaços
            valor = valor.Replace("€", "").Replace("$", "").Replace(" ", "").Trim();
            
            // Tentar parse com diferentes culturas
            if (decimal.TryParse(valor, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal resultado))
                return resultado;
            
            if (decimal.TryParse(valor, NumberStyles.Any, new CultureInfo("pt-PT"), out resultado))
                return resultado;

            return 0;
        }

        private TipoPagamento MapearTipoPagamento(string tipo)
        {
            return tipo.ToLower() switch
            {
                "online" => TipoPagamento.BookingCom,
                "booking" => TipoPagamento.BookingCom,
                "cartao" or "cartão" => TipoPagamento.CartaoCredito,
                "dinheiro" => TipoPagamento.Dinheiro,
                _ => TipoPagamento.TransferenciaBancaria
            };
        }
    }
}

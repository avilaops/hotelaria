using Hotelaria.Models;
using System.Globalization;
using System.Text.RegularExpressions;

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

            // Detectar separador automaticamente (TSV ou CSV)
            char separador = DetectarSeparador(linhas[0]);
            resultado.TotalLinhas = linhas.Length - 1; // Excluir cabeçalho

            // Validar cabeçalho
            var cabecalho = linhas[0].Split(separador);
            if (!ValidarCabecalho(cabecalho, resultado))
            {
                return resultado;
            }

            // Processar linhas de dados
            for (int i = 1; i < linhas.Length; i++)
            {
                try
                {
                    var dados = ProcessarLinha(linhas[i], i + 1, separador);
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

        private char DetectarSeparador(string primeiraLinha)
        {
            // Contar ocorrências de tabulação e vírgula
            int tabs = primeiraLinha.Count(c => c == '\t');
            int virgulas = primeiraLinha.Count(c => c == ',');
            int pontoVirgulas = primeiraLinha.Count(c => c == ';');

            // Retornar o separador mais frequente
            if (tabs >= virgulas && tabs >= pontoVirgulas)
                return '\t';
            if (pontoVirgulas >= virgulas)
                return ';';
            return ',';
        }

        private bool ValidarCabecalho(string[] cabecalho, ImportacaoResultado resultado)
        {
            // Validar número mínimo de colunas
            if (cabecalho.Length < 10)
            {
                resultado.Erros.Add($"Cabeçalho inválido: encontradas {cabecalho.Length} colunas, esperado mínimo de 10");
                resultado.LinhasComErro = 1;
                return false;
            }

            // Campos essenciais esperados
            var camposEssenciais = new[] { "nome", "checkin", "checkout", "reserva" };
            var cabecalhoLower = cabecalho.Select(c => c.ToLower().Trim()).ToArray();

            foreach (var campo in camposEssenciais)
            {
                if (!cabecalhoLower.Any(c => c.Contains(campo)))
                {
                    resultado.Erros.Add($"⚠️ Aviso: Campo '{campo}' não encontrado no cabeçalho");
                }
            }

            return true;
        }

        private ReservaImport ProcessarLinha(string linha, int numeroLinha, char separador)
        {
            var colunas = SepararColunas(linha, separador);
            var dados = new ReservaImport();

            try
            {
                // Mapear colunas baseado na planilha
                if (colunas.Length >= 16)
                {
                    dados.Nome = LimparTexto(colunas[0]);
                    dados.NumeroDocumento = LimparTexto(colunas[1]);
                    dados.Pais = LimparTexto(colunas[2]);
                    dados.TipoDocumento = LimparTexto(colunas[3]);
                    dados.Cama = LimparTexto(colunas[4]);
                    dados.CheckIn = ParseData(colunas[5]);
                    dados.CheckOut = ParseData(colunas[6]);
                    dados.DiasPessoas = ParseInt(colunas[7]);
                    dados.ValorPagamento = ParseDecimal(colunas[8]);
                    dados.TipoPagamento = LimparTexto(colunas[9]);
                    dados.TaxaBooking = ParseDecimal(colunas[10]);
                    dados.TaxaPagamento = ParseDecimal(colunas[11]);
                    dados.NumeroReserva = LimparTexto(colunas[12]);
                    dados.Diaria = ParseDecimal(colunas[13]);
                    dados.Total = ParseDecimal(colunas[14]);
                    
                    // Campos opcionais
                    if (colunas.Length > 18)
                        dados.FormaPagamento = LimparTexto(colunas[18]);
                    if (colunas.Length > 19)
                        dados.DataPagamento = ParseData(colunas[19]);

                    // Extrair número do quarto da coluna "Cama"
                    ExtractQuartoEPessoas(dados);

                    // Validar dados essenciais
                    ValidarDados(dados);
                }
                else
                {
                    dados.IsValid = false;
                    dados.Erros.Add($"Número insuficiente de colunas ({colunas.Length} de 16 esperadas)");
                }
            }
            catch (Exception ex)
            {
                dados.IsValid = false;
                dados.Erros.Add($"Erro ao processar: {ex.Message}");
            }

            return dados;
        }

        private string[] SepararColunas(string linha, char separador)
        {
            // Se for vírgula, considerar aspas para campos com vírgulas dentro
            if (separador == ',')
            {
                var regex = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                return regex.Split(linha)
                    .Select(c => c.Trim('"').Trim())
                    .ToArray();
            }

            return linha.Split(separador);
        }

        private string LimparTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;
            
            return texto.Trim().Trim('"').Trim();
        }

        private void ExtractQuartoEPessoas(ReservaImport dados)
        {
            // Extrair número do quarto (ex: "Q 3 - Cama 01" -> 3)
            var cama = dados.Cama;
            if (!string.IsNullOrEmpty(cama))
            {
                // Tentar encontrar padrão "Q 3" ou "Quarto 3"
                var match = Regex.Match(cama, @"[Qq]\s*(\d+)");
                if (match.Success && int.TryParse(match.Groups[1].Value, out int numeroQuarto))
                {
                    dados.NumeroQuarto = numeroQuarto;
                }
                else
                {
                    // Tentar extrair qualquer número
                    match = Regex.Match(cama, @"(\d+)");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out numeroQuarto))
                    {
                        dados.NumeroQuarto = numeroQuarto;
                    }
                }
            }

            // Se não encontrou quarto, marcar como erro
            if (dados.NumeroQuarto == 0)
            {
                dados.NumeroQuarto = 1; // Quarto padrão
                dados.Erros.Add($"Quarto não identificado na coluna 'Cama': '{cama}'. Atribuído quarto 1");
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

            if (string.IsNullOrWhiteSpace(dados.NumeroDocumento))
            {
                dados.Erros.Add("⚠️ Documento não informado");
            }

            if (!dados.CheckIn.HasValue)
            {
                dados.IsValid = false;
                dados.Erros.Add("Data de Check-in inválida ou não informada");
            }

            if (!dados.CheckOut.HasValue)
            {
                dados.IsValid = false;
                dados.Erros.Add("Data de Check-out inválida ou não informada");
            }

            if (dados.CheckIn.HasValue && dados.CheckOut.HasValue)
            {
                if (dados.CheckIn >= dados.CheckOut)
                {
                    dados.IsValid = false;
                    dados.Erros.Add($"Check-out ({dados.CheckOut:dd/MM/yyyy}) deve ser posterior ao Check-in ({dados.CheckIn:dd/MM/yyyy})");
                }

                // Validar datas não muito antigas ou futuras
                if (dados.CheckIn < DateTime.Now.AddYears(-2))
                {
                    dados.Erros.Add($"⚠️ Check-in muito antigo: {dados.CheckIn:dd/MM/yyyy}");
                }

                if (dados.CheckOut > DateTime.Now.AddYears(2))
                {
                    dados.Erros.Add($"⚠️ Check-out muito distante: {dados.CheckOut:dd/MM/yyyy}");
                }
            }

            if (string.IsNullOrWhiteSpace(dados.NumeroReserva))
            {
                dados.IsValid = false;
                dados.Erros.Add("Número de reserva é obrigatório");
            }

            if (dados.Total <= 0)
            {
                dados.Erros.Add("⚠️ Valor total da reserva é zero ou inválido");
            }

            if (dados.Diaria <= 0)
            {
                dados.Erros.Add("⚠️ Valor da diária é zero ou inválido");
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
                            Nome = item.NomeHospede,
                            Email = item.EmailHospede,
                            Telefone = item.TelefoneHospede,
                            Documento = item.DocumentoHospede,
                            Pais = item.PaisHospede,
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

                    // Determinar forma de pagamento
                    var formaPagamento = MapearFormaPagamento(item.FormaPagamento);
                    var pagoOnline = item.TipoPagamento.ToLower().Contains("online") || 
                                   item.TipoPagamento.ToLower().Contains("booking");

                    // Calcular valores
                    var taxaTurismo = item.TaxaBooking;
                    var comissao = item.TaxaPagamento > 0 ? item.TaxaPagamento : item.TaxaBooking;
                    var valorComissaoMaisTaxa = comissao + taxaTurismo;
                    var diariaLivreTaxa = item.Diaria - (taxaTurismo / Math.Max(item.DiasPessoas, 1));
                    var livreTx = item.Total - valorComissaoMaisTaxa;

                    // Criar reserva com todos os campos
                    var reserva = new Reserva
                    {
                        NumeroReserva = item.NumeroReserva,
                        HospedeId = hospede.Id,
                        QuartoId = quarto.Id,
                        CheckIn = item.CheckIn!.Value,
                        CheckOut = item.CheckOut!.Value,
                        DataReserva = DateTime.Now,
                        Status = DateTime.Now >= item.CheckIn ? StatusReserva.CheckInRealizado : StatusReserva.Confirmada,
                        
                        // Valores financeiros
                        ValorTotal = item.Total,
                        Comissao = comissao,
                        TaxaTurismo = taxaTurismo,
                        DiariaLivreTaxa = diariaLivreTaxa,
                        ValorComissaoMaisTaxa = valorComissaoMaisTaxa,
                        LivreTx = livreTx,
                        DiariaForaPaga = item.DiariaPaga,
                        
                        // Pagamento
                        TipoPagamento = MapearTipoPagamento(item.TipoPagamento),
                        FormaPagamento = formaPagamento,
                        DataPagamento = item.DataPagamento,
                        PagoOnline = pagoOnline,
                        
                        // Informações do hóspede (redundância para relatório)
                        NumeroDocumentoHospede = item.NumeroDocumento,
                        DataNascimentoHospede = item.DataNascimento,
                        PaisHospede = item.Pais,
                        TipoDocumentoHospede = item.TipoDocumento,
                        
                        // Informações do quarto
                        NumeroQuarto = quarto.Numero,
                        TipoCama = item.Cama,
                        
                        // Hóspedes
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

        private FormaPagamento MapearFormaPagamento(string forma)
        {
            if (string.IsNullOrWhiteSpace(forma))
                return FormaPagamento.Dinheiro;

            return forma.ToLower() switch
            {
                var f when f.Contains("dinheiro") => FormaPagamento.Dinheiro,
                var f when f.Contains("cartão") || f.Contains("cartao") || f.Contains("credito") => FormaPagamento.CartaoCredito,
                var f when f.Contains("debito") || f.Contains("débito") => FormaPagamento.CartaoDebito,
                var f when f.Contains("transferencia") || f.Contains("transferência") => FormaPagamento.TransferenciaBancaria,
                var f when f.Contains("pix") => FormaPagamento.PIX,
                var f when f.Contains("online") || f.Contains("booking") => FormaPagamento.Online,
                var f when f.Contains("mbway") || f.Contains("mb way") => FormaPagamento.MBWay,
                var f when f.Contains("multibanco") => FormaPagamento.Multibanco,
                _ => FormaPagamento.Dinheiro
            };
        }
    }
}

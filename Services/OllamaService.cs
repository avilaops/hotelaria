using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hotelaria.Services
{
    public class OllamaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly string _defaultModel;

        public OllamaService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _baseUrl = _configuration["OLLAMA_BASE_URL"] ?? "http://localhost:11434";
            _defaultModel = _configuration["OLLAMA_MODEL"] ?? "llama3.2";
            
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<string> GerarResposta(string prompt, string? model = null)
        {
            try
            {
                var requestModel = model ?? _defaultModel;
                
                var request = new
                {
                    model = requestModel,
                    prompt = prompt,
                    stream = false
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/generate", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OllamaResponse>(responseContent);

                return result?.Response ?? "Desculpe, não consegui gerar uma resposta.";
            }
            catch (Exception ex)
            {
                return $"Erro ao comunicar com Ollama: {ex.Message}";
            }
        }

        public async Task<string> AnalisarOcupacao(DateTime dataInicio, DateTime dataFim, List<Models.Reserva> reservas)
        {
            var totalReservas = reservas.Count;
            var valorTotal = reservas.Sum(r => r.ValorTotal);
            var duracaoMedia = reservas.Average(r => (r.CheckOut - r.CheckIn).Days);

            var prompt = $@"Como especialista em gestão hoteleira, analise os seguintes dados e forneça insights:

Período: {dataInicio:dd/MM/yyyy} a {dataFim:dd/MM/yyyy}
Total de Reservas: {totalReservas}
Receita Total: € {valorTotal:N2}
Duração Média de Estadia: {duracaoMedia:N1} dias

Forneça uma análise concisa incluindo:
1. Avaliação do desempenho
2. Tendências identificadas
3. 3 recomendações práticas para melhorar a ocupação e receita

Seja direto e objetivo.";

            return await GerarResposta(prompt);
        }

        public async Task<string> SugerirPreco(string tipoQuarto, int capacidade, DateTime data, decimal precoAtual)
        {
            var diaSemana = data.DayOfWeek;
            var fimDeSemana = diaSemana == DayOfWeek.Saturday || diaSemana == DayOfWeek.Sunday;

            var prompt = $@"Como especialista em revenue management hoteleiro, sugira um preço otimizado:

Tipo de Quarto: {tipoQuarto}
Capacidade: {capacidade} pessoas
Data: {data:dd/MM/yyyy} ({diaSemana})
Preço Atual: € {precoAtual:N2}
É fim de semana: {(fimDeSemana ? "Sim" : "Não")}

Considerando:
- Sazonalidade
- Dia da semana
- Competitividade do mercado português

Forneça:
1. Preço sugerido (apenas o valor em euros)
2. Breve justificativa (máximo 2 linhas)

Seja objetivo e prático.";

            return await GerarResposta(prompt);
        }

        public async Task<string> GerarRelatorioFinanceiro(decimal receitaMensal, decimal despesas, int totalReservas)
        {
            var lucro = receitaMensal - despesas;
            var margemLucro = receitaMensal > 0 ? (lucro / receitaMensal) * 100 : 0;

            var prompt = $@"Como analista financeiro hoteleiro, analise estes dados e forneça insights:

Receita Mensal: € {receitaMensal:N2}
Despesas: € {despesas:N2}
Lucro: € {lucro:N2}
Margem de Lucro: {margemLucro:N1}%
Total de Reservas: {totalReservas}

Forneça:
1. Avaliação da saúde financeira (1-2 linhas)
2. Principais métricas de atenção
3. 3 recomendações para otimizar receita

Seja conciso e acionável.";

            return await GerarResposta(prompt);
        }

        public async Task<string> OtimizarDescricaoQuarto(string tipoQuarto, int capacidade, List<string> comodidades)
        {
            var comodidadesTexto = string.Join(", ", comodidades);

            var prompt = $@"Crie uma descrição atrativa para um quarto de hotel:

Tipo: {tipoQuarto}
Capacidade: {capacidade} pessoas
Comodidades: {comodidadesTexto}

Crie uma descrição:
- Em português de Portugal
- Máximo 3 linhas
- Atrativa e profissional
- Destacando os principais benefícios

Apenas a descrição, sem títulos ou formatação extra.";

            return await GerarResposta(prompt);
        }

        public async Task<List<string>> ListarModelosDisponiveis()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/tags");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ModelsResponse>(content);

                return result?.Models?.Select(m => m.Name).ToList() ?? new List<string>();
            }
            catch
            {
                return new List<string> { _defaultModel };
            }
        }

        public async Task<bool> VerificarConexao()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/tags");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    // Classes para deserialização das respostas
    public class OllamaResponse
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "";

        [JsonPropertyName("response")]
        public string Response { get; set; } = "";

        [JsonPropertyName("done")]
        public bool Done { get; set; }
    }

    public class ModelsResponse
    {
        [JsonPropertyName("models")]
        public List<ModelInfo> Models { get; set; } = new();
    }

    public class ModelInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("modified_at")]
        public DateTime ModifiedAt { get; set; }
    }
}

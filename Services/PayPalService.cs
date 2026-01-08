namespace Hotelaria.Services
{
    public class PayPalService
    {
        private readonly ConfigurationService _config;
        private readonly HttpClient _httpClient;
        private bool _isConnected = false;
        private string _lastError = string.Empty;
        private string _currentEnvironment = "Sandbox";

        public PayPalService(ConfigurationService config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
            _currentEnvironment = _config.GetEnvironment("paypal");
        }

        public async Task<bool> TestConnection()
        {
            try
            {
                var clientId = _config.GetSecureValue("PAYPAL_ID");
                var apiToken = _config.GetSecureValue("PAYPAL_TOKEN_API");
                _currentEnvironment = _config.GetEnvironment("paypal");

                if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiToken))
                {
                    _lastError = "Credenciais não configuradas";
                    _isConnected = false;
                    return false;
                }

                // Selecionar URL baseada no ambiente
                var baseUrl = _currentEnvironment == "Production" 
                    ? "https://api-m.paypal.com"
                    : "https://api-m.sandbox.paypal.com";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

                // Teste simples de conexão
                var response = await _httpClient.GetAsync($"{baseUrl}/v1/oauth2/token");
                _isConnected = response.IsSuccessStatusCode;
                
                if (!_isConnected)
                {
                    _lastError = $"Erro de conexão: {response.StatusCode}";
                }
                else
                {
                    _lastError = string.Empty;
                }

                return _isConnected;
            }
            catch (Exception ex)
            {
                _lastError = $"Erro: {ex.Message}";
                _isConnected = false;
                return false;
            }
        }

        public bool IsConnected() => _isConnected;
        public string GetLastError() => _lastError;

        public async Task<string> CreatePayment(decimal amount, string currency = "USD")
        {
            if (!_isConnected)
            {
                await TestConnection();
            }

            if (!_isConnected)
            {
                return string.Empty;
            }

            return $"PAYPAL-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }

        public Dictionary<string, object> GetStatus()
        {
            return new Dictionary<string, object>
            {
                ["Connected"] = _isConnected,
                ["ClientId"] = _config.GetMaskedValue("PAYPAL_ID"),
                ["LastError"] = _lastError,
                ["Environment"] = _currentEnvironment
            };
        }
    }
}

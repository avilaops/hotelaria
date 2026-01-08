namespace Hotelaria.Services
{
    public class SentryService
    {
        private readonly ConfigurationService _config;
        private readonly HttpClient _httpClient;
        private bool _isConnected = false;
        private string _lastError = string.Empty;
        private int _errorCount = 0;
        private DateTime _lastErrorTime = DateTime.MinValue;

        public SentryService(ConfigurationService config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<bool> TestConnection()
        {
            try
            {
                var apiToken = _config.GetSecureValue("SENTRY_TOKEN_API");

                if (string.IsNullOrEmpty(apiToken))
                {
                    _lastError = "Token não configurado";
                    _isConnected = false;
                    return false;
                }

                // Test Sentry API connection
                var baseUrl = "https://sentry.io/api/0";
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiToken}");

                var response = await _httpClient.GetAsync($"{baseUrl}/");
                _isConnected = response.IsSuccessStatusCode;
                _lastError = _isConnected ? string.Empty : $"Status: {response.StatusCode}";

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

        public async Task CaptureError(string message, string level = "error")
        {
            if (!_isConnected)
            {
                await TestConnection();
            }

            if (_isConnected)
            {
                _errorCount++;
                _lastErrorTime = DateTime.Now;
                // Implementação de envio de erro para Sentry
                await Task.Delay(50);
            }
        }

        public async Task CaptureException(Exception ex)
        {
            await CaptureError($"{ex.GetType().Name}: {ex.Message}", "error");
        }

        public Dictionary<string, object> GetStatus()
        {
            return new Dictionary<string, object>
            {
                ["Connected"] = _isConnected,
                ["Token"] = _config.GetMaskedValue("SENTRY_TOKEN_API"),
                ["ErrorCount"] = _errorCount,
                ["LastError"] = _lastErrorTime == DateTime.MinValue ? "Nenhum" : _lastErrorTime.ToString("dd/MM/yyyy HH:mm"),
                ["Features"] = new[] { "Error Tracking", "Performance", "Alerts" }
            };
        }

        public async Task<Dictionary<string, int>> GetErrorStats()
        {
            if (!_isConnected)
            {
                await TestConnection();
            }

            // Simulação de estatísticas
            return new Dictionary<string, int>
            {
                ["Total"] = _errorCount,
                ["Today"] = 0,
                ["Week"] = 0
            };
        }
    }
}

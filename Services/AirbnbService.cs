namespace Hotelaria.Services
{
    public class AirbnbService
    {
        private readonly ConfigurationService _config;
        private readonly HttpClient _httpClient;
        private bool _isConnected = false;
        private string _lastError = string.Empty;

        public AirbnbService(ConfigurationService config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<bool> TestConnection()
        {
            try
            {
                var clientKey = _config.GetSecureValue("AIRBNB_CLIENT_KEY");
                var secretKey = _config.GetSecureValue("AIRBNB_SECRET_KEY");

                if (string.IsNullOrEmpty(clientKey) || string.IsNullOrEmpty(secretKey))
                {
                    _lastError = "Credenciais não configuradas";
                    _isConnected = false;
                    return false;
                }

                // Test Airbnb API connection
                var baseUrl = "https://api.airbnb.com";
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("X-Airbnb-API-Key", clientKey);

                var response = await _httpClient.GetAsync($"{baseUrl}/v2/");
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

        public Dictionary<string, object> GetStatus()
        {
            return new Dictionary<string, object>
            {
                ["Connected"] = _isConnected,
                ["ClientKey"] = _config.GetMaskedValue("AIRBNB_CLIENT_KEY"),
                ["LastError"] = _lastError,
                ["Features"] = new[] { "Sync Reservas", "Disponibilidade", "Preços" }
            };
        }

        public async Task<List<object>> SyncReservations()
        {
            if (!_isConnected)
            {
                await TestConnection();
            }

            if (!_isConnected)
            {
                return new List<object>();
            }

            // Implementação de sincronização de reservas
            await Task.Delay(100);
            return new List<object>();
        }

        public async Task<bool> UpdateAvailability(DateTime start, DateTime end, bool available)
        {
            if (!_isConnected)
            {
                await TestConnection();
            }

            return _isConnected;
        }
    }
}

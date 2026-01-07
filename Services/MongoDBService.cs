namespace Hotelaria.Services
{
    public class MongoDBService
    {
        private readonly ConfigurationService _config;
        private bool _isConnected = false;
        private string _lastError = string.Empty;
        private string _databaseName = "hotelaria";

        public MongoDBService(ConfigurationService config)
        {
            _config = config;
        }

        public async Task<bool> TestConnection()
        {
            try
            {
                var connectionString = _config.GetSecureValue("MONGO_ATLAS_URI");

                if (string.IsNullOrEmpty(connectionString))
                {
                    _lastError = "Connection string não configurada";
                    _isConnected = false;
                    return false;
                }

                // Simulação de teste de conexão
                // Em produção, usar MongoDB.Driver
                await Task.Delay(100);
                
                _isConnected = true;
                _lastError = string.Empty;
                
                return true;
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
                ["Database"] = _databaseName,
                ["ConnectionString"] = _config.GetMaskedValue("MONGO_ATLAS_URI"),
                ["LastError"] = _lastError,
                ["Provider"] = "MongoDB Atlas"
            };
        }

        public async Task<bool> SaveReserva(object reserva)
        {
            if (!_isConnected)
            {
                await TestConnection();
            }

            if (!_isConnected)
            {
                return false;
            }

            // Implementação de salvamento no MongoDB
            // Por enquanto, retorna sucesso para demonstração
            await Task.Delay(50);
            return true;
        }

        public async Task<List<T>> GetAll<T>() where T : class
        {
            if (!_isConnected)
            {
                await TestConnection();
            }

            // Implementação de busca no MongoDB
            return new List<T>();
        }
    }
}

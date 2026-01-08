using System.Security.Cryptography;
using System.Text;

namespace Hotelaria.Services
{
    public class ConfigurationService
    {
        private readonly Dictionary<string, string> _secureConfig = new();
        private AuthService? _authService; // REMOVIDO readonly
        
        public ConfigurationService()
        {
            LoadEnvironmentVariables();
        }

        // Método para injetar AuthService após inicialização
        public void SetAuthService(AuthService authService)
        {
            _authService = authService;
        }

        private void LoadEnvironmentVariables()
        {
            // PayPal
            _secureConfig["PAYPAL_ID"] = GetEnvironmentVariable("PAYPAL_ID");
            _secureConfig["PAYPAL_TOKEN_API"] = GetEnvironmentVariable("PAYPAL_TOKEN_API");
            _secureConfig["PAYPAL_ENVIRONMENT"] = GetEnvironmentVariable("PAYPAL_ENVIRONMENT") ?? "Sandbox";
            
            // MongoDB
            _secureConfig["MONGO_ATLAS_URI"] = GetEnvironmentVariable("MONGO_ATLAS_URI");
            _secureConfig["MONGO_DATABASE"] = GetEnvironmentVariable("MONGO_DATABASE") ?? "hotelaria";
            
            // Airbnb
            _secureConfig["AIRBNB_CLIENT_KEY"] = GetEnvironmentVariable("AIRBNB_CLIENT_KEY");
            _secureConfig["AIRBNB_SECRET_KEY"] = GetEnvironmentVariable("AIRBNB_SECRET_KEY");
            
            // Sentry
            _secureConfig["SENTRY_TOKEN_API"] = GetEnvironmentVariable("SENTRY_TOKEN_API");
        }

        private string GetEnvironmentVariable(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? string.Empty;
        }

        /// <summary>
        /// Valida acesso usando AuthService ao invés de credenciais hardcoded
        /// CORRIGIDO: Removidas senhas hardcoded por motivos de segurança
        /// </summary>
        public bool ValidateAccess()
        {
            if (_authService == null)
                return false;
                
            return _authService.EstaAutenticado() && 
                   _authService.TemPermissao(Models.PerfilUsuario.Administrador);
        }

        public Dictionary<string, bool> GetIntegrationStatus()
        {
            return new Dictionary<string, bool>
            {
                ["PayPal"] = !string.IsNullOrEmpty(_secureConfig["PAYPAL_ID"]),
                ["MongoDB"] = !string.IsNullOrEmpty(_secureConfig["MONGO_ATLAS_URI"]),
                ["Airbnb"] = !string.IsNullOrEmpty(_secureConfig["AIRBNB_CLIENT_KEY"]),
                ["Sentry"] = !string.IsNullOrEmpty(_secureConfig["SENTRY_TOKEN_API"])
            };
        }

        public string GetMaskedValue(string key)
        {
            if (!_secureConfig.ContainsKey(key) || string.IsNullOrEmpty(_secureConfig[key]))
                return "Não configurado";

            var value = _secureConfig[key];
            if (value.Length <= 8)
                return "***";
            
            return $"{value.Substring(0, 4)}...{value.Substring(value.Length - 4)}";
        }

        public string? GetSecureValue(string key)
        {
            return _secureConfig.ContainsKey(key) ? _secureConfig[key] : null;
        }

        public bool UpdateConfiguration(string key, string value)
        {
            if (_secureConfig.ContainsKey(key))
            {
                _secureConfig[key] = value;
                return true;
            }
            
            // Permitir adicionar novas configurações
            _secureConfig[key] = value;
            return true;
        }

        public Dictionary<string, string> GetAllMaskedConfigs()
        {
            var masked = new Dictionary<string, string>();
            foreach (var key in _secureConfig.Keys)
            {
                masked[key] = GetMaskedValue(key);
            }
            return masked;
        }

        public string GetEnvironment(string service)
        {
            return service.ToLower() switch
            {
                "paypal" => _secureConfig.ContainsKey("PAYPAL_ENVIRONMENT") ? _secureConfig["PAYPAL_ENVIRONMENT"] : "Sandbox",
                _ => "Production"
            };
        }
    }
}

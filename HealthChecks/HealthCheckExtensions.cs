using Microsoft.Extensions.Diagnostics.HealthChecks;
using Hotelaria.Services;

namespace Hotelaria.HealthChecks
{
    /// <summary>
    /// Health check para MongoDB
    /// </summary>
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly MongoDBService _mongoService;
        private readonly ILogger<MongoDbHealthCheck> _logger;

        public MongoDbHealthCheck(MongoDBService mongoService, ILogger<MongoDbHealthCheck> logger)
        {
            _mongoService = mongoService;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var isConnected = await _mongoService.TestConnection();
                
                if (isConnected)
                {
                    var data = new Dictionary<string, object>
                    {
                        ["status"] = "connected",
                        ["database"] = "hotelaria",
                        ["provider"] = "MongoDB Atlas"
                    };
                    
                    _logger.LogDebug("MongoDB health check: Healthy");
                    return HealthCheckResult.Healthy("MongoDB is connected", data);
                }
                else
                {
                    var error = _mongoService.GetLastError();
                    var data = new Dictionary<string, object>
                    {
                        ["status"] = "disconnected",
                        ["error"] = error
                    };
                    
                    _logger.LogWarning("MongoDB health check: Degraded - {Error}", error);
                    return HealthCheckResult.Degraded($"MongoDB connection issue: {error}", null, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MongoDB health check failed");
                return HealthCheckResult.Unhealthy("MongoDB health check failed", ex);
            }
        }
    }

    /// <summary>
    /// Health check para integração PayPal
    /// </summary>
    public class PayPalHealthCheck : IHealthCheck
    {
        private readonly ConfigurationService _config;
        private readonly ILogger<PayPalHealthCheck> _logger;

        public PayPalHealthCheck(ConfigurationService config, ILogger<PayPalHealthCheck> logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var paypalId = _config.GetSecureValue("PAYPAL_ID");
                var isConfigured = !string.IsNullOrEmpty(paypalId);
                
                var data = new Dictionary<string, object>
                {
                    ["configured"] = isConfigured,
                    ["environment"] = _config.GetEnvironment("paypal")
                };
                
                if (isConfigured)
                {
                    _logger.LogDebug("PayPal health check: Healthy");
                    return Task.FromResult(HealthCheckResult.Healthy("PayPal integration is configured", data));
                }
                else
                {
                    _logger.LogDebug("PayPal health check: Degraded (not configured)");
                    return Task.FromResult(HealthCheckResult.Degraded("PayPal integration not configured", null, data));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "PayPal health check failed");
                return Task.FromResult(HealthCheckResult.Unhealthy("PayPal check failed", ex));
            }
        }
    }

    /// <summary>
    /// Health check para integração Airbnb
    /// </summary>
    public class AirbnbHealthCheck : IHealthCheck
    {
        private readonly ConfigurationService _config;
        private readonly ILogger<AirbnbHealthCheck> _logger;

        public AirbnbHealthCheck(ConfigurationService config, ILogger<AirbnbHealthCheck> logger)
        {
            _config = config;
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var airbnbKey = _config.GetSecureValue("AIRBNB_CLIENT_KEY");
                var isConfigured = !string.IsNullOrEmpty(airbnbKey);
                
                var data = new Dictionary<string, object>
                {
                    ["configured"] = isConfigured
                };
                
                if (isConfigured)
                {
                    _logger.LogDebug("Airbnb health check: Healthy");
                    return Task.FromResult(HealthCheckResult.Healthy("Airbnb integration is configured", data));
                }
                else
                {
                    _logger.LogDebug("Airbnb health check: Degraded (not configured)");
                    return Task.FromResult(HealthCheckResult.Degraded("Airbnb integration not configured", null, data));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Airbnb health check failed");
                return Task.FromResult(HealthCheckResult.Unhealthy("Airbnb check failed", ex));
            }
        }
    }

    /// <summary>
    /// Health check de memória
    /// </summary>
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly ILogger<MemoryHealthCheck> _logger;
        private const long DEGRADED_THRESHOLD = 1024L * 1024L * 1024L * 1; // 1 GB
        private const long UNHEALTHY_THRESHOLD = 1024L * 1024L * 1024L * 2; // 2 GB

        public MemoryHealthCheck(ILogger<MemoryHealthCheck> logger)
        {
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var allocated = GC.GetTotalMemory(forceFullCollection: false);
                var data = new Dictionary<string, object>
                {
                    ["allocatedBytes"] = allocated,
                    ["allocatedMB"] = allocated / 1024 / 1024,
                    ["gen0Collections"] = GC.CollectionCount(0),
                    ["gen1Collections"] = GC.CollectionCount(1),
                    ["gen2Collections"] = GC.CollectionCount(2)
                };

                if (allocated < DEGRADED_THRESHOLD)
                {
                    _logger.LogDebug("Memory health check: Healthy ({AllocatedMB} MB)", allocated / 1024 / 1024);
                    return Task.FromResult(HealthCheckResult.Healthy("Memory usage is normal", data));
                }
                else if (allocated < UNHEALTHY_THRESHOLD)
                {
                    _logger.LogWarning("Memory health check: Degraded ({AllocatedMB} MB)", allocated / 1024 / 1024);
                    return Task.FromResult(HealthCheckResult.Degraded("Memory usage is elevated", null, data));
                }
                else
                {
                    _logger.LogError("Memory health check: Unhealthy ({AllocatedMB} MB)", allocated / 1024 / 1024);
                    return Task.FromResult(HealthCheckResult.Unhealthy("Memory usage is critical", null, data));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Memory health check failed");
                return Task.FromResult(HealthCheckResult.Unhealthy("Memory check failed", ex));
            }
        }
    }
}

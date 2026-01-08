using Hotelaria.Models;
using System.Text.Json;

namespace Hotelaria.Services
{
    /// <summary>
    /// Serviço de auditoria para rastrear ações dos usuários
    /// Implementa compliance com LGPD/GDPR
    /// </summary>
    public class AuditService
    {
        private readonly ILogger<AuditService> _logger;
        private readonly AuthService _authService;
        private readonly List<AuditLog> _auditLogs = new();

        public AuditService(ILogger<AuditService> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        /// <summary>
        /// Registra uma ação no log de auditoria
        /// </summary>
        public void LogAction(string action, string entity, object? details = null, string? ipAddress = null)
        {
            var usuario = _authService.ObterUsuarioAtual();
            
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Timestamp = DateTime.UtcNow,
                Usuario = usuario?.Username ?? "Sistema",
                UsuarioId = usuario?.Id ?? 0,
                Action = action,
                Entity = entity,
                Details = details != null ? JsonSerializer.Serialize(details) : null,
                IpAddress = ipAddress ?? "Unknown"
            };
            
            _auditLogs.Add(auditLog);
            
            _logger.LogInformation(
                "AUDIT: {Action} on {Entity} by {Usuario} (ID: {UsuarioId}) at {Timestamp}", 
                auditLog.Action, 
                auditLog.Entity, 
                auditLog.Usuario, 
                auditLog.UsuarioId, 
                auditLog.Timestamp
            );
        }

        /// <summary>
        /// Obtém logs de auditoria com filtros
        /// </summary>
        public List<AuditLog> ObterLogs(
            DateTime? inicio = null, 
            DateTime? fim = null, 
            string? usuario = null, 
            string? entity = null)
        {
            var query = _auditLogs.AsEnumerable();
            
            if (inicio.HasValue)
                query = query.Where(a => a.Timestamp >= inicio.Value);
            
            if (fim.HasValue)
                query = query.Where(a => a.Timestamp <= fim.Value);
            
            if (!string.IsNullOrEmpty(usuario))
                query = query.Where(a => a.Usuario.Contains(usuario, StringComparison.OrdinalIgnoreCase));
            
            if (!string.IsNullOrEmpty(entity))
                query = query.Where(a => a.Entity.Contains(entity, StringComparison.OrdinalIgnoreCase));
            
            return query.OrderByDescending(a => a.Timestamp).ToList();
        }

        /// <summary>
        /// Obtém estatísticas de auditoria
        /// </summary>
        public Dictionary<string, int> ObterEstatisticas(DateTime? inicio = null, DateTime? fim = null)
        {
            var query = _auditLogs.AsEnumerable();
            
            if (inicio.HasValue)
                query = query.Where(a => a.Timestamp >= inicio.Value);
            
            if (fim.HasValue)
                query = query.Where(a => a.Timestamp <= fim.Value);
            
            return new Dictionary<string, int>
            {
                ["TotalActions"] = query.Count(),
                ["UniqueUsers"] = query.Select(a => a.UsuarioId).Distinct().Count(),
                ["Creates"] = query.Count(a => a.Action.Contains("CREATE", StringComparison.OrdinalIgnoreCase)),
                ["Updates"] = query.Count(a => a.Action.Contains("UPDATE", StringComparison.OrdinalIgnoreCase)),
                ["Deletes"] = query.Count(a => a.Action.Contains("DELETE", StringComparison.OrdinalIgnoreCase))
            };
        }
    }

    /// <summary>
    /// Modelo de log de auditoria
    /// </summary>
    public class AuditLog
    {
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public string Action { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string IpAddress { get; set; } = string.Empty;
    }
}

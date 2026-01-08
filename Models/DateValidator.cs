using System.Globalization;

namespace Hotelaria.Models
{
    /// <summary>
    /// Validador robusto de datas com regras de negócio
    /// Corrige Defeito Crítico #13
    /// </summary>
    public static class DateValidator
    {
        /// <summary>
        /// Valida se uma data é válida e dentro de limites razoáveis
        /// </summary>
        public static ValidationResult ValidateDate(DateTime? date, string fieldName = "Data")
        {
            if (!date.HasValue)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"{fieldName} é obrigatória"
                };
            }

            // Data não pode ser anterior a 1900 (limite sistema)
            if (date.Value.Year < 1900)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"{fieldName} não pode ser anterior a 1900"
                };
            }

            // Data não pode ser posterior a 100 anos no futuro
            if (date.Value > DateTime.Now.AddYears(100))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = $"{fieldName} não pode ser superior a 100 anos no futuro"
                };
            }

            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// Valida data de check-in (não pode ser no passado distante)
        /// </summary>
        public static ValidationResult ValidateCheckInDate(DateTime checkIn)
        {
            var basicValidation = ValidateDate(checkIn, "Data de check-in");
            if (!basicValidation.IsValid)
                return basicValidation;

            // Check-in não pode ser mais de 30 dias no passado
            if (checkIn < DateTime.Today.AddDays(-30))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Data de check-in não pode ser anterior a 30 dias atrás"
                };
            }

            // Check-in não pode ser mais de 2 anos no futuro
            if (checkIn > DateTime.Today.AddYears(2))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Data de check-in não pode ser superior a 2 anos no futuro"
                };
            }

            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// Valida data de check-out (deve ser posterior ao check-in)
        /// </summary>
        public static ValidationResult ValidateCheckOutDate(DateTime checkIn, DateTime checkOut)
        {
            var basicValidation = ValidateDate(checkOut, "Data de check-out");
            if (!basicValidation.IsValid)
                return basicValidation;

            // Check-out deve ser posterior ao check-in
            if (checkOut <= checkIn)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Data de check-out deve ser posterior à data de check-in"
                };
            }

            // Duração máxima de estadia: 365 dias
            var diasEstadia = (checkOut - checkIn).Days;
            if (diasEstadia > 365)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Duração máxima de estadia é 365 dias"
                };
            }

            // Duração mínima de estadia: 1 dia
            if (diasEstadia < 1)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Duração mínima de estadia é 1 dia"
                };
            }

            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// Valida data de reserva
        /// </summary>
        public static ValidationResult ValidateReservaDate(DateTime dataReserva, DateTime checkIn)
        {
            var basicValidation = ValidateDate(dataReserva, "Data da reserva");
            if (!basicValidation.IsValid)
                return basicValidation;

            // Data da reserva não pode ser posterior ao check-in
            if (dataReserva > checkIn)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Data da reserva não pode ser posterior ao check-in"
                };
            }

            // Data da reserva não pode ser mais de 2 anos no passado
            if (dataReserva < DateTime.Today.AddYears(-2))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Data da reserva não pode ser anterior a 2 anos atrás"
                };
            }

            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// Parse de data com múltiplos formatos
        /// </summary>
        public static (bool Success, DateTime? Date, string Error) ParseDate(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return (false, null, "Data não pode ser vazia");
            }

            // Formatos aceitos
            string[] formats = new[]
            {
                "dd/MM/yyyy",
                "dd-MM-yyyy",
                "yyyy-MM-dd",
                "dd/MM/yyyy HH:mm:ss",
                "dd-MM-yyyy HH:mm:ss",
                "yyyy-MM-dd HH:mm:ss",
                "MM/dd/yyyy",
                "M/d/yyyy"
            };

            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(
                    dateString.Trim(),
                    format,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime result))
                {
                    return (true, result, string.Empty);
                }
            }

            // Tentar parse genérico
            if (DateTime.TryParse(dateString, out DateTime genericResult))
            {
                return (true, genericResult, string.Empty);
            }

            return (false, null, $"Formato de data inválido: {dateString}");
        }

        /// <summary>
        /// Valida intervalo de datas para busca/filtro
        /// </summary>
        public static ValidationResult ValidateDateRange(DateTime? inicio, DateTime? fim)
        {
            if (!inicio.HasValue || !fim.HasValue)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Datas de início e fim são obrigatórias"
                };
            }

            if (fim < inicio)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Data de fim deve ser posterior à data de início"
                };
            }

            // Intervalo máximo de 5 anos
            var dias = (fim.Value - inicio.Value).Days;
            if (dias > 1825) // 5 anos
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Intervalo máximo de busca é 5 anos"
                };
            }

            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// Verifica se data está em dia útil (útil para validações futuras)
        /// </summary>
        public static bool IsDiaUtil(DateTime date)
        {
            // Sábado ou Domingo
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return false;

            // TODO: Adicionar feriados nacionais se necessário
            return true;
        }

        /// <summary>
        /// Calcula número de dias úteis entre duas datas
        /// </summary>
        public static int ContarDiasUteis(DateTime inicio, DateTime fim)
        {
            int diasUteis = 0;
            for (DateTime data = inicio; data <= fim; data = data.AddDays(1))
            {
                if (IsDiaUtil(data))
                    diasUteis++;
            }
            return diasUteis;
        }

        /// <summary>
        /// Normaliza data para início do dia (00:00:00)
        /// </summary>
        public static DateTime NormalizeToStartOfDay(DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Normaliza data para fim do dia (23:59:59)
        /// </summary>
        public static DateTime NormalizeToEndOfDay(DateTime date)
        {
            return date.Date.AddDays(1).AddTicks(-1);
        }
    }

    /// <summary>
    /// Resultado de validação
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        
        public static ValidationResult Success() => new ValidationResult { IsValid = true };
        public static ValidationResult Failure(string message) => new ValidationResult { IsValid = false, ErrorMessage = message };
    }
}

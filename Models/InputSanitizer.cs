using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace Hotelaria.Models
{
    /// <summary>
    /// Classe para sanitização de inputs e prevenção de XSS
    /// </summary>
    public static class InputSanitizer
    {
        /// <summary>
        /// Sanitiza HTML removendo tags perigosas
        /// </summary>
        public static string SanitizeHtml(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            
            return HtmlEncoder.Default.Encode(input);
        }
        
        /// <summary>
        /// Remove caracteres perigosos para SQL/NoSQL
        /// </summary>
        public static string SanitizeForDatabase(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            
            // Remover caracteres perigosos
            return Regex.Replace(input, @"[<>""'/\\;]", "");
        }
        
        /// <summary>
        /// Sanitiza nome de arquivo
        /// </summary>
        public static string SanitizeFileName(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return string.Empty;
            
            // Remover caracteres inválidos de arquivo
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = string.Join("", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));
            
            // Limitar tamanho
            if (sanitized.Length > 255)
                sanitized = sanitized.Substring(0, 255);
            
            return sanitized;
        }
        
        /// <summary>
        /// Valida email
        /// </summary>
        public static bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// Sanitiza input genérico
        /// </summary>
        public static string Sanitize(string? input, int maxLength = 1000)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            
            // Remover espaços extras
            input = input.Trim();
            
            // Limitar tamanho
            if (input.Length > maxLength)
                input = input.Substring(0, maxLength);
            
            // Remover caracteres de controle
            input = Regex.Replace(input, @"[\x00-\x08\x0B\x0C\x0E-\x1F]", "");
            
            return input;
        }
    }
}

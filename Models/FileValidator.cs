using Microsoft.AspNetCore.Components.Forms;
using System.Text;

namespace Hotelaria.Models
{
    /// <summary>
    /// Validador avançado de arquivos com verificação de MIME type real
    /// Corrige Defeito Crítico #11
    /// OWASP File Upload Validation
    /// </summary>
    public static class FileValidator
    {
        // Configurações
        private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB
        private const long MaxFileSizeForPreview = 2 * 1024 * 1024; // 2 MB para preview
        
        // Extensões permitidas
        private static readonly HashSet<string> AllowedExtensions = new()
        {
            ".csv",
            ".tsv",
            ".txt"
        };

        // Magic numbers (primeiros bytes) para validação real de tipo
        private static readonly Dictionary<string, byte[][]> FileMagicNumbers = new()
        {
            // CSV/TSV/TXT (text files geralmente começam com caracteres ASCII ou UTF-8 BOM)
            {
                "text", new[]
                {
                    new byte[] { 0xEF, 0xBB, 0xBF }, // UTF-8 BOM
                    new byte[] { 0xFF, 0xFE },       // UTF-16 LE BOM
                    new byte[] { 0xFE, 0xFF }        // UTF-16 BE BOM
                }
            }
        };

        // Extensões perigosas que nunca devem ser permitidas
        private static readonly HashSet<string> DangerousExtensions = new()
        {
            ".exe", ".dll", ".bat", ".cmd", ".com", ".ps1",
            ".vbs", ".js", ".jar", ".app", ".deb", ".rpm",
            ".sh", ".py", ".php", ".asp", ".aspx", ".jsp",
            ".scr", ".msi", ".apk", ".dmg"
        };

        /// <summary>
        /// Valida arquivo de upload completo
        /// </summary>
        public static async Task<FileValidationResult> ValidateFileAsync(IBrowserFile file)
        {
            var result = new FileValidationResult
            {
                FileName = InputSanitizer.SanitizeFileName(file.Name)
            };

            // 1. Validar nome do arquivo
            if (string.IsNullOrWhiteSpace(file.Name))
            {
                result.AddError("Nome do arquivo é inválido");
                return result;
            }

            // 2. Validar extensão
            var extension = Path.GetExtension(file.Name).ToLowerInvariant();
            
            if (string.IsNullOrEmpty(extension))
            {
                result.AddError("Arquivo deve ter uma extensão");
                return result;
            }

            if (DangerousExtensions.Contains(extension))
            {
                result.AddError($"Extensão {extension} é perigosa e não é permitida");
                return result;
            }

            if (!AllowedExtensions.Contains(extension))
            {
                result.AddError($"Extensão {extension} não é permitida. Permitidas: {string.Join(", ", AllowedExtensions)}");
                return result;
            }

            // 3. Validar tamanho
            if (file.Size <= 0)
            {
                result.AddError("Arquivo está vazio");
                return result;
            }

            if (file.Size > MaxFileSizeBytes)
            {
                result.AddError($"Arquivo muito grande. Tamanho máximo: {FormatFileSize(MaxFileSizeBytes)}");
                return result;
            }

            result.FileSize = file.Size;

            // 4. Validar MIME type declarado
            if (string.IsNullOrWhiteSpace(file.ContentType))
            {
                result.AddWarning("Content-Type não declarado");
            }
            else if (!IsAllowedMimeType(file.ContentType))
            {
                result.AddError($"Content-Type '{file.ContentType}' não é permitido");
                return result;
            }

            // 5. Ler primeiros bytes para validação real (magic numbers)
            try
            {
                using var stream = file.OpenReadStream(MaxFileSizeBytes);
                var buffer = new byte[Math.Min(8192, file.Size)]; // Ler primeiros 8KB
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    // Verificar se é texto válido
                    if (!IsValidTextFile(buffer, bytesRead))
                    {
                        result.AddError("Arquivo não parece ser um arquivo de texto válido");
                        return result;
                    }

                    // Detectar encoding
                    result.DetectedEncoding = DetectEncoding(buffer, bytesRead);

                    // Validar delimitador
                    var delimitadorDetectado = DetectDelimiter(buffer, bytesRead);
                    if (delimitadorDetectado != null)
                    {
                        result.DetectedDelimiter = delimitadorDetectado;
                    }
                }
            }
            catch (Exception ex)
            {
                result.AddError($"Erro ao ler arquivo: {ex.Message}");
                return result;
            }

            // 6. Validações específicas para CSV/TSV
            if (extension == ".csv" || extension == ".tsv")
            {
                var csvValidation = await ValidateCsvStructureAsync(file);
                if (!csvValidation.IsValid)
                {
                    result.AddError(csvValidation.ErrorMessage);
                    return result;
                }

                result.LineCount = csvValidation.LineCount;
                result.HasHeader = csvValidation.HasHeader;
            }

            result.IsValid = !result.Errors.Any();
            return result;
        }

        /// <summary>
        /// Valida estrutura básica de CSV
        /// </summary>
        private static async Task<CsvValidationResult> ValidateCsvStructureAsync(IBrowserFile file)
        {
            var result = new CsvValidationResult();

            try
            {
                using var stream = file.OpenReadStream(MaxFileSizeBytes);
                using var reader = new StreamReader(stream);

                var firstLine = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(firstLine))
                {
                    result.ErrorMessage = "Arquivo CSV está vazio";
                    return result;
                }

                // Verificar se primeira linha é cabeçalho
                result.HasHeader = IsLikelyHeader(firstLine);

                // Contar linhas (máximo 10000 para não travar)
                int lineCount = 1;
                while (!reader.EndOfStream && lineCount < 10000)
                {
                    await reader.ReadLineAsync();
                    lineCount++;
                }

                result.LineCount = lineCount;
                result.IsValid = true;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = $"Erro ao validar CSV: {ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// Verifica se é arquivo de texto válido (sem bytes binários suspeitos)
        /// </summary>
        private static bool IsValidTextFile(byte[] buffer, int length)
        {
            // Verificar UTF-8 BOM
            if (length >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                return true;

            // Verificar UTF-16 BOM
            if (length >= 2 && ((buffer[0] == 0xFF && buffer[1] == 0xFE) || (buffer[0] == 0xFE && buffer[1] == 0xFF)))
                return true;

            // Contar bytes suspeitos (não-texto)
            int suspiciousBytes = 0;
            for (int i = 0; i < length; i++)
            {
                byte b = buffer[i];
                
                // Permitir: ASCII imprimível (32-126), tab (9), newline (10, 13), espaço
                if (b < 9 || (b > 13 && b < 32) || b == 127)
                {
                    // Permitir UTF-8 multi-byte
                    if (b < 128)
                    {
                        suspiciousBytes++;
                    }
                }
            }

            // Se mais de 10% são bytes suspeitos, provavelmente não é texto
            return suspiciousBytes < (length * 0.1);
        }

        /// <summary>
        /// Detecta encoding do arquivo
        /// </summary>
        private static string DetectEncoding(byte[] buffer, int length)
        {
            // UTF-8 BOM
            if (length >= 3 && buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)
                return "UTF-8 (BOM)";

            // UTF-16 LE BOM
            if (length >= 2 && buffer[0] == 0xFF && buffer[1] == 0xFE)
                return "UTF-16 LE";

            // UTF-16 BE BOM
            if (length >= 2 && buffer[0] == 0xFE && buffer[1] == 0xFF)
                return "UTF-16 BE";

            // Tentar detectar UTF-8 sem BOM
            try
            {
                var decoded = Encoding.UTF8.GetString(buffer, 0, length);
                if (decoded.Any(c => c > 127)) // Contém caracteres UTF-8
                    return "UTF-8";
            }
            catch { }

            return "ASCII/Latin1";
        }

        /// <summary>
        /// Detecta delimitador CSV (vírgula, ponto-e-vírgula, tab)
        /// </summary>
        private static string? DetectDelimiter(byte[] buffer, int length)
        {
            var text = Encoding.UTF8.GetString(buffer, 0, Math.Min(length, 1000)); // Analisar primeira linha
            var firstLine = text.Split('\n')[0];

            var delimiters = new[] { ',', ';', '\t', '|' };
            var counts = delimiters.ToDictionary(d => d, d => firstLine.Count(c => c == d));

            var maxDelimiter = counts.OrderByDescending(kvp => kvp.Value).FirstOrDefault();
            if (maxDelimiter.Value > 0)
            {
                return maxDelimiter.Key switch
                {
                    ',' => "Vírgula (,)",
                    ';' => "Ponto-e-vírgula (;)",
                    '\t' => "Tab (\\t)",
                    '|' => "Pipe (|)",
                    _ => null
                };
            }

            return null;
        }

        /// <summary>
        /// Verifica se primeira linha parece ser cabeçalho
        /// </summary>
        private static bool IsLikelyHeader(string line)
        {
            // Cabeçalhos geralmente contêm letras e não apenas números
            var parts = line.Split(new[] { ',', ';', '\t', '|' });
            if (parts.Length == 0) return false;

            var textParts = parts.Count(p => p.Any(char.IsLetter));
            return textParts > parts.Length / 2;
        }

        /// <summary>
        /// Verifica se MIME type é permitido
        /// </summary>
        private static bool IsAllowedMimeType(string mimeType)
        {
            var allowedMimeTypes = new[]
            {
                "text/csv",
                "text/plain",
                "text/tab-separated-values",
                "application/csv",
                "application/vnd.ms-excel" // Excel pode exportar como CSV
            };

            return allowedMimeTypes.Any(allowed => 
                mimeType.StartsWith(allowed, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Formata tamanho de arquivo
        /// </summary>
        public static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }

        /// <summary>
        /// Verifica se arquivo é adequado para preview
        /// </summary>
        public static bool IsSuitableForPreview(long fileSize)
        {
            return fileSize <= MaxFileSizeForPreview;
        }
    }

    /// <summary>
    /// Resultado de validação de arquivo
    /// </summary>
    public class FileValidationResult
    {
        public bool IsValid { get; set; }
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string? DetectedEncoding { get; set; }
        public string? DetectedDelimiter { get; set; }
        public int? LineCount { get; set; }
        public bool? HasHeader { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        public void AddError(string error) => Errors.Add(error);
        public void AddWarning(string warning) => Warnings.Add(warning);

        public string GetSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Arquivo: {FileName}");
            sb.AppendLine($"Tamanho: {FileValidator.FormatFileSize(FileSize)}");
            
            if (DetectedEncoding != null)
                sb.AppendLine($"Encoding: {DetectedEncoding}");
            
            if (DetectedDelimiter != null)
                sb.AppendLine($"Delimitador: {DetectedDelimiter}");
            
            if (LineCount.HasValue)
                sb.AppendLine($"Linhas: {LineCount}");
            
            if (HasHeader.HasValue)
                sb.AppendLine($"Cabeçalho: {(HasHeader.Value ? "Sim" : "Não")}");

            if (Warnings.Any())
            {
                sb.AppendLine("\nAvisos:");
                foreach (var warning in Warnings)
                    sb.AppendLine($"  - {warning}");
            }

            if (Errors.Any())
            {
                sb.AppendLine("\nErros:");
                foreach (var error in Errors)
                    sb.AppendLine($"  - {error}");
            }

            return sb.ToString();
        }
    }

    /// <summary>
    /// Resultado de validação de CSV
    /// </summary>
    internal class CsvValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public int LineCount { get; set; }
        public bool HasHeader { get; set; }
    }
}

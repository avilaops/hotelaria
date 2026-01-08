namespace Hotelaria.Models
{
    public class ReservaImport
    {
        public string Nome { get; set; } = string.Empty;
        public string NomeHospede { get; set; } = string.Empty;
        public string EmailHospede { get; set; } = string.Empty;
        public string TelefoneHospede { get; set; } = string.Empty;
        public string DocumentoHospede { get; set; } = string.Empty;
        public string PaisHospede { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string Cama { get; set; } = string.Empty;
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public int DiasPessoas { get; set; }
        public decimal ValorPagamento { get; set; }
        public string TipoPagamento { get; set; } = string.Empty;
        public decimal TaxaBooking { get; set; }
        public decimal TaxaPagamento { get; set; }
        public string NumeroReserva { get; set; } = string.Empty;
        public decimal Diaria { get; set; }
        public decimal Total { get; set; }
        public decimal LivreTX { get; set; }
        public decimal DiariaPaga { get; set; }
        public string FormaPagamento { get; set; } = string.Empty;
        public DateTime? DataPagamento { get; set; }
        
        // Campos de validação
        public bool IsValid { get; set; } = true;
        public List<string> Erros { get; set; } = new();
        
        // Dados processados
        public int NumeroQuarto { get; set; }
        public int NumeroAdultos { get; set; }
        public int NumeroCriancas { get; set; }
    }

    public class ImportacaoResultado
    {
        public int TotalLinhas { get; set; }
        public int LinhasImportadas { get; set; }
        public int LinhasComErro { get; set; }
        public List<string> Erros { get; set; } = new();
        public List<ReservaImport> DadosProcessados { get; set; } = new();
    }
}

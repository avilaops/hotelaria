namespace Hotelaria.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public string NumeroReserva { get; set; } = string.Empty;
        public int HospedeId { get; set; }
        public Hospede? Hospede { get; set; }
        public int QuartoId { get; set; }
        public Quarto? Quarto { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime DataReserva { get; set; } = DateTime.Now;
        public StatusReserva Status { get; set; } = StatusReserva.Pendente;
        
        // Campos Financeiros Detalhados
        public decimal ValorTotal { get; set; }
        public decimal Comissao { get; set; }
        public decimal TaxaTurismo { get; set; } // Taxa Booking
        public decimal DiariaLivreTaxa { get; set; } // Valor da diária sem taxas
        public decimal ValorComissaoMaisTaxa { get; set; } // Total comissão + tx turismo
        public decimal LivreTx { get; set; } // Valor livre de taxas
        public decimal DiariaForaPaga { get; set; } // Diária paga fora do sistema
        
        // Informações de Pagamento
        public TipoPagamento TipoPagamento { get; set; }
        public FormaPagamento FormaPagamento { get; set; } // Dinheiro, Cartão, etc
        public DateTime? DataPagamento { get; set; }
        public bool PagoOnline { get; set; } // Se foi pago via Booking/Online
        
        // Informações do Documento do Hóspede (redundância para relatório)
        public string? NumeroDocumentoHospede { get; set; }
        public DateTime? DataNascimentoHospede { get; set; }
        public string? PaisHospede { get; set; }
        public string? TipoDocumentoHospede { get; set; }
        
        // Informações do Quarto
        public string? NumeroQuarto { get; set; }
        public string? TipoCama { get; set; } // Ex: "Q 3 - Cama 01"
        
        // Hóspedes
        public int NumeroAdultos { get; set; }
        public int NumeroCriancas { get; set; }
        
        // Observações
        public string? Observacoes { get; set; }

        // Propriedades Calculadas
        public int DiasHospedagem => (CheckOut - CheckIn).Days;
        public int TotalPessoas => NumeroAdultos + NumeroCriancas;
        public int DiasPessoas => DiasHospedagem * TotalPessoas;
    }

    public enum StatusReserva
    {
        Pendente,
        Confirmada,
        CheckInRealizado,
        CheckOutRealizado,
        Cancelada,
        NoShow
    }

    public enum TipoPagamento
    {
        TransferenciaBancaria,
        CartaoCredito,
        Dinheiro,
        PIX,
        BookingCom
    }

    public enum FormaPagamento
    {
        Dinheiro,
        CartaoCredito,
        CartaoDebito,
        TransferenciaBancaria,
        PIX,
        Online,
        MBWay,
        Multibanco
    }
}

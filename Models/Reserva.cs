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
        public decimal ValorTotal { get; set; }
        public decimal Comissao { get; set; }
        public TipoPagamento TipoPagamento { get; set; }
        public int NumeroAdultos { get; set; }
        public int NumeroCriancas { get; set; }
        public string? Observacoes { get; set; }

        public int DiasHospedagem => (CheckOut - CheckIn).Days;
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
}

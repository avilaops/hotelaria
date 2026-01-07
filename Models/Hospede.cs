namespace Hotelaria.Models
{
    public class Hospede
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public DateTime? DataNascimento { get; set; }
        public string Pais { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}

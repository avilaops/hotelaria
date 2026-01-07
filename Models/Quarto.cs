namespace Hotelaria.Models
{
    public class Quarto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public TipoQuarto Tipo { get; set; }
        public int Capacidade { get; set; }
        public decimal PrecoPorNoite { get; set; }
        public StatusQuarto Status { get; set; } = StatusQuarto.Disponivel;
        public string Descricao { get; set; } = string.Empty;
        public List<string> Comodidades { get; set; } = new List<string>();
    }

    public enum TipoQuarto
    {
        Standard,
        Deluxe,
        Suite,
        Presidential
    }

    public enum StatusQuarto
    {
        Disponivel,
        Ocupado,
        Manutencao,
        Limpeza
    }
}

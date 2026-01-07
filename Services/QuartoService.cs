using Hotelaria.Models;

namespace Hotelaria.Services
{
    public class QuartoService
    {
        private readonly List<Quarto> _quartos = new();
        private int _nextId = 1;

        public QuartoService()
        {
            // Dados de exemplo
            AdicionarQuarto(new Quarto 
            { 
                Numero = "101", 
                Tipo = TipoQuarto.Standard, 
                Capacidade = 2, 
                PrecoPorNoite = 150.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Quarto standard com cama de casal",
                Comodidades = new List<string> { "Wi-Fi", "TV", "Ar condicionado", "Frigobar" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "102", 
                Tipo = TipoQuarto.Standard, 
                Capacidade = 2, 
                PrecoPorNoite = 150.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Quarto standard com duas camas de solteiro",
                Comodidades = new List<string> { "Wi-Fi", "TV", "Ar condicionado", "Frigobar" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "201", 
                Tipo = TipoQuarto.Deluxe, 
                Capacidade = 3, 
                PrecoPorNoite = 280.00m,
                Status = StatusQuarto.Ocupado,
                Descricao = "Quarto deluxe com varanda",
                Comodidades = new List<string> { "Wi-Fi", "TV Smart", "Ar condicionado", "Frigobar", "Varanda", "Cofre" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "301", 
                Tipo = TipoQuarto.Suite, 
                Capacidade = 4, 
                PrecoPorNoite = 450.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Suíte com sala de estar",
                Comodidades = new List<string> { "Wi-Fi", "TV Smart", "Ar condicionado", "Frigobar", "Varanda", "Cofre", "Banheira", "Sala de estar" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "401", 
                Tipo = TipoQuarto.Presidential, 
                Capacidade = 6, 
                PrecoPorNoite = 1200.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Suíte presidencial com vista panorâmica",
                Comodidades = new List<string> { "Wi-Fi", "TV Smart", "Ar condicionado", "Frigobar", "Varanda", "Cofre", "Banheira de hidromassagem", "Sala de estar", "Sala de jantar", "Cozinha" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "103", 
                Tipo = TipoQuarto.Standard, 
                Capacidade = 2, 
                PrecoPorNoite = 150.00m,
                Status = StatusQuarto.Limpeza,
                Descricao = "Quarto standard em limpeza",
                Comodidades = new List<string> { "Wi-Fi", "TV", "Ar condicionado", "Frigobar" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "104", 
                Tipo = TipoQuarto.Standard, 
                Capacidade = 2, 
                PrecoPorNoite = 150.00m,
                Status = StatusQuarto.Manutencao,
                Descricao = "Quarto em manutenção",
                Comodidades = new List<string> { "Wi-Fi", "TV", "Ar condicionado", "Frigobar" }
            });
        }

        public List<Quarto> ObterTodos() => _quartos;

        public Quarto? ObterPorId(int id) => _quartos.FirstOrDefault(q => q.Id == id);

        public List<Quarto> ObterDisponiveis() => _quartos.Where(q => q.Status == StatusQuarto.Disponivel).ToList();

        public void AdicionarQuarto(Quarto quarto)
        {
            quarto.Id = _nextId++;
            _quartos.Add(quarto);
        }

        public void AtualizarQuarto(Quarto quarto)
        {
            var index = _quartos.FindIndex(q => q.Id == quarto.Id);
            if (index != -1)
            {
                _quartos[index] = quarto;
            }
        }

        public void RemoverQuarto(int id)
        {
            var quarto = _quartos.FirstOrDefault(q => q.Id == id);
            if (quarto != null)
            {
                _quartos.Remove(quarto);
            }
        }

        public List<Quarto> FiltrarPorTipo(TipoQuarto? tipo)
        {
            if (tipo == null)
                return _quartos;

            return _quartos.Where(q => q.Tipo == tipo).ToList();
        }

        public List<Quarto> FiltrarPorStatus(StatusQuarto? status)
        {
            if (status == null)
                return _quartos;

            return _quartos.Where(q => q.Status == status).ToList();
        }

        public List<Quarto> BuscarQuartosDisponiveis(DateTime checkIn, DateTime checkOut, int capacidade)
        {
            return _quartos.Where(q => 
                q.Status == StatusQuarto.Disponivel && 
                q.Capacidade >= capacidade
            ).ToList();
        }
    }
}

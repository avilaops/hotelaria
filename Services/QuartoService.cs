using Hotelaria.Models;

namespace Hotelaria.Services
{
    public class QuartoService
    {
        private readonly List<Quarto> _quartos = new();
        private int _nextId = 1;

        public QuartoService()
        {
            // Dados de exemplo com número de vagas
            AdicionarQuarto(new Quarto 
            { 
                Numero = "1", 
                Tipo = TipoQuarto.Standard, 
                Capacidade = 2,
                NumeroVagas = 8, // 8 vagas/camas
                PrecoPorNoite = 20.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Quarto compartilhado com 8 camas",
                Comodidades = new List<string> { "Wi-Fi", "Armários", "Ar condicionado" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "2", 
                Tipo = TipoQuarto.Standard, 
                Capacidade = 2,
                NumeroVagas = 6, // 6 vagas/camas
                PrecoPorNoite = 20.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Quarto compartilhado com 6 camas",
                Comodidades = new List<string> { "Wi-Fi", "Armários", "Ar condicionado" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "3", 
                Tipo = TipoQuarto.Standard, 
                Capacidade = 2,
                NumeroVagas = 4, // 4 vagas/camas
                PrecoPorNoite = 25.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Quarto compartilhado com 4 camas",
                Comodidades = new List<string> { "Wi-Fi", "Armários", "Ar condicionado", "Frigobar" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "4", 
                Tipo = TipoQuarto.Deluxe, 
                Capacidade = 3,
                NumeroVagas = 3, // 3 vagas/camas
                PrecoPorNoite = 30.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Quarto compartilhado deluxe com 3 camas",
                Comodidades = new List<string> { "Wi-Fi", "Armários", "Ar condicionado", "Frigobar", "Banheiro privativo" }
            });
            
            AdicionarQuarto(new Quarto 
            { 
                Numero = "5", 
                Tipo = TipoQuarto.Suite, 
                Capacidade = 2,
                NumeroVagas = 2, // 2 vagas/camas
                PrecoPorNoite = 50.00m,
                Status = StatusQuarto.Disponivel,
                Descricao = "Quarto privado com 2 camas",
                Comodidades = new List<string> { "Wi-Fi", "TV", "Ar condicionado", "Frigobar", "Banheiro privativo", "Varanda" }
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

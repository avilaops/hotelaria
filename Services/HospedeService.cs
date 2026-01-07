using Hotelaria.Models;

namespace Hotelaria.Services
{
    public class HospedeService
    {
        private readonly List<Hospede> _hospedes = new();
        private int _nextId = 1;

        public HospedeService()
        {
            // Dados de exemplo
            AdicionarHospede(new Hospede { Nome = "Matheus Ferreira", Email = "matheus@email.com", Telefone = "+55 11 98765-4321", Documento = "123.456.789-00", Pais = "Brasil" });
            AdicionarHospede(new Hospede { Nome = "Dinesh Chaudhary", Email = "dinesh@email.com", Telefone = "+91 98765-43210", Documento = "ABC123456", Pais = "Índia" });
            AdicionarHospede(new Hospede { Nome = "Yañez Gonzalo", Email = "yanez@email.com", Telefone = "+34 612-345-678", Documento = "X1234567A", Pais = "Espanha" });
            AdicionarHospede(new Hospede { Nome = "Om Thakkar", Email = "om@email.com", Telefone = "+91 98123-45678", Documento = "DEF789012", Pais = "Índia" });
            AdicionarHospede(new Hospede { Nome = "Daniel Aileen", Email = "daniel@email.com", Telefone = "+1 555-123-4567", Documento = "987654321", Pais = "Estados Unidos" });
            AdicionarHospede(new Hospede { Nome = "Zach Jarretau", Email = "zach@email.com", Telefone = "+61 412-345-678", Documento = "AUS987654", Pais = "Austrália" });
        }

        public List<Hospede> ObterTodos() => _hospedes;

        public Hospede? ObterPorId(int id) => _hospedes.FirstOrDefault(h => h.Id == id);

        public void AdicionarHospede(Hospede hospede)
        {
            hospede.Id = _nextId++;
            hospede.DataCadastro = DateTime.Now;
            _hospedes.Add(hospede);
        }

        public void AtualizarHospede(Hospede hospede)
        {
            var index = _hospedes.FindIndex(h => h.Id == hospede.Id);
            if (index != -1)
            {
                _hospedes[index] = hospede;
            }
        }

        public void RemoverHospede(int id)
        {
            var hospede = _hospedes.FirstOrDefault(h => h.Id == id);
            if (hospede != null)
            {
                _hospedes.Remove(hospede);
            }
        }

        public List<Hospede> Buscar(string termo)
        {
            if (string.IsNullOrWhiteSpace(termo))
                return _hospedes;

            termo = termo.ToLower();
            return _hospedes.Where(h => 
                h.Nome.ToLower().Contains(termo) ||
                h.Email.ToLower().Contains(termo) ||
                h.Documento.ToLower().Contains(termo)
            ).ToList();
        }
    }
}

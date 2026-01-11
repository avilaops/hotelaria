using Hotelaria.Models;

namespace Hotelaria.Services
{
    public class HospedeService
    {
        private readonly List<Hospede> _hospedes = new();
        private int _nextId = 1;

        public HospedeService()
        {
            // Dados de exemplo expandidos
            AdicionarHospede(new Hospede { Nome = "Matheus Ferreira", Email = "matheus@email.com", Telefone = "+55 11 98765-4321", Documento = "123.456.789-00", Pais = "Brasil" });
            AdicionarHospede(new Hospede { Nome = "Dinesh Chaudhary", Email = "dinesh@email.com", Telefone = "+91 98765-43210", Documento = "ABC123456", Pais = "Índia" });
            AdicionarHospede(new Hospede { Nome = "Yañez Gonzalo", Email = "yanez@email.com", Telefone = "+34 612-345-678", Documento = "X1234567A", Pais = "Espanha" });
            AdicionarHospede(new Hospede { Nome = "Om Thakkar", Email = "om@email.com", Telefone = "+91 98123-45678", Documento = "DEF789012", Pais = "Índia" });
            AdicionarHospede(new Hospede { Nome = "Daniel Aileen", Email = "daniel@email.com", Telefone = "+1 555-123-4567", Documento = "987654321", Pais = "Estados Unidos" });
            AdicionarHospede(new Hospede { Nome = "Zach Jarretau", Email = "zach@email.com", Telefone = "+61 412-345-678", Documento = "AUS987654", Pais = "Austrália" });
            
            // Novos hóspedes
            AdicionarHospede(new Hospede { Nome = "Sophie Martin", Email = "sophie@email.com", Telefone = "+33 6 12 34 56 78", Documento = "FR123456", Pais = "França" });
            AdicionarHospede(new Hospede { Nome = "Lucas Silva", Email = "lucas@email.com", Telefone = "+55 21 99876-5432", Documento = "234.567.890-11", Pais = "Brasil" });
            AdicionarHospede(new Hospede { Nome = "Emma Johnson", Email = "emma@email.com", Telefone = "+44 7700 900123", Documento = "UK987654", Pais = "Reino Unido" });
            AdicionarHospede(new Hospede { Nome = "Hans Mueller", Email = "hans@email.com", Telefone = "+49 170 1234567", Documento = "DE789456", Pais = "Alemanha" });
            AdicionarHospede(new Hospede { Nome = "Maria Garcia", Email = "maria@email.com", Telefone = "+34 622-345-678", Documento = "ES456789", Pais = "Espanha" });
            AdicionarHospede(new Hospede { Nome = "João Costa", Email = "joao@email.com", Telefone = "+55 11 98765-1234", Documento = "345.678.901-22", Pais = "Brasil" });
            AdicionarHospede(new Hospede { Nome = "Anna Kowalski", Email = "anna@email.com", Telefone = "+48 601 234 567", Documento = "PL123789", Pais = "Polônia" });
            AdicionarHospede(new Hospede { Nome = "Marco Rossi", Email = "marco@email.com", Telefone = "+39 320 1234567", Documento = "IT789123", Pais = "Itália" });
            AdicionarHospede(new Hospede { Nome = "Sarah Lee", Email = "sarah@email.com", Telefone = "+1 415-555-6789", Documento = "USA456789", Pais = "Estados Unidos" });
            AdicionarHospede(new Hospede { Nome = "Pierre Dubois", Email = "pierre@email.com", Telefone = "+33 6 98 76 54 32", Documento = "FR789456", Pais = "França" });
            AdicionarHospede(new Hospede { Nome = "Yuki Tanaka", Email = "yuki@email.com", Telefone = "+81 90-1234-5678", Documento = "JP123456", Pais = "Japão" });
            AdicionarHospede(new Hospede { Nome = "Carlos Rodriguez", Email = "carlos@email.com", Telefone = "+52 55 1234 5678", Documento = "MX789456", Pais = "México" });
            AdicionarHospede(new Hospede { Nome = "Isabella Santos", Email = "isabella@email.com", Telefone = "+55 31 98765-4321", Documento = "456.789.012-33", Pais = "Brasil" });
            AdicionarHospede(new Hospede { Nome = "Thomas Berg", Email = "thomas@email.com", Telefone = "+46 70 123 45 67", Documento = "SE123789", Pais = "Suécia" });
            AdicionarHospede(new Hospede { Nome = "Lisa Anderson", Email = "lisa@email.com", Telefone = "+1 212-555-7890", Documento = "USA789123", Pais = "Estados Unidos" });
            AdicionarHospede(new Hospede { Nome = "Pavel Novak", Email = "pavel@email.com", Telefone = "+420 777 123 456", Documento = "CZ456789", Pais = "República Tcheca" });
            AdicionarHospede(new Hospede { Nome = "Camila Oliveira", Email = "camila@email.com", Telefone = "+55 41 99876-5432", Documento = "567.890.123-44", Pais = "Brasil" });
            AdicionarHospede(new Hospede { Nome = "Henrik Larsson", Email = "henrik@email.com", Telefone = "+46 73 456 78 90", Documento = "SE789456", Pais = "Suécia" });
            AdicionarHospede(new Hospede { Nome = "Natalia Petrov", Email = "natalia@email.com", Telefone = "+7 916 123 4567", Documento = "RU123456", Pais = "Rússia" });
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

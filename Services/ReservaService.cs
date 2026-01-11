using Hotelaria.Models;

namespace Hotelaria.Services
{
    public class ReservaService
    {
        private readonly List<Reserva> _reservas = new();
        private int _nextId = 1;
        private readonly HospedeService _hospedeService;
        private readonly QuartoService _quartoService;

        public ReservaService(HospedeService hospedeService, QuartoService quartoService)
        {
            _hospedeService = hospedeService;
            _quartoService = quartoService;
            InicializarDadosExemplo();
        }

        private void InicializarDadosExemplo()
        {
            var hospedes = _hospedeService.ObterTodos();
            var quartos = _quartoService.ObterTodos();

            if (!hospedes.Any() || !quartos.Any()) return;

            int reservaId = 0;
            
            // Quarto 1 (8 vagas) - Preencher apenas 3 vagas como exemplo
            CriarReservasQuarto(quartos[0], 1, hospedes[0], new DateTime(2026, 1, 7), new DateTime(2026, 1, 10), ref reservaId);
            CriarReservasQuarto(quartos[0], 1, hospedes[1], new DateTime(2026, 1, 10), new DateTime(2026, 1, 14), ref reservaId);
            CriarReservasQuarto(quartos[0], 1, hospedes[2], new DateTime(2026, 1, 14), new DateTime(2026, 1, 18), ref reservaId);
            CriarReservasQuarto(quartos[0], 1, hospedes[3], new DateTime(2026, 1, 18), new DateTime(2026, 1, 22), ref reservaId);
            CriarReservasQuarto(quartos[0], 1, hospedes[4], new DateTime(2026, 1, 22), new DateTime(2026, 1, 26), ref reservaId);
            CriarReservasQuarto(quartos[0], 1, hospedes[5], new DateTime(2026, 1, 26), new DateTime(2026, 1, 31), ref reservaId);
            
            CriarReservasQuarto(quartos[0], 2, hospedes[6], new DateTime(2026, 1, 7), new DateTime(2026, 1, 12), ref reservaId);
            CriarReservasQuarto(quartos[0], 2, hospedes[7], new DateTime(2026, 1, 12), new DateTime(2026, 1, 17), ref reservaId);
            CriarReservasQuarto(quartos[0], 2, hospedes[8], new DateTime(2026, 1, 17), new DateTime(2026, 1, 23), ref reservaId);
            CriarReservasQuarto(quartos[0], 2, hospedes[9], new DateTime(2026, 1, 23), new DateTime(2026, 1, 31), ref reservaId);
            
            CriarReservasQuarto(quartos[0], 3, hospedes[10], new DateTime(2026, 1, 8), new DateTime(2026, 1, 13), ref reservaId);
            CriarReservasQuarto(quartos[0], 3, hospedes[11], new DateTime(2026, 1, 13), new DateTime(2026, 1, 19), ref reservaId);
            CriarReservasQuarto(quartos[0], 3, hospedes[12], new DateTime(2026, 1, 19), new DateTime(2026, 1, 25), ref reservaId);
            CriarReservasQuarto(quartos[0], 3, hospedes[13], new DateTime(2026, 1, 25), new DateTime(2026, 1, 31), ref reservaId);

            // Quarto 2 (6 vagas) - Preencher 2 vagas
            CriarReservasQuarto(quartos[1], 1, hospedes[14], new DateTime(2026, 1, 7), new DateTime(2026, 1, 11), ref reservaId);
            CriarReservasQuarto(quartos[1], 1, hospedes[15], new DateTime(2026, 1, 11), new DateTime(2026, 1, 16), ref reservaId);
            CriarReservasQuarto(quartos[1], 1, hospedes[16], new DateTime(2026, 1, 16), new DateTime(2026, 1, 21), ref reservaId);
            CriarReservasQuarto(quartos[1], 1, hospedes[17], new DateTime(2026, 1, 21), new DateTime(2026, 1, 31), ref reservaId);
            
            CriarReservasQuarto(quartos[1], 2, hospedes[18], new DateTime(2026, 1, 7), new DateTime(2026, 1, 14), ref reservaId);
            CriarReservasQuarto(quartos[1], 2, hospedes[19], new DateTime(2026, 1, 14), new DateTime(2026, 1, 20), ref reservaId);
            CriarReservasQuarto(quartos[1], 2, hospedes[20], new DateTime(2026, 1, 20), new DateTime(2026, 1, 31), ref reservaId);

            // Quarto 3 (4 vagas) - Preencher 2 vagas
            CriarReservasQuarto(quartos[2], 1, hospedes[21], new DateTime(2026, 1, 7), new DateTime(2026, 1, 15), ref reservaId);
            CriarReservasQuarto(quartos[2], 1, hospedes[22], new DateTime(2026, 1, 15), new DateTime(2026, 1, 24), ref reservaId);
            CriarReservasQuarto(quartos[2], 1, hospedes[23], new DateTime(2026, 1, 24), new DateTime(2026, 1, 31), ref reservaId);
            
            CriarReservasQuarto(quartos[2], 2, hospedes[0], new DateTime(2026, 1, 9), new DateTime(2026, 1, 17), ref reservaId);
            CriarReservasQuarto(quartos[2], 2, hospedes[1], new DateTime(2026, 1, 17), new DateTime(2026, 1, 26), ref reservaId);

            // Quarto 4 (3 vagas) - Preencher 2 vagas
            CriarReservasQuarto(quartos[3], 1, hospedes[2], new DateTime(2026, 1, 7), new DateTime(2026, 1, 12), ref reservaId);
            CriarReservasQuarto(quartos[3], 1, hospedes[3], new DateTime(2026, 1, 12), new DateTime(2026, 1, 19), ref reservaId);
            CriarReservasQuarto(quartos[3], 1, hospedes[4], new DateTime(2026, 1, 19), new DateTime(2026, 1, 31), ref reservaId);
            
            CriarReservasQuarto(quartos[3], 2, hospedes[5], new DateTime(2026, 1, 8), new DateTime(2026, 1, 16), ref reservaId);
            CriarReservasQuarto(quartos[3], 2, hospedes[6], new DateTime(2026, 1, 16), new DateTime(2026, 1, 28), ref reservaId);

            // Quarto 5 (2 vagas) - Preencher ambas
            CriarReservasQuarto(quartos[4], 1, hospedes[7], new DateTime(2026, 1, 7), new DateTime(2026, 1, 13), ref reservaId);
            CriarReservasQuarto(quartos[4], 1, hospedes[8], new DateTime(2026, 1, 13), new DateTime(2026, 1, 20), ref reservaId);
            CriarReservasQuarto(quartos[4], 1, hospedes[9], new DateTime(2026, 1, 20), new DateTime(2026, 1, 31), ref reservaId);
            
            CriarReservasQuarto(quartos[4], 2, hospedes[10], new DateTime(2026, 1, 7), new DateTime(2026, 1, 16), ref reservaId);
            CriarReservasQuarto(quartos[4], 2, hospedes[11], new DateTime(2026, 1, 16), new DateTime(2026, 1, 25), ref reservaId);
            CriarReservasQuarto(quartos[4], 2, hospedes[12], new DateTime(2026, 1, 25), new DateTime(2026, 1, 31), ref reservaId);
        }

        private void CriarReservasQuarto(Quarto quarto, int vaga, Hospede hospede, DateTime checkIn, DateTime checkOut, ref int reservaId)
        {
            var dias = (checkOut - checkIn).Days;
            var valorTotal = quarto.PrecoPorNoite * dias;
            var comissao = valorTotal * 0.15m;

            AdicionarReserva(new Reserva
            {
                NumeroReserva = $"{5000000 + reservaId}",
                HospedeId = hospede.Id,
                QuartoId = quarto.Id,
                CheckIn = checkIn,
                CheckOut = checkOut,
                DataReserva = checkIn.AddDays(-7),
                Status = checkIn.Date <= DateTime.Today ? StatusReserva.CheckInRealizado : StatusReserva.Confirmada,
                NumeroAdultos = 1,
                NumeroCriancas = 0,
                TipoPagamento = TipoPagamento.TransferenciaBancaria,
                ValorTotal = Math.Round(valorTotal, 2),
                Comissao = Math.Round(comissao, 2),
                Observacoes = $"Vaga:{vaga}"
            });
            
            reservaId++;
        }

        public List<Reserva> ObterTodas()
        {
            foreach (var reserva in _reservas)
            {
                reserva.Hospede = _hospedeService.ObterPorId(reserva.HospedeId);
                reserva.Quarto = _quartoService.ObterPorId(reserva.QuartoId);
            }
            return _reservas;
        }

        public Reserva? ObterPorId(int id)
        {
            var reserva = _reservas.FirstOrDefault(r => r.Id == id);
            if (reserva != null)
            {
                reserva.Hospede = _hospedeService.ObterPorId(reserva.HospedeId);
                reserva.Quarto = _quartoService.ObterPorId(reserva.QuartoId);
            }
            return reserva;
        }

        public void AdicionarReserva(Reserva reserva)
        {
            reserva.Id = _nextId++;
            if (string.IsNullOrEmpty(reserva.NumeroReserva))
            {
                reserva.NumeroReserva = GerarNumeroReserva();
            }
            _reservas.Add(reserva);
        }

        public void AtualizarReserva(Reserva reserva)
        {
            var index = _reservas.FindIndex(r => r.Id == reserva.Id);
            if (index != -1)
            {
                _reservas[index] = reserva;
            }
        }

        public void RemoverReserva(int id)
        {
            var reserva = _reservas.FirstOrDefault(r => r.Id == id);
            if (reserva != null)
            {
                _reservas.Remove(reserva);
            }
        }

        public List<Reserva> FiltrarPorPeriodo(DateTime? dataInicio, DateTime? dataFim)
        {
            var reservas = ObterTodas();

            if (dataInicio.HasValue)
            {
                reservas = reservas.Where(r => r.CheckIn >= dataInicio.Value).ToList();
            }

            if (dataFim.HasValue)
            {
                reservas = reservas.Where(r => r.CheckOut <= dataFim.Value).ToList();
            }

            return reservas;
        }

        public List<Reserva> FiltrarPorStatus(StatusReserva? status)
        {
            if (status == null)
                return ObterTodas();

            return ObterTodas().Where(r => r.Status == status).ToList();
        }

        public List<Reserva> BuscarPorHospede(string nomeHospede)
        {
            if (string.IsNullOrWhiteSpace(nomeHospede))
                return ObterTodas();

            nomeHospede = nomeHospede.ToLower();
            return ObterTodas().Where(r => 
                r.Hospede != null && r.Hospede.Nome.ToLower().Contains(nomeHospede)
            ).ToList();
        }

        public List<Reserva> BuscarPorNumeroReserva(string numeroReserva)
        {
            if (string.IsNullOrWhiteSpace(numeroReserva))
                return ObterTodas();

            return ObterTodas().Where(r => 
                r.NumeroReserva.Contains(numeroReserva, StringComparison.OrdinalIgnoreCase)
            ).ToList();
        }

        public decimal CalcularValorTotal(int quartoId, DateTime checkIn, DateTime checkOut)
        {
            var quarto = _quartoService.ObterPorId(quartoId);
            if (quarto == null) return 0;

            var dias = (checkOut - checkIn).Days;
            return quarto.PrecoPorNoite * dias;
        }

        private string GerarNumeroReserva()
        {
            return $"{5000000 + _nextId}";
        }

        public Dictionary<string, int> ObterEstatisticas()
        {
            var reservas = ObterTodas();
            return new Dictionary<string, int>
            {
                { "Total", reservas.Count },
                { "Confirmadas", reservas.Count(r => r.Status == StatusReserva.Confirmada) },
                { "CheckIn", reservas.Count(r => r.Status == StatusReserva.CheckInRealizado) },
                { "Pendentes", reservas.Count(r => r.Status == StatusReserva.Pendente) },
                { "Canceladas", reservas.Count(r => r.Status == StatusReserva.Cancelada) }
            };
        }
    }
}

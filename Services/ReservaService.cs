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

            if (hospedes.Any() && quartos.Any())
            {
                // Matheus Ferreira - Quarto 1
                AdicionarReserva(new Reserva
                {
                    NumeroReserva = "5553486426",
                    HospedeId = hospedes[0].Id,
                    QuartoId = quartos[0].Id,
                    CheckIn = new DateTime(2026, 1, 7),
                    CheckOut = new DateTime(2026, 1, 8),
                    DataReserva = new DateTime(2026, 1, 4),
                    Status = StatusReserva.Confirmada,
                    NumeroAdultos = 1,
                    NumeroCriancas = 0,
                    TipoPagamento = TipoPagamento.TransferenciaBancaria,
                    ValorTotal = 5.78m,
                    Comissao = 1.28m
                });

                // Dinesh Chaudhary - Quarto 1
                AdicionarReserva(new Reserva
                {
                    NumeroReserva = "5581128004",
                    HospedeId = hospedes[1].Id,
                    QuartoId = quartos[0].Id,
                    CheckIn = new DateTime(2026, 1, 7),
                    CheckOut = new DateTime(2026, 1, 8),
                    DataReserva = new DateTime(2026, 1, 6),
                    Status = StatusReserva.Confirmada,
                    NumeroAdultos = 1,
                    NumeroCriancas = 0,
                    TipoPagamento = TipoPagamento.TransferenciaBancaria,
                    ValorTotal = 8.42m,
                    Comissao = 1.64m
                });

                // Yañez Gonzalo - Quarto 4
                AdicionarReserva(new Reserva
                {
                    NumeroReserva = "5980122694",
                    HospedeId = hospedes[2].Id,
                    QuartoId = quartos[3].Id,
                    CheckIn = new DateTime(2026, 1, 7),
                    CheckOut = new DateTime(2026, 1, 8),
                    DataReserva = new DateTime(2026, 1, 6),
                    Status = StatusReserva.Confirmada,
                    NumeroAdultos = 1,
                    NumeroCriancas = 0,
                    TipoPagamento = TipoPagamento.TransferenciaBancaria,
                    ValorTotal = 8.64m,
                    Comissao = 1.93m
                });

                // Om Thakkar - Quarto 3
                AdicionarReserva(new Reserva
                {
                    NumeroReserva = "6015137919",
                    HospedeId = hospedes[3].Id,
                    QuartoId = quartos[2].Id,
                    CheckIn = new DateTime(2026, 1, 7),
                    CheckOut = new DateTime(2026, 1, 8),
                    DataReserva = new DateTime(2026, 1, 7),
                    Status = StatusReserva.Confirmada,
                    NumeroAdultos = 1,
                    NumeroCriancas = 0,
                    TipoPagamento = TipoPagamento.TransferenciaBancaria,
                    ValorTotal = 10.08m,
                    Comissao = 2.20m
                });

                // Daniel Aileen - Quarto 5
                AdicionarReserva(new Reserva
                {
                    NumeroReserva = "6144892752",
                    HospedeId = hospedes[4].Id,
                    QuartoId = quartos[4].Id,
                    CheckIn = new DateTime(2026, 1, 7),
                    CheckOut = new DateTime(2026, 1, 8),
                    DataReserva = new DateTime(2025, 12, 11),
                    Status = StatusReserva.Confirmada,
                    NumeroAdultos = 1,
                    NumeroCriancas = 0,
                    TipoPagamento = TipoPagamento.TransferenciaBancaria,
                    ValorTotal = 10.20m,
                    Comissao = 2.22m
                });

                // Zach Jarretau - Quarto 1
                AdicionarReserva(new Reserva
                {
                    NumeroReserva = "6184570733",
                    HospedeId = hospedes[5].Id,
                    QuartoId = quartos[0].Id,
                    CheckIn = new DateTime(2026, 1, 7),
                    CheckOut = new DateTime(2026, 1, 10),
                    DataReserva = new DateTime(2025, 12, 16),
                    Status = StatusReserva.Confirmada,
                    NumeroAdultos = 1,
                    NumeroCriancas = 0,
                    TipoPagamento = TipoPagamento.TransferenciaBancaria,
                    ValorTotal = 21.60m,
                    Comissao = 4.75m
                });
            }
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
            return $"{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
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

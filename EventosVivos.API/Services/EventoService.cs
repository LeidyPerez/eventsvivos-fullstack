using EventosVivos.API.DTOs;
using EventosVivos.API.Interfaces;
using EventosVivos.API.Models;

namespace EventosVivos.API.Services
{
    public class EventoService : IEventoService
    {
        private readonly List<Evento> _eventos = new();
        private readonly List<Reserva> _reservas = new();
        private readonly List<Venue> _venues = new()
        {
            new Venue { Id = 1, Nombre = "Auditorio Central", Capacidad = 200, Ciudad = "Bogotá" },
            new Venue { Id = 2, Nombre = "Sala Norte", Capacidad = 50, Ciudad = "Bogotá" },
            new Venue { Id = 3, Nombre = "Arena Sur", Capacidad = 500, Ciudad = "Medellín" }
        };

        public Evento CrearEvento(CrearEventoDto dto)
        {
            var venue = _venues.FirstOrDefault(v => v.Id == dto.VenueId)
                ?? throw new Exception("Venue no encontrado");

            if (dto.CapacidadMaxima > venue.Capacidad)
                throw new Exception($"La capacidad máxima no puede exceder la del venue ({venue.Capacidad})");

            if (dto.FechaHoraInicio <= DateTime.Now)
                throw new Exception("La fecha de inicio debe ser futura");

            if (dto.FechaHoraFin <= dto.FechaHoraInicio)
                throw new Exception("La fecha de fin debe ser posterior al inicio");

            // RN-02: Verificar superposición de venues
            var superposicion = _eventos.Any(e =>
                e.VenueId == dto.VenueId &&
                e.Estado == "activo" &&
                e.FechaHoraInicio < dto.FechaHoraFin &&
                e.FechaHoraFin > dto.FechaHoraInicio);

            if (superposicion)
                throw new Exception("El venue ya tiene un evento en ese horario");

            // RN-03: Restricción horario nocturno en fines de semana
            if ((dto.FechaHoraInicio.DayOfWeek == DayOfWeek.Saturday ||
                 dto.FechaHoraInicio.DayOfWeek == DayOfWeek.Sunday) &&
                 dto.FechaHoraInicio.Hour >= 22)
                throw new Exception("Los eventos en fin de semana no pueden iniciar después de las 22:00");

            var tiposValidos = new[] { "conferencia", "taller", "concierto" };
            if (!tiposValidos.Contains(dto.TipoEvento.ToLower()))
                throw new Exception("Tipo de evento inválido. Use: conferencia, taller o concierto");

            var evento = new Evento
            {
                VenueId = dto.VenueId,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                CapacidadMaxima = dto.CapacidadMaxima,
                FechaHoraInicio = dto.FechaHoraInicio,
                FechaHoraFin = dto.FechaHoraFin,
                PrecioEntrada = dto.PrecioEntrada,
                TipoEvento = dto.TipoEvento.ToLower(),
                Estado = "activo"
            };

            _eventos.Add(evento);
            return evento;
        }

        public List<Evento> ListarEventos(string? tipo, DateTime? fechaInicio, DateTime? fechaFin, int? venueId, string? estado, string? titulo)
        {
            // RN-06: Actualizar estado de eventos completados
            foreach (var e in _eventos)
                if (DateTime.Now > e.FechaHoraFin && e.Estado == "activo")
                    e.Estado = "completado";

            var query = _eventos.AsQueryable();

            if (!string.IsNullOrEmpty(tipo))
                query = query.Where(e => e.TipoEvento == tipo.ToLower());

            if (fechaInicio.HasValue)
                query = query.Where(e => e.FechaHoraInicio >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(e => e.FechaHoraInicio <= fechaFin.Value);

            if (venueId.HasValue)
                query = query.Where(e => e.VenueId == venueId.Value);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(e => e.Estado == estado.ToLower());

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(e => e.Titulo.ToLower().Contains(titulo.ToLower()));

            return query.ToList();
        }

        public Reserva CrearReserva(CrearReservaDto dto)
        {
            var evento = _eventos.FirstOrDefault(e => e.Id == dto.EventoId)
                ?? throw new Exception("Evento no encontrado");

            if (evento.Estado != "activo")
                throw new Exception("No se pueden hacer reservas en eventos que no están activos");

            // RN-04: No reservas con menos de 1 hora
            if ((evento.FechaHoraInicio - DateTime.Now).TotalHours < 1)
                throw new Exception("No se permiten reservas para eventos que inician en menos de 1 hora");

            // RN-05 y RF-03: Límite de entradas
            int maxEntradas = (evento.FechaHoraInicio - DateTime.Now).TotalHours < 24 ? 5 :
                              evento.PrecioEntrada > 100 ? 10 : int.MaxValue;

            if (dto.Cantidad > maxEntradas)
                throw new Exception($"Máximo {maxEntradas} entradas por transacción");

            // Verificar disponibilidad
            var entradasVendidas = _reservas
                .Where(r => r.EventoId == dto.EventoId && r.Estado == "confirmada")
                .Sum(r => r.Cantidad);

            var entradasPerdidas = _reservas
                .Where(r => r.EventoId == dto.EventoId && r.EntradasPerdidas)
                .Sum(r => r.Cantidad);

            var disponibles = evento.CapacidadMaxima - entradasVendidas - entradasPerdidas;

            if (dto.Cantidad > disponibles)
                throw new Exception($"Solo hay {disponibles} entradas disponibles");

            var reserva = new Reserva
            {
                EventoId = dto.EventoId,
                Cantidad = dto.Cantidad,
                NombreComprador = dto.NombreComprador,
                EmailComprador = dto.EmailComprador,
                Estado = "pendiente_pago"
            };

            _reservas.Add(reserva);
            return reserva;
        }

        public Reserva ConfirmarPago(Guid reservaId)
        {
            var reserva = _reservas.FirstOrDefault(r => r.Id == reservaId)
                ?? throw new Exception("Reserva no encontrada");

            if (reserva.Estado == "confirmada")
                throw new Exception("La reserva ya está confirmada");

            if (reserva.Estado == "cancelada")
                throw new Exception("No se puede confirmar una reserva cancelada");

            reserva.Estado = "confirmada";
            reserva.CodigoReserva = $"EV-{new Random().Next(100000, 999999)}";

            return reserva;
        }

        public Reserva CancelarReserva(Guid reservaId)
        {
            var reserva = _reservas.FirstOrDefault(r => r.Id == reservaId)
                ?? throw new Exception("Reserva no encontrada");

            if (reserva.Estado == "cancelada")
                throw new Exception("La reserva ya está cancelada");

            if (reserva.Estado == "pendiente_pago")
                throw new Exception("Solo se pueden cancelar reservas confirmadas");

            var evento = _eventos.First(e => e.Id == reserva.EventoId);

            // RN-07: Penalización si cancela con menos de 48 horas
            if ((evento.FechaHoraInicio - DateTime.Now).TotalHours < 48)
                reserva.EntradasPerdidas = true;

            reserva.Estado = "cancelada";
            reserva.FechaCancelacion = DateTime.Now;

            return reserva;
        }

        public object ObtenerReporteOcupacion(Guid eventoId)
        {
            var evento = _eventos.FirstOrDefault(e => e.Id == eventoId)
                ?? throw new Exception("Evento no encontrado");

            var reservasEvento = _reservas.Where(r => r.EventoId == eventoId).ToList();

            var entradasVendidas = reservasEvento
                .Where(r => r.Estado == "confirmada")
                .Sum(r => r.Cantidad);

            var entradasPerdidas = reservasEvento
                .Where(r => r.EntradasPerdidas)
                .Sum(r => r.Cantidad);

            var disponibles = evento.CapacidadMaxima - entradasVendidas - entradasPerdidas;
            var porcentaje = evento.CapacidadMaxima > 0
                ? (double)entradasVendidas / evento.CapacidadMaxima * 100
                : 0;

            return new
            {
                EventoId = evento.Id,
                Titulo = evento.Titulo,
                EntradasVendidas = entradasVendidas,
                EntradasDisponibles = disponibles,
                PorcentajeOcupacion = Math.Round(porcentaje, 2),
                TotalIngresos = entradasVendidas * evento.PrecioEntrada,
                Estado = evento.Estado
            };
        }
    }
}
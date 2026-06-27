using EventosVivos.API.DTOs;
using EventosVivos.API.Models;

namespace EventosVivos.API.Interfaces
{
    public interface IEventoService
    {
        Evento CrearEvento(CrearEventoDto dto);
        List<Evento> ListarEventos(string? tipo, DateTime? fechaHoraInicio, DateTime? fechaHoraFin, int? venueId, string? estado, string? titulo);
        Reserva CrearReserva(CrearReservaDto dto);
        Reserva ConfirmarPago(Guid reservaId);
        Reserva CancelarReserva(Guid reservaId);
        object ObtenerReporteOcupacion(Guid eventoId);
    }
}

using EventosVivos.API.DTOs;
using EventosVivos.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EventosVivos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly IEventoService _service;

        public EventosController(IEventoService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult CrearEvento([FromBody] CrearEventoDto dto)
        {
            try
            {
                var evento = _service.CrearEvento(dto);
                return CreatedAtAction(nameof(CrearEvento), new { id = evento.Id }, evento);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult ListarEventos(
            [FromQuery] string? tipo,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] int? venueId,
            [FromQuery] string? estado,
            [FromQuery] string? titulo)
        {
            var eventos = _service.ListarEventos(tipo, fechaInicio, fechaFin, venueId, estado, titulo);
            return Ok(eventos);
        }

        [HttpPost("reservas")]
        public IActionResult CrearReserva([FromBody] CrearReservaDto dto)
        {
            try
            {
                var reserva = _service.CrearReserva(dto);
                return CreatedAtAction(nameof(CrearReserva), new { id = reserva.Id }, reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("reservas/{id}/confirmar")]
        public IActionResult ConfirmarPago(Guid id)
        {
            try
            {
                var reserva = _service.ConfirmarPago(id);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("reservas/{id}/cancelar")]
        public IActionResult CancelarReserva(Guid id)
        {
            try
            {
                var reserva = _service.CancelarReserva(id);
                return Ok(reserva);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{id}/reporte")]
        public IActionResult ObtenerReporte(Guid id)
        {
            try
            {
                var reporte = _service.ObtenerReporteOcupacion(id);
                return Ok(reporte);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
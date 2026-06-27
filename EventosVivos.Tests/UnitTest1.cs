using EventosVivos.API.DTOs;
using EventosVivos.API.Services;

namespace EventosVivos.Tests
{
    public class EventoServiceTests
    {
        private readonly EventoService _service;

        public EventoServiceTests()
        {
            _service = new EventoService();
        }

        private CrearEventoDto CrearEventoDtoValido() => new CrearEventoDto
        {
            Titulo = "Conferencia de Tech",
            Descripcion = "Una conferencia sobre tecnología moderna",
            VenueId = 1,
            CapacidadMaxima = 100,
            FechaHoraInicio = DateTime.Now.AddDays(10),
            FechaHoraFin = DateTime.Now.AddDays(10).AddHours(8),
            PrecioEntrada = 50,
            TipoEvento = "conferencia"
        };

        // PRUEBA 1: Crear evento con datos válidos
        [Fact]
        public void CrearEvento_ConDatosValidos_RetornaEvento()
        {
            // Arrange
            var dto = CrearEventoDtoValido();

            // Act
            var resultado = _service.CrearEvento(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("activo", resultado.Estado);
            Assert.Equal(dto.Titulo, resultado.Titulo);
        }

        // PRUEBA 2: Capacidad mayor al venue
        [Fact]
        public void CrearEvento_CapacidadMayorAlVenue_LanzaExcepcion()
        {
            // Arrange — Auditorio Central tiene capacidad 200
            var dto = CrearEventoDtoValido();
            dto.CapacidadMaxima = 500;

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.CrearEvento(dto));
            Assert.Contains("capacidad", ex.Message.ToLower());
        }

        // PRUEBA 3: Evento en fin de semana después de las 22:00
        [Fact]
        public void CrearEvento_FinDeSemanaAfter22_LanzaExcepcion()
        {
            // Arrange — buscar próximo sábado
            var dto = CrearEventoDtoValido();
            var proximoSabado = DateTime.Now.AddDays(1);
            while (proximoSabado.DayOfWeek != DayOfWeek.Saturday)
                proximoSabado = proximoSabado.AddDays(1);

            dto.FechaHoraInicio = new DateTime(proximoSabado.Year, proximoSabado.Month, proximoSabado.Day, 23, 0, 0);
            dto.FechaHoraFin = dto.FechaHoraInicio.AddHours(2);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.CrearEvento(dto));
            Assert.Contains("22:00", ex.Message);
        }

        // PRUEBA 4: Tipo de evento inválido
        [Fact]
        public void CrearEvento_TipoInvalido_LanzaExcepcion()
        {
            // Arrange
            var dto = CrearEventoDtoValido();
            dto.TipoEvento = "festival";

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.CrearEvento(dto));
            Assert.Contains("tipo", ex.Message.ToLower());
        }

        // PRUEBA 5: Reservar entradas disponibles
        [Fact]
        public void CrearReserva_ConEntradasDisponibles_RetornaReserva()
        {
            // Arrange
            var evento = _service.CrearEvento(CrearEventoDtoValido());
            var dto = new CrearReservaDto
            {
                EventoId = evento.Id,
                Cantidad = 2,
                NombreComprador = "Juan Pérez",
                EmailComprador = "juan@test.com"
            };

            // Act
            var resultado = _service.CrearReserva(dto);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("pendiente_pago", resultado.Estado);
            Assert.Equal(2, resultado.Cantidad);
        }

        // PRUEBA 6: Reservar más entradas de las disponibles
        [Fact]
        public void CrearReserva_SinEntradasDisponibles_LanzaExcepcion()
        {
            // Arrange
            var dto = CrearEventoDtoValido();
            dto.CapacidadMaxima = 2;
            var evento = _service.CrearEvento(dto);

            // Reservar y confirmar las 2 entradas disponibles
            var reservaDto = new CrearReservaDto
            {
                EventoId = evento.Id,
                Cantidad = 2,
                NombreComprador = "Juan Pérez",
                EmailComprador = "juan@test.com"
            };
            var reserva = _service.CrearReserva(reservaDto);
            _service.ConfirmarPago(reserva.Id);

            // Intentar reservar una más
            var reservaDto2 = new CrearReservaDto
            {
                EventoId = evento.Id,
                Cantidad = 1,
                NombreComprador = "María López",
                EmailComprador = "maria@test.com"
            };

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.CrearReserva(reservaDto2));
            Assert.Contains("disponibles", ex.Message.ToLower());
        }

        // PRUEBA 7: Confirmar pago
        [Fact]
        public void ConfirmarPago_ReservaPendiente_RetornaConfirmada()
        {
            // Arrange
            var evento = _service.CrearEvento(CrearEventoDtoValido());
            var reserva = _service.CrearReserva(new CrearReservaDto
            {
                EventoId = evento.Id,
                Cantidad = 1,
                NombreComprador = "Ana García",
                EmailComprador = "ana@test.com"
            });

            // Act
            var resultado = _service.ConfirmarPago(reserva.Id);

            // Assert
            Assert.Equal("confirmada", resultado.Estado);
            Assert.NotNull(resultado.CodigoReserva);
            Assert.StartsWith("EV-", resultado.CodigoReserva);
        }
    }
}
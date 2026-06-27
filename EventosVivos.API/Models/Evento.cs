namespace EventosVivos.API.Models
{
    public class Evento
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int VenueId { get; set; }
        public int CapacidadMaxima { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public decimal PrecioEntrada { get; set; }
        public string TipoEvento { get; set; } = string.Empty;
        public string Estado { get; set; } = "activo";
        public List<Reserva> Reservas { get; set; } = new();

    }
}

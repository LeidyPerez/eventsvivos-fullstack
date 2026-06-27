namespace EventosVivos.API.Models
{
    public class Reserva
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EventoId { get; set; }
        public int Cantidad { get; set; }
        public string NombreComprador { get; set; } = string.Empty;
        public string EmailComprador { get; set; } = string.Empty;
        public string Estado { get; set; } = "pendiente_pago";
        public string? CodigoReserva { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaCancelacion { get; set; }
        public bool EntradasPerdidas { get; set; } = false;
    }
}

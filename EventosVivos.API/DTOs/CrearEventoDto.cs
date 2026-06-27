using System.ComponentModel.DataAnnotations;

namespace EventosVivos.API.DTOs
{
    public class CrearEventoDto
    {
        [Required]
        [MinLength(5), MaxLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [MinLength(10), MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int VenueId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CapacidadMaxima { get; set; }

        [Required]
        public DateTime FechaHoraInicio { get; set; }

        [Required]
        public DateTime FechaHoraFin { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PrecioEntrada { get; set; }

        [Required]
        public string TipoEvento { get; set; } = string.Empty;

    }
}

using System.ComponentModel.DataAnnotations;

namespace EventosVivos.API.DTOs
{
    public class CrearReservaDto
    {
        [Required]
        public Guid EventoId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        public string NombreComprador { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string EmailComprador { get; set; } = string.Empty;

    }
}

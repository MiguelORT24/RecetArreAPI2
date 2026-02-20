using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetArreAPI2.Models
{
    public class Ingrediente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = default!;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string UnidadMedida { get; set; } = default!;

        [StringLength(100, MinimumLength = 2)]
        public string? Descripcion { get; set; }

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;

        // ---- Relación con ApplicationUser (quién creó el ingrediente) ----
        [ForeignKey("ApplicationUser")]
        public string? CreadoPorUsuarioId { get; set; }

        public ApplicationUser? CreadoPorUsuario { get; set; }

    }
}

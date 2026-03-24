using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetArreAPI2.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [StringLength(700, MinimumLength = 0)]
        public string Contenido { get; set; } = default!;

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;

        // Relación con ApplicationUser (quién creó el comentario)
        [ForeignKey(nameof(CreadoPorUsuario))]
        public string? CreadoPorUsuarioId { get; set; }

        public ApplicationUser? CreadoPorUsuario { get; set; }

        // Relación con Receta (a qué receta pertenece el comentario)
        [ForeignKey(nameof(Receta))]
        public int RecetaId { get; set; }

        public Receta? Receta { get; set; }

    }
}

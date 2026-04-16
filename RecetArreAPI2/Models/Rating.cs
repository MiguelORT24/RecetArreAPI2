using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetArreAPI2.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Calificacion { get; set; }

        public DateTime CalificadoUtc { get; set; } = DateTime.UtcNow;

        // Relación con ApplicationUser (quién calificó)
        [ForeignKey(nameof(CalificadoPorUsuario))]
        public string? CalificadoPorUsuarioId { get; set; }


        public ApplicationUser? CalificadoPorUsuario { get; set; }

        // Un rating pertenece a una receta
        [ForeignKey(nameof(Receta))]
        public int RecetaId { get; set; }

        public Receta? Receta { get; set; }

    }
}

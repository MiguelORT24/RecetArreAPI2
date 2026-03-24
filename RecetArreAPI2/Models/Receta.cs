using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace RecetArreAPI2.Models
{
    public class Receta
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Nombre { get; set; } = default!;

        [Required] 
        public string? Instrucciones { get; set; }

        public DateTime CreadoUtc { get; set; } = DateTime.UtcNow;

        // Relación con ApplicationUser (quién creó la categoría)
        [ForeignKey("ApplicationUser")]
        public string? CreadoPorUsuarioId { get; set; }

        public ApplicationUser? CreadoPorUsuario { get; set; }

        // Relación muchos-a-muchos con Tiempo a través de Rec_Tiem
        public ICollection<Rec_Tiem> Rec_Tiems { get; set; } = new List<Rec_Tiem>();
        
        // Relación muchos-a-muchos con Ingrediente a través de Ing_Rec
        public ICollection<Ing_Rec> Ing_Recs { get; set; } = new List<Ing_Rec>();
        
        // Relación muchos-a-muchos con Categoria a través de Cat_Rec
        public ICollection<Cat_Rec> Cat_Recs { get; set; } = new List<Cat_Rec>();
        
        // Comentarios asociados a la receta
        public ICollection<Comentario> Comentarios { get; set; } = new List<Comentario>();
    }
}




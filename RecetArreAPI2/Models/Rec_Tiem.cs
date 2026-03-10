using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecetArreAPI2.Models
{
    // Entidad intermedia para la relación muchos-a-muchos entre Receta y Tiempo
    public class Rec_Tiem
    {
        // FK hacia Receta
        public int RecetaId { get; set; }
        public Receta? Receta { get; set; }

        // FK hacia Tiempo
        public int TiempoId { get; set; }
        public Tiempo? Tiempo { get; set; }
    }
}

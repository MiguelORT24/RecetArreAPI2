using RecetArreAPI2.Migrations;

namespace RecetArreAPI2.Models
{
    public class Cat_Rec
    {
        // FK hacia Receta
        public int RecetaId { get; set; }
        public Receta? Receta { get; set; }

        // FK hacia Categoria
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

    }
}

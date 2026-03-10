namespace RecetArreAPI2.Models
{
    public class Ing_Rec
    {
        // FK hacia Receta
        public int RecetaId { get; set; }
        public Receta? Receta { get; set; }

        // FK hacia Ingrediente
        public int IngredienteId { get; set; }
        public Ingrediente? Ingrediente { get; set; }
    }
}

using Swashbuckle.AspNetCore.Annotations;
namespace RestauSimplon
{
    public class Categorie
    {
        public int Id { get; set; }
        public int? IdArticles {  get; set; }
        public string Nom { get; set; } = null!;

    }
    public class CategorieDTO
    {
        [SwaggerSchema("Nom de la catégorie")]
        public string Nom { get; set; } = null!;
        [SwaggerSchema("Id de l'article")]
        public int? IdArticles { get; set; }

        public CategorieDTO(Categorie categorie)
        {
            this.Nom = categorie.Nom;
            this.IdArticles= categorie.IdArticles;
        }
    }
}

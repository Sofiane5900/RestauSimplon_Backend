using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public ICollection<Article> Articles { get; set; }
    }

    public class CategorieDTO
    {
        [SwaggerSchema("Nom de la catégorie")]
        public string Nom { get; set; }

        [SwaggerSchema("Liste des articles de la catégorie")]
        public ICollection<Article> Articles { get; set; }

        public CategorieDTO(Categorie categorie)
        {
            this.Nom = categorie.Nom;
            this.Articles = categorie.Articles;
        }
    }
}

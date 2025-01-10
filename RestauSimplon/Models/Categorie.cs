using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Models
{
    public class Categorie
    {
        public int Id { get; }
        public string Nom { get; set; }
        public ICollection<Article> Articles { get; set; }
    }

    public class CategorieDTO
    {
        [SwaggerSchema("Id de la catégorie")]
        public int Id { get; }

        [SwaggerSchema("Nom de la catégorie")]
        public string Nom { get; set; }

        public CategorieDTO() { }

        public CategorieDTO(Categorie categorie)
        {
            this.Id = categorie.Id;
            this.Nom = categorie.Nom;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon
{
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string Categorie { get; set; } = null!;
        public int Prix { get; set; }
    }
    public class ArticleItemDTO
    {
        [SwaggerSchema("ID de l'article")]
        public int Id { get; set; }

        [SwaggerSchema("Nom de l'article")]
        public string Nom { get; set; } = null!;

        [SwaggerSchema("Catégorie de l'article")]
        public string Categorie { get; set; } = null!;

        [SwaggerSchema("Prix de l'article")]
        public int Prix { get; set; }


        public ArticleItemDTO(Article articleItem)
        {
            this.Id = articleItem.Id;
            this.Nom = articleItem.Nom;
            this.Categorie = articleItem.Categorie;
            this.Prix = articleItem.Prix;
        }
    }
}

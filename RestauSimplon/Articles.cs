using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon
{
    public class Articles
    {
        public int Id { get; set; }
        public string Nom { get; set; } = null!;
        public string Categorie { get; set; } = null!;
        public int Prix { get; set; }
    }

    public class ArticlesItemDTO
    {
        [SwaggerSchema("ID de l'article")]
        public int Id { get; set; }

        [SwaggerSchema("Nom de l'article")]
        public string Nom { get; set; } = null!;

        [SwaggerSchema("Catégorie de l'article")]
        public string Categorie { get; set; } = null!;

        [SwaggerSchema("Prix de l'article")]
        public int Prix { get; set; }

        public ArticlesItemDTO(Articles articlesItem)
        {
            this.Id = articlesItem.Id;
            this.Nom = articlesItem.Nom;
            this.Categorie = articlesItem.Categorie;
            this.Prix = articlesItem.Prix;
        }
    }
}

/* Ceci est un commentaire (lol) */

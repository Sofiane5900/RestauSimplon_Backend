using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public decimal Prix { get; set; }
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        public ICollection<CommandeArticle> CommandeArticles { get; set; }
    }

    public class ArticleItemDTO
    {
        [SwaggerSchema("Id de l'article")]
        public int Id { get; set; }

        [SwaggerSchema("Nom de l'article")]
        public string Nom { get; set; }

        [SwaggerSchema("Prix de l'article")]
        public decimal Prix { get; set; }

        [SwaggerSchema("ID de la catégorie")]
        public int CategorieId { get; set; }

        public ArticleItemDTO() { }

        public ArticleItemDTO(Article article)
        {
            this.Id = article.Id;
            this.Nom = article.Nom;
            this.Prix = article.Prix;
            this.CategorieId = article.CategorieId;
        }
    }
}

/* Ceci est un commentaire (lol) */

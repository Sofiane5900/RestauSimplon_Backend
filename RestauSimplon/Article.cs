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
<<<<<<< HEAD:RestauSimplon/Articles.cs

    public class ArticlesItemDTO
=======
    public class ArticleItemDTO
>>>>>>> db7a5b16d947cb0def00e120bd06a0475a528ad1:RestauSimplon/Article.cs
    {
        [SwaggerSchema("ID de l'article")]
        public int Id { get; set; }

        [SwaggerSchema("Nom de l'article")]
        public string Nom { get; set; } = null!;

        [SwaggerSchema("Catégorie de l'article")]
        public string Categorie { get; set; } = null!;

        [SwaggerSchema("Prix de l'article")]
        public int Prix { get; set; }

<<<<<<< HEAD:RestauSimplon/Articles.cs
        public ArticlesItemDTO(Articles articlesItem)
=======

        public ArticleItemDTO(Article articleItem)
>>>>>>> db7a5b16d947cb0def00e120bd06a0475a528ad1:RestauSimplon/Article.cs
        {
            this.Id = articleItem.Id;
            this.Nom = articleItem.Nom;
            this.Categorie = articleItem.Categorie;
            this.Prix = articleItem.Prix;
        }
    }
}
<<<<<<< HEAD:RestauSimplon/Articles.cs

/* Ceci est un commentaire (lol) */
=======
>>>>>>> db7a5b16d947cb0def00e120bd06a0475a528ad1:RestauSimplon/Article.cs

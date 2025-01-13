using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Models
{
    public class Commande
    {
        public int Id { get; }
        public int ClientId { get; } // Ceci réprésente la clé étrangère de la table Client
        public Client Client { get; set; } // Ceci represente la relation avec la table Client
        public DateTime Date { get; set; }
        public decimal PrixTotal { get; set; }

        // Ceci represente la relation avec la table Article
        public ICollection<Article> Articles { get; set; }

        // Ceci represente la relation avec la table CommandeArticle
        public ICollection<CommandeArticle> CommandeArticles { get; set; }

        // Constructor to initialize the collections
        public Commande()
        {
            Articles = new List<Article>();
            CommandeArticles = new List<CommandeArticle>();
        }
    }

    public class CommandeItemDTO
    {
        public int CommandeId { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<ArticleItemDTO> Articles { get; set; }
        public decimal PrixTotal { get; set; }
        public int Quantite { get; set; }
    }

    public class CommandePostDTO
    {
        public int ClientId { get; set; }
        public required List<ArticleQuantiteDTO> Articles { get; set; }
    }

    public class ArticleQuantiteDTO
    {
        public int ArticleId { get; set; }
        public int Quantite { get; set; }
    }


}

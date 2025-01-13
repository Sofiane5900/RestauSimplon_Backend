using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Models
{
    public class Commande
    {
        public int Id { get; }

        // Ceci réprésente la clé étrangère de la table Client
        public int ClientId { get; }

        // Ceci represente la relation avec la table Client
        public Client Client { get; set; }
        public DateTime Date { get; set; }
        public decimal PrixTotal { get; set; }

        // Ceci represente la relation avec la table Article
        public ICollection<Article> Articles { get; set; }

        // Ceci represente la relation avec la table CommandeArticle
        public ICollection<CommandeArticle> CommandeArticles { get; set; }
    }

    public class CommandeItemDTO
    {
        public int CommandeId { get; set; }
        public string ClientName { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<ArticleItemDTO> Articles { get; set; }
        public decimal PrixTotal { get; set; }
    }
}

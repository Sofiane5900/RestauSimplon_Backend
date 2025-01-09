using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon
{
    public class Commande
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal PrixTotal { get; set; }

        // Ceci réprésente la clé étrangère de la table Client
        public int ClientId { get; set; }

        // Ceci represente la relation avec la table Client
        public Client Client { get; set; }

        // Ceci represente la relation avec la table Article
        public ICollection<Article> Articles { get; set; }

        // Ceci represente la relation avec la table CommandeArticle
        public ICollection<CommandeArticle> CommandeArticles { get; set; }
    }

    public class CommandeItemDTO
    {
        [SwaggerSchema("Client de la commande")]
        public ClientDTO Client { get; set; }

        [SwaggerSchema("Liste des articles de la commande")]
        public ICollection<Article> Articles { get; set; }

        [SwaggerSchema("Date de la commande")]
        public DateTime Date { get; set; }

        [SwaggerSchema("Prix total de la commande")]
        public decimal PrixTotal { get; set; }

        public CommandeItemDTO(CommandeItemDTO commandeItem)
        {
            this.Client = commandeItem.Client;
            this.Articles = commandeItem.Articles;
            this.Date = commandeItem.Date;
            this.PrixTotal = commandeItem.PrixTotal;
        }
    }
}

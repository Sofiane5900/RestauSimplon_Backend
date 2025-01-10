using Microsoft.EntityFrameworkCore;
using RestauSimplon;
using Swashbuckle.AspNetCore.Annotations;
namespace RestaurantAPI_Training
{
    public class Commande
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int PrixTotal { get; set; }

        // Ceci réprésente la clé étrangère de la table Client
        public int ClientId { get; set; }

        // Ceci réprésente la clé étrangère de la table Article
        public ICollection<int> ArticlesId { get; set; }

        // Ceci represente la relation avec la table Client
        public Client Client { get; set; }

        // Ceci represente la relation avec la table Article
        public ICollection<Article> Articles { get; set; }
    }

    public class CommandeItemDTO
    {
        [SwaggerSchema("Client de la commande")]
        public Client Client { get; set; }
        public int ClientId { get; set; }
        [SwaggerSchema("Liste des articles de la commande")]
        public ICollection<Article> Articles { get; set; }
        public ICollection<int> ArticlesId { get; set; }

        [SwaggerSchema("Date de la commande")]
        public DateTime Date { get; set; }

        [SwaggerSchema("Prix total de la commande")]
        public int PrixTotal { get; set; }

        // Constructeur prenant une instance de Commande
        public CommandeItemDTO(Commande commande)
        {
            this.Client = commande.Client;
            this.ClientId = commande.ClientId;
            this.Articles = commande.Articles;
            this.ArticlesId = commande.ArticlesId;
            this.Date = commande.Date;
            this.PrixTotal = commande.PrixTotal;
        }
    }

}

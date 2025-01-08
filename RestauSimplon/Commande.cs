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

        // Ceci represente la relation avec la table Client
        public Client Client { get; set; } = null!;

        // Ceci represente la relation avec la table Article
        public ICollection<Article> Articles { get; set; } = null!;
    }

    public class CommandeItemDTO
    {
        [SwaggerSchema("Client de la commande")]
        public Client Client { get; set; } = null!;

        [SwaggerSchema("Date de la commande")]
        public DateTime Date { get; set; }

        [SwaggerSchema("Prix total de la commande")]
        public int PrixTotal { get; set; }

        public CommandeItemDTO(CommandeItemDTO commandeItem)
        {
            this.Date = commandeItem.Date;
            this.PrixTotal = commandeItem.PrixTotal;
        }
    }
}

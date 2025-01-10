using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Models
{
    public class Commande
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal PrixTotal { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<CommandeArticle> CommandeArticles { get; set; } // Assure-toi que cette propriété est définie
    }


    public class CommandeItemDTO
    {

        [SwaggerSchema("Client de la commande")]
        public Client Client { get; set; }

        public ICollection<CommandeArticle> CommandeArticles { get; set; }
        [SwaggerSchema("Date de la commande")]
        public DateTime Date { get; set; }

        [SwaggerSchema("Prix total de la commande")]
        public decimal PrixTotal { get; set; }

        public CommandeItemDTO(Commande commandeItem)
        {
            Client = commandeItem.Client;
            CommandeArticles = commandeItem.CommandeArticles;
            Date = commandeItem.Date;
            PrixTotal = commandeItem.PrixTotal;
        }

    }
    public class CommandeSummaryDTO
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public ICollection<int> ArticleIds { get; set; }
    }
    public class CreateCommandeDTO
    {
        public int ClientId { get; set; }
        public ICollection<int> ArticleIds { get; set; }
        public DateTime Date { get; set; }
        public decimal PrixTotal { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace RestaurantAPI_Training
{
    public class Commande
    {
        public int Id { get; set; }
        public string Date { get; set; } = null!;
        public int PrixTotal { get; set; }
    }
    public class CommandeItemDTO
    {
        [SwaggerSchema("ID de la commande")]
        public int Id { get; set; }
        [SwaggerSchema("Date de la commande")]
        public string Date { get; set; } = null!;

        [SwaggerSchema("Prix total de la commande")]
        public int PrixTotal { get; set; }

        public CommandeItemDTO(CommandeItemDTO commandeItem)
        {
            this.Id = commandeItem.Id;
            this.Date = commandeItem.Date;
            this.PrixTotal = commandeItem.PrixTotal;
        }
    }
}

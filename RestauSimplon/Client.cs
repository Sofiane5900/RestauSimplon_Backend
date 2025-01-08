using System.ComponentModel.DataAnnotations;
using RestaurantAPI_Training;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon
{
    public class Client
    {
        public int Id { get; }
        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public string Adresse { get; set; } = null!;
        public int Phone { get; set; }

        public ICollection<Commande> Commandes { get; set; }
    }

    public class ClientDTO
    {
        [SwaggerSchema("Nom du Client")]
        public string Nom { get; set; } = null!;

        [SwaggerSchema("Prenom du Client")]
        public string Prenom { get; set; } = null!;

        [SwaggerSchema("L'adresse du Client ")]
        public string Adresse { get; set; } = null!;

        [SwaggerSchema("Telephone du Client ")]
        public int Phone { get; set; }

        [SwaggerSchema("Liste des commandes du client")]
        public ICollection<Commande> Commandes { get; set; }

        public ClientDTO(Client client)
        {
            this.Nom = client.Nom;
            this.Prenom = client.Prenom;
            this.Adresse = client.Adresse;
            this.Phone = client.Phone;
            this.Commandes = client.Commandes;
        }
    }
}

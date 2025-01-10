using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Models
{
    // TODO : Virer l'objet commande (commandes : null dans l'API)
    public class Client
    {
        public int Id { get; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse { get; set; }
        public string Phone { get; set; }
        public ICollection<Commande> Commandes { get; set; }
    }

    public class ClientDTO
    {
        public int Id { get; }
        [SwaggerSchema("Nom du Client")]
        public string Nom { get; set; }

        [SwaggerSchema("Prenom du Client")]
        public string Prenom { get; set; }

        [SwaggerSchema("L'adresse du Client ")]
        public string Adresse { get; set; }

        [SwaggerSchema("Telephone du Client ")]
        public string Phone { get; set; }


        public ClientDTO() { }

        public ClientDTO(Client client)
        {
            Id = client.Id;
            Nom = client.Nom;
            Prenom = client.Prenom;
            Adresse = client.Adresse;
            Phone = client.Phone;
        }
    }
}

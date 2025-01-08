using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
namespace RestauSimplon
{
    public class Clients
    {
        public int Id { get; }

        public string Nom { get; set; } = null!;
        public string Prenom { get; set; } = null!;
        public string Adresse { get; set; } = null!;
        public int Phone { get; set; }

    }
    public class ClientsDTO
    {


        [SwaggerSchema("Nom du Client")]
        public string Nom { get; set; } = null!;
        [SwaggerSchema("Prenom du Client")]
        public string Prenom { get; set;} = null!;
        [SwaggerSchema("L'adresse du Client ")]
        public string Adresse { get; set;} = null!;
        [SwaggerSchema("Telephone du Client ")]
        public int Phone { get; set; }

        public ClientsDTO(Clients clients)
        {
            this.Nom = clients.Nom;
            this.Prenom = clients.Prenom;
            this.Adresse = clients.Adresse;
            this.Phone = clients.Phone;
        }


    }
}

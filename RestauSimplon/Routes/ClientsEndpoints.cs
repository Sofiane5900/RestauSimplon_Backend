using Microsoft.EntityFrameworkCore;
using RestauSimplon.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Routes
{
    public static class ClientsEndpoints
    {
        public static RouteGroupBuilder ClientsRoutes(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllClients);
            group.MapGet("/{id}", GetClientById);
            group.MapPost("/", CreateClient);
            group.MapPut("/{id}", UpdateClient);
            group.MapDelete("/{id}", DeleteClient);
            return group;
        }

        // GET - Recupére tout les clients
        [SwaggerOperation(
            Summary = "Récupère la liste des clients",
            Description = "Récupère la liste de tout les clients du restaurant",
            OperationId = "GetAllClients",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<ClientDTO>))]
        static async Task<IResult> GetAllClients(RestauDbContext db)
        {
            if (await db.Client.CountAsync() == 0)
            {
                return Results.NotFound();
            }
            return TypedResults.Ok(await db.Client.ToArrayAsync());
        }

        // GET - Rechercher un client spécifique (par son id)
        [SwaggerOperation(
            Summary = "Récupere un client par son Id",
            Description = "Récupère un client spécifique du Restaurant en spécifiant son Id ",
            OperationId = "GetClientById",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ClientDTO>))]
        static async Task<IResult> GetClientById(int Id, RestauDbContext db)
        {
            var client = await db.Client.FindAsync(Id);
            if (client == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(new ClientDTO(client));
        }

        // POST - Ajouter un client
        [SwaggerOperation(
            Summary = "Ajout d'un client",
            Description = "Ajouter un nouveau client pour le restaurant",
            OperationId = "CreateClient",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ClientDTO>))]
        static async Task<IResult> CreateClient(ClientDTO clientDTO, RestauDbContext db)
        {
            var client = new Client
            {
                Nom = clientDTO.Nom,
                Prenom = clientDTO.Prenom,
                Adresse = clientDTO.Adresse,
                Phone = clientDTO.Phone,
            };

            if (
                string.IsNullOrEmpty(clientDTO.Nom)
                || string.IsNullOrEmpty(clientDTO.Prenom)
                || string.IsNullOrEmpty(clientDTO.Adresse)
                || string.IsNullOrEmpty(clientDTO.Phone)
            )
            {
                return Results.BadRequest(
                    "Erreur, veuillez renseignez tout les champs correctement."
                );
            }
            db.Add(client);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/articles/{client.Id}", new ClientDTO(client));
        }

        // PUT - Mettre à jour un client
        [SwaggerOperation(
            Summary = "Mettre a jour un client",
            Description = "Mettre a jour un client pour le restaurant",
            OperationId = "UpdateClient",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ClientDTO>))]
        static async Task<IResult> UpdateClient(int Id, ClientDTO clientDTO, RestauDbContext db)
        {
            var clientToUpdate = db.Client.Find(Id);
            if (clientToUpdate == null)
            {
                return Results.NotFound();
            }

            clientToUpdate.Nom = clientDTO.Nom;
            clientToUpdate.Prenom = clientDTO.Prenom;
            clientToUpdate.Adresse = clientDTO.Adresse;
            clientToUpdate.Phone = clientDTO.Phone;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        // DELETE - Supprimer un client
        [SwaggerOperation(
            Summary = "Suppression d'un client",
            Description = "Suppression d'un client pour le restaurantw",
            OperationId = "CreateClient",
            Tags = new[] { "Clients" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ClientDTO>))]
        static async Task<IResult> DeleteClient(int Id, RestauDbContext db)
        {
            var client = await db.Client.FindAsync(Id);
            if (client == null)
            {
                return Results.NotFound();
            }
            db.Client.Remove(client);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    }
}

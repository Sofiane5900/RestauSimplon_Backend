// RouteCommandes.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using RestauSimplon;
using Swashbuckle.AspNetCore.Annotations;
using RestaurantAPI_Training;

public static class RouteCommandes
{
    public static void MapRouteCommandes(this IEndpointRouteBuilder routes)
    {
        RouteGroupBuilder commande = routes.MapGroup("/commandes");
        commande.MapGet("/", GetAllCommandes);

        // GET - Recupére tout les commandes
        [SwaggerOperation(
            Summary = "Récupère la liste des commandes",
            Description = "Récupère la liste de tout les commandes du restaurant",
            OperationId = "GetCommandes",
            Tags = new[] { "Commandes" }
        )]
        static async Task<IResult> GetAllCommandes(RestauDbContext db)
        {
            return TypedResults.Ok(await db.Commande.ToListAsync());
        }

        [SwaggerOperation(
            Summary = "Récupère une commande par ID",
            Description = "Récupère une commande spécifique en utilisant son ID",
            OperationId = "GetIdCommande",
            Tags = new[] { "Commandes" }
        )]
        static async Task<IResult> GetIdCommande(RestauDbContext db, int Id)
        {
            return TypedResults.Ok(await db.Commande.FindAsync(Id));
        }

        [SwaggerOperation(
            Summary = "Supprime une commande par ID",
            Description = "Supprime une commande spécifique en utilisant son ID",
            OperationId = "DeleteIdCommande",
            Tags = new[] { "Commandes" }
        )]
        static async Task<IResult> DeleteIdCommande(RestauDbContext db, int Id)
        {
            if (await db.Commande.FindAsync(Id) is Commande commande)
            {
                db.Commande.Remove(commande);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }

        [SwaggerOperation(
            Summary = "Mise à jour d'une commande",
            Description = "Met à jour une commande spécifique en utilisant son ID et les nouvelles données",
            OperationId = "UpdateCommande",
            Tags = new[] { "Commandes" }
        )]
        static async Task<IResult> UpdateCommande(int id, CommandeItemDTO commandeItemDTO, RestauDbContext db)
        {
            var commande = await db.Commande.FindAsync(id);

            if (commande is null) return TypedResults.NotFound();

            commande.ClientId = commandeItemDTO.ClientId;
            commande.Articles = commandeItemDTO.Articles;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        [SwaggerOperation(
            Summary = "Crée une nouvelle commande",
            Description = "Crée une nouvelle commande en utilisant les données fournies",
            OperationId = "CreateCommande",
            Tags = new[] { "Commandes" }
        )]
        static async Task<IResult> CreateCommande(CommandeItemDTO commandeItemDTO, RestauDbContext db)
        {
            var commandeItem = new Commande
            {
                ClientId = commandeItemDTO.ClientId,
                Articles = commandeItemDTO.Articles,
                Client = commandeItemDTO.Client,
                Date = commandeItemDTO.Date,
                PrixTotal = commandeItemDTO.PrixTotal,
            };

            db.Commande.Add(commandeItem);
            await db.SaveChangesAsync();

            // Utiliser le nouveau constructeur ici
            commandeItemDTO = new CommandeItemDTO(commandeItem);

            return TypedResults.Created($"/commandes/{commandeItem.Id}", commandeItemDTO);
        }
    }
}

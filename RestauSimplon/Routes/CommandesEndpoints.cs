using Microsoft.EntityFrameworkCore;
using RestauSimplon; // Add this using directive
using RestauSimplon.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Routes
{
    public static class CommandesEndpoints
    {
        public static RouteGroupBuilder CommandesRoutes(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllCommandes);
            group.MapGet("/{id}", GetCommandeById);
            group.MapPost("/", CreateCommande);
            group.MapPut("/{id}", UpdateCommande);
            group.MapDelete("/{id}", DeleteCommande);
            group.MapGet("/history", GetCommandesHistory);
            return group;
        }

        // GET - Recupére toutes les commandes
        [SwaggerOperation(
            Summary = "Récupère la liste des commandes",
            Description = "Récupère la liste de toutes les commandes du restaurant",
            OperationId = "GetAllCommandes",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<Commande>))]
        static async Task<IResult> GetAllCommandes(RestauDbContext db)
        {
            if (await db.Commande.CountAsync() == 0)
            {
                return Results.NotFound();
            }
            var commandes = await db
                .Commande.Include(c => c.Client)
                .Include(c => c.CommandeArticles)
                .ThenInclude(ca => ca.Article)
                .ToArrayAsync();
            return TypedResults.Ok(commandes);
        }

        // GET - Rechercher une commande spécifique (par son id)
        [SwaggerOperation(
            Summary = "Récupere une commande par son Id",
            Description = "Récupère une commande spécifique du Restaurant en spécifiant son Id ",
            OperationId = "GetCommandeById",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK!", typeof(Commande))]
        static async Task<IResult> GetCommandeById(int Id, RestauDbContext db)
        {
            var commande = await db
                .Commande.Include(c => c.Client)
                .Include(c => c.CommandeArticles)
                .ThenInclude(ca => ca.Article)
                .FirstOrDefaultAsync(c => c.Id == Id);
            if (commande == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(commande);
        }

        // POST - Ajouter une commande
        [SwaggerOperation(
            Summary = "Ajout d'une commande",
            Description = "Ajouter une nouvelle commande pour le restaurant",
            OperationId = "CreateCommande",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK!", typeof(Commande))]
        static async Task<IResult> CreateCommande(CommandeDTO commandeDTO, RestauDbContext db)
        {
            if (commandeDTO.ClientId == 0)
            {
                return Results.BadRequest("A valid client is required for the order.");
            }

            if (commandeDTO.ArticleIds == null || !commandeDTO.ArticleIds.Any())
            {
                return Results.BadRequest("At least one article is required for the order.");
            }

            var client = await db.Client.FindAsync(commandeDTO.ClientId);
            if (client == null)
            {
                return Results.BadRequest("Client not found");
            }

            var articles = await db
                .Article.Where(a => commandeDTO.ArticleIds.Contains(a.Id))
                .ToListAsync();
            if (articles.Count != commandeDTO.ArticleIds.Count)
            {
                return Results.BadRequest("One or more articles not found");
            }

            var commande = new Commande
            {
                Client = client,
                Date = DateTime.Now,
                PrixTotal = articles.Sum(a => a.Prix),
                CommandeArticles = articles
                    .Select(a => new CommandeArticle { Article = a })
                    .ToList(),
            };

            try
            {
                db.Add(commande);
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Results.Problem("An error occurred while creating the order: " + ex.Message);
            }

            return Results.Created($"/commandes/{commande.Id}", commande);
        }

        // PUT - Mettre à jour une commande
        [SwaggerOperation(
            Summary = "Mettre a jour une commande",
            Description = "Mettre a jour une commande pour le restaurant",
            OperationId = "UpdateCommande",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK!", typeof(Commande))]
        static async Task<IResult> UpdateCommande(int Id, Commande commande, RestauDbContext db)
        {
            var commandeToUpdate = await db
                .Commande.Include(c => c.Client)
                .Include(c => c.CommandeArticles)
                .ThenInclude(ca => ca.Article)
                .FirstOrDefaultAsync(c => c.Id == Id);
            if (commandeToUpdate == null)
            {
                return Results.NotFound();
            }

            if (commande.Client == null || commande.Client.Id == 0)
            {
                return Results.BadRequest("A valid client is required for the order.");
            }

            if (commande.CommandeArticles == null || !commande.CommandeArticles.Any())
            {
                return Results.BadRequest("At least one article is required for the order.");
            }

            var client = await db.Client.FindAsync(commande.Client.Id);
            if (client == null)
            {
                return Results.BadRequest("Client not found");
            }

            commandeToUpdate.Date = commande.Date;
            commandeToUpdate.PrixTotal = commande.PrixTotal;
            // Update only necessary properties
            commandeToUpdate.Client = client; // Ensure only the client is updated

            try
            {
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Results.Problem("An error occurred while updating the order: " + ex.Message);
            }

            return TypedResults.NoContent();
        }

        // DELETE - Supprimer une commande
        [SwaggerOperation(
            Summary = "Suppression d'une commande",
            Description = "Suppression d'une commande pour le restaurant",
            OperationId = "DeleteCommande",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK!", typeof(Commande))]
        static async Task<IResult> DeleteCommande(int Id, RestauDbContext db)
        {
            var commande = await db.Commande.FindAsync(Id);
            if (commande == null)
            {
                return Results.NotFound();
            }

            db.Commande.Remove(commande);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        // GET - Récupérer l'historique des commandes
        [SwaggerOperation(
            Summary = "Récupère l'historique des commandes",
            Description = "Récupère l'historique de toutes les commandes du restaurant",
            OperationId = "GetCommandesHistory",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<CommandeItemDTO>))]
        static async Task<IResult> GetCommandesHistory(
            RestauDbContext db,
            int page = 1,
            int pageSize = 10
        )
        {
            var commandes = await db
                .Commande.Include(c => c.Client)
                .Include(c => c.CommandeArticles)
                .ThenInclude(ca => ca.Article)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CommandeItemDTO
                {
                    CommandeId = c.Id,
                    ClientName = c.Client.Nom,
                    Date = c.Date,
                    Articles = c.CommandeArticles.Select(ca => new ArticleItemDTO
                    {
                        Nom = ca.Article.Nom,
                        Prix = ca.Article.Prix,
                    }),
                    PrixTotal = c.PrixTotal,
                })
                .ToArrayAsync();

            return TypedResults.Ok(commandes);
        }
    }

    public class CommandeDTO
    {
        public int ClientId { get; set; }
        public List<int> ArticleIds { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc; // Add this using directive
using Microsoft.EntityFrameworkCore;
using RestauSimplon;
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
            //group.MapGet("/history", GetCommandesHistory);
            return group;
        }

        // GET - Recupére toutes les commandes
        [SwaggerOperation(
            Summary = "Récupère la liste des commandes",
            Description = "Récupère la liste de toutes les commandes du restaurant",
            OperationId = "GetAllCommandes",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK", typeof(IEnumerable<CommandeItemDTO>))]
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
                .Select(c => new CommandeItemDTO
                {
                    CommandeId = c.Id,
                    ClientName = c.Client.Nom,
                    Date = c.Date,
                    Articles = c
                        .CommandeArticles.Select(ca => new ArticleItemDTO
                        {
                            Id = ca.Article.Id, // Ensure Id is set
                            Nom = ca.Article.Nom,
                            CategorieId = ca.Article.CategorieId, // Ensure CategorieId is set
                            Prix = ca.Article.Prix,
                        })
                        .ToList(), // Convert to List
                    PrixTotal = c.PrixTotal,
                })
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
        [SwaggerResponse(200, "OK!", typeof(CommandeItemDTO))]
        static async Task<IResult> GetCommandeById(int Id, RestauDbContext db)
        {
            var commande = await db
                .Commande.Include(c => c.Client)
                .Include(c => c.CommandeArticles)
                .ThenInclude(ca => ca.Article)
                .Select(c => new CommandeItemDTO
                {
                    CommandeId = c.Id,
                    ClientName = c.Client.Nom,
                    Date = c.Date,
                    Articles = c
                        .CommandeArticles.Select(ca => new ArticleItemDTO
                        {
                            Id = ca.Article.Id, // Ensure Id is set
                            Nom = ca.Article.Nom,
                            CategorieId = ca.Article.CategorieId, // Ensure CategorieId is set
                            Prix = ca.Article.Prix,
                        })
                        .ToList(), // Convert to List
                    PrixTotal = c.PrixTotal,
                })
                .FirstOrDefaultAsync(c => c.CommandeId == Id);
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
        [SwaggerResponse(200, "OK!", typeof(CommandeItemDTO))]
        static async Task<IResult> CreateCommande(
            CommandePostDTO commandePostDTO,
            RestauDbContext db
        )
        {
            var client = await db.Client.FindAsync(commandePostDTO.ClientId);
            if (client == null)
            {
                return Results.BadRequest("Client not found");
            }

            var articles = await db
                .Article.Where(a => commandePostDTO.ArticleIds.Contains(a.Id))
                .ToListAsync();
            if (articles.Count != commandePostDTO.ArticleIds.Count)
            {
                return Results.BadRequest("One or more articles not found");
            }

            var commande = new Commande
            {
                Client = client,
                Date = DateTime.Now,
                PrixTotal = articles.Sum(a => a.Prix),
                CommandeArticles = articles
                    .Select(a => new CommandeArticle { Article = a, ArticleId = a.Id })
                    .ToList(),
            };

            db.Add(commande);
            await db.SaveChangesAsync();
            return Results.Created(
                $"/commandes/{commande.Id}",
                new CommandeItemDTO
                {
                    CommandeId = commande.Id,
                    ClientName = client.Nom,
                    Date = commande.Date,
                    Articles = commande
                        .CommandeArticles.Select(ca => new ArticleItemDTO
                        {
                            Id = ca.Article.Id,
                            Nom = ca.Article.Nom,
                            CategorieId = ca.Article.CategorieId,
                            Prix = ca.Article.Prix,
                        })
                        .ToList(),
                    PrixTotal = commande.PrixTotal,
                }
            );
        }

        // PUT - Mettre à jour une commande
        [SwaggerOperation(
            Summary = "Mettre a jour les articles d'une commande",
            Description = "Mettre a jour les articles d'une commande pour le restaurant",
            OperationId = "UpdateCommande",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK!", typeof(CommandeItemDTO))]
        static async Task<IResult> UpdateCommande(
            int Id,
            [FromBody] List<int> articleIds,
            RestauDbContext db
        )
        {
            var commandeToUpdate = await db
                .Commande.Include(c => c.CommandeArticles)
                .ThenInclude(ca => ca.Article)
                .FirstOrDefaultAsync(c => c.Id == Id);
            if (commandeToUpdate == null)
            {
                return Results.NotFound();
            }

            var articles = await db.Article.Where(a => articleIds.Contains(a.Id)).ToListAsync();
            if (articles.Count != articleIds.Count)
            {
                return Results.BadRequest("One or more articles not found");
            }

            // Update the articles in the commande
            commandeToUpdate.CommandeArticles = articles
                .Select(a => new CommandeArticle { Article = a, ArticleId = a.Id })
                .ToList();
            commandeToUpdate.PrixTotal = articles.Sum(a => a.Prix); // Update the total price

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        // DELETE - Supprimer une commande
        [SwaggerOperation(
            Summary = "Suppression d'une commande",
            Description = "Suppression d'une commande pour le restaurant",
            OperationId = "DeleteCommande",
            Tags = new[] { "Commandes" }
        )]
        [SwaggerResponse(200, "OK!", typeof(CommandeItemDTO))]
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
        //[SwaggerOperation(
        //    Summary = "Récupère l'historique des commandes",
        //    Description = "Récupère l'historique de toutes les commandes du restaurant",
        //    OperationId = "GetCommandesHistory",
        //    Tags = new[] { "Commandes" }
        //)]
        //[SwaggerResponse(200, "OK", typeof(IEnumerable<CommandeItemDTO>))]
    }
}

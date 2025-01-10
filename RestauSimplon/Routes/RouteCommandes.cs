// RouteCommandes.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestauSimplon;
using RestauSimplon.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Mvc;
public static class RouteCommandes
{

    public static RouteGroupBuilder CommandesRoutes(this RouteGroupBuilder group)
    {
        group.MapGet("/", GetAllCommandes);
        group.MapGet("/{id}", GetCommandeById);
        group.MapPost("/", CreateCommande);
        group.MapPut("/{id}", UpdateCommande);
        group.MapDelete("/{id}", DeleteCommande);
        return group;
    }

    // GET - Recupére toutes les commandes
    [SwaggerOperation(
        Summary = "Récupère la liste des commandes",
        Description = "Récupère la liste de toutes les commandes du restaurant",
        OperationId = "GetAllCommandes",
        Tags = new[] { "Commandes" }
    )]
    [SwaggerResponse(200, "OK", typeof(IEnumerable<CommandeSummaryDTO>))]
    static async Task<IResult> GetAllCommandes(RestauDbContext db)
    {
        if (await db.Commande.CountAsync() == 0)
        {
            return Results.NotFound();
        }

        var commandes = await db.Commande
            .Include(commande => commande.CommandeArticles)
            .ThenInclude(commandearticle=> commandearticle.Article)
            .Select(commande => new CommandeSummaryDTO
            {
                Id = commande.Id,
                ClientId = commande.ClientId,
                ArticleIds = commande.CommandeArticles.Select(commandearticle => commandearticle.ArticleId).ToList()
            })
            .ToListAsync();

        return TypedResults.Ok(commandes);
    }


    // GET - Rechercher une commande spécifique (par son id)
    // GET - Rechercher une commande spécifique (par son id)
    [SwaggerOperation(
        Summary = "Récupere une commande par son Id",
        Description = "Récupère une commande spécifique du Restaurant en spécifiant son Id ",
        OperationId = "GetCommandeById",
        Tags = new[] { "Commandes" }
    )]
    [SwaggerResponse(200, "OK!", typeof(IEnumerable<CommandeItemDTO>))]
    [HttpGet("{id}", Name = "GetCommandeById")]
    static async Task<IResult> GetCommandeById(int Id, RestauDbContext db)
    {
        var commande = await db.Commande.FindAsync(Id);
        if (commande == null)
        {
            return Results.NotFound();
        }
        return Results.Ok(new CommandeItemDTO(commande));
    }

    // POST - Ajouter une commande
    // POST - Ajouter une commande
    [SwaggerOperation(
        Summary = "Ajout d'une commande",
        Description = "Ajouter une nouvelle commande pour le restaurant",
        OperationId = "CreateCommande",
        Tags = new[] { "Commandes" }
    )]
    [SwaggerResponse(200, "OK!", typeof(IEnumerable<CreateCommandeDTO>))]
    public static async Task<IResult> CreateCommande(CreateCommandeDTO createCommandeDTO, RestauDbContext db)
    {
        var commande = new Commande
        {
            ClientId = createCommandeDTO.ClientId,
            Date = createCommandeDTO.Date,
            PrixTotal = createCommandeDTO.PrixTotal,
            CommandeArticles = createCommandeDTO.ArticleIds.Select(articleId => new CommandeArticle { ArticleId = articleId }).ToList()
        };

        db.Commande.Add(commande);
        await db.SaveChangesAsync();

        return Results.CreatedAtRoute("GetCommandeById", new { id = commande.Id }, commande);
    }

    // PUT - Mettre à jour une commande
    [SwaggerOperation(
        Summary = "Mettre a jour une commande",
        Description = "Mettre a jour une commande pour le restaurant",
        OperationId = "UpdateCommande",
        Tags = new[] { "Commandes" }
    )]
    [SwaggerResponse(200, "OK!", typeof(IEnumerable<CommandeItemDTO>))]
    static async Task<IResult> UpdateCommande(int Id, CommandeItemDTO commandeDTO, RestauDbContext db)
    {
        var commandeToUpdate = db.Commande.Find(Id);
        if (commandeToUpdate == null)
        {
            return Results.NotFound();
        }

        commandeToUpdate.Date = commandeDTO.Date;
        commandeToUpdate.PrixTotal = commandeDTO.PrixTotal;
        commandeToUpdate.ClientId = commandeDTO.Client.Id;
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
    [SwaggerResponse(200, "OK!", typeof(IEnumerable<CommandeItemDTO>))]
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
}


using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using RestauSimplon.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Routes
{
    // TODO : Gerer les éxceptions (champs vide etc...)
    public static class ArticlesEndpoints
    {
        public static RouteGroupBuilder ArticlesRoutes(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllArticles);
            group.MapGet("/{id}", GetArticlesById);
            group.MapPost("/", CreateArticle);
            group.MapPut("/{id}", UpdateArticle);
            group.MapDelete("/{id}", DeleteArticle);
            return group;
        }

        // GET - Recupére tout les articles
        [SwaggerOperation(
            Summary = "Récupère la liste des articles",
            Description = "Récupère la liste de tout les articles du restaurant",
            OperationId = "GetArticles",
            Tags = new[] { "Articles" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ArticleItemDTO>))]
        static async Task<IResult> GetAllArticles(RestauDbContext db)
        {
            if (await db.Article.CountAsync() == 0)
            {
                return Results.NotFound();
            }
            return TypedResults.Ok(
                await db.Article.Select(x => new ArticleItemDTO(x)).ToArrayAsync()
            );
        }

        // GET - Rechercher un article spécifique (par son id)
        [SwaggerOperation(
            Summary = "Récupere un article par son Id",
            Description = "Récupère un article spécifique du Restaurant en spécifiant son Id ",
            OperationId = "GetArticlesById",
            Tags = new[] { "Articles" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ArticleItemDTO>))]
        static async Task<IResult> GetArticlesById(int id, RestauDbContext db)
        {
            var article = await db.Article.FindAsync(id);
            if (article == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(new ArticleItemDTO(article));
        }

        // POST - Ajouter un article
        [SwaggerOperation(
            Summary = "Ajout d'un article",
            Description = "Ajouter un nouveau article pour le restaurant",
            OperationId = "CreateArticle",
            Tags = new[] { "Articles" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ArticleItemDTO>))]
        static async Task<IResult> CreateArticle(ArticlePostDTO articleDTO, RestauDbContext db)
        {
            var article = new Article
            {
                Nom = articleDTO.Nom,
                Prix = articleDTO.Prix,
                CategorieId = articleDTO.CategorieId,
            };

            // Si les champ nom/categorieId sont vide ou le prix est inférieur a 0, j'envoie une bad request (400)
            if (string.IsNullOrEmpty(article.Nom) || article.Prix < 0 || article.CategorieId == 0)
            {
                return Results.BadRequest(
                    "Erreur, veuillez renseignez tout les champs correctement."
                );
            }

            db.Article.Add(article);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/articles/{article.Id}", new ArticleItemDTO(article));
        }

        // PUT - Mettre a jour un article
        [SwaggerOperation(
            Summary = "Mettre a jour un article",
            Description = "Mettre a jour un article pour le restaurant",
            OperationId = "UpdateArticle",
            Tags = new[] { "Articles" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ArticlePostDTO>))]
        static async Task<IResult> UpdateArticle(
            int Id,
            ArticlePostDTO articlePostDTO,
            RestauDbContext db
        )
        {
            var articleToUpdate = await db.Article.FindAsync(Id);

            if (articleToUpdate is null)
            {
                return Results.NotFound();
            }

            articleToUpdate.Nom = articlePostDTO.Nom;
            articleToUpdate.CategorieId = articlePostDTO.CategorieId;
            articleToUpdate.Prix = articlePostDTO.Prix;
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        [SwaggerOperation(
            Summary = "Supprimer un article",
            Description = "Supprimer un article un article pour le restaurant",
            OperationId = "DeleteArticle",
            Tags = new[] { "Articles" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<ArticleItemDTO>))]
        static async Task<IResult> DeleteArticle(int Id, RestauDbContext db)
        {
            var article = await db.Article.FindAsync(Id);
            if (article == null)
            {
                return Results.NotFound();
            }

            db.Article.Remove(article);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
    }
}

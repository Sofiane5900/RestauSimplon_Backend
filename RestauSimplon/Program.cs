using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestauSimplon;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<RestauDbContext>(options =>
{
    options.UseSqlite("Data Source=RestauSimplonDb.db");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "RestauSimplon API",
            Version = "v1",
            Description = "Une API pour gérer le restaurant de Simplon",
            Contact = new OpenApiContact
            {
                Name = "Simplon",
                Email = "simplon@example.com",
                Url = new Uri("https://simplon.co"),
            },
        }
    );
    c.EnableAnnotations();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestauSimplon v1");
        c.RoutePrefix = "";
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

/* CATEGORIES */

RouteGroupBuilder categories = app.MapGroup("/categories");
categories.MapGet("/", GetAllCategories);
categories.MapPost("/", CreateCategorie);
categories.MapPut("/", UpdateCategorie);
categories.MapDelete("/", DeleteCategorie);

// GET - Recupére tout les categories
[SwaggerOperation(
    Summary = "Récupère la liste des catégories",
    Description = "Récupère la liste de toutes les catégories du restaurant",
    OperationId = "GetCategories",
    Tags = new[] { "Categories" }
)]
[SwaggerResponse(200, "Liste des catégories trouvée", typeof(IEnumerable<CategorieDTO>))]
static async Task<IResult> GetAllCategories(RestauDbContext db)
{
    return TypedResults.Ok(await db.Categorie.ToArrayAsync());
}

// POST - Ajouter une catégorie
[SwaggerOperation(
    Summary = "Ajout d'une catégorie",
    Description = "Ajoute une catégorie pour les articles du restaurant",
    OperationId = "CreateCategorie",
    Tags = new[] { "Categories" }
)]
static Task<IResult> CreateCategorie(CategorieDTO categorieDTO, RestauDbContext db)
{
    var categorie = new Categorie { Nom = categorieDTO.Nom };
    db.Categorie.Add(categorie);
    db.SaveChanges();
    return Task.FromResult<IResult>(
        Results.Created($"/categories/{categorie.Id}", new CategorieDTO(categorie))
    );
}

// PUT - Mettre à jour une catégorie
[SwaggerOperation(
    Summary = "Mettre a jour une catégorie",
    Description = "Mettre a jour une catégorie pour les articles du restaurant",
    OperationId = "UpdateCategorie",
    Tags = new[] { "Categories" }
)]
[SwaggerResponse(200, "Catégorie modifiée avec succés!", typeof(IEnumerable<CategorieDTO>))]
static Task<IResult> UpdateCategorie(int Id, CategorieDTO categorieDTO, RestauDbContext db)
{
    var categorieToUpdate = db.Categorie.Find(Id);
    if (categorieToUpdate == null)
    {
        return Task.FromResult<IResult>(Results.NotFound());
    }
    categorieToUpdate.Nom = categorieDTO.Nom;
    db.SaveChanges();
    return Task.FromResult<IResult>(Results.Ok(new CategorieDTO(categorieToUpdate)));
}

[SwaggerOperation(
    Summary = "Supprimer une catégorie",
    Description = "Supprimer une catégorie pour les articles du restaurant",
    OperationId = "DeleteCategorie",
    Tags = new[] { "Categories" }
)]
[SwaggerResponse(200, "Catégorie supprimée avec succés!", typeof(IEnumerable<CategorieDTO>))]
static async Task<IResult> DeleteCategorie(int Id, RestauDbContext db)
{
    var categorie = await db.Categorie.FindAsync(Id);
    if (categorie is null)
    {
        return TypedResults.NoContent();
    }
    db.Categorie.Remove(categorie);
    await db.SaveChangesAsync();
    return TypedResults.NotFound();
}

RouteGroupBuilder articles = app.MapGroup("/articles");
articles.MapGet("/", GetAllArticles);
articles.MapGet("/{id}", GetArticlesById);
articles.MapPost("/", CreateArticle);
articles.MapPut("/", UpdateArticle);
articles.MapDelete("/", DeleteArticle);

// GET - Recupére tout les articles
[SwaggerOperation(
    Summary = "Récupère la liste des articles",
    Description = "Récupère la liste de tout les articles du restaurant",
    OperationId = "GetArticles",
    Tags = new[] { "Articles" }
)]
[SwaggerResponse(200, "Succès! Liste des articles recupérée", typeof(IEnumerable<ArticleItemDTO>))]
static async Task<IResult> GetAllArticles(RestauDbContext db)
{
    if (await db.Article.CountAsync() == 0)
    {
        return Results.NotFound();
    }
    return TypedResults.Ok(await db.Article.Select(x => new ArticleItemDTO(x)).ToArrayAsync());
}

// GET - Rechercher un article spécifique (par son id)
[SwaggerOperation(
    Summary = "Récupere un article par son Id",
    Description = "Récupère un article spécifique du Restaurant en spécifiant son Id ",
    OperationId = "GetArticlesById",
    Tags = new[] { "Articles" }
)]
[SwaggerResponse(
    200,
    "L'Article a était trouvée avec succés !",
    typeof(IEnumerable<ArticleItemDTO>)
)]
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
[SwaggerResponse(200, "L'Article a était crée avec succés !", typeof(IEnumerable<ArticleItemDTO>))]
static Task<IResult> CreateArticle(ArticleItemDTO articleDTO, RestauDbContext db)
{
    var article = new Article
    {
        Id = articleDTO.Id,
        Nom = articleDTO.Nom,
        Prix = articleDTO.Prix,
        CategorieId = articleDTO.CategorieId,
    };

    db.Article.Add(article);
    db.SaveChanges();
    return Task.FromResult<IResult>(
        Results.Created($"/articles/{article.Id}", new ArticleItemDTO(article))
    );
}

// PUT - Mettre a jour un article
[SwaggerOperation(
    Summary = "Mettre a jour un article",
    Description = "Mettre a jour un article pour le restaurant",
    OperationId = "UpdateArticle",
    Tags = new[] { "Articles" }
)]
[SwaggerResponse(
    200,
    "L'Article a était mis a jour avec succés !",
    typeof(IEnumerable<ArticleItemDTO>)
)]
static async Task<IResult> UpdateArticle(int Id, ArticleItemDTO articleDTO, RestauDbContext db)
{
    var articleToUpdate = await db.Article.FindAsync(Id);

    if (articleToUpdate is null)
    {
        return Results.NotFound();
    }

    articleToUpdate.Nom = articleDTO.Nom;
    articleToUpdate.CategorieId = articleDTO.CategorieId;
    articleToUpdate.Prix = articleDTO.Prix;
    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

[SwaggerOperation(
    Summary = "Supprimer un article",
    Description = "Supprimer un article un article pour le restaurant",
    OperationId = "DeleteArticle",
    Tags = new[] { "Articles" }
)]
[SwaggerResponse(
    200,
    "L'Article a était supprimer avec succés !",
    typeof(IEnumerable<ArticleItemDTO>)
)]
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
app.UseHttpsRedirection();

app.Run();

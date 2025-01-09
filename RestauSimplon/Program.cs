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
            Description = "Une API pour g�rer le restaurant de Simplon",
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

RouteGroupBuilder articles = app.MapGroup("/articles");
articles.MapGet("/", GetAllArticles);
articles.MapGet("/{id}", GetArticlesById);
articles.MapPost("/", PostArticles);

// GET - Recup�re tout les articles
[SwaggerOperation(
    Summary = "R�cup�re la liste des articles",
    Description = "R�cup�re la liste de tout les articles du restaurant",
    OperationId = "GetArticles",
    Tags = new[] { "Articles" }
)]
[SwaggerResponse(200, "Succ�s! Liste des articles recup�r�e", typeof(IEnumerable<ArticleItemDTO>))]
static async Task<IResult> GetAllArticles(RestauDbContext db)
{
    return TypedResults.Ok(await db.Article.Select(x => new ArticleItemDTO(x)).ToArrayAsync());
}

// GET - Rechercher un article sp�cifique (par son id)
[SwaggerOperation(
    Summary = "R�cupere un article par son Id",
    Description = "R�cup�re un article sp�cifique du Restaurant en sp�cifiant son Id ",
    OperationId = "GetArticlesById",
    Tags = new[] { "Articles" }
)]
[SwaggerResponse(
    200,
    "L'Article a �tait trouv�e avec succ�s !",
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

static Task<IResult> PostArticles(Article article, RestauDbContext db)
{
    db.Article.Add(article);
    db.SaveChanges();
    return Task.FromResult<IResult>(
        Results.Created($"/articles/{article.Id}", new ArticleItemDTO(article))
    );
}

app.UseHttpsRedirection();

app.Run();



/// public enum Type { Entree, Plat, Dessert } dans le OnModelCreating

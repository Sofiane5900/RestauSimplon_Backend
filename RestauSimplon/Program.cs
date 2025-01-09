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

// GET - Tout les articles
app.MapGet(
    "/articles",
    [SwaggerOperation(
        Summary = "Récupère la liste des articles",
        Description = "Récupère la liste des articles du restaurant",
        OperationId = "GetArticles",
        Tags = new[] { "Articles" }
    )]
    async (RestauDbContext db) =>
    {
        return await db.Article.ToListAsync();
    }
);

app.UseHttpsRedirection();

app.Run();



/// public enum Type { Entree, Plat, Dessert } dans le OnModelCreating

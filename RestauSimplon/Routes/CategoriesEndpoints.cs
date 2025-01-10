using Microsoft.EntityFrameworkCore;
using RestauSimplon.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Routes
{
    public static class CategoriesEndpoints
    {
        public static RouteGroupBuilder CategoriesRoutes(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllCategories);
            group.MapPost("/", CreateCategorie);
            group.MapPut("/{id}", UpdateCategorie);
            group.MapDelete("/{id}", DeleteCategorie);
            return group;
        }

        [SwaggerOperation(
            Summary = "Récupèrer la liste des catégories",
            Tags = new[] { "Categories" }
        )]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<CategorieDTO>))]
        static async Task<IResult> GetAllCategories(RestauDbContext db)
        {
            var categories = await db.Categorie.ToListAsync();
            var categoriesDTO = categories.Select(c => new CategorieDTO(c)).ToList();
            return TypedResults.Ok(categoriesDTO);
        }

        [SwaggerOperation(Summary = "Ajout d'une catégorie", Tags = new[] { "Categories" })]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<CategorieDTO>))]
        static async Task<IResult> CreateCategorie(CategorieDTO categorieDTO, RestauDbContext db)
        {
            var categorie = new Categorie { Nom = categorieDTO.Nom };
            db.Categorie.Add(categorie);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/categories/{categorie.Id}", new CategorieDTO(categorie));
        }

        [SwaggerOperation(Summary = "Mettre à jour une catégorie", Tags = new[] { "Categories" })]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<CategorieDTO>))]
        static async Task<IResult> UpdateCategorie(
            int id,
            CategorieDTO categorieDTO,
            RestauDbContext db
        )
        {
            var categorieToUpdate = await db.Categorie.FindAsync(id);
            if (categorieToUpdate == null)
                return Results.NotFound();

            categorieToUpdate.Nom = categorieDTO.Nom;
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        [SwaggerOperation(Summary = "Supprimer une catégorie", Tags = new[] { "Categories" })]
        [SwaggerResponse(200, "OK!", typeof(IEnumerable<CategorieDTO>))]
        static async Task<IResult> DeleteCategorie(int id, RestauDbContext db)
        {
            var categorie = await db.Categorie.FindAsync(id);
            if (categorie == null)
                return Results.NotFound();

            db.Categorie.Remove(categorie);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
    }
}

using RestauSimplon;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon
{
    public class ArticlesItemDTO
    {
        [SwaggerSchema("ID de l'article")]
        public int Id { get; set; }

        [SwaggerSchema("Nom de l'article")]
        public string? Nom { get; set; }

        [SwaggerSchema("Catégorie de l'article")]
        public string? Categorie { get; set; }

        [SwaggerSchema("Prix de l'article")]
        public int Prix { get; set; }

        public ArticlesItemDTO() { }
        public ArticlesItemDTO(Articles articlesItem) => (Id, Nom, Categorie, Prix) = (articlesItem.Id, articlesItem.Nom, articlesItem.Categorie, articlesItem.Prix);
    }
}

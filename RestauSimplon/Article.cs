﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon
{
    public class Article
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public int Prix { get; set; }
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

        // Ceci represente la relation avec la table Commande
        public ICollection<CommandeArticle> CommandeArticles { get; set; }
    }

    public class ArticleItemDTO
    {
        [SwaggerSchema("Nom de l'article")]
        public string Nom { get; set; }

        [SwaggerSchema("Catégorie de l'article")]
        public int CategorieId { get; set; }

        [SwaggerSchema("Prix de l'article")]
        public int Prix { get; set; }

        public ArticleItemDTO(Article articleItem)
        {
            this.Nom = articleItem.Nom;
            this.CategorieId = articleItem.CategorieId;
            this.Prix = articleItem.Prix;
        }
    }
}

/* Ceci est un commentaire (lol) */

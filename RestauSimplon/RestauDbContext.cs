﻿using Microsoft.EntityFrameworkCore;
using RestauSimplon.Models;

namespace RestauSimplon
{
    public class RestauDbContext : DbContext
    {
        public RestauDbContext(DbContextOptions<RestauDbContext> options)
            : base(options) { }

        public DbSet<Client> Client { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Categorie> Categorie { get; set; }
        public DbSet<Commande> Commande { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Création de nos tables
            modelBuilder.Entity<Client>().ToTable("Client");
            modelBuilder.Entity<Article>().ToTable("Article");
            modelBuilder.Entity<Categorie>().ToTable("Categorie");
            modelBuilder.Entity<Commande>().ToTable("Commande");
            modelBuilder.Entity<CommandeArticle>().ToTable("CommandeArticle");

            // On spécifie les ID des tables
            modelBuilder.Entity<Client>().HasKey(c => c.Id);
            modelBuilder.Entity<Article>().HasKey(a => a.Id);
            modelBuilder.Entity<Categorie>().HasKey(c => c.Id);
            modelBuilder.Entity<Commande>().HasKey(c => c.Id);
            modelBuilder
                .Entity<CommandeArticle>()
                .HasKey(ca => new { ca.CommandeId, ca.ArticleId });

            // On spécifie les relations entre les tables
            modelBuilder
                .Entity<Commande>()
                .HasOne(c => c.Client) // Une commande est passée par un client
                .WithMany(cl => cl.Commandes) // Un client peut passer plusieurs commandes
                .HasForeignKey(c => c.ClientId); // La clé étrangère de la table Commande est ClientId

            modelBuilder
                .Entity<Article>()
                .HasOne(a => a.Categorie) // Un article appartient à une catégorie
                .WithMany(c => c.Articles) // Une catégorie peut contenir plusieurs articles
                .HasForeignKey(a => a.CategorieId); // La clé étrangère de la table Article est CategorieId

            modelBuilder
                .Entity<CommandeArticle>()
                .HasOne(ca => ca.Commande) // Relation avec Commande
                .WithMany(c => c.CommandeArticles) // Une commande peut contenir plusieurs articles
                .HasForeignKey(ca => ca.CommandeId);

            modelBuilder
                .Entity<CommandeArticle>()
                .HasOne(ca => ca.Article) // Relation avec Article
                .WithMany(a => a.CommandeArticles) // Un article peut être dans plusieurs commandes
                .HasForeignKey(ca => ca.ArticleId);

            //Validations Client
            modelBuilder.Entity<Client>().Property(c => c.Nom).IsRequired();
            modelBuilder.Entity<Client>().Property(c => c.Prenom).IsRequired();
            modelBuilder.Entity<Client>().Property(c => c.Adresse).IsRequired();
            modelBuilder.Entity<Client>().Property(c => c.Phone).IsRequired();
            //Validations Article
            modelBuilder.Entity<Article>().Property(a => a.Nom).IsRequired();
            modelBuilder.Entity<Article>().Property(a => a.CategorieId).IsRequired();
            modelBuilder.Entity<Article>().Property(a => a.Prix).IsRequired();
            //Validations Categorie
            modelBuilder.Entity<Categorie>().Property(c => c.Nom).IsRequired();
            //Validations Commande
            modelBuilder.Entity<Commande>().Property(c => c.Date).IsRequired();
            modelBuilder.Entity<Commande>().Property(c => c.PrixTotal).IsRequired();
        }
    }
}

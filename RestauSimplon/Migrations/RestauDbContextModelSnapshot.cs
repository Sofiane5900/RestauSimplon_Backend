﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestauSimplon;

#nullable disable

namespace RestauSimplon.Migrations
{
    [DbContext(typeof(RestauDbContext))]
    partial class RestauDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.0");

            modelBuilder.Entity("RestauSimplon.Models.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategorieId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CommandeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Prix")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategorieId");

                    b.HasIndex("CommandeId");

                    b.ToTable("Article", (string)null);
                });

            modelBuilder.Entity("RestauSimplon.Models.Categorie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categorie", (string)null);
                });

            modelBuilder.Entity("RestauSimplon.Models.Client", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("RestauSimplon.Models.Commande", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("PrixTotal")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ClientId");

                    b.ToTable("Commande", (string)null);
                });

            modelBuilder.Entity("RestauSimplon.Models.CommandeArticle", b =>
                {
                    b.Property<int>("CommandeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ArticleId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CommandeId", "ArticleId");

                    b.HasIndex("ArticleId");

                    b.ToTable("CommandeArticle", (string)null);
                });

            modelBuilder.Entity("RestauSimplon.Models.Article", b =>
                {
                    b.HasOne("RestauSimplon.Models.Categorie", "Categorie")
                        .WithMany("Articles")
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestauSimplon.Models.Commande", null)
                        .WithMany("Articles")
                        .HasForeignKey("CommandeId");

                    b.Navigation("Categorie");
                });

            modelBuilder.Entity("RestauSimplon.Models.Commande", b =>
                {
                    b.HasOne("RestauSimplon.Models.Client", "Client")
                        .WithMany("Commandes")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("RestauSimplon.Models.CommandeArticle", b =>
                {
                    b.HasOne("RestauSimplon.Models.Article", "Article")
                        .WithMany("CommandeArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestauSimplon.Models.Commande", "Commande")
                        .WithMany("CommandeArticles")
                        .HasForeignKey("CommandeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");

                    b.Navigation("Commande");
                });

            modelBuilder.Entity("RestauSimplon.Models.Article", b =>
                {
                    b.Navigation("CommandeArticles");
                });

            modelBuilder.Entity("RestauSimplon.Models.Categorie", b =>
                {
                    b.Navigation("Articles");
                });

            modelBuilder.Entity("RestauSimplon.Models.Client", b =>
                {
                    b.Navigation("Commandes");
                });

            modelBuilder.Entity("RestauSimplon.Models.Commande", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("CommandeArticles");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(RecipeDbContext))]
    partial class RecipeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Ingridient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("IdRecipe")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("IdRecipe");

                    b.ToTable("Ingridient", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AuthorName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdAuthor")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("NumberOfPersons")
                        .HasColumnType("int");

                    b.Property<string>("PhotoUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortDescription")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("TimeCosts")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdAuthor");

                    b.ToTable("Recipe", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Step", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("IdRecipe")
                        .HasColumnType("int");

                    b.Property<int>("NumberOfStep")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdRecipe");

                    b.ToTable("Step", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Tag", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("About")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("RecipeTag", b =>
                {
                    b.Property<int>("RecipesByTagId")
                        .HasColumnType("int");

                    b.Property<int>("TagsId")
                        .HasColumnType("int");

                    b.HasKey("RecipesByTagId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("RecipesTagsMapping", (string)null);
                });

            modelBuilder.Entity("RecipeUser", b =>
                {
                    b.Property<int>("LikeRecipesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersLikesId")
                        .HasColumnType("int");

                    b.HasKey("LikeRecipesId", "UsersLikesId");

                    b.HasIndex("UsersLikesId");

                    b.ToTable("UsersRecipesLikeMapping", (string)null);
                });

            modelBuilder.Entity("RecipeUser1", b =>
                {
                    b.Property<int>("FavoriteRecipesId")
                        .HasColumnType("int");

                    b.Property<int>("UsersStarsId")
                        .HasColumnType("int");

                    b.HasKey("FavoriteRecipesId", "UsersStarsId");

                    b.HasIndex("UsersStarsId");

                    b.ToTable("UsersRecipesStarMapping", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Ingridient", b =>
                {
                    b.HasOne("Domain.Entities.RecipeEntities.Recipe", "Recipe")
                        .WithMany("IngridientForCooking")
                        .HasForeignKey("IdRecipe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Recipe", b =>
                {
                    b.HasOne("Domain.Entities.User", "Author")
                        .WithMany("PersonalRecipes")
                        .HasForeignKey("IdAuthor")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Step", b =>
                {
                    b.HasOne("Domain.Entities.RecipeEntities.Recipe", "Recipe")
                        .WithMany("StepOfCooking")
                        .HasForeignKey("IdRecipe")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("RecipeTag", b =>
                {
                    b.HasOne("Domain.Entities.RecipeEntities.Recipe", null)
                        .WithMany()
                        .HasForeignKey("RecipesByTagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.RecipeEntities.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RecipeUser", b =>
                {
                    b.HasOne("Domain.Entities.RecipeEntities.Recipe", null)
                        .WithMany()
                        .HasForeignKey("LikeRecipesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersLikesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RecipeUser1", b =>
                {
                    b.HasOne("Domain.Entities.RecipeEntities.Recipe", null)
                        .WithMany()
                        .HasForeignKey("FavoriteRecipesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersStarsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.RecipeEntities.Recipe", b =>
                {
                    b.Navigation("IngridientForCooking");

                    b.Navigation("StepOfCooking");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("PersonalRecipes");
                });
#pragma warning restore 612, 618
        }
    }
}

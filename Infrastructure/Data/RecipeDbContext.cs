using Domain.Entities;
using Domain.Entities.Map;
using Infrastructure.Data.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;
public class RecipeDbContext : DbContext
{
    public RecipeDbContext( DbContextOptions<RecipeDbContext> options )
        : base( options )
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<Ingridient> ingridients { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<RecipesTagsMapping> RTMap { get; set; }
    public DbSet<UserRecipeLikeMapping> URLikeMap { get; set; }
    public DbSet<UserRecipeStarMapping> URStarMap { get; set; }

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
        base.OnConfiguring( optionsBuilder );

        optionsBuilder.UseSqlServer( "Server=localhost\\SQLEXPRESS;Database=Recipes;Trusted_Connection=True;TrustServerCertificate=True;" );
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
        base.OnModelCreating( modelBuilder );

        modelBuilder.ApplyConfiguration( new IngridientConfiguration() );
        modelBuilder.ApplyConfiguration( new RecipeConfiguration() );
        modelBuilder.ApplyConfiguration( new RecipeTagMapConfiguration() );
        modelBuilder.ApplyConfiguration( new StepConfiguration() );
        modelBuilder.ApplyConfiguration( new TagConfiguration() );
        modelBuilder.ApplyConfiguration( new UserConfiguration() );
        modelBuilder.ApplyConfiguration( new UserRecipeLikeMapConfiguration() );
        modelBuilder.ApplyConfiguration( new UserRecipeStarMapConfiguration() );
    }
}

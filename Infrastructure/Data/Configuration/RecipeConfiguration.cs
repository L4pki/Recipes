using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.RecipeEntities;

namespace Infrastructure.Data.Configuration;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure( EntityTypeBuilder<Recipe> builder )
    {
        builder.ToTable( nameof( Recipe ) )
               .HasKey( r => r.Id );

        builder.Property( r => r.Name )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( r => r.ShortDescription )
               .HasMaxLength( 200 )
               .IsRequired();

        builder.Property( r => r.PhotoUrl )
               .IsRequired();

        builder.Property( r => r.TimeCosts )
               .IsRequired();

        builder.Property( r => r.NumberOfPersons )
               .IsRequired();

        builder.HasOne( r => r.Author )
               .WithMany( a => a.PersonalRecipes )
               .HasForeignKey( a => a.IdAuthor )
               .OnDelete( DeleteBehavior.Restrict );

        builder.HasMany( c => c.Tags )
               .WithMany( s => s.RecipesByTag )
               .UsingEntity( j => j.ToTable( "RecipesTagsMapping" ) );

        builder.HasMany( c => c.UsersLikes )
               .WithMany( s => s.LikeRecipes )
               .UsingEntity( j => j.ToTable( "UsersRecipesLikeMapping" ) );

        builder.HasMany( c => c.UsersStars )
               .WithMany( s => s.FavoriteRecipes )
               .UsingEntity( j => j.ToTable( "UsersRecipesStarMapping" ) );
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configuration;
public class UserRecipeStarMapConfiguration : IEntityTypeConfiguration<UserRecipeStarMapping>
{
    public void Configure( EntityTypeBuilder<UserRecipeStarMapping> builder )
    {
        builder.ToTable( nameof( UserRecipeStarMapping ) )
               .HasKey( ru => new { ru.IdRecipe, ru.IdUser } );

        builder.HasOne( ru => ru.User )
               .WithMany( u => u.FavoriteRecipes )
               .HasForeignKey( rt => rt.IdUser )
               .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( rt => rt.Recipe )
               .WithMany( t => t.UsersStars )
               .HasForeignKey( rt => rt.IdRecipe )
               .OnDelete( DeleteBehavior.Restrict );
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configuration;
public class UserRecipeLikeMapConfiguration : IEntityTypeConfiguration<UserRecipeLikeMapping>
{
    public void Configure( EntityTypeBuilder<UserRecipeLikeMapping> builder )
    {
        builder.ToTable( nameof( UserRecipeLikeMapping ) )
               .HasKey( ru => new { ru.IdRecipe, ru.IdUser } );

        builder.HasOne( ru => ru.User )
               .WithMany( u => u.LikeRecipes )
               .HasForeignKey( rt => rt.IdUser )
               .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( rt => rt.Recipe )
               .WithMany( t => t.Likers )
               .HasForeignKey( rt => rt.IdRecipe )
               .OnDelete( DeleteBehavior.Restrict );
    }
}

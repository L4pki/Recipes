using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Map;

namespace Infrastructure.Data.Configuration;
public class RecipeTagMapConfiguration : IEntityTypeConfiguration<RecipesTagsMapping>
{
    public void Configure( EntityTypeBuilder<RecipesTagsMapping> builder )
    {
        builder.ToTable( nameof( RecipesTagsMapping ) )
               .HasKey( rt => new { rt.IdRecipe, rt.IdTag } );

        builder.HasOne( rt => rt.Recipe )
               .WithMany( r => r.Tags )
               .HasForeignKey( rt => rt.IdRecipe )
               .OnDelete( DeleteBehavior.Restrict );

        builder.HasOne( rt => rt.Tag )
               .WithMany( t => t.RecipesByTag )
               .HasForeignKey( rt => rt.IdTag )
               .OnDelete( DeleteBehavior.Restrict );
    }
}

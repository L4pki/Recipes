using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.RecipeEntities;

namespace Infrastructure.Data.Configuration;

public class IngridientConfiguration : IEntityTypeConfiguration<Ingridient>
{
    public void Configure( EntityTypeBuilder<Ingridient> builder )
    {
        builder.ToTable( nameof( Ingridient ) )
               .HasKey( i => i.Id );

        builder.Property( i => i.Title )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( i => i.Description )
               .HasMaxLength( 200 )
               .IsRequired();

        builder.HasOne( i => i.Recipe )
               .WithMany( a => a.IngridientForCooking )
               .HasForeignKey( i => i.IdRecipe )
               .OnDelete( DeleteBehavior.Restrict );
    }
}

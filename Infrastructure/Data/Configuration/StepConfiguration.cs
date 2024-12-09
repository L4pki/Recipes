using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.RecipeEntities;

namespace Infrastructure.Data.Configuration;
public class StepConfiguration : IEntityTypeConfiguration<Step>
{
    public void Configure( EntityTypeBuilder<Step> builder )
    {
        builder.ToTable( nameof( Step ) )
               .HasKey( s => s.Id );

        builder.Property( s => s.NumberOfStep )
               .IsRequired();

        builder.Property( s => s.Description )
               .HasMaxLength( 200 )
               .IsRequired();

        builder.HasOne( s => s.Recipe )
               .WithMany( a => a.StepOfCooking )
               .HasForeignKey( s => s.IdRecipe )
               .OnDelete( DeleteBehavior.Cascade );
    }
}

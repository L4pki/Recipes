using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

        builder.Property( r => r.PhotoURL )
               .IsRequired();

        builder.Property( r => r.TimeCosts )
               .IsRequired();

        builder.Property( r => r.NumberOfPersons )
               .IsRequired();

        builder.Property( r => r.Likes )
               .IsRequired();

        builder.Property( r => r.Stars )
               .IsRequired();

        builder.HasOne( r => r.Author )
               .WithMany( a => a.PersonalRecipes )
               .HasForeignKey( a => a.IdAuthor )
               .OnDelete( DeleteBehavior.Restrict );
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.RecipeEntities;

namespace Infrastructure.Data.Configuration;
public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure( EntityTypeBuilder<Tag> builder )
    {
        builder.ToTable( nameof( Tag ) )
               .HasKey( r => r.Id );

        builder.Property( r => r.Name )
               .HasMaxLength( 100 )
               .IsRequired();
    }
}

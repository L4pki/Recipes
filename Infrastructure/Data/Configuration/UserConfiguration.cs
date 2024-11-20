using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure( EntityTypeBuilder<User> builder )
    {
        builder.ToTable( nameof( User ) )
               .HasKey( u => u.Id );

        builder.Property( u => u.Name )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( u => u.About )
               .HasMaxLength( 200 )
               .IsRequired();

        builder.Property( u => u.Login )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.Property( u => u.PasswordHash )
               .HasMaxLength( 100 )
               .IsRequired();

        builder.HasMany( a => a.PersonalRecipes )
               .WithOne( r => r.Author )
               .HasForeignKey( r => r.IdAuthor )
               .OnDelete( DeleteBehavior.Restrict );
    }
}

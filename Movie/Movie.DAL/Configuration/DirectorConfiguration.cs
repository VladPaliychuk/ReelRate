using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities;

namespace Movie.DAL.Configuration;

public class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FirstName)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(d => d.LastName)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(d => d.Info)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Image)
            .IsRequired();

        builder.HasMany(d => d.FilmDirectors)
            .WithOne(fd => fd.Director)
            .HasForeignKey(fd => fd.DirectorId);
    }
}
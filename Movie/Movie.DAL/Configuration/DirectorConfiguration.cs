using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities;

namespace Movie.DAL.Configuration;

public class DirectorConfiguration : IEntityTypeConfiguration<Director>
{
    public void Configure(EntityTypeBuilder<Director> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.FullName)
            .IsRequired()
            .HasMaxLength(40);
        
        builder.Property(d => d.BirthDate)
            .IsRequired(false)
            .HasMaxLength(10);
        
        builder.Property(d => d.BirthPlace)
            .IsRequired(false)
            .HasMaxLength(100);
        
        builder.Property(d => d.Info)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Image)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.HasMany(d => d.FilmDirectors)
            .WithOne(fd => fd.Director)
            .HasForeignKey(fd => fd.DirectorId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities;

namespace Movie.DAL.Configuration;

public class FilmConfiguration : IEntityTypeConfiguration<Film>
{
    public void Configure(EntityTypeBuilder<Film> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(f => f.ReleaseDate)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(f => f.Description)
            .IsRequired()
            .HasMaxLength(2000);
        
        builder.Property(f => f.Duration)
            .IsRequired()
            .HasMaxLength(15);
        
        builder.Property(f => f.Country)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(f => f.AgeRestriction)
            .IsRequired()
            .HasMaxLength(3);
        
        builder.Property(f => f.Image)
            .IsRequired(false)
            .HasMaxLength(1000);
        
        builder.HasOne(f => f.Rating)
            .WithOne(r => r.Film)
            .HasForeignKey<Rating>(r => r.FilmId);
        
        builder.HasMany(f => f.FilmActors)
            .WithOne(fa => fa.Film)
            .HasForeignKey(fa => fa.FilmId);

        builder.HasMany(f => f.FilmDirectors)
            .WithOne(fd => fd.Film)
            .HasForeignKey(fd => fd.FilmId);

        builder.HasMany(f => f.FilmGenres)
            .WithOne(fg => fg.Film)
            .HasForeignKey(fg => fg.FilmId);
    }
}
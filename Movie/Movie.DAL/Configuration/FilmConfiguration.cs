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
            .IsRequired();

        builder.Property(f => f.Description)
            .IsRequired();
        
        builder.Property(f => f.Duration)
            .IsRequired();
        
        builder.Property(f => f.Country)
            .IsRequired();
        
        builder.Property(f => f.AgeRestriction)
            .IsRequired();
        
        builder.Property(f => f.Image)
            .IsRequired(false);
        
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
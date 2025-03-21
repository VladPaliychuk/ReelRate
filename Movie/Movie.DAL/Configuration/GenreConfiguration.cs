using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities;

namespace Movie.DAL.Configuration;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(15);

        builder.HasMany(g => g.FilmGenres)
            .WithOne(fg => fg.Genre)
            .HasForeignKey(fg => fg.GenreId);
    }
}
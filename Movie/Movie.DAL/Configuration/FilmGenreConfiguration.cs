using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities.RelationsEntity;

namespace Movie.DAL.Configuration;

public class FilmGenreConfiguration : IEntityTypeConfiguration<FilmGenre>
{
    public void Configure(EntityTypeBuilder<FilmGenre> builder)
    {
        builder.HasKey(fg => new { fg.FilmId, fg.GenreId });

        builder.HasOne(fg => fg.Film)
            .WithMany(f => f.FilmGenres)
            .HasForeignKey(fg => fg.FilmId);

        builder.HasOne(fg => fg.Genre)
            .WithMany(g => g.FilmGenres)
            .HasForeignKey(fg => fg.GenreId);
    }
}
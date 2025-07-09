using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities.RelationsEntities;

namespace Movie.DAL.Configuration;

public class FilmDirectorConfiguration : IEntityTypeConfiguration<FilmDirector>
{
    public void Configure(EntityTypeBuilder<FilmDirector> builder)
    {
        builder.HasKey(fd => new { fd.FilmId, fd.DirectorId });

        builder.HasOne(fd => fd.Film)
            .WithMany(f => f.FilmDirectors)
            .HasForeignKey(fd => fd.FilmId);

        builder.HasOne(fd => fd.Director)
            .WithMany(d => d.FilmDirectors)
            .HasForeignKey(fd => fd.DirectorId);
    }
}
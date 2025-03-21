using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities.RelationsEntity;

namespace Movie.DAL.Configuration;

public class FilmActorConfiguration : IEntityTypeConfiguration<FilmActor>
{
    public void Configure(EntityTypeBuilder<FilmActor> builder)
    {
        builder.HasKey(fa => new { fa.FilmId, fa.ActorId });

        builder.HasOne(fa => fa.Film)
            .WithMany(f => f.FilmActors)
            .HasForeignKey(fa => fa.FilmId);

        builder.HasOne(fa => fa.Actor)
            .WithMany(a => a.FilmActors)
            .HasForeignKey(fa => fa.ActorId);
    }
}
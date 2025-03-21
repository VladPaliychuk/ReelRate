using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities;

namespace Movie.DAL.Configuration;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FirstName)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(a => a.LastName)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(d => d.Info)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Image)
            .IsRequired();

        builder.HasMany(a => a.FilmActors)
            .WithOne(fa => fa.Actor)
            .HasForeignKey(fa => fa.ActorId);
    }
}
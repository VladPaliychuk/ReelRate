using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities;

namespace Movie.DAL.Configuration;

public class ActorConfiguration : IEntityTypeConfiguration<Actor>
{
    public void Configure(EntityTypeBuilder<Actor> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullName)
            .IsRequired()
            .HasMaxLength(40);
        
        builder.Property(a => a.BirthDate)
            .IsRequired(false)
            .HasMaxLength(10);
        
        builder.Property(a => a.BirthPlace)
            .IsRequired(false)
            .HasMaxLength(100);
        
        builder.Property(d => d.Info)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(d => d.Image)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.HasMany(a => a.FilmActors)
            .WithOne(fa => fa.Actor)
            .HasForeignKey(fa => fa.ActorId);
    }
}
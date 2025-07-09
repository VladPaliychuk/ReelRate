using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.DAL.Entities;

namespace Movie.DAL.Configuration;

public class RatingConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.HasKey(r => r.Id);
        
        builder.HasOne(r => r.Film)
            .WithOne(f => f.Rating)
            .HasForeignKey<Rating>(r => r.FilmId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
        // Ensure each film can only have one rating record
        builder.HasIndex(r => r.FilmId).IsUnique();
        
        builder.Property(r => r.Score)
            .IsRequired()
            .HasColumnType("decimal(3,1)")
            .HasDefaultValue(0.0m); 
        
        builder.Property(r => r.Count)
            .IsRequired()
            .HasDefaultValue(0);
    }
}
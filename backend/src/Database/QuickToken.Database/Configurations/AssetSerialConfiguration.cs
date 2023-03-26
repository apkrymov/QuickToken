using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickToken.Database.Models;

namespace QuickToken.Database.Configurations;

public class AssetSerialConfiguration : IEntityTypeConfiguration<AssetSerial>
{
    public void Configure(EntityTypeBuilder<AssetSerial> builder)
    {
        builder.ToTable("asset_serial")
            .HasKey(p => p.Id);

        builder.Property(p => p.Price)
            .HasColumnName("price");
        
        builder.Property(p => p.DailyInterestRate)
            .HasColumnName("daily_interest_rate");
        
        builder.Property(p => p.IpoTimestamp)
            .HasColumnName("ipo_timestamp");
        
        builder.Property(p => p.BurnTimestamp)
            .HasColumnName("burn_timestamp");

        builder.HasMany(p => p.Assets)
            .WithOne(p => p.AssetSerial)
            .HasForeignKey(p => p.AssetSerialId);
    }
}
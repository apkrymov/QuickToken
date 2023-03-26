using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickToken.Database.Models;

namespace QuickToken.Database.Configurations;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.ToTable("asset")
            .HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.TokenId)
            .HasColumnName("token_id");
        
        builder.Property(p => p.WalletId)
            .HasColumnName("wallet_id");
        
        builder.Property(p => p.AssetSerialId)
            .HasColumnName("asset_serial_id");
        
        builder.HasOne(p => p.Wallet)
            .WithMany(p => p.Assets);
        
        builder.HasOne(p => p.AssetSerial)
            .WithMany(p => p.Assets);

        builder.HasIndex(p => p.TokenId)
            .IsUnique();
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickToken.Database.Models;

namespace QuickToken.Database.Configurations;

public class WalletSnapshotConfiguration : IEntityTypeConfiguration<WalletSnapshot>
{
    public void Configure(EntityTypeBuilder<WalletSnapshot> builder)
    {
        builder.ToTable("wallet_snapshot")
            .HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");
        
        builder.Property(p => p.Timestamp)
            .HasColumnName("timestamp");

        builder.Property(p => p.Eth)
            .HasColumnName("eth");

        builder.Property(p => p.Currency)
            .HasColumnName("currency");
        
        builder.HasOne(p => p.Wallet)
            .WithMany(p => p.Snapshots)
            .HasForeignKey(p => p.WalletId);

        builder.HasIndex(p => new { p.WalletId, p.Timestamp });
    }
}
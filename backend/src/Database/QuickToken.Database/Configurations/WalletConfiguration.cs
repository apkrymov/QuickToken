using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickToken.Database.Models;
using QuickToken.Shared.Eth;

namespace QuickToken.Database.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("wallet")
            .HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.Address)
            .HasColumnName("address")
            .HasMaxLength(Wallet.AddressMaxLength);

        builder.Property(p => p.AccountId)
            .HasColumnName("account_id");
        
        builder.Property(p => p.Eth)
            .HasColumnName("eth");

        builder.Property(p => p.Currency)
            .HasColumnName("currency");
        
        builder.HasOne(p => p.Account)
            .WithOne(p => p.Wallet);
        
        builder.HasMany(p => p.Assets)
            .WithOne(p => p.Wallet)
            .HasForeignKey(p => p.WalletId);

        builder.HasIndex(p => p.Address)
            .IsUnique();

        builder.Property(p => p.LastUpdateAt)
            .HasColumnName("last_update_at");
        
        builder.Property(p => p.ForceCacheUpdate)
            .HasColumnName("force_cache_update");
        
        builder.HasIndex(p => p.LastUpdateAt);

        builder.HasIndex(p => p.ForceCacheUpdate);

        Seed(builder);
    }

    private void Seed(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasData(new Wallet
        {
            Id = Guid.NewGuid(),
            Address = DexContract.Address
        });
    }
}
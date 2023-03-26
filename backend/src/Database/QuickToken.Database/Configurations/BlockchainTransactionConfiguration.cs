using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickToken.Database.Models;

namespace QuickToken.Database.Configurations;

public class BlockchainTransactionConfiguration : IEntityTypeConfiguration<BlockchainTransaction>
{
    public void Configure(EntityTypeBuilder<BlockchainTransaction> builder)
    {
        builder.ToTable("blockchain_transaction")
            .HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnName("id");

        builder.Property(p => p.InputPayload)
            .HasColumnName("input_payload")
            .HasMaxLength(BlockchainTransaction.InputPayloadMaxLength)
            .IsRequired();

        builder.Property(p => p.OutputPayload)
            .HasColumnName("output_payload")
            .HasMaxLength(BlockchainTransaction.OutputPayloadMaxLength);
        
        builder.Property(p => p.State)
            .HasColumnName("state");
        
        builder.Property(p => p.Hash)
            .HasColumnName("hash");
        
        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(p => p.LastUpdateAt)
            .HasColumnName("last_update_at");

        builder.HasIndex(p => p.State);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickToken.Database.Models;

namespace QuickToken.Database.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("account")
            .HasKey(p => p.Id);
        
        builder.Property(p => p.Id)
            .HasColumnName("id");
        
        builder.Property(p => p.Roles)
            .HasColumnName("roles")
            .HasMaxLength(Account.RolesMaxLength)
            .IsRequired();

        builder.Property(p => p.Login)
            .HasColumnName("login")
            .HasMaxLength(Account.LoginMaxLength);

        builder.Property(p => p.Password)
            .HasColumnName("password")
            .HasMaxLength(Account.PasswordMaxLength);

        builder.HasOne(p => p.Wallet)
            .WithOne(p => p.Account)
            .HasForeignKey<Wallet>(p => p.AccountId);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(p => p.LastAuthAt)
            .HasColumnName("last_auth_at");

        builder.HasIndex(p => p.Login)
            .IsUnique();
    }
}
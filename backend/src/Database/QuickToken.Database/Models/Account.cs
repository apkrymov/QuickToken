namespace QuickToken.Database.Models;

public class Account
{
    public Guid Id { get; set; }

    public const int RolesMaxLength = 50;
    public string Roles { get; set; }

    public const int LoginMaxLength = 50;
    public string? Login { get; set; }

    public const int PasswordMaxLength = 50;
    public string? Password { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset LastAuthAt { get; set; }
    
    public Wallet Wallet { get; set; }
}
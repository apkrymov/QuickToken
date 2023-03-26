namespace QuickToken.Shared.Web.Models;

public class JwtClaims
{
    public Guid Id { get; set; }
    
    public string[] Roles { get; set; }
}
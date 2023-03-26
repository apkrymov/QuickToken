using System.Security.Claims;

namespace QuickToken.Shared.Web.RBAC;

public static class ClaimsPrincipalExtensions
{
    public static string[] Roles(this ClaimsPrincipal user)
    {
        var roles = user.FindAll(ClaimTypes.Role);
        return roles.Select(p => p.Value).ToArray();
    }
    
    public static Guid Id(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}
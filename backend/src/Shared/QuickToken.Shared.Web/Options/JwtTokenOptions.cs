using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace QuickToken.Shared.Web.Options;

public class JwtTokenOptions
{
    public string Issuer { get; set; }
    
    public string Audience { get; set; }
    
    public TimeSpan MaxExpiration { get; set; }
    
    public string PublicKey { get; set; }
    
    public string PrivateKey { get; set; }

    public RsaSecurityKey GetPublicKey()
    {
        var securityKey = RSA.Create();
        securityKey.ImportSubjectPublicKeyInfo(Convert.FromBase64String(PublicKey), out int _);
        return new RsaSecurityKey(securityKey);
    }
    
    public RsaSecurityKey GetPrivateKey()
    {
        var securityKey = RSA.Create();
        securityKey.ImportPkcs8PrivateKey(Convert.FromBase64String(PrivateKey), out int _);
        return new RsaSecurityKey(securityKey);
    }
}
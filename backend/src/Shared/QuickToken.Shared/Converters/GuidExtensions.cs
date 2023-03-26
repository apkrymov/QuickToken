using System.Numerics;

namespace QuickToken.Shared.Converters;

public static class GuidExtensions
{
    public static BigInteger ToBigInteger(this Guid guid)
    {
        return new BigInteger(guid.ToByteArray(), isUnsigned: true, isBigEndian: false);
    }
    
    public static Guid Create(this BigInteger tokenId)
    {
        var result = new byte[16];
        var bytes = tokenId.ToByteArray(isUnsigned: true, isBigEndian: false);
        Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
        return new Guid(result);
    }
}
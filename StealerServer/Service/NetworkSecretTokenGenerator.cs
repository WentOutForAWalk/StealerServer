using System.Security.Cryptography;

namespace StealerServer.Service;

public static class NetworkSecretTokenGenerator
{
    private static readonly byte[] _secretBytes = new byte[]
    {
        208, 154, 208, 187, 208, 176, 209, 129, 209, 129, 208, 184, 209, 135, 208, 181, 209, 129, 208, 186, 208, 184, 208, 185, 32, 208, 183, 208, 176, 209, 130, 209, 143, 208, 182, 208, 189, 208, 190, 208, 185, 32, 208, 180, 208, 181, 208, 191, 209, 128, 208, 181, 209, 129, 209, 129, 208, 184, 208, 178, 208, 189, 209, 139, 208, 185, 32, 209, 129, 208, 184, 208, 189, 208, 180, 209, 128, 208, 190, 208, 188, 32, 209, 129, 32, 208, 178, 209, 139, 209, 128, 208, 176, 208, 182, 208, 181, 208, 189, 208, 189, 208, 190, 208, 185, 32, 208, 180, 208, 184, 209, 129, 208, 188, 208, 190, 209, 128, 209, 132, 208, 190, 209, 132, 208, 190, 208, 177, 208, 184, 208, 181, 208, 185, 32, 208, 184, 32, 209, 131, 209, 129, 208, 181, 209, 133, 208, 190, 208, 183, 208, 181, 208, 188, 208, 181, 208, 189, 208, 181, 208, 189, 208, 189, 208, 190, 208, 185, 32, 208, 177, 208, 190, 208, 187, 209, 140, 209, 142, 46, 32, 208, 154, 208, 186, 208, 176, 208, 182, 208, 180, 209, 131, 32, 209, 133, 209, 131, 208, 185, 208, 187, 208, 190, 46
    };

    public static string GenerateCurrentToken(long minuteOffset = 0)
    {
        long currentMinute = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMinutes + minuteOffset;
        byte[] timeBytes = BitConverter.GetBytes(currentMinute);

        using (var hmac = new HMACSHA256(_secretBytes))
        {
            byte[] hashBytes = hmac.ComputeHash(timeBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }


}


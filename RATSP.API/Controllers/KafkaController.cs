using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using RATSP.Common.Models;
using RATSP.Common.Services;
using System.IO.Compression;
using RATSP.Common.Interfaces;

namespace RATSP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KafkaController : ControllerBase
{
    private readonly apiKafkaProducer _kafkaProducer;
    private readonly IRedisService _redisService;

    public KafkaController(apiKafkaProducer kafkaProducer, IRedisService redisService)
    {
        _kafkaProducer = kafkaProducer;
        _redisService = redisService;
    }

    [HttpPost("Publish")]
    public async Task<IActionResult> Publish([FromBody] CreateExcelDocumentsRequest createExcelDocumentsRequest)
    {
        createExcelDocumentsRequest.RequestId = Guid.NewGuid();
        var message = JsonConvert.SerializeObject(createExcelDocumentsRequest);
        await _kafkaProducer.SendMessageAsync("excel-topic", message);
    
        var timeout = TimeSpan.FromMinutes(5);  // Максимальное время ожидания - 30 секунд
        var pollInterval = TimeSpan.FromSeconds(1); // Интервал для проверки Redis каждые 1 секунду
        var stopwatch = Stopwatch.StartNew();  // Секундомер для отслеживания времени

        byte[] zipBytes = null;
    
        while (stopwatch.Elapsed < timeout)
        {
            zipBytes = await _redisService.GetAsync($"zip-archive:{createExcelDocumentsRequest.RequestId}");

            if (zipBytes != null)
            {
                // Если данные найдены, выходим из цикла
                break;
            }

            // Ждем перед следующей проверкой
            await Task.Delay(pollInterval);
        }

        // Если zipBytes по-прежнему null, возвращаем ошибку
        if (zipBytes == null)
        {
            return StatusCode(500, "Error: Could not generate the file in time");
        }

        // Возвращаем zip файл клиенту
        return new FileStreamResult(new MemoryStream(zipBytes), "application/zip")
        {
            FileDownloadName = "generated_documents.zip"
        };
    }
}
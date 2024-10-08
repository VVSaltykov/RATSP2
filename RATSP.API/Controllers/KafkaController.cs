using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using RATSP.Common.Models;
using RATSP.Common.Services;
using System.IO.Compression;

namespace RATSP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KafkaController : ControllerBase
{
    private readonly apiKafkaProducer _kafkaProducer;
    private readonly apiKafkaConsumer _kafkaConsumer;

    public KafkaController(apiKafkaProducer kafkaProducer, apiKafkaConsumer kafkaConsumer)
    {
        _kafkaProducer = kafkaProducer;
        _kafkaConsumer = kafkaConsumer;
    }

    [HttpPost("Publish")]
    public async Task Publish([FromBody] CreateExcelDocumentsRequest createExcelDocumentsRequest)
    {
        var message = JsonConvert.SerializeObject(createExcelDocumentsRequest);
        await _kafkaProducer.SendMessageAsync("excel-topic", message);
    }

    [HttpGet("GetExcelDocument")]
    public async Task<string> GetExcelDocuments()
    {
        var excelDocuments = _kafkaConsumer.GetExcelDocuments();

        // Генерация zip-файла из полученных Excel документов
        var zipBytes = CreateZipFile(excelDocuments);

        var base64Zip = Convert.ToBase64String(zipBytes);

        // Возвращаем zip-файл в виде байтов
        return base64Zip;
    }

    private byte[] CreateZipFile(List<IWorkbook> workbooks)
    {
        using var zipStream = new MemoryStream();
        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            for (int i = 0; i < workbooks.Count; i++)
            {
                var workbook = workbooks[i];
                var fileName = $"document_{i + 1}.xlsx";

                // Сохранение workbook в MemoryStream
                using var entryStream = archive.CreateEntry(fileName).Open();
                workbook.Write(entryStream); // Сохраняем IWorkbook в поток
            }
        }
        return zipStream.ToArray();
    }
}
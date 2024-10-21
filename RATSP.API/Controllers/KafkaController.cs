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

    public KafkaController(apiKafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }

    [HttpPost("Publish")]
    public async Task Publish([FromBody] CreateExcelDocumentsRequest createExcelDocumentsRequest)
    {
        var message = JsonConvert.SerializeObject(createExcelDocumentsRequest);
        await _kafkaProducer.SendMessageAsync("excel-topic", message);
    }
}
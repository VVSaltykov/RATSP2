using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RATSP.Common.Models;
using RATSP.Common.Services;

namespace RATSP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class KafkaController : ControllerBase
{
    private readonly KafkaProducer _kafkaProducer;

    public KafkaController(KafkaProducer kafkaProducer)
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
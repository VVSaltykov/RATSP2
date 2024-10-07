using Confluent.Kafka;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RATSP.Common.Interfaces;
using RATSP.Common.Models;
using RATSP.WebCommon.Models;

namespace RATSP.GrossService.Services;

public class KafkaConsumer
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly string _topic;
    private readonly ExcelService excelService;
    private readonly ExcelValuesService excelValuesService;
    private readonly ICompaniesService companiesService;
    private readonly IFractionsService fractionsService;

    public KafkaConsumer(string bootstrapServers, string groupId, string topic, ExcelService _excelService,
        ExcelValuesService _excelValuesService, ICompaniesService _companiesService, IFractionsService _fractionsService)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Null, string>(config).Build();
        _topic = topic;
        excelService = _excelService;
        excelValuesService = _excelValuesService;
        companiesService = _companiesService;
        fractionsService = _fractionsService;
    }

    public void StartConsuming()
    {
        _consumer.Subscribe(_topic);

        try
        {
            while (true)
            {
                var consumeResult = _consumer.Consume();
                Console.WriteLine($"Received message: {consumeResult.Message.Value}");

                // Десериализация сообщения
                var createExcelRequest = JsonConvert.DeserializeObject<CreateExcelDocumentsRequest>(consumeResult.Message.Value);
                
                // Обработка запроса
                ProcessCreateExcelRequest(createExcelRequest);
            }
        }
        catch (ConsumeException e)
        {
            Console.WriteLine($"Error occurred: {e.Error.Reason}");
        }
    }

    private async Task ProcessCreateExcelRequest(CreateExcelDocumentsRequest request)
    {
        List<Company> selectedCompanies = new List<Company>();
        
        byte[] workbookBytes = request.WorkbookBytes;
        using var memoryStream = new MemoryStream(workbookBytes);
        IWorkbook workbook = new XSSFWorkbook(memoryStream);
        
        List<ExcelValues> excelValuesList = await excelValuesService.AddExcelValues(workbook);

        foreach (var selectedCompanyName in request.SelectedCompanies)
        {
            var company = await companiesService.ReadByName(selectedCompanyName);
            selectedCompanies.Add(company);
        }
        
        List<Fraction> fractions = await fractionsService.Read();
        DateOnly selectedDate = DateOnly.Parse(request.SelectedDate);
        bool grossIn = request.GrossIn;
        bool grossOut = request.GrossOut;
        bool debit = request.Debit;
        bool credit = request.Credit;

        try
        {
            var excelDocuments = await excelService.CreateExcelDocuments(
                excelValuesList,
                selectedCompanies,
                fractions,
                selectedDate,
                grossIn,
                grossOut,
                debit,
                credit
            );

            foreach (var excelDocument in excelDocuments)
            {
                var filePath = Path.Combine("path/to/save", $"ExcelDocument_{Guid.NewGuid()}.xlsx");
                await File.WriteAllBytesAsync(filePath, excelDocument);
                Console.WriteLine($"Excel document saved to: {filePath}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing request: {ex.Message}");
        }
    }
}
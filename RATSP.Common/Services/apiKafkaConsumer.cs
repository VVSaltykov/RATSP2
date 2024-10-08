using Confluent.Kafka;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RATSP.Common.Interfaces;
using RATSP.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;

namespace RATSP.Common.Services
{
    public class apiKafkaConsumer
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly string _topic;
        private List<IWorkbook>? excelDocuments;

        public apiKafkaConsumer(string bootstrapServers, string groupId, string topic)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Null, string>(config).Build();
            _topic = topic;
            excelDocuments = new List<IWorkbook>();
        }

        public async void StartConsuming()
        {
            _consumer.Subscribe(_topic);

            try
            {
                while (true)
                {
                    var consumeResult = _consumer.Consume();
                    Console.WriteLine($"Received message: {consumeResult.Message.Value}");

                    // Десериализация сообщения
                    List<byte[]>? _excelDocuments = JsonConvert.DeserializeObject<List<byte[]>>(consumeResult.Message.Value);

                    excelDocuments = await GetExcelDocumentsFromBytesAsync(_excelDocuments);
                }
            }
            catch (ConsumeException e)
            {
                Console.WriteLine($"Error occurred: {e.Error.Reason}");
            }
        }

        public List<IWorkbook> GetExcelDocuments()
        {
            return excelDocuments;
        }

        private async Task<List<IWorkbook>> GetExcelDocumentsFromBytesAsync(List<byte[]> byteArrays)
        {
            var workbooks = new List<IWorkbook>();

            foreach (var byteArray in byteArrays)
            {
                using (var stream = new MemoryStream(byteArray))
                {
                    // Create a workbook from the byte array
                    IWorkbook workbook = new XSSFWorkbook(stream);
                    workbooks.Add(workbook);
                }
            }

            return workbooks;
        }
    }
}

using Grpc.Core;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RATSP.WebCommon.Models;

namespace RATSP.GrossService.Services;

public class ExcelValuesServiceImpl : ExcelValuesService.ExcelValuesServiceBase
{
    private readonly ExcelValuesService _excelValuesService;

    public ExcelValuesServiceImpl(ExcelValuesService excelValuesService)
    {
        _excelValuesService = excelValuesService;
    }
    
    public override Task<AddExcelValuesReply> AddExcelValues(AddExcelValuesRequest request, ServerCallContext context)
    {
        // Преобразование входящего byte[] в IWorkbook
        using var stream = new MemoryStream(request.WorkbookBytes.ToByteArray());
        IWorkbook workbook = new XSSFWorkbook(stream);
        
        // Используем существующий метод для обработки Excel
        List<ExcelValues> excelValuesList = _excelValuesService.AddExcelValues(workbook);

        // Преобразуем список ExcelValues в формат gRPC
        var reply = new AddExcelValuesReply();
        foreach (var excelValue in excelValuesList)
        {
            reply.ExcelValuesList.Add(new ExcelValue
            {
                Number = excelValue.Number,
                Insurer = excelValue.Insurer,
                Policyholder = excelValue.Policyholder,
                ContractNumber = excelValue.ContractNumber,
                StartDate = excelValue.StartDate,
                EndDate = excelValue.EndDate,
                Currency = excelValue.Currency,
                InsuranceAmountLiabilityLimit = excelValue.InsuranceAmount_LiabilityLimit,
                AccruedBonus100 = excelValue.AccruedBonus100,
                GrossPremium = excelValue.GrossPremium,
                ReinsurerCommissionPercent = excelValue.ReinsurerCommissionPercent,
                ReinsurerCommission = excelValue.ReinsurerCommission,
                AdministratorCommissionPercent = excelValue.AdministratorCommissionPercent,
                AdministratorCommission = excelValue.AdministratorCommission,
                NetPremium = excelValue.NetPremium,
                PremiumPercent = excelValue.PremiumPercent,
                PaymentRateReturnRate = excelValue.PaymentRate_ReturnRate,
                RefundPremium = excelValue.RefundPremium,
                AdministratorCommissionRub = excelValue.AdministratorCommissionRub,
                SanctionsRisk = excelValue.SanctionsRisk,
                ReinsurerFraction = excelValue.ReinsurerFraction,
                PaymentDate = excelValue.PaymentDate,
                PaymentSumm = excelValue.PaymentSumm,
                PaymentNumber = excelValue.PaymentNumber,
                Comment = excelValue.Comment,
                InsuranceType = excelValue.InsuranceType,
                PaymentContract = excelValue.PaymentContract
            });
        }

        return Task.FromResult(reply);
    }
}
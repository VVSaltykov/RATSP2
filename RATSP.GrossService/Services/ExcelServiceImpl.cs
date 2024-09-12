using Grpc.Core;
using RATSP.Common.Interfaces;
using RATSP.Common.Models;
using RATSP.WebCommon.Models;

namespace RATSP.GrossService.Services;

public class ExcelServiceImpl : ExcelService.ExcelServiceBase
{
    private readonly ICompaniesService _companiesService;
    private readonly IFractionsService _fractionsService;
    private readonly ExcelService _excelService;

    public ExcelServiceImpl(ICompaniesService companiesService, IFractionsService fractionsService, ExcelService excelService)
    {
        _companiesService = companiesService;
        _fractionsService = fractionsService;
        _excelService = excelService;
    }
    
    public override async Task<CreateExcelDocumentsReply> CreateExcelDocuments(CreateExcelDocumentsRequest request, ServerCallContext context)
    {
        var excelValuesList = request.ExcelValuesList.Select(e => new ExcelValues
        {
            Number = e.Number,
            Insurer = e.Insurer,
            Policyholder = e.Policyholder,
            ContractNumber = e.ContractNumber,
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            Currency = e.Currency,
            InsuranceAmount_LiabilityLimit = e.InsuranceAmountLiabilityLimit,
            AccruedBonus100 = e.AccruedBonus100,
            GrossPremium = e.GrossPremium,
            ReinsurerCommissionPercent = e.ReinsurerCommissionPercent,
            ReinsurerCommission = e.ReinsurerCommission,
            AdministratorCommissionPercent = e.AdministratorCommissionPercent,
            AdministratorCommission = e.AdministratorCommission,
            NetPremium = e.NetPremium,
            PremiumPercent = e.PremiumPercent,
            PaymentRate_ReturnRate = e.PaymentRateReturnRate,
            RefundPremium = e.RefundPremium,
            AdministratorCommissionRub = e.AdministratorCommissionRub,
            SanctionsRisk = e.SanctionsRisk,
            ReinsurerFraction = e.ReinsurerFraction,
            PaymentDate = e.PaymentDate,
            PaymentSumm = e.PaymentSumm,
            PaymentNumber = e.PaymentNumber,
            Comment = e.Comment,
            InsuranceType = e.InsuranceType,
            PaymentContract = e.PaymentContract
        }).ToList();

        var selectedCompaniesNames = request.SelectedCompanies.ToList();

        var selectedCompanies = new List<Company>();
        foreach (var companyName in selectedCompaniesNames)
        {
            var company = await _companiesService.ReadByName(companyName);
            if (company != null) // Проверка на null
            {
                selectedCompanies.Add(company);
            }
        }
    
        var fractions = await _fractionsService.Read();
        var selectedDate = DateOnly.Parse(request.SelectedDate);

        var excelByteArrays = await _excelService.CreateExcelDocuments(
            excelValuesList, selectedCompanies, fractions,
            selectedDate, request.GrossIn, request.GrossOut, request.Debit, request.Credit);

        return new CreateExcelDocumentsReply
        {
            ExcelFiles = { excelByteArrays.Select(b => Google.Protobuf.ByteString.CopyFrom(b)) }
        };
    }

}
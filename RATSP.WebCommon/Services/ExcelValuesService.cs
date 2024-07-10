using NPOI.SS.UserModel;
using RATSP.WebCommon.Models;

namespace RATSP.WebCommon.Services;

public static class ExcelValuesService
{
    public static List<ExcelValues> AddExcelValues(IWorkbook workBook)
    {
        List<ExcelValues> excelValuesList = new List<ExcelValues>();
        ISheet sheet = workBook.GetSheetAt(0);
                    
        for (int i = 2; i <= sheet.LastRowNum; i++)
        {
            IRow row = sheet.GetRow(i);

            if (row != null)
            {
                ExcelValues excelValues = new ExcelValues
                {
                    Number = row.GetCell(0)?.ToString(),
                    Insurer = row.GetCell(1)?.ToString(),
                    Policyholder = row.GetCell(2)?.ToString(),
                    ContractNumber = row.GetCell(3)?.ToString(),
                    StartDate = row.GetCell(4)?.ToString(),
                    EndDate = row.GetCell(5)?.ToString(),
                    Currency = row.GetCell(6)?.ToString(),
                    InsuranceAmount_LiabilityLimit = row.GetCell(7)?.ToString(),
                    AccruedBonus100 = row.GetCell(8)?.ToString(),
                    GrossPremium = row.GetCell(9)?.ToString(),
                    ReinsurerCommission = row.GetCell(10)?.ToString(),
                    ReinsurerCommissionPercent = row.GetCell(11)?.ToString(),
                    AdministratorCommissionPercent = row.GetCell(12)?.ToString(),
                    AdministratorCommission = row.GetCell(13)?.ToString(),
                    NetPremium = row.GetCell(14)?.ToString(),
                    PremiumPercent = row.GetCell(15)?.ToString(),
                    PaymentRate_ReturnRate = row.GetCell(16)?.ToString(),
                    RefundPremium = row.GetCell(17)?.ToString(),
                    AdministratorCommissionRub = row.GetCell(18)?.ToString(),
                    SanctionsRisk = row.GetCell(19)?.ToString(),
                    ReinsurerFraction = row.GetCell(20)?.ToString(),
                    PaymentDate = row.GetCell(21)?.ToString(),
                    PaymentSumm = row.GetCell(22)?.ToString(),
                    PaymentNumber = row.GetCell(23)?.ToString(),
                    Comment = row.GetCell(24)?.ToString(),
                    InsuranceType = row.GetCell(25)?.ToString(),
                    PaymentContract = row.GetCell(26)?.ToString(),
                };

                excelValuesList.Add(excelValues);
            }
        }

        return excelValuesList;
    }
}
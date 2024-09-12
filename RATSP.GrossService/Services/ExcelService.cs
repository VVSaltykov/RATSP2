using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RATSP.Common.Models;
using RATSP.GrossService.Functions;
using RATSP.WebCommon.Models;

namespace RATSP.GrossService.Services;

public partial class ExcelService
{
    public async Task<List<byte[]>> CreateExcelDocuments(List<ExcelValues> excelValuesList,
        List<Company> selectedCompanies, List<Fraction> fractions,
        DateOnly selectedDate, bool GrossIn, bool GrossOut, bool Debit, bool Credit)
    {
        var excelByteArrays = new List<byte[]>();

        foreach (var company in selectedCompanies)
        {
            using var memoryStream = new MemoryStream();
            var workbook = new XSSFWorkbook();

            if (GrossOut)
            {
                ISheet sheet = workbook.CreateSheet("Исходящее");
                OutFunctions.DrawingTableHeader(sheet, company, fractions, selectedDate);
                var companyExcelValuesList = excelValuesList.Where(e => e.Insurer == company.Name).ToList();
                OutFunctions.DrawingTable(workbook, sheet, companyExcelValuesList, company, fractions, selectedDate);
            }

            if (Debit)
            {
                ISheet sheet = workbook.CreateSheet("Дебет-нота");
                DebitFunctions.DrawingTable(workbook, sheet, excelValuesList, company, fractions, selectedDate);
            }

            if (GrossIn)
            {
                ISheet sheet = workbook.CreateSheet("Входящее");
                InFunctions.DrawingTableHeader(sheet, company, fractions, selectedDate);
                InFunctions.DrawingTable(workbook, sheet, excelValuesList, company, fractions, selectedDate);
            }

            if (Credit)
            {
                ISheet sheet = workbook.CreateSheet("Кредит-нота");
                CreditFunctions.DrawingTable(workbook, sheet, excelValuesList, company, fractions, selectedDate);
            }

            workbook.Write(memoryStream);
            excelByteArrays.Add(memoryStream.ToArray());
        }

        return excelByteArrays;
    }
}
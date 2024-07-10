using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using RATSP.Common.Models;
using RATSP.WebCommon.Functions;
using RATSP.WebCommon.Models;

namespace RATSP.WebCommon.Services;

public static class ExcelService
{
    public static void CreateExcelDocument(List<ExcelValues> excelValuesList,
        IList<Company> companies, List<Fraction> fractions,
        DateOnly selectedDate, bool GrossIn,
        bool GrossOut, bool Debit, bool Credit)
    {
        IWorkbook workbook = new XSSFWorkbook();

        if (GrossIn)
        {
            foreach (var company in companies)
            {
                ISheet sheet = workbook.CreateSheet("Исходящее");
                
                OutFunctions.DrawingTableHeader(sheet, company, fractions, selectedDate);

                List<ExcelValues> companyExcelValuesList =
                    excelValuesList.Where(e => e.Insurer == company.Name).ToList();
                
                OutFunctions.DrawingTable(sheet, companyExcelValuesList, company, fractions, selectedDate);
            }
        }
        if (Debit)
        {
            ISheet sheet = workbook.CreateSheet("Дебет-нота");
        }
        if (GrossOut)
        {
            ISheet sheet = workbook.CreateSheet("Входящее");
        }
        if (Credit)
        {
            ISheet sheet = workbook.CreateSheet("Кредит-нота");
        }
    }
}
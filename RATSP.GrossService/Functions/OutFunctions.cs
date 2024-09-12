using System.Globalization;
using NPOI.SS.UserModel;
using RATSP.Common.Models;
using RATSP.GrossService.Utils;
using RATSP.WebCommon.Models;

namespace RATSP.GrossService.Functions;

public static class OutFunctions
{
    public static void DrawingTableHeader(ISheet sheet, Company company,
        List<Fraction> fractions, DateOnly selectedDate)
    {
        string formattedDate = selectedDate.ToDateTime(TimeOnly.MinValue)
            .ToString("dd MMMM yyyy г.", new CultureInfo("ru-RU"));
        
        var companyFraction = fractions.FirstOrDefault(f => f.CompanyId == company.Id &&
                                                   f.Start < selectedDate && f.End > selectedDate);
        
        string text = $"Бордеро исходящих рисков по страхованию рисков «терроризм» и/или «диверсия» \u2116 38-4-2023 от {formattedDate}";
    
        ExcelHelper.SetCellValue(sheet, 1, 1,
            text, "Calibri", 14, (25.27, 18));
    
        ExcelHelper.SetCellValue(sheet, 2, 1,
            "к договору № Д-522111/11 от \"21\" ноября 2011 г.",
            "Calibri", 14, (25.27, 18));
    
        ExcelHelper.SetCellValue(sheet, 4, 1,
            "Перестрахователь:",
            "Calibri", 14, (25.27, 18));
    
        ExcelHelper.SetCellValue(sheet, 4, 2,
            $"{company.Name}",
            "Calibri", 12, (20.4, 18));
    
        ExcelHelper.SetCellValue(sheet, 6, 1,
            "Доля ответственности Перестрахователя в отчетном периоде:",
            "Calibri", 14, (25.27, 18));
        
        ExcelHelper.SetCellValue(sheet, 6, 6,
            $"{companyFraction.Value}",
            "Calibri", 12, (15.87, 18));
        
        ExcelHelper.SetCellValue(sheet, 8, 1,
            "Отчетный период:",
            "Calibri", 14, (25.27, 18));
        
        ExcelHelper.SetCellValue(sheet, 8, 2,
            $"{companyFraction.Start} - {companyFraction.End}",
            "Calibri", 12, (20.4, 18));
        
        ExcelHelper.SetCellValue(sheet, 10, 3,
            "Период страхования / перестрахования",
            "Calibri", 9, (12, 25.4),
            mergeRegion: (10, 10, 3, 4), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 8,
            "К начислению в РАТСП",
            "Calibri", 9, (15.73, 25.4),
            mergeRegion: (10, 10, 8, 14), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 15,
            "К оплате в РАТСП",
            "Calibri", 9, (11.4, 25.4),
            mergeRegion: (10, 10, 15, 20), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 20,
            "Справочно",
            "Calibri", 9, (10, 25.4), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 0,
            "\u2116 п/п",
            "Calibri", 9, (5.27, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 1,
            "Страхователь",
            "Calibri", 9, (25.27, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 2,
            "Договор страхования/ Номер полиса",
            "Calibri", 9, (20.4, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 3,
            "Начало договора",
            "Calibri", 9, (12, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 4,
            "Окончание договора",
            "Calibri", 9, (13.73, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 5,
            "Валюта договора",
            "Calibri", 9, (8.6, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 6,
            "Страховая сумма / Лимит ответственности в валюте договора / Приоритет (100%)",
            "Calibri", 9, (15.87, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 7,
            "100% начисленной премии по риску \"терроризм\"в валюте договора\n",
            "Calibri", 9, (12.73, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 8,
            "Ответственность в перестрахование в валюте договора\n",
            "Calibri", 9, (15.73, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 9,
            "Брутто-премия в валюте договора\n",
            "Calibri", 9, (10.6, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 10,
            "Комиссия перестрахователя, %\n",
            "Calibri", 9, (10.13, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 11,
            "Вознаграждение администратора в %\n",
            "Calibri", 9, (10.13, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 12,
            "Комиссия перестрахователя к начислению\n",
            "Calibri", 9, (10.13, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 13,
            "Вознаграждение администратора в валюте договора\n",
            "Calibri", 9, (10.6, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 14,
            "Нетто-премия в валюте договора\n",
            "Calibri", 9, (11, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 15,
            "Процент поступившей премии\n",
            "Calibri", 9, (11.4, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 16,
            "Курс оплаты премии Страхователем / Курс возврата премии Страхователю\n",
            "Calibri", 9, (11.73, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 17,
            "Нетто-премия в руб. / Возврат премии Перестрахователю (со знаком \" - \")\n",
            "Calibri", 9, (12.73, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 18,
            "Номер/Дата платежа/Доля/Санкционность/Комментарий\n",
            "Calibri", 9, (16.27, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 19,
            "Вознаграждение администратора в руб.\n",
            "Calibri", 9, (10, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 20,
            "Процент оплаты договора, %\n",
            "Calibri", 9, (10, 93), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 21,
            "Вид страхования\n",
            "Calibri", 9, (16, 93), applyBorders: true);
    }

    public static void DrawingTable(IWorkbook workbook, ISheet sheet, List<ExcelValues> excelValuesList,
        Company company, List<Fraction> fractions, DateOnly selectedDate)
    {
        int rowCount = 12;
        int number = 1;
        decimal reportingPeriodSum = 0;
        decimal reportingPeriodAward = 0;
        decimal previousPeriodSum = 0;
        decimal previousPeriodAward = 0;
        
        var companyFraction = fractions.FirstOrDefault(f => f.CompanyId == company.Id &&
                                                            f.Start < selectedDate && f.End > selectedDate);
        
        ExcelHelper.SetCellValue(sheet, rowCount, 0,
            "договоры отчетного периода\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\n",
            "Calibri", 9, (5.27, 14.3),
            mergeRegion: (rowCount, rowCount, 0, 21), applyBorders: true);

        rowCount++;
        
        excelValuesList.RemoveAll(excelValues =>
        {
            if (DateOnly.Parse(excelValues.StartDate) >= companyFraction.Start && DateOnly.Parse(excelValues.StartDate) <= companyFraction.End) 
            {
                ExcelHelper.SetCellValue(sheet, rowCount, 0,
                    $"{number}",
                    "Calibri", 11, (16, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 1,
                    $"{excelValues.Policyholder}",
                    "Calibri", 11, (25.27, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 2,
                    $"{excelValues.ContractNumber}",
                    "Calibri", 11, (20.4, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 3,
                    $"{excelValues.StartDate}",
                    "Calibri", 11, (12, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 4,
                    $"{excelValues.EndDate}",
                    "Calibri", 11, (13.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 5,
                    $"{excelValues.Currency}",
                    "Calibri", 11, (8.6, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 6,
                    $"{excelValues.InsuranceAmount_LiabilityLimit}",
                    "Calibri", 11, (15.87, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 7,
                    $"{excelValues.AccruedBonus100}",
                    "Calibri", 11, (12.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 8,
                    $"{Convert.ToDecimal(excelValues.InsuranceAmount_LiabilityLimit) * ((100 - companyFraction.Value)/100)}",
                    "Calibri", 11, (15.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 9,
                    $"{excelValues.GrossPremium}",
                    "Calibri", 11, (10.6, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 10,
                    $"{excelValues.ReinsurerCommissionPercent}",
                    "Calibri", 11, (10.13, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 11,
                    $"{excelValues.AdministratorCommissionPercent}",
                    "Calibri", 11, (10.13, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 12,
                    $"{excelValues.ReinsurerCommission}",
                    "Calibri", 11, (10.13, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 13,
                    $"{excelValues.AdministratorCommission}",
                    "Calibri", 11, (10.6, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 14,
                    $"{excelValues.NetPremium}",
                    "Calibri", 11, (11, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 15,
                    $"{excelValues.PremiumPercent}",
                    "Calibri", 11, (11.4, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 16,
                    $"{excelValues.PaymentRate_ReturnRate}",
                    "Calibri", 11, (11.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 17,
                    $"{Convert.ToDecimal(excelValues.NetPremium) * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate)}",
                    "Calibri", 11, (12.73, 28.5), applyBorders: true);

                reportingPeriodSum += (Convert.ToDecimal(excelValues.NetPremium) *
                                                           Convert.ToDecimal(excelValues.PaymentRate_ReturnRate));
                
                ExcelHelper.SetCellValue(sheet, rowCount, 18,
                    $"{number} / {excelValues.PaymentDate} / {companyFraction} / {excelValues.SanctionsRisk} / {excelValues.Comment}",
                    "Calibri", 11, (16.27, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 19,
                    $"{excelValues.AdministratorCommissionRub}",
                    "Calibri", 11, (10, 28.5), applyBorders: true);

                reportingPeriodAward += Convert.ToDecimal(excelValues.AdministratorCommissionRub);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 20,
                    $"{excelValues.PaymentContract}",
                    "Calibri", 11, (10, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 21,
                    $"{excelValues.InsuranceType}",
                    "Calibri", 11, (16, 28.5), applyBorders: true);
            

                rowCount++;
                number++;
                
                return true; 
            }
            return false; 
        });
        
        ExcelHelper.SetCellValue(sheet, rowCount, 14,
            "Итого по договорам отчетного периода:",
            "Calibri", 10, (11, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount, 17,
            $"{reportingPeriodSum}",
            "Calibri", 10, (12.73, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount, 19,
            $"{reportingPeriodAward}",
            "Calibri", 10, (10, 14.3), applyBorders: true);

        rowCount++;
        number = 1;
        
        ExcelHelper.SetCellValue(sheet, rowCount, 0,
            "договоры предыдущих отчетных периодов\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\n",
            "Calibri", 9, (5.27, 14.3),
            mergeRegion: (rowCount, rowCount, 0, 21), applyBorders: true);

        rowCount++;
        
        foreach (var excelValues in excelValuesList)
        {
            if (DateOnly.Parse(excelValues.StartDate) < companyFraction.Start || DateOnly.Parse(excelValues.StartDate) > companyFraction.End)
            {
                ExcelHelper.SetCellValue(sheet, rowCount, 0,
                    $"{number}",
                    "Calibri", 11, (16, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 1,
                    $"{excelValues.Policyholder}",
                    "Calibri", 11, (25.27, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 2,
                    $"{excelValues.ContractNumber}",
                    "Calibri", 11, (20.4, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 3,
                    $"{excelValues.StartDate}",
                    "Calibri", 11, (12, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 4,
                    $"{excelValues.EndDate}",
                    "Calibri", 11, (13.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 5,
                    $"{excelValues.Currency}",
                    "Calibri", 11, (8.6, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 6,
                    $"{excelValues.InsuranceAmount_LiabilityLimit}",
                    "Calibri", 11, (15.87, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 7,
                    $"{excelValues.AccruedBonus100}",
                    "Calibri", 11, (12.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 8,
                    $"{Convert.ToDecimal(excelValues.InsuranceAmount_LiabilityLimit) * ((100 - companyFraction.Value)/100)}",
                    "Calibri", 11, (15.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 9,
                    $"{excelValues.GrossPremium}",
                    "Calibri", 11, (10.6, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 10,
                    $"{excelValues.ReinsurerCommissionPercent}",
                    "Calibri", 11, (10.13, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 11,
                    $"{excelValues.AdministratorCommissionPercent}",
                    "Calibri", 11, (10.13, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 12,
                    $"{excelValues.ReinsurerCommission}",
                    "Calibri", 11, (10.13, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 13,
                    $"{excelValues.AdministratorCommission}",
                    "Calibri", 11, (10.6, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 14,
                    $"{excelValues.NetPremium}",
                    "Calibri", 11, (11, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 15,
                    $"{excelValues.PremiumPercent}",
                    "Calibri", 11, (11.4, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 16,
                    $"{excelValues.PaymentRate_ReturnRate}",
                    "Calibri", 11, (11.73, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 17,
                    $"{Convert.ToDecimal(excelValues.RefundPremium) * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate)}",
                    "Calibri", 11, (12.73, 28.5), applyBorders: true);
                
                previousPeriodSum += (Convert.ToDecimal(excelValues.RefundPremium) *
                                                         Convert.ToDecimal(excelValues.PaymentRate_ReturnRate));
                
                ExcelHelper.SetCellValue(sheet, rowCount, 18,
                    $"{number} / {excelValues.PaymentDate} / {companyFraction} / {excelValues.SanctionsRisk} / {excelValues.Comment}",
                    "Calibri", 11, (16.27, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 19,
                    $"{excelValues.AdministratorCommissionRub}",
                    "Calibri", 11, (10, 28.5), applyBorders: true);
                
                previousPeriodAward += Convert.ToDecimal(excelValues.AdministratorCommissionRub);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 20,
                    $"{excelValues.PaymentContract}",
                    "Calibri", 11, (10, 28.5), applyBorders: true);
                
                ExcelHelper.SetCellValue(sheet, rowCount, 21,
                    $"{excelValues.InsuranceType}",
                    "Calibri", 11, (16, 28.5), applyBorders: true);
            }
            
            rowCount++;
            number++;
        }
        
        ExcelHelper.SetCellValue(sheet, rowCount, 14,
            "Итого по договорам предыдущих отчетных периодов:",
            "Calibri", 10, (11, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount, 17,
            $"{previousPeriodSum}",
            "Calibri", 10, (12.73, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount, 19,
            $"{previousPeriodAward}",
            "Calibri", 10, (10, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 2, 14,
            "ИТОГО К ПЕРЕЧИСЛЕНИЮ:",
            "Calibri", 10, (11, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 2, 17,
            $"{reportingPeriodSum + previousPeriodSum}",
            "Calibri", 10, (10, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 2, 19,
            $"{reportingPeriodAward + previousPeriodAward}",
            "Calibri", 10, (10, 14.3), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 5, 1,
            "Перестрахователь:",
            "Calibri", 14, (25.27, 18), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 5, 8,
            "Администратор:",
            "Calibri", 14, (15.73, 18), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 7, 8,
            "Директор по перестрахованию ООО \"Индустриальный страховой брокер\"",
            "Calibri", 14, (15.73, 18), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 10, 8,
            "_____________________ А.С. Кониль",
            "Calibri", 14, (15.73, 18), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 10, 1,
            "_____________________",
            "Calibri", 14, (25.27, 18), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, rowCount + 11, 8,
            "Доверенность б/н от 03.05.2023г.",
            "Calibri", 14, (15.73, 18), applyBorders: true);
        
        // string imagePath = "~/wwwroot/Печать.png";
        //
        // byte[] bytes = File.ReadAllBytes(imagePath);
        //
        // int pictureIndex = workbook.AddPicture(bytes, PictureType.JPEG);
        //
        // IDrawing drawing = sheet.CreateDrawingPatriarch();
        // IClientAnchor anchor = drawing.CreateAnchor(0, 0, 0, 0, 8, rowCount + 5, 9, rowCount + 13);
        //
        // IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
        // picture.Resize(); 
    }
}
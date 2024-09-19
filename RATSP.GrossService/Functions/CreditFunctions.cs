using System.Globalization;
using NPOI.SS.UserModel;
using RATSP.Common.Models;
using RATSP.GrossService.Utils;
using RATSP.WebCommon.Models;

namespace RATSP.GrossService.Functions;

public static class CreditFunctions
{
    public static void DrawingTable(IWorkbook workbook, ISheet sheet, List<ExcelValues> excelValuesList,
        Company company, List<Fraction> fractions, DateOnly selectedDate)
    {
        decimal reportingPerioudGross = 0;
        decimal reportingPerioudInsurerCommision = 0;
        decimal reportingPerioudNetPremium = 0;
        decimal previousPerioudGross = 0;
        decimal previousPeriodNetPremium = 0;
        decimal sumGrossPremiumPaid = 0;
        decimal sumCommisionPaid = 0;
        decimal sumNetPremiumPaid = 0;
        decimal grossPremium = 0;
        decimal commission = 0;
        decimal netPremium = 0;
        decimal sumAdminCommissionPaid = 0;
        
        string formattedDate = selectedDate.ToDateTime(TimeOnly.MinValue)
            .ToString("dd MMMM yyyy г.", new CultureInfo("ru-RU"));
        
        var companyFraction = fractions.FirstOrDefault(f => f.CompanyId == company.Id &&
                                                            f.Start < selectedDate && f.End > selectedDate);
        
        string text = $"Кредит - Нота \u2116 38-4-2023 от {formattedDate}";
    
        ExcelHelper.SetCellValue(sheet, 0, 1,
            text, "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 1, 1,
            "к договору № Д-522111/11 от \"21\" ноября 2011 г.", "Calibri",
            11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 3, 1,
            $"{company.Name}", "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 5, 1,
            $"Период: {companyFraction.Start} - {companyFraction.End}", "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 7, 1,
            "№", "Calibri", 10, (6.87, 40.1), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 7, 2,
            "Позиция", "Calibri", 10, (43, 40.1), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 7, 3,
            "Сумма к перечислению в руб.", "Calibri", 10, (19.6, 40.1), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 8, 1,
            "1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 8, 2,
            "Начислено по доле участия в РАТСП за отчетный период",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 8, 3,
            $"", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 9, 1,
            "1,1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 9, 2,
            "Брутто-премия к начислению по доле участия в РАТСП",
            "Calibri", 10, (43, 29), applyBorders: true);

        foreach (var excelValues in excelValuesList)
        {
            try
            {
                if (DateOnly.Parse(excelValues.StartDate) >= companyFraction.Start &&
                DateOnly.Parse(excelValues.StartDate) <= companyFraction.End && excelValues.Insurer != company.Name
                && (excelValues.PaymentNumber == "1" || string.IsNullOrWhiteSpace(excelValues.PaymentNumber)))
                {
                    reportingPerioudGross += (Convert.ToDecimal(excelValues.AccruedBonus100)) * (companyFraction.Value / 100)
                                                * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                
                    reportingPerioudNetPremium += (Convert.ToDecimal(excelValues.AccruedBonus100)) * (companyFraction.Value / 100)
                        * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate)
                        * ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent))/100);
                }
                else
                {
                    bool sanctionality = false;
                    
                    string[] keywords = { "корректировка", "расторжение", "увеличение", "снятие", "снижение", "премия", "продление" };

                    if (excelValues.SanctionsRisk == "Да") sanctionality = true;
                    
                    var _companyFraction = fractions.FirstOrDefault(f => f.CompanyId == company.Id &&
                                                                         f.Start <= DateOnly.Parse(excelValues.StartDate) &&
                                                                         f.End >= DateOnly.Parse(excelValues.StartDate) &&
                                                                         f.Sanctionality == sanctionality);

                    if (keywords.Any(k => excelValues.Comment.ToLower().Contains(k)))
                    {
                        if (string.IsNullOrWhiteSpace(excelValues.PaymentSumm))
                        {
                            previousPerioudGross += (Convert.ToDecimal(excelValues.AccruedBonus100)) * (_companyFraction.Value / 100)
                                * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                    
                            previousPeriodNetPremium += (Convert.ToDecimal(excelValues.AccruedBonus100)) * (_companyFraction.Value / 100)
                                * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate)
                                * ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent))/100);
                        }
                        else
                        {
                            previousPerioudGross += Convert.ToDecimal(excelValues.PaymentSumm) * _companyFraction.Value / 100 
                                                    * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                    
                            previousPeriodNetPremium += Convert.ToDecimal(excelValues.PaymentSumm) * _companyFraction.Value / 100 
                                                        * Convert.ToDecimal(excelValues.PremiumPercent) * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                        }
                    }
                    else
                    {
                        previousPerioudGross += 0;
                        previousPeriodNetPremium += 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        reportingPerioudInsurerCommision = (reportingPerioudGross + previousPerioudGross) - (reportingPerioudNetPremium + previousPeriodNetPremium);

        ExcelHelper.SetCellValue(sheet, 9, 3,
            $"{reportingPerioudGross + previousPerioudGross}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 1,
            "1,2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 2,
            "Сумма начисленной комиссии",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 3,
            $"{reportingPerioudInsurerCommision}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 1,
            "1,3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 2,
            "Нетто-премия к начислению по доле участия в РАТСП",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 3,
            $"{reportingPerioudNetPremium + previousPeriodNetPremium}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 12, 1,
            "2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 12, 2,
            "Фактические платежи",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 12, 3,
            "", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 13, 1,
            "2,1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 13, 2,
            "Сумма оплаченной брутто-премии",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 15, 1,
            "2,3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 15, 2,
            "Сумма оплаченной нетто-премии",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        foreach (var excelValues in excelValuesList)
        {
            try
            {
                if (excelValues.Insurer != company.Name)
                {
                    bool sanctionality = false;

                    if (excelValues.SanctionsRisk == "Да") sanctionality = true;
                    
                    decimal _netPremium;
                    decimal _grossPremium;
                
                    var _companyFraction = fractions.FirstOrDefault(f => f.CompanyId == company.Id &&
                                                                         f.Start <= DateOnly.Parse(excelValues.StartDate) &&
                                                                         f.End >= DateOnly.Parse(excelValues.StartDate) &&
                                                                         f.Sanctionality == sanctionality);

                    if (excelValues.PaymentNumber == "1")
                    {
                        _netPremium = Convert.ToDecimal(excelValues.AccruedBonus100) * _companyFraction.Value / 100
                                      * ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent))/100)
                                      * Convert.ToDecimal(excelValues.PremiumPercent)
                                      * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                        
                        _grossPremium = _netPremium / ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent))/100);
                    }
                    else if (string.IsNullOrWhiteSpace(excelValues.PaymentNumber))
                    {
                        _netPremium = Convert.ToDecimal(excelValues.AccruedBonus100) * _companyFraction.Value / 100
                                      * ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent)) / 100)
                                      * Convert.ToDecimal(excelValues.PremiumPercent)
                                      * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                        
                        _grossPremium = _netPremium / ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent))/100);
                    }
                    else
                    {
                        // if (string.IsNullOrWhiteSpace(excelValues.PaymentSumm))
                        // {
                        //     _netPremium = Convert.ToDecimal(excelValues.AccruedBonus100) * _companyFraction.Value / 100
                        //                   * ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent))/100)
                        //                   * Convert.ToDecimal(excelValues.PremiumPercent)
                        //                   * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                        //
                        //     _grossPremium = _netPremium /
                        //                     ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent)) / 100);
                        // }
                        // else
                        // {
                        //     _netPremium = Convert.ToDecimal(excelValues.PaymentSumm) * _companyFraction.Value / 100 
                        //                   * Convert.ToDecimal(excelValues.PremiumPercent) * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                        //     
                        //     _grossPremium = Convert.ToDecimal(excelValues.PaymentSumm) * _companyFraction.Value / 100
                        //                     * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                        // }
                        
                        _netPremium = Convert.ToDecimal(excelValues.AccruedBonus100) * _companyFraction.Value / 100
                                      * ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent))/100)
                                      * Convert.ToDecimal(excelValues.PremiumPercent)
                                      * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
                        
                        _grossPremium = _netPremium /
                                        ((100 - Convert.ToDecimal(excelValues.ReinsurerCommissionPercent)) / 100);
                    }
                
                    if (_netPremium >= 0)
                    {
                        sumNetPremiumPaid += _netPremium;
                    }
                    else
                    {
                        netPremium += _netPremium;
                    }
                
                    if (_grossPremium >= 0)
                    {
                        sumGrossPremiumPaid += _grossPremium;
                    }
                    else
                    {
                        grossPremium += _grossPremium;
                    }   
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        ExcelHelper.SetCellValue(sheet, 13, 3,
            $"{sumGrossPremiumPaid}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 15, 3,
            $"{sumNetPremiumPaid}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 14, 1,
            "2,2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 14, 2,
            "Сумма оплаченной комиссии",
            "Calibri", 10, (43, 29), applyBorders: true);

        sumCommisionPaid = sumGrossPremiumPaid - sumNetPremiumPaid;
        
        ExcelHelper.SetCellValue(sheet, 14, 3,
            $"{sumCommisionPaid}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 16, 1,
            "3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 16, 2,
            "Возврат премии",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 16, 3,
            "", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 17, 1,
            "3,1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 17, 2,
            "Брутто-премия",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 17, 3,
            $"{grossPremium}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 18, 1,
            "3,2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 18, 2,
            "Комиссия",
            "Calibri", 10, (43, 29), applyBorders: true);

        commission = grossPremium - netPremium;
        
        ExcelHelper.SetCellValue(sheet, 18, 3,
            $"{commission}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 19, 1,
            "3,3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 19, 2,
            "Нетто-премия",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 19, 3,
            $"{netPremium}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 22, 1,
            "5,1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 22, 2,
            "Итого:",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 22, 3,
            $"{sumNetPremiumPaid + netPremium}", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 25, 1,
            "Итого к перечислению комиссионного вознаграждения администратора (рублей):",
            "Calibri", 11, (6.87, 31.1));
        
        ExcelHelper.SetCellValue(sheet, 25, 3,
            $"{sumNetPremiumPaid + netPremium}", "Calibri", 11, (19.6, 31.1));
        
        ExcelHelper.SetCellValue(sheet, 27, 1,
            "Данная сумма будет перечислена на счет Вашей компании.",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 28, 1,
            "Банковские реквизиты",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 30, 1,
            $"{company.Name}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 31, 1,
            $"{company.INN}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 32, 1,
            $"{company.KPP}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 33, 1,
            $"{company.PC}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 34, 1,
            $"{company.BankName}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 35, 1,
            $"{company.BIK}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 36, 1,
            $"{company.KC}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 40, 1,
            "Администратор:",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 40, 3,
            "Директор по перестрахованию",
            "Calibri", 11, (19.6, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 41, 3,
            "ООО \"Индустриальный страховой брокер\"",
            "Calibri", 11, (19.6, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 44, 3,
            "___________________   А.С. Кониль",
            "Calibri", 11, (19.6, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 45, 3,
            "Доверенность б/н от 03.05.2023 г.",
            "Calibri", 11, (19.6, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 49, 1,
            $"{company.Name}",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 52, 3,
            "___________________",
            "Calibri", 11, (19.6, 14.3));
        
        // string imagePath = "~/wwwroot/Печать.png";
        //
        // byte[] bytes = File.ReadAllBytes(imagePath);
        //
        // int pictureIndex = workbook.AddPicture(bytes, PictureType.JPEG);
        //
        // IDrawing drawing = sheet.CreateDrawingPatriarch();
        // IClientAnchor anchor = drawing.CreateAnchor(0, 0, 0, 0, 3, 38, 3, 47);
        //
        // IPicture picture = drawing.CreatePicture(anchor, pictureIndex);
        // picture.Resize();
    }
}
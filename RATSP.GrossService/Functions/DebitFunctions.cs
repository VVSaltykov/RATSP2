using System.Globalization;
using NPOI.SS.UserModel;
using RATSP.Common.Models;
using RATSP.GrossService.Utils;
using RATSP.WebCommon.Models;

namespace RATSP.GrossService.Functions;

public static class DebitFunctions
{
    public static void DrawingTable(IWorkbook workbook, ISheet sheet, List<ExcelValues> excelValuesList,
        Company company, List<Fraction> fractions, DateOnly selectedDate)
    {
        decimal reportingPerioudGross = 0;
        decimal reportingPerioudInsurerCommision = 0;
        decimal reportingPerioudNetPremium = 0;
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
                                                            f.Start <= selectedDate && f.End >= selectedDate);

        excelValuesList = excelValuesList.Where(c => c.Insurer == company.Name).ToList();
        
        string text = $"Дебет - Нота \u2116 38-4-2023 от {formattedDate}";
    
        ExcelHelper.SetCellValue(sheet, 0, 1,
            text, "Calibri", 11, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 1, 1,
            "к договору № Д-522111/11 от \"21\" ноября 2011 г.", "Calibri",
            11, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 3, 1,
            $"{company.Name}", "Calibri", 11, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 5, 1,
            $"Период: {companyFraction.Start} - {companyFraction.End}", "Calibri", 11, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 7, 1,
            "№", "Calibri", 10, (6.87, 40.1), applyBorders: true, isBold: true, textCenter: true);
        
        ExcelHelper.SetCellValue(sheet, 7, 2,
            "Позиция", "Calibri", 10, (43, 40.1), applyBorders: true, isBold: true, textCenter: true);
        
        ExcelHelper.SetCellValue(sheet, 7, 3,
            "Сумма к перечислению в руб.", "Calibri", 10, (19.6, 40.1), applyBorders: true,
            isBold: true, wrapText: true, textCenter: true);
        
        ExcelHelper.SetCellValue(sheet, 8, 1,
            "1,1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 8, 2,
            "Брутто начисленная премия в перестрахование за отчетный период",
            "Calibri", 10, (43, 29), applyBorders: true);

        foreach (var excelValues in excelValuesList)
        {
            if (DateOnly.Parse(excelValues.StartDate) >= companyFraction.Start && DateOnly.Parse(excelValues.StartDate) <= companyFraction.End)
            {
                reportingPerioudGross += (Convert.ToDecimal(excelValues.NetPremium) + Convert.ToDecimal(excelValues.ReinsurerCommission)) 
                                                * Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
            }
        }

        ExcelHelper.SetCellValue(sheet, 8, 3,
            reportingPerioudGross, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 9, 1,
            "1,2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 9, 2,
            "Начисленная перестраховочная комиссия",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        foreach (var excelValues in excelValuesList)
        {
            if (DateOnly.Parse(excelValues.StartDate) >= companyFraction.Start && DateOnly.Parse(excelValues.StartDate) <= companyFraction.End)
            {
                reportingPerioudInsurerCommision += Convert.ToDecimal(excelValues.ReinsurerCommission);
            }
        }
        
        ExcelHelper.SetCellValue(sheet, 9, 3,
            reportingPerioudInsurerCommision, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 1,
            "1,3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 10, 2,
            "Нетто начисленная премия в перестрахование за отчётный период",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        foreach (var excelValues in excelValuesList)
        {
            if (DateOnly.Parse(excelValues.StartDate) >= companyFraction.Start && DateOnly.Parse(excelValues.StartDate) <= companyFraction.End)
            {
                reportingPerioudNetPremium += Convert.ToDecimal(excelValues.NetPremium) *
                                         Convert.ToDecimal(excelValues.PaymentRate_ReturnRate);
            }
        }
        
        ExcelHelper.SetCellValue(sheet, 10, 3,
            reportingPerioudNetPremium, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 1,
            "2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 2,
            "Фактические платежи",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 11, 3,
            "", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 12, 1,
            "2,1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 12, 2,
            "Сумма оплаченной брутто-премии",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 14, 1,
            "2,3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 14, 2,
            "Сумма оплаченной нетто-премии",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        foreach (var excelValues in excelValuesList)
        {
            if (DateOnly.Parse(excelValues.StartDate) < companyFraction.Start)
            {
                bool sanctionality = false;

                if (excelValues.SanctionsRisk == "Да") sanctionality = true;

                decimal _netPremium;
                decimal _grossPremium;

                var _companyFraction = fractions.FirstOrDefault(f => f.CompanyId == company.Id &&
                                                                     f.Start <= DateOnly.Parse(excelValues.StartDate) &&
                                                                     f.End >= DateOnly.Parse(excelValues.StartDate) &&
                                                                     f.Sanctionality == sanctionality);
                
                if (_companyFraction != null)
                {
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
        }
        
        ExcelHelper.SetCellValue(sheet, 12, 3,
            sumGrossPremiumPaid, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 14, 3,
            sumNetPremiumPaid, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 13, 1,
            "2,2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 13, 2,
            "Сумма оплаченной комиссии",
            "Calibri", 10, (43, 29), applyBorders: true);

        sumCommisionPaid = sumGrossPremiumPaid - sumNetPremiumPaid;
        
        ExcelHelper.SetCellValue(sheet, 13, 3,
            sumCommisionPaid, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 15, 1,
            "3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 15, 2,
            "Возврат премии",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 15, 3,
            "", "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 16, 1,
            "3,1", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 16, 2,
            "Брутто-премия",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 16, 3,
            grossPremium, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 17, 1,
            "3,2", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 17, 2,
            "Комиссия",
            "Calibri", 10, (43, 29), applyBorders: true);

        commission = grossPremium - netPremium;
        
        ExcelHelper.SetCellValue(sheet, 17, 3,
            commission, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 18, 1,
            "3,3", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 18, 2,
            "Нетто-премия",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 18, 3,
            netPremium, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 19, 1,
            "4", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 19, 2,
            "Сумма оплаченного комиссионного вознаграждения администратора",
            "Calibri", 10, (43, 29), applyBorders: true);
        
        foreach (var excelValues in excelValuesList)
        {
            sumAdminCommissionPaid += Convert.ToDecimal(excelValues.AdministratorCommissionRub);
        }
        
        ExcelHelper.SetCellValue(sheet, 19, 3,
            sumAdminCommissionPaid, "Calibri", 10, (19.6, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 20, 1,
            "", "Calibri", 10, (6.87, 29), applyBorders: true);
        
        ExcelHelper.SetCellValue(sheet, 20, 2,
            "Итого нетто-премия в перестрахование:",
            "Calibri", 10, (43, 29), applyBorders: true, isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 20, 3,
            sumNetPremiumPaid, "Calibri", 10, (19.6, 29), applyBorders: true, isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 23, 2,
            "Итого к перечислению нетто-премии в перестрахование (рублей):",
            "Calibri", 11, (43, 31.1), isBold: true, wrapText: true, textTop: true);
        
        ExcelHelper.SetCellValue(sheet, 23, 3,
            sumNetPremiumPaid, "Calibri", 11, (19.6, 31.1), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 25, 2,
            "Итого к перечислению комиссионного вознаграждения администратора (рублей):",
            "Calibri", 11, (43, 31.1), isBold: true, wrapText: true, textTop: true);
        
        ExcelHelper.SetCellValue(sheet, 25, 3,
            sumAdminCommissionPaid, "Calibri", 11, (19.6, 31.1), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 27, 1,
            "Просим Вас осуществить платеж вышеуказанной суммы на счет Администратора РАТСП",
            "Calibri", 10, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 28, 1,
            $"в срок до {formattedDate}",
            "Calibri", 10, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 29, 1,
            "Общество с ограниченной ответственностью \"Индустриальный страховой брокер\"",
            "Calibri", 10, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 31, 1,
            "ИНН 7710869528",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 32, 1,
            "КПП 772701001",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 33, 1,
            "р/с  40701810938000000057",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 34, 1,
            "в ПАО \"Сбербанк России\" г. Москва ",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 35, 1,
            "БИК  044525225",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 36, 1,
            "к/с   30101810400000000225",
            "Calibri", 11, (6.87, 14.3));
        
        ExcelHelper.SetCellValue(sheet, 39, 1,
            "Администратор:",
            "Calibri", 11, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 39, 3,
            "Директор по перестрахованию",
            "Calibri", 11, (19.6, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 40, 3,
            "ООО \"Индустриальный страховой брокер\"",
            "Calibri", 11, (19.6, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 43, 3,
            "___________________   А.С. Кониль",
            "Calibri", 11, (19.6, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 44, 3,
            "Доверенность б/н от 03.05.2023 г.",
            "Calibri", 11, (19.6, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 48, 1,
            $"{company.Name}",
            "Calibri", 11, (6.87, 14.3), isBold: true);
        
        ExcelHelper.SetCellValue(sheet, 51, 3,
            "___________________",
            "Calibri", 11, (19.6, 14.3), isBold: true);
        
        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", "CAS.png");
        
        AddImageToSheet(sheet, workbook, imagePath, 37);
    }
    
    private static void AddImageToSheet(ISheet sheet, IWorkbook workbook, string imagePath, int rowCount)
    {
        // Загрузка изображения из файла
        using (var stream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
        {
            byte[] imageBytes = new byte[stream.Length];
            stream.Read(imageBytes, 0, (int)stream.Length);

            // Получение индекса для изображения
            int pictureIndex = workbook.AddPicture(imageBytes, PictureType.PNG);

            // Создание объект для добавления изображения
            IDrawing drawing = sheet.CreateDrawingPatriarch();
            IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();
        
            // Устанавливаем координаты для изображения
            anchor.Col1 = 3; // Столбец, в который будет вставлено изображение
            anchor.Row1 = rowCount; // Ряд, в котором будет вставлено изображение
            anchor.Col2 = 4; // Конечный столбец (можно изменить)
            anchor.Row2 = rowCount + 10; // Высота изображения, можно подстроить

            // Добавление изображения в ячейку
            drawing.CreatePicture(anchor, pictureIndex);
        }
    }
}
using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace RATSP.GrossService.Utils;

public static class ExcelHelper
{
    public static void SetCellValue(ISheet sheet, int rowIndex, int cellIndex, object value,
        string fontName, short fontSize, (double columnWidth, double rowHeight) dimensions, 
        (int startRow, int endRow, int startColumn, int endColumn)? mergeRegion = null, 
        bool applyBorders = false, bool isBold = false, bool wrapText = false, bool textCenter = false,
        bool textTop = false, int decimalPlaces = 2)
    {
        // Создание книги для формата .xlsx
        IWorkbook workbook = sheet.Workbook;

        // Настройка шрифта
        IFont font = workbook.CreateFont();
        font.FontHeightInPoints = fontSize; // Размер шрифта
        font.FontName = fontName; // Имя шрифта
        font.IsBold = isBold;

        // Настройка стиля ячейки
        ICellStyle cellStyle = workbook.CreateCellStyle();
        cellStyle.SetFont(font);

        // Настройка границ ячейки, если применимо
        if (applyBorders)
        {
            cellStyle.BorderTop = BorderStyle.Thin;
            cellStyle.BorderBottom = BorderStyle.Thin;
            cellStyle.BorderLeft = BorderStyle.Thin;
            cellStyle.BorderRight = BorderStyle.Thin;
        }

        if (textCenter)
        {
            cellStyle.Alignment = HorizontalAlignment.Center; // Горизонтальное выравнивание
            cellStyle.VerticalAlignment = VerticalAlignment.Center; // Вертикальное выравнивание
        }
        
        if (textTop)
        {
            cellStyle.VerticalAlignment = VerticalAlignment.Top; // Вертикальное выравнивание
        }
        
        cellStyle.WrapText = wrapText;
        
        bool isNumeric = value is double or float or decimal or int or long;
        if (isNumeric)
        {
            // Установить числовой формат с указанным количеством знаков после запятой
            IDataFormat dataFormat = workbook.CreateDataFormat();
            string format = $"0.{new string('0', decimalPlaces)}"; // Например: "0.00" для 2 знаков
            cellStyle.DataFormat = dataFormat.GetFormat(format);
        }

        // Получить или создать строку
        IRow row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);

        // Установить высоту строки (умножаем на 20 для получения точности в единицах высоты строки)
        row.Height = (short)(dimensions.rowHeight * 20);

        // Получить или создать ячейку
        ICell cell = row.GetCell(cellIndex) ?? row.CreateCell(cellIndex);

        // Установить значение ячейки
        if (isNumeric)
        {
            // Для чисел
            switch (value)
            {
                case double dValue:
                    cell.SetCellValue(dValue);
                    break;
                case float fValue:
                    cell.SetCellValue((double)fValue);
                    break;
                case decimal decValue:
                    cell.SetCellValue((double)decValue);
                    break;
                case int intValue:
                    cell.SetCellValue(intValue);
                    break;
                case long longValue:
                    cell.SetCellValue((double)longValue);
                    break;
            }
        }
        else
        {
            // Для строк или смешанных значений
            cell.SetCellValue(value?.ToString() ?? string.Empty);
        }

        // Применить стиль ячейки
        cell.CellStyle = cellStyle;

        // Установить ширину столбца (умножаем на 256 для получения точности в единицах ширины столбца)
        sheet.SetColumnWidth(cellIndex, (int)(dimensions.columnWidth * 256));

        // Объединение ячеек, если указано
        if (mergeRegion.HasValue)
        {
            CellRangeAddress region = new CellRangeAddress(
                mergeRegion.Value.startRow, mergeRegion.Value.endRow,
                mergeRegion.Value.startColumn, mergeRegion.Value.endColumn
            );
            sheet.AddMergedRegion(region);
            
            if (applyBorders)
            {
                for (int rowIdx = mergeRegion.Value.startRow; rowIdx <= mergeRegion.Value.endRow; rowIdx++)
                {
                    IRow mergedRow = sheet.GetRow(rowIdx) ?? sheet.CreateRow(rowIdx);
                    for (int colIdx = mergeRegion.Value.startColumn; colIdx <= mergeRegion.Value.endColumn; colIdx++)
                    {
                        ICell mergedCell = mergedRow.GetCell(colIdx) ?? mergedRow.CreateCell(colIdx);
                        mergedCell.CellStyle = cellStyle;
                    }
                }
            }
        }
    }
}
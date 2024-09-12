using NPOI.SS.UserModel;
using NPOI.SS.Util;

namespace RATSP.GrossService.Utils;

public static class ExcelHelper
{
    public static void SetCellValue(ISheet sheet, int rowIndex, int cellIndex, string value,
        string fontName, short fontSize, (double columnWidth, double rowHeight) dimensions, 
        (int startRow, int endRow, int startColumn, int endColumn)? mergeRegion = null, 
        bool applyBorders = false)
    {
        // Создание книги для формата .xlsx
        IWorkbook workbook = sheet.Workbook;

        // Настройка шрифта
        IFont font = workbook.CreateFont();
        font.FontHeightInPoints = fontSize; // Размер шрифта
        font.FontName = fontName; // Имя шрифта

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

        // Получить или создать строку
        IRow row = sheet.GetRow(rowIndex) ?? sheet.CreateRow(rowIndex);

        // Установить высоту строки (умножаем на 20 для получения точности в единицах высоты строки)
        row.Height = (short)(dimensions.rowHeight * 20);

        // Получить или создать ячейку
        ICell cell = row.GetCell(cellIndex) ?? row.CreateCell(cellIndex);

        // Установить значение ячейки
        cell.SetCellValue(value);

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
        }
    }
}
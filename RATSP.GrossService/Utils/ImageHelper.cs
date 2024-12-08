using ImageMagick;
using NPOI.SS.UserModel;
using SkiaSharp;

namespace RATSP.GrossService.Utils;

public static class ImageHelper
{
    public static void AddImageToSheet(ISheet sheet, IWorkbook workbook, string imagePath, int rowCount, int colCountStart, int colCountEnd)
    {
        byte[] compressedImage = CompressPngImage(imagePath); 

        int pictureIndex = workbook.AddPicture(compressedImage, PictureType.PNG);

        IDrawing drawing = sheet.CreateDrawingPatriarch();
        IClientAnchor anchor = workbook.GetCreationHelper().CreateClientAnchor();

        anchor.Col1 = colCountStart; 
        anchor.Row1 = rowCount; 
        anchor.Col2 = colCountEnd; 
        anchor.Row2 = rowCount + 10; 

        anchor.AnchorType = AnchorType.MoveAndResize;

        drawing.CreatePicture(anchor, pictureIndex);
    }

    private static byte[] CompressPngImage(string imagePath)
    {
        using (var image = new MagickImage(imagePath))
        {
            image.Strip();

            image.Quality = 1;

            using (var ms = new MemoryStream())
            {
                image.Write(ms);
                return ms.ToArray();
            }
        }
    }
}
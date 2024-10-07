namespace RATSP.Common.Models;

public class CreateExcelDocumentsRequest
{
    public List<string?> SelectedCompanies { get; set; }
    public string SelectedDate { get; set; }
    public bool GrossIn { get; set; }
    public bool GrossOut { get; set; }
    public bool Debit { get; set; }
    public bool Credit { get; set; }
    public byte[] WorkbookBytes { get; set; }
}
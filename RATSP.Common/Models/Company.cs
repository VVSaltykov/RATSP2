using RATSP.Common.Definitions.Enums;

namespace RATSP.Common.Models;

public class Company : BaseEntity<Guid>
{
    public string? Name { get; set; }
    public int? Number { get; set; }
    public Participation? Participation { get; set; }
    public string? Position { get; set; }
    public string? FIO { get; set; }
    public string? INN { get; set; }
    public string? KPP { get; set; }
    public string? PC { get; set; }
    public string? BankName { get; set; }
    public string? BIK { get; set; }
    public string? KC { get; set; }
    public string? Address { get; set; }
    public string? MailAddress { get; set; }
    
    public Fraction Fraction { get; set; }
}
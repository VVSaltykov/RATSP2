namespace RATSP.Common.Models;

public class Fraction : BaseEntity<int>
{
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    public decimal Value { get; set; }
    public bool Sanctionality { get; set; }
    
    public Guid CompanyId { get; set; }
    public Company? Company { get; set; }
}
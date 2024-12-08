namespace RATSP.GrossService.Definitions.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class InsuranceTypeAttribute : Attribute
{
    public string ShortCode { get; }
    public string Keywords { get; }

    public InsuranceTypeAttribute(string shortCode, string keywords = "")
    {
        ShortCode = shortCode;
        Keywords = keywords;
    }
}
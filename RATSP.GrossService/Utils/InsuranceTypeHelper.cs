using System.ComponentModel;
using System.Reflection;
using RATSP.GrossService.Definitions.Attributes;
using RATSP.GrossService.Definitions.Enums;

namespace RATSP.GrossService.Utils;

public static class InsuranceTypeHelper
{
    public static string GetShortType(string detailedType)
    {
        // Получаем все значения enum
        var values = Enum.GetValues(typeof(InsuranceType)).Cast<InsuranceType>();

        foreach (var value in values)
        {
            // Получаем атрибут InsuranceTypeAttribute
            var attribute = value
                .GetType()
                .GetField(value.ToString())
                ?.GetCustomAttributes(typeof(InsuranceTypeAttribute), false)
                .FirstOrDefault() as InsuranceTypeAttribute;

            // Если атрибут найден и ключевые слова соответствуют входной строке
            if (attribute != null && !string.IsNullOrEmpty(attribute.Keywords) && 
                attribute.Keywords.Contains(detailedType, StringComparison.OrdinalIgnoreCase))
            {
                return attribute.ShortCode;
            }
        }

        return null; // Если соответствие не найдено
    }
}
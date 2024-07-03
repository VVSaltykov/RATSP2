using System.ComponentModel.DataAnnotations;
using System.Reflection;
using RATSP.WebCommon.Models;

namespace RATSP.WebCommon.Utils;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum enumValue)
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()
            ?.GetName() ?? enumValue.ToString();
    }
    
    public static List<EnumDisplayItem<T>> GetFilteredEnumDisplayItems<T>(Func<T, bool> filter) where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<T>()
            .Where(filter)
            .Select(e => new EnumDisplayItem<T>
            {
                Value = e,
                DisplayName = e.GetDisplayName()
            })
            .ToList();
    }
}
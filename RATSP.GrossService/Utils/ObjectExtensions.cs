using System.Reflection;
using Newtonsoft.Json;

namespace RATSP.GrossService.Utils;

public static class ObjectExtensions
{
    public static bool ArePropertiesEqual<T>(T obj1, T obj2)
    {
        if (obj1 == null || obj2 == null) return false;

        var type = typeof(T);
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var value1 = property.GetValue(obj1);
            var value2 = property.GetValue(obj2);

            if (!Equals(value1, value2))
            {
                return false;
            }
        }

        return true;
    }
    
    public static T Clone<T>(this T source)
    {
        if (ReferenceEquals(source, null)) return default;

        var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
        return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source), deserializeSettings);
    }
}
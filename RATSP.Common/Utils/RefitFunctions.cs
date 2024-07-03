using Newtonsoft.Json;
using Refit;

namespace RATSP.Common.Utils;

public class RefitFunctions
{
    public static T GetRefitService<T>(HttpClient httpClient)
    {
        try
        {
            return RestService.For<T>(httpClient, new RefitSettings
            {
                ContentSerializer = new NewtonsoftJsonContentSerializer(new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                })
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while creating the Refit service:");
            Console.WriteLine(ex.Message);
            return default(T);
        }
    }
}
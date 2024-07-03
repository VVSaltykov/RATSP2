using RATSP.Common.Interfaces;
using RATSP.Common.Models;
using RATSP.Common.Utils;
using Refit;

namespace RATSP.Common.Services;

public class CompaniesService : ICompaniesService
{
    private readonly ICompaniesService _companiesService;

    public CompaniesService(HttpClient httpClient)
    {
        _companiesService = RefitFunctions.GetRefitService<ICompaniesService>(httpClient);
    }
    
    public async Task Create(Company company)
    {
        await _companiesService.Create(company);
    }

    public async Task<List<Company>?> Read()
    {
        try
        {
            var companies = await _companiesService.Read();
            return companies;
        }
        catch (ApiException apiEx)
        {
            // Логирование ошибки или выполнение других действий
            Console.WriteLine($"Error calling API: {apiEx.StatusCode}");
            Console.WriteLine(apiEx.Content);

            // Бросаем исключение или возвращаем null, или другую подходящую обработку
            return null;
        }
        catch (Exception ex)
        {
            // Логирование других исключений или выполнение других действий
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return null;
        }
    }
}
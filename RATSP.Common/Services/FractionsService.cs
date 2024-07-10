using RATSP.Common.Interfaces;
using RATSP.Common.Models;
using RATSP.Common.Utils;

namespace RATSP.Common.Services;

public class FractionsService : IFractionsService
{
    private readonly IFractionsService _fractionsService;
    
    public FractionsService(HttpClient httpClient)
    {
        _fractionsService = RefitFunctions.GetRefitService<IFractionsService>(httpClient);
    }

    public async Task Create(Fraction fraction)
    {
        await _fractionsService.Create(fraction);
    }

    public async Task Update(Fraction fraction)
    {
        await _fractionsService.Update(fraction);
    }

    public async Task<List<Fraction>> Read()
    {
        var fractions = await _fractionsService.Read();
        return fractions;
    }

    public async Task<List<Fraction>> ReadByCompany(Company company)
    {
        var fractions = await _fractionsService.ReadByCompany(company);
        return fractions;
    }
}
using RATSP.Common.Models;
using Refit;

namespace RATSP.Common.Interfaces;

public interface IFractionsService
{
    [Post("/api/Fraction/Create")]
    Task Create(Fraction fraction);

    [Post("/api/Fraction/Update")]
    Task Update(Fraction fraction);

    [Post("/api/Fraction/ReadAll")]
    Task<List<Fraction>> Read();
}
using RATSP.Common.Models;
using Refit;

namespace RATSP.Common.Interfaces;

public interface ICompaniesService
{
    [Post("/api/Companies/Create")]
    Task Create(Company company);

    [Post("/api/Companies/ReadAll")]
    Task<List<Company>?> Read();

    [Post("/api/Companies/Update")]
    Task Update(Company company);

    [Post("/api/Companies/ReadByName")]
    Task<Company> ReadByName(string companyName);
}
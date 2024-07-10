using Microsoft.AspNetCore.Mvc;
using RATSP.API.Repositories;
using RATSP.Common.Models;

namespace RATSP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CompaniesController : ControllerBase
{
    private readonly CompaniesRepository CompaniesRepository;

    public CompaniesController(CompaniesRepository companiesRepository)
    {
        CompaniesRepository = companiesRepository;
    }
    
    [HttpPost("Create")]
    public async Task Create(Company company)
    {
        await CompaniesRepository.Create(company);
    }
    
    [HttpPost("Update")]
    public async Task Update(Company company)
    {
        await CompaniesRepository.Update(company);
    }
    
    [HttpPost("ReadAll")]
    public async Task<List<Company>?> Read()
    {
        var companies = (await CompaniesRepository.Read()).ToList();

        return companies;
    }
    
    [HttpPost("ReadByName")]
    public async Task<Company> ReadByName(string companyName)
    {
        var company = await CompaniesRepository.ReadFirst(c => c.Name == companyName);
        return company;
    }
}
using Microsoft.AspNetCore.Mvc;
using RATSP.API.Repositories;
using RATSP.Common.Models;

namespace RATSP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FractionController : ControllerBase
{
    private readonly FractionRepository FractionRepository;

    public FractionController(FractionRepository fractionRepository)
    {
        FractionRepository = fractionRepository;
    }

    [HttpPost("Create")]
    public async Task Create(Fraction fraction)
    {
        await FractionRepository.Create(fraction);
    }

    [HttpPost("Update")]
    public async Task Update(Fraction fraction)
    {
        await FractionRepository.Update(fraction);
    }

    [HttpPost("ReadAll")]
    public async Task<List<Fraction>> Read()
    {
        var fractions = (await FractionRepository.Read(include: f => f.Company)).ToList();
        return fractions;
    }
}
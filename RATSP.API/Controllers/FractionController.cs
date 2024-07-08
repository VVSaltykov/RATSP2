using Microsoft.AspNetCore.Mvc;
using RATSP.API.Repositories;

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
}
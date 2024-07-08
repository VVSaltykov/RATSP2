using RATSP.Common.Models;

namespace RATSP.API.Repositories;

public class FractionRepository : BaseCRUDRepository<Fraction, int>
{
    private readonly AppDbContext AppDbContext;
    
    public FractionRepository(AppDbContext appDbContext) : base(dbContext: appDbContext)
    {
        AppDbContext = appDbContext;
    }
}
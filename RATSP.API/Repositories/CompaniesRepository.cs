using RATSP.Common.Models;

namespace RATSP.API.Repositories;

public class CompaniesRepository : BaseCRUDRepository<Company, Guid>
{
    private readonly AppDbContext AppDbContext;
    
    public CompaniesRepository(AppDbContext appDbContext) : base(dbContext: appDbContext)
    {
        AppDbContext = appDbContext;
    }
}
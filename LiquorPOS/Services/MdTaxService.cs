using LiquorPOS.Models;                    // Product, TaxAmount
using Microsoft.EntityFrameworkCore;

namespace LiquorPOS.Services
{
    /// <summary>
    /// Maryland implementation: 6 % general, 9 % alcohol, etc.
    /// </summary>
    public class MdTaxService : ITaxService
    {
        private readonly IDbContextFactory<LiquorDbContext> _dbFactory;

        public MdTaxService(IDbContextFactory<LiquorDbContext> dbFactory)
            => _dbFactory = dbFactory;

        public IEnumerable<TaxAmount> GetTaxBreakdown(Product prod,
                                                      int qty,
                                                      DateTime when)
        {
            decimal unitPrice = prod.Price ?? 0m;          // null → 0

            using var db = _dbFactory.CreateDbContext();   // sync; no await

            return db.ProductTaxComponents
                     .Include(l => l.TaxComponent)
                     .Where(l => l.CodeNum == prod.CodeNum)
                     .Select(l => l.TaxComponent)
                     .Select(tc =>
                         new TaxAmount(tc.Name,
                             Math.Round(unitPrice * tc.Rate * qty, 2,
                                        MidpointRounding.AwayFromZero)))
                     .ToList();
        }
    }
}

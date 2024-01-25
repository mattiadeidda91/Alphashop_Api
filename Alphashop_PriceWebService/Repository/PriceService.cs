using Alphashop_PriceWebService.Models;
using Alphashop_PriceWebService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alphashop_PriceWebService.Repository
{
    public class PriceService : IPriceService
    {
        private readonly AlphaShopDbContext alphaShopDbContext;
        public PriceService(AlphaShopDbContext alphaShopDbContext) 
        { 
            this.alphaShopDbContext = alphaShopDbContext;
        }

        public async Task<DettListino>? GetPriceByCodArt(string codArt)
        {
            return await alphaShopDbContext.DettListini
                .Where(d => d.CodArt == codArt)
                    .Include(a => a.Listino)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> InsertPrice(DettListino price)
        {
            _ = await alphaShopDbContext.AddAsync(price);
            return await Save();
        }

        public async Task<bool> UpdatePrice(DettListino price)
        {
            _ = alphaShopDbContext.Update(price);
            return await Save();
        }

        public async Task<bool> DeletePrice(DettListino price)
        {
            _ = alphaShopDbContext.Remove(price);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await alphaShopDbContext.SaveChangesAsync() >= 0;
        }
    }
}

using Alphashop_PriceWebService.Models;
using Alphashop_PriceWebService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alphashop_PriceWebService.Repository
{
    public class ListinoService : IListinoService
    {
        private readonly AlphaShopDbContext alphaShopDbContext;

        public ListinoService(AlphaShopDbContext alphaShopDbContext) 
        {
            this.alphaShopDbContext = alphaShopDbContext;
        }

        public async Task<Listino>? GetById(string id)
        {
            return await alphaShopDbContext.Listini
                .AsNoTracking()
                .Where(d => d.Id == id)
                    .Include(a => a.DettListini)
                .FirstOrDefaultAsync();
        }

        public async Task<Listino>? GetByDesc(string description)
        {
            return await alphaShopDbContext.Listini
                .AsNoTracking()
                .Where(d => d.Descrizione.Contains(description))
                    .Include(a => a.DettListini)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> Insert(Listino listino)
        {
            _ = await alphaShopDbContext.AddAsync(listino);
            return await Save();
        }

        public async Task<bool> Update(Listino listino)
        {
            _ = alphaShopDbContext.Update(listino);
            return await Save();
        }

        public async Task<bool> Delete(Listino listino)
        {
            _ = alphaShopDbContext.Remove(listino);
            return await Save();
        }

        private async Task<bool> Save()
        {
            return await alphaShopDbContext.SaveChangesAsync() >= 0;
        }
    }
}

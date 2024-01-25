using Alphashop_ArticoliWebService.Models;
using Alphashop_ArticoliWebService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Alphashop_ArticoliWebService.Repository
{
    public class ArticoliRepository : IArticoliRepository
    {
        private readonly AlphaShopDbContext alphaShopDbContext;
        /* per la paginazione lato backend ?*/
        //private const int pageSize = 10;
        //private int pageNumber = 1; //far passare dal front-end

        public ArticoliRepository(AlphaShopDbContext alphaShopDbContext) 
        { 
            this.alphaShopDbContext = alphaShopDbContext;
        }

        public async Task<IEnumerable<Articoli>>? GetArticoliByDescription(string description)
        {
            return await alphaShopDbContext.Articoli
                .Where(a => a.Descrizione!.Contains(description))
                    .Include(a => a.BarCodes)
                    .Include(a => a.FamAssort)
                    .Include(a => a.Iva)
                .OrderBy(a => a.Descrizione)
                //Gestire la paginazione
                //.Skip((pageNumber - 1) * pageSize)
                //.Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<Articoli>>? GetArticoliByDescription(string description, string idCategoria)
        {
            var isNumeric = int.TryParse(idCategoria, out var idFam);

            if(isNumeric)
            {
                return await alphaShopDbContext.Articoli
                .Where(a => a.Descrizione!.Contains(description))
                .Where(a => a.IdFamAss == idFam)
                    .Include(a => a.BarCodes)
                    .Include(a => a.FamAssort)
                    .Include(a => a.Iva)
                .OrderBy(a => a.Descrizione)
                .ToListAsync();
            }
            else
            {
                return await GetArticoliByDescription(description);
            }
        }

        public async Task<Articoli>? GetArticoliCodArt(string codArt)
        {
            return await alphaShopDbContext.Articoli
                .Where(a => a.CodArt!.Equals(codArt))
                    .Include(a => a.BarCodes)
                    .Include(a => a.FamAssort)
                    .Include(a => a.Iva)
                .FirstOrDefaultAsync();
        }

        public async Task<Articoli>? GetArticoliCodArtNoReferences(string codArt)
        {
            //Metodo da utilizzare in fase di eliminazione siccome l'altro si porta dietro anche le classi che referenzia e il DB si spacca
            return await alphaShopDbContext.Articoli
                .Where(a => a.CodArt!.Equals(codArt))
                .FirstOrDefaultAsync();
        }

        public async Task<Articoli>? GetArticoliByBarcode(string barcode)
        {
            return await alphaShopDbContext.BarCode
                .Include(b => b.Articolo.BarCodes)
                .Include(b => b.Articolo.FamAssort)
                .Include(b => b.Articolo.Iva)
                    .Where(a => a.Barcode!.Equals(barcode))
                    .Select(b => b.Articolo)
                    .FirstOrDefaultAsync();
        }    

        public async Task<bool> InsertArticolo(Articoli articolo)
        {
            _ = await alphaShopDbContext.AddAsync(articolo);
            return await Save();
        }

        public async Task<bool> UpdateArticolo(Articoli articolo)
        {
            _ = alphaShopDbContext.Update(articolo);
            return await Save();
        }

        public async Task<bool> DeleteArticolo(Articoli articolo)
        {
            _ = alphaShopDbContext.Remove(articolo);
            return await Save();
        }

        public async Task<bool> Exists(string codArt)
        {
            return await alphaShopDbContext.Articoli.AnyAsync(a => a.CodArt!.Equals(codArt));
        }

        private async Task<bool> Save()
        {
            return await alphaShopDbContext.SaveChangesAsync() >= 0;
        }

        //IVA
        public async Task<IEnumerable<Iva>> GetAllIva()
        {
            return await alphaShopDbContext.Iva.OrderBy(a => a.Aliquota).ToListAsync();
        }

        //FAMASSORT
        public async Task<IEnumerable<FamAssort>> GetAllFamAssort()
        {
            return await alphaShopDbContext.FamAssort.OrderBy(f => f.Id).ToListAsync();
        }
    }
}

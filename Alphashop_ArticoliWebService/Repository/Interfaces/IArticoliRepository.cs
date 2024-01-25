using Alphashop_ArticoliWebService.Models;

namespace Alphashop_ArticoliWebService.Repository.Interfaces
{
    public interface IArticoliRepository
    {
        //ARTICOLO
        Task<IEnumerable<Articoli>>? GetArticoliByDescription(string description);
        Task<IEnumerable<Articoli>>? GetArticoliByDescription(string description, string idCategoria);
        Task<Articoli>? GetArticoliByBarcode(string barcode);
        Task<Articoli>? GetArticoliCodArt(string codArt);
        Task<Articoli>? GetArticoliCodArtNoReferences(string codArt);
        Task<bool> InsertArticolo(Articoli articolo);
        Task<bool> UpdateArticolo(Articoli articolo);
        Task<bool> DeleteArticolo(Articoli articolo);
        Task<bool> Exists(string codArt);

        //IVA
        Task<IEnumerable<Iva>> GetAllIva();

        //FAMASSORT
        Task<IEnumerable<FamAssort>> GetAllFamAssort();
    }
}

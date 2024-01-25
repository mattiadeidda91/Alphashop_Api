using Alphashop_PriceWebService.Models;

namespace Alphashop_PriceWebService.Repository.Interfaces
{
    public interface IListinoService
    {
        Task<Listino>? GetById(string id);
        Task<Listino>? GetByDesc(string description);
        Task<bool> Insert(Listino listino);
        Task<bool> Update(Listino listino);
        Task<bool> Delete(Listino listino);
    }
}

using Alphashop_PriceWebService.Models;

namespace Alphashop_PriceWebService.Repository.Interfaces
{
    public interface IPriceService
    {
        Task<DettListino>? GetPriceByCodArt(string codArt);
        Task<bool> InsertPrice(DettListino price);
        Task<bool> UpdatePrice(DettListino price);
        Task<bool> DeletePrice(DettListino price);
    }
}

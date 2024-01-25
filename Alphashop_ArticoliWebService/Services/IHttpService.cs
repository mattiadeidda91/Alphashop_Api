namespace Alphashop_ArticoliWebService.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string token, string url) where T: class;
    }
}

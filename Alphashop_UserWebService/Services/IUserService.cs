using Alphashop_UserWebService.Models;

namespace Alphashop_UserWebService.Services
{
    public interface IUserService
    {
        Task<Utenti> GetUser(string UserId);
        Task<Utenti> GetUserToDelete(string UserId);
        Task<Utenti> GetUserByCodFid(string CodFid);
        Task<ICollection<Utenti>> GetAll();
        Task<bool> InsUtente(Utenti utente);
        Task<bool> UpdUtente(Utenti utente);
        Task<bool> DelUtente(Utenti utente);
        Task<bool> Authenticate(string username, string password);
        Task<string> GetToken(string username);
    }
}

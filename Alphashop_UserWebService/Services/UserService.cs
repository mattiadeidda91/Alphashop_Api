using Alphashop_UserWebService.Models;
using Alphashop_UserWebService.Options;
using Alphashop_UserWebService.Repository;
using Alphashop_UserWebService.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Alphashop_UserWebService.Services
{
    public class UserService : IUserService
    {
        private readonly AlphaShopDbContext alphaShopDbContext;
        private readonly JwtTokenOptions jwtTokenOptions;
        public UserService(AlphaShopDbContext alphaShopDbContext, IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            this.alphaShopDbContext = alphaShopDbContext;
            this.jwtTokenOptions = jwtTokenOptions.Value;
        }

        public async Task<Utenti> GetUser(string UserId)
        {
            return await this.alphaShopDbContext.Utenti
                .Where(c => c.UserId == UserId)
                .Include(r => r.Profili)
                .FirstOrDefaultAsync();
        }

        public async Task<Utenti> GetUserToDelete(string UserId)
        {
            return await this.alphaShopDbContext.Utenti
                //.AsNoTracking()
                .Where(c => c.UserId == UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<Utenti> GetUserByCodFid(string CodFid)
        {
            return await this.alphaShopDbContext.Utenti
                //.AsNoTracking()
                .Where(c => c.CodFidelity == CodFid)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Utenti>> GetAll()
        {
            return await this.alphaShopDbContext.Utenti
                .Include(r => r.Profili)
                .OrderBy(c => c.UserId)
                .ToListAsync();
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            bool retVal = false;

            PasswordHasher Hasher = new PasswordHasher();

            Utenti utente = await this.GetUser(username);

            if (utente != null)
            {
                string EncryptPwd = utente.Password;
                retVal = Hasher.Check(EncryptPwd, password).Verified;
            }

            return retVal;
        }

        public async Task<bool> InsUtente(Utenti utente)
        {
            this.alphaShopDbContext.Add(utente);
            return await Salva();
        }

        public async Task<bool> UpdUtente(Utenti utente)
        {
            this.alphaShopDbContext.Update(utente);
            return await Salva();
        }

        public async Task<bool> DelUtente(Utenti utente)
        {
            this.alphaShopDbContext.Remove(utente);
            return await Salva();
        }

        private async Task<bool> Salva()
        {
            var saved = await this.alphaShopDbContext.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }

        //Recupero del JWT TOKEN
        public async Task<string> GetToken(string username)
        {
            /* Creazione del JWT Token */

            //recupero l'utente dal DB
            var user = await GetUser(username);

            var tokenHandler = new JwtSecurityTokenHandler(); // creo un'istanza del JwtSecurityTokenHandler
            var key = Encoding.ASCII.GetBytes(this.jwtTokenOptions.Secret); //creo e codifico la key tramite la variabile d'ambiente Secret

            var userRoles = user.Profili; //recupero i ruoli dell'utente da DB
            var claims = new List<Claim>(); //creo una lista di claims
            claims.Add(new Claim(ClaimTypes.Name, user.UserId)); //aggiungo il name al claim

            foreach (var role in userRoles)
            {
                //aggiungo alla lista di claims tutti i ruoli dell'utente
                claims.Add(new Claim(ClaimTypes.Role, role.Tipo));
            }

            //creo token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), //passiamo i claims generati
                Expires = DateTime.UtcNow.AddSeconds(jwtTokenOptions.Expiration), //settiamo l'expiration date del token
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature) //impostiamo la key di Signature e l'algoritmo di codifica
            };

            //creazione del token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // ritorno il token generato
            return tokenHandler.WriteToken(token);
        }
    }
}

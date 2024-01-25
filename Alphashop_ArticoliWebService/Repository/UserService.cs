//NON SERVE CON L'USO DEL JWT TOKEN

//using Alphashop_ArticoliWebService.Models;
//using Alphashop_ArticoliWebService.Repository.Interfaces;
//using Alphashop_ArticoliWebService.Security;
//using Microsoft.EntityFrameworkCore;

//namespace Alphashop_ArticoliWebService.Repository
//{
//    public class UserService : IUserService
//    {
//        private readonly AlphaShopDbContext alphaShopDbContext;
//        public UserService(AlphaShopDbContext alphaShopDbContext) 
//        { 
//            this.alphaShopDbContext = alphaShopDbContext;
//        }

//        public bool Authenticate(string username, string password)
//        {
//            var res = false;

//            var hasher = new PasswordHasher();

//            var user = GetUser(username);

//            if (user != null) 
//            {
//                var encryptPsw = user.Password; //psw criptata sul db
//                res = hasher.Check(encryptPsw, password).Verified;
//            }

//            return res;
//        }

//        public Utenti GetUser(string username)
//        {
//            return alphaShopDbContext.Utenti
//                .Include(u => u.Profili)
//                .Where(u => u.UserId == username)
//                .FirstOrDefault();
//        }
//    }
//}

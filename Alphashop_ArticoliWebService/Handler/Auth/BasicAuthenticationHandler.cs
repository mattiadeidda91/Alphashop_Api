//NON SERVE CON L'USO DEL JWT TOKEN

//using Alphashop_ArticoliWebService.Models;
//using Alphashop_ArticoliWebService.Repository.Interfaces;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Options;
//using System.Net.Http.Headers;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;

//namespace Alphashop_ArticoliWebService.Handler.Auth
//{
//    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
//    {
//        private readonly IUserService userService;
//        public BasicAuthenticationHandler(
//            IOptionsMonitor<AuthenticationSchemeOptions> options,
//            ILoggerFactory logger,
//            UrlEncoder encoder,
//            ISystemClock clock,
//            IUserService userService)
//            : base(options, logger, encoder, clock) 
//        {
//            this.userService = userService;
//        }

//        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            //ANDREBBERO TRASFORMATI I METODI ASYNCRONI SUL IUserService

//            if (!Request.Headers.ContainsKey("Authorization"))
//                return AuthenticateResult.Fail("Missin Authorization header");
//            try
//            {
//                //Recupero informazioni dall'header sul valore "Authorization"
//                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
//                var credialsBytes = Convert.FromBase64String(authHeader.Parameter);
//                var credentials = Encoding.UTF8.GetString(credialsBytes).Split(':');

//                var username = credentials[0];
//                var psw = credentials[1];

//                bool isOk = userService.Authenticate(username, psw);

//                if (isOk)
//                {
//                    Utenti? user = userService.GetUser(username);
//                    var userProfiles = user.Profili;

//                    //creazione dei Claims con name and Roles
//                    var claims = new List<Claim>
//                    {
//                        new Claim(ClaimTypes.Name, user.UserId)
//                    };

//                    foreach (var profile in userProfiles)
//                    {
//                        claims.Add(new Claim(ClaimTypes.Role, profile.Tipo));
//                    }

//                    //Creazione ticket di authenticazione dell'utente
//                    var identiy = new ClaimsIdentity(claims, Scheme.Name);
//                    var principal = new ClaimsPrincipal(identiy);
//                    var ticket = new AuthenticationTicket(principal, Scheme.Name);

//                    return AuthenticateResult.Success(ticket);
//                }
//                else
//                    return AuthenticateResult.Fail("Utente o password errati!");
//            }
//            catch (Exception ex)
//            {
//                return AuthenticateResult.Fail(ex.Message);
//            }
//        }
//    }
//}

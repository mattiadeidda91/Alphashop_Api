namespace Alphashop_UserWebService.Models.Dtos
{
    public class JwtTokenDto
    {
        public string token { get; set; }

        public JwtTokenDto(string token) 
        { 
            this.token = token;
        }
    }
}

namespace Alphashop_UserWebService.Options
{
    public class JwtTokenOptions
    {
        public string Secret { get; set; }
        public int Expiration { get; set; }
    }
}

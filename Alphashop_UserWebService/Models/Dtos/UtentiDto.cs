namespace Alphashop_UserWebService.Models.Dtos
{
    public class UtentiDto
    {
        public string CodFidelity { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Abilitato { get; set; }
        public virtual List<ProfiliDto> Profili { get; set; }
    }

    public class ProfiliDto
    {
        public string Tipo { get; set; }
    }
}

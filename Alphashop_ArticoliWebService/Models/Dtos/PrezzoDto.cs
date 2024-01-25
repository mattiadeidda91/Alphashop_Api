using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models.Dtos
{
    public class PrezzoDto
    { 
        //Copiata da DettListinoDto

        public int Id { get; set; }

        [Required]
        public string CodArt { get; set; }

        [Required]
        public string IdList { get; set; }

        [Required]
        public decimal Prezzo { get; set; }

        //public ListinoDto? Listino { get; set; }
    }
}

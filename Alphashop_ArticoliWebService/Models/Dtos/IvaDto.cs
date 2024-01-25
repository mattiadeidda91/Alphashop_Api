using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models.Dtos
{
    public class IvaDto
    {
        public int? IdIva { get; set; }

        public string? Descrizione { get; set; }

        [Required]
        public short Aliquota { get; set; }
    }
}

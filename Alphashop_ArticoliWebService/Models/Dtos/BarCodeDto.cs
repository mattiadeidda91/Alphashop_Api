using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models.Dtos
{
    public class BarCodeDto
    {
        [Required]
        public string? Barcode { get; set; }
        public string? IdTipoArt { get; set; }
    }
}

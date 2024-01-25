using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models
{
    public class BarCode
    {
        [Key]
        [StringLength(13, MinimumLength=8, ErrorMessage = "Il Barcode deve avere da 8 a 13 cifre")]
        public string? Barcode { get; set; }
        
        [Required]
        public string? IdTipoArt { get; set; }

        public string? CodArt { get; set; }

        //Proprietà di collegamento classi Models
        public virtual Articoli? Articolo { get; set; }
    }
}

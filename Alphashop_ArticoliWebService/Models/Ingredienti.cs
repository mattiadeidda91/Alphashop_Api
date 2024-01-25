using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models
{
    public class Ingredienti
    {
        [Key]
        public string? CodArt { get; set; }

        public string? Info { get; set; }

        public virtual Articoli? Articolo { get; set; }
    }
}

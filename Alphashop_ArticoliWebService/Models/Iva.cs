using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models
{
    public class Iva
    {
        [Key]
        public int? IdIva { get; set; }

        public string? Descrizione { get; set; }

        [Required]
        public short Aliquota { get; set; }

        public virtual IEnumerable<Articoli>? Articoli { get; set; }
    }
}

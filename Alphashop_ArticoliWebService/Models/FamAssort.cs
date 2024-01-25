using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models
{
    public class FamAssort
    {
        [Key]
        public int? Id { get; set; }

        public string? Descrizione { get; set; }

        public virtual IEnumerable<Articoli>? Articoli { get; set; }
    }
}

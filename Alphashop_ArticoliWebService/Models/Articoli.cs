using System.ComponentModel.DataAnnotations;

namespace Alphashop_ArticoliWebService.Models
{
    public class Articoli
    {
        [Key]
        [MinLength(5, ErrorMessage = "Il numero minimo di caratteri è 5")]
        [MaxLength(30, ErrorMessage = "Il numero massimo di caratteri è 30")]
        [Required]
        public string? CodArt { get; set; }  //Chiave primaria

        [MinLength(5, ErrorMessage = "Il numero minimo di caratteri è 5")]
        [MaxLength(80, ErrorMessage = "Il numero massimo di caratteri è 80")]
        public string? Descrizione { get; set; }

        [Range(0, 100, ErrorMessage = "I pezzi per cartone devono essere compresi tra 0 e 100")]
        public short? PzCart { get; set; }

        [Range(0.01, 100, ErrorMessage = "Il peso netto deve essere compreso tra 0.01 e 100")]
        public double? PesoNetto { get; set; }

        public string? Um { get; set; }

        public string? CodStat { get; set; }

        public int? IdIva { get; set; }

        public int? IdFamAss { get; set; }

        public string? IdStatoArt { get; set; }

        public DateTime? DataCreazione { get; set; }

        /* Proprietà di collegamento classi Models */
        public virtual IEnumerable<BarCode>? BarCodes { get; set; }
        public virtual Ingredienti? Ingrediente { get; set; }

        //Solo lettura
        public virtual FamAssort? FamAssort { get; set; }
        public virtual Iva? Iva { get; set; }
    }
}

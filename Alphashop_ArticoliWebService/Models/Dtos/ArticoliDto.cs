using System.Globalization;

namespace Alphashop_ArticoliWebService.Models.Dtos
{
    public class ArticoliDto
    {
        public string? CodArt { get; set; }
        public string? Descrizione { get; set; }
        public string? Um { get; set; }
        public string? CodStat { get; set; }
        public short? PzCart { get; set; }
        public double? PesoNetto { get; set; }
        public string? IdStatoArt { get; set; }
        public DateTime? DataCreazione { get; set; }
        public IEnumerable<BarCodeDto>? BarCodes { get; set; }
        public int? IdIva { get; set; }
        public int? IdFamAss { get; set; }
        public decimal Prezzo { get; set; }

        //Usa direttamente le proprietà e non i riferimenti agli ogget
        //public IvaDto? Iva { get; set; }
        //public FamAssortDto? FamAssort { get; set; }

    }
}

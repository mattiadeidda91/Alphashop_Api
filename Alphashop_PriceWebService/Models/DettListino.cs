namespace Alphashop_PriceWebService.Models
{
    public class DettListino
    {
        public int Id { get; set; }
        public string CodArt { get; set; }
        public string IdList { get; set; }
        public decimal Prezzo { get; set; }

        public virtual Listino Listino { get; set; }
    }
}

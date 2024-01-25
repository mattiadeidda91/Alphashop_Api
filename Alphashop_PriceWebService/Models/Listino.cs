namespace Alphashop_PriceWebService.Models
{
    public class Listino
    {
        public string Id { get; set; }
        public string Descrizione { get; set; }
        public string Obsoleto { get; set; }

        public virtual IEnumerable<DettListino> DettListini { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Alphashop_PriceWebService.Models.Dtos
{
    public class ListinoDto
    {
        public string Id { get; set; }

        [Required]
        public string Descrizione { get; set; }
        public bool Obsoleto { get; set; } = false;

        public IEnumerable<DettListinoDto> DettListini { get; set; }
    }
}

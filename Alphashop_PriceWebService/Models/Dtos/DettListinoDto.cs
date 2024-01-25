using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Alphashop_PriceWebService.Models.Dtos
{
    public class DettListinoDto
    {
        public int Id { get; set; }

        [Required]
        public string CodArt { get; set; }

        [Required]
        public string IdList { get; set; }

        [Required]
        public decimal Prezzo { get; set; }

        //Aggiunto in Program.cs per non generare l'errore della ricorsività degli oggetti, altrimenti commentare questa proprietà e quindi non restituirla
        //builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        public ListinoDto? Listino { get; set; } 
    }
}

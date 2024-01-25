using Microsoft.AspNetCore.Mvc;

namespace Alphashop.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalutiController : ControllerBase
    {
        public SalutiController()
        {
        }


        [HttpGet(Name = "GetSaluti")]
        public string GetSaluti(string name) 
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new Exception("\"Error: name parameter cannot be empty or null\"");
                }
                else
                {
                    if(name == "Marco")
                        throw new Exception($"\"Error: user '{name}' cannot be access\"");
                }

                return $"\"Saluti {name}, sono la tua webApi backend C#\"";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
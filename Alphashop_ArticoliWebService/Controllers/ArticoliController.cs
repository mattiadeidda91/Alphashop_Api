using Alphashop_ArticoliWebService.Models;
using Alphashop_ArticoliWebService.Models.Dtos;
using Alphashop_ArticoliWebService.Models.Responses;
using Alphashop_ArticoliWebService.Repository.Interfaces;
using Alphashop_ArticoliWebService.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Alphashop_ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "ADMIN, USER")] //Per gestire l'autorizzazione, sono abilitati gli utenti USER e ADMIN
    public class ArticoliController : ControllerBase
    {
        private readonly IArticoliRepository articoliRepository;
        private readonly IHttpService httpService;
        private readonly IMapper mapper;
        public ArticoliController(IArticoliRepository articoliRepository, IHttpService httpService, IMapper mapper)
        {
            this.articoliRepository = articoliRepository;
            this.httpService = httpService;
            this.mapper = mapper;
        }

        [HttpGet("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMsg))]
        public async Task<ActionResult<ResponseMsg>> Authenticate()
        {
            try
            {
                return Ok(new ResponseMsg(true, "Login success", StatusCodes.Status200OK));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("getbydescription/{description}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type= typeof(IEnumerable<ArticoliDto>))]
        public async Task<ActionResult<IEnumerable<ArticoliDto>>> GetByDescription(string description, [FromQuery] string? idCategoria)
        {
            try
            {
                var articoli = await articoliRepository.GetArticoliByDescription(description, idCategoria)!;

                if (!ModelState.IsValid)
                    return BadRequest(new ResponseMsg(false, string.Join("-", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)), StatusCodes.Status400BadRequest));

                if (articoli == null || articoli?.Count() == 0)
                    return NotFound(new ResponseMsg(false, $"No results with filter: '{description}'", StatusCodes.Status404NotFound));

                var articoliDto = mapper.Map<IEnumerable<ArticoliDto>>(articoli);

                foreach(var art in articoliDto)
                {
                    var token = Request.Headers["Authorization"];
                    var priceDto = await httpService.Get<PrezzoDto>(token, $"http://localhost:5041/Price/getpricebycodart/{art.CodArt}");
                    if(priceDto != null)
                        art.Prezzo = priceDto.Prezzo;
                }    

                return Ok(articoliDto);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("getbycodart/{codArt}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticoliDto))]
        //[AllowAnonymous] //è richiamabile anche se non si è autenticati
        public async Task<ActionResult<ArticoliDto>> GetByCodArt(string codArt)
        {
            try
            {
                //Token JWT per la comunicazione con la webApi PriceWebService
                var tokenJwt = Request.Headers.ContainsKey("Authorization") ? 
                        Request.Headers["Authorization"] : 
                        throw new Exception("Non è stato possibile recuperare il token");

                if (string.IsNullOrEmpty(tokenJwt))
                    return Problem("Il token non può essere vuoto o null");

                var exists = await articoliRepository.Exists(codArt);

                if (!exists)
                    return NotFound(new ResponseMsg(false, $"Articolo with '{codArt}' not found", StatusCodes.Status404NotFound));

                var articolo = await articoliRepository.GetArticoliCodArt(codArt)!;

                if (!ModelState.IsValid)
                    return BadRequest(new ResponseMsg(false, string.Join("-", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)), StatusCodes.Status400BadRequest));

                var articoliDto = mapper.Map<ArticoliDto>(articolo);

                //Get Price
                var priceDto = await httpService.Get<PrezzoDto>(tokenJwt!, $"http://localhost:5041/Price/getpricebycodart/{codArt}");

                if(priceDto != null)
                    articoliDto.Prezzo = priceDto.Prezzo;

                return Ok(articoliDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("getbybarcode/{barCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticoliDto))]
        public async Task<ActionResult<ArticoliDto>> GetByBarcode(string barCode)
        {
            try
            {
                var articolo = await articoliRepository.GetArticoliByBarcode(barCode)!;

                if (!ModelState.IsValid)
                    return BadRequest(new ResponseMsg(false, string.Join("-", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)), StatusCodes.Status400BadRequest));

                if (articolo == null)
                    return NotFound(new ResponseMsg(false, $"No results with filter: '{barCode}'", StatusCodes.Status404NotFound));

                var articoliDto = mapper.Map<ArticoliDto>(articolo);

                //Get Price
                var priceDto = await httpService.Get<PrezzoDto>(Request.Headers["Authorization"], $"http://localhost:5041/Price/getpricebycodart/{articoliDto.CodArt}");

                if (priceDto != null)
                    articoliDto.Prezzo = priceDto.Prezzo;

                return Ok(articoliDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMsg))]
        [Authorize(Roles ="ADMIN")] //solo gli utenti ADMIN posso usare questa action
        public async Task<ActionResult<ResponseMsg>> Create([FromBody] Articoli articoloDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseMsg(false, string.Join("-", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)), StatusCodes.Status400BadRequest));

                if(articoloDto.IdIva == -1)
                    return StatusCode(400, new ResponseMsg(false, $"Aliquota Iva non selezionata!", StatusCodes.Status400BadRequest));
                if(articoloDto.Um == "-1")
                    return StatusCode(400, new ResponseMsg(false, $"Unità di misura non selezionata!", StatusCodes.Status400BadRequest));

                var exists = await articoliRepository.Exists(articoloDto.CodArt!);

                if (!exists)
                {
                    //Inserire dataAnnotation validation nel Dto e usare quello da passare come oggetto all'api
                    //var articolo = mapper.Map<Articoli>(articoloDto);

                    articoloDto.DataCreazione = DateTime.Now;
                    var result = await articoliRepository.InsertArticolo(articoloDto)!;

                    if (!result)
                    {
                        //ModelState.AddModelError("", $"Errore durante l'inserimento dell'Articolo {articoloDto.CodArt}");
                        return StatusCode(500, new ResponseMsg(false, $"Errore durante l'inserimento dell'Articolo {articoloDto.CodArt}", StatusCodes.Status500InternalServerError));
                    }
                }
                else
                {
                    //Soluzione temporanea, creare una classe di Error
                    //ModelState.AddModelError("", $"Articolo con codiceArticolo '{articoloDto.CodArt}' già presente in anagrafica! Impossibile inserire l'articolo!");
                    return StatusCode(422, new ResponseMsg(false, $"Articolo con codiceArticolo '{articoloDto.CodArt}' già presente in anagrafica! Impossibile inserire l'articolo!", StatusCodes.Status422UnprocessableEntity));
                }

                return Ok(new ResponseMsg(true, "Articolo creato correttamente", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMsg))]
        [Authorize(Roles = "ADMIN")] //solo gli utenti ADMIN posso usare questa action
        public async Task<ActionResult<bool>> Update([FromBody] Articoli articoloDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ResponseMsg(false, string.Join("-", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)), StatusCodes.Status404NotFound));

                if (articoloDto.IdIva == -1)
                    return StatusCode(400, new ResponseMsg(false, $"Aliquota Iva non selezionata!", StatusCodes.Status400BadRequest));
                if (articoloDto.Um == "-1")
                    return StatusCode(400, new ResponseMsg(false, $"Unità di misura non selezionata!", StatusCodes.Status400BadRequest));

                var exists = await articoliRepository.Exists(articoloDto.CodArt!);

                if (exists)
                {
                    //Inserire dataAnnotation validation nel Dto e usare quello da passare come oggetto all'api
                    //var articolo = mapper.Map<Articoli>(articoloDto);
                    var result = await articoliRepository.UpdateArticolo(articoloDto)!;

                    if (!result)
                    {
                        //ModelState.AddModelError("", $"Errore durante l'aggiornamento dell'Articolo {articoloDto.CodArt}");
                        return StatusCode(500, new ResponseMsg(false, $"Errore durante l'aggiornamento dell'Articolo {articoloDto.CodArt}", StatusCodes.Status500InternalServerError));
                    }
                }
                else
                {
                    //ModelState.AddModelError("", $"L'Articolo con codiceArticolo '{articoloDto.CodArt}' non è presente in anagrafica! Impossibile aggiornare l'articolo!");
                    return StatusCode(422, new ResponseMsg(false, $"L'Articolo con codiceArticolo '{articoloDto.CodArt}' non è presente in anagrafica! Impossibile aggiornare l'articolo!", StatusCodes.Status422UnprocessableEntity));
                }

                return Ok(new ResponseMsg(true, "Articolo modificato correttamente", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpDelete("delete/{codArt}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMsg))]
        [Authorize(Roles = "ADMIN")] //solo gli utenti ADMIN posso usare questa action
        public async Task<ActionResult<ResponseMsg>> Delete(string codArt)
        {
            try
            {
                if (string.IsNullOrEmpty(codArt))
                {
                    //ModelState.AddModelError("", $"CodArt cannot be null or empty");
                    return BadRequest(new ResponseMsg(false, "CodArt cannot be null or empty", StatusCodes.Status400BadRequest));
                }

                var exists = await articoliRepository.Exists(codArt);

                if (exists)
                {
                    //Inserire dataAnnotation validation nel Dto e usare quello da passare come oggetto all'api
                    //var articolo = mapper.Map<Articoli>(articoloDto);

                    var articolo = await articoliRepository.GetArticoliCodArtNoReferences(codArt)!;

                    if (articolo == null)
                    {
                        //ModelState.AddModelError("", $"Errore durante il recupero dell'Articolo con codice '{codArt}'");
                        return StatusCode(500, new ResponseMsg(false, $"Errore durante il recupero dell'Articolo con codice '{codArt}'", StatusCodes.Status500InternalServerError));
                    }
                    else
                    {
                        var result = await articoliRepository.DeleteArticolo(articolo)!;

                        if (!result)
                        {
                            //ModelState.AddModelError("", $"Errore durante l'eliminazione dell'Articolo {codArt}");
                            return StatusCode(500, new ResponseMsg(false, $"Errore durante l'eliminazione dell'Articolo {codArt}", StatusCodes.Status500InternalServerError));
                        }
                    }
                }
                else
                {
                    //Soluzione temporanea, creare una classe di Error
                    //ModelState.AddModelError("", $"L'Articolo con codiceArticolo '{codArt}' non è presente in anagrafica! Impossibile eliminare l'articolo!");
                    return StatusCode(422, new ResponseMsg(false, $"L'Articolo con codiceArticolo '{codArt}' non è presente in anagrafica! Impossibile eliminare l'articolo!", StatusCodes.Status422UnprocessableEntity));
                }

                return Ok(new ResponseMsg(true, "Articolo eliminato correttamente", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }

        [HttpGet("exists/{codArt}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseMsg))]
        public async Task<ActionResult<ResponseMsg>> Exists(string codArt)
        {
            try
            {
                var articolo = await articoliRepository.Exists(codArt)!;

                return Ok(new ResponseMsg(true, articolo.ToString(), 200));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseMsg(false, ex.Message, StatusCodes.Status500InternalServerError));
            }
        }
    }
}
using Alphashop_PriceWebService.Models;
using Alphashop_PriceWebService.Models.Dtos;
using Alphashop_PriceWebService.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alphashop_PriceWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "ADMIN, USER")]
    public class PriceController : ControllerBase
    {
        private readonly IPriceService priceService;
        private readonly IMapper mapper;

        public PriceController(IPriceService priceService, IMapper mapper)
        {
            this.priceService = priceService;
            this.mapper = mapper;
        }

        [HttpGet("getpricebycodart/{codart}")]
        public async Task<ActionResult<DettListinoDto>> GetByCodArt(string codart)
        {
            try
            {
                if (string.IsNullOrEmpty(codart))
                    return BadRequest("Il codice articolo non può essere vuoto o null");

                var price = await priceService.GetPriceByCodArt(codart)!;

                if (price == null)
                    return NotFound($"Non è stato trovato un prezzo per l'Articolo con codice {codart}");

                return Ok(mapper.Map<DettListinoDto>(price));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<bool>> Create(DettListinoDto priceDto)
        {
            try
            {
                var price = mapper.Map<DettListino>(priceDto);

                var res = await priceService.InsertPrice(price)!;

                if (!res)
                    return Problem($"Errore durante l'inserimento dei nuovo prezzo!");

                return Ok(res);
            }
            catch (Exception ex) 
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<bool>> Update(DettListinoDto priceDto)
        {
            try
            {
                var price = mapper.Map<DettListino>(priceDto);

                var res = await priceService.UpdatePrice(price)!;

                if (!res)
                    return Problem($"Errore durante l'aggiornamento del prezzo!");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("delete/{codart}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<bool>> Delete(string codart)
        {
            try
            {
                if (string.IsNullOrEmpty(codart))
                    return BadRequest("Il codice articolo non può essere vuoto o null");

                var price = await priceService.GetPriceByCodArt(codart)!;

                if (price == null)
                    return NotFound($"Non è stato trovato un prezzo per l'Articolo con codice {codart}");

                var res = await priceService.DeletePrice(price)!;

                if (!res)
                    return Problem($"Errore durante l'eliminazione del prezzo!");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
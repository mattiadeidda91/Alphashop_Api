using Alphashop_PriceWebService.Models.Dtos;
using Alphashop_PriceWebService.Models;
using Alphashop_PriceWebService.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Alphashop_PriceWebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "ADMIN, USER")]
    public class ListinoController : ControllerBase
    {
        private readonly IListinoService listinoService;
        private readonly IMapper mapper;

        public ListinoController(IListinoService priceService, IMapper mapper)
        {
            this.listinoService = priceService;
            this.mapper = mapper;
        }

        [HttpGet("getbyid/{id}")]
        public async Task<ActionResult<ListinoDto>> GetById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Id Listono non può essere vuoto o null");

                var listino = await listinoService.GetById(id)!;

                if (listino == null)
                    return NotFound($"Non è stato trovato un Listino con id {id}");

                var dto = mapper.Map<ListinoDto>(listino);

                return Ok(dto);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("getbydescription/{desc}")]
        public async Task<ActionResult<ListinoDto>> GetByDescription(string desc)
        {
            try
            {
                if (string.IsNullOrEmpty(desc))
                    return BadRequest("La descrizione non può essere vuota o null");

                var listino = await listinoService.GetByDesc(desc)!;

                if (listino == null)
                    return NotFound($"Non è stato trovato un Listino con descrizione {desc}");

                return Ok(mapper.Map<ListinoDto>(listino));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<bool>> Create(ListinoDto listinoDto)
        {
            try
            {
                var listino = mapper.Map<Listino>(listinoDto);

                var res = await listinoService.Insert(listino)!;

                if (!res)
                    return Problem($"Errore durante l'inserimento del nuovo listino!");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("update")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<bool>> Update(ListinoDto listinoDto)
        {
            try
            {
                var listino = mapper.Map<Listino>(listinoDto);

                var res = await listinoService.Update(listino)!;

                if (!res)
                    return Problem($"Errore durante l'aggiornamento del listino!");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Id Listono non può essere vuoto o null");

                var listino = await listinoService.GetById(id)!;

                if (listino == null)
                    return NotFound($"Non è stato trovato un Listino con id {id}");

                var res = await listinoService.Delete(listino)!;

                if (!res)
                    return Problem($"Errore durante l'eliminazione del listino!");

                return Ok(res);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
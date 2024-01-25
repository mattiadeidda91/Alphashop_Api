using Alphashop_ArticoliWebService.Models.Dtos;
using Alphashop_ArticoliWebService.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Alphashop_ArticoliWebService.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class FamAssortController : ControllerBase
    {
        private readonly IArticoliRepository articoliRepository;
        private readonly IMapper mapper;

        public FamAssortController(IArticoliRepository articoliRepository, IMapper mapper)
        {
            this.articoliRepository = articoliRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<FamAssortDto>))]
        public async Task<ActionResult<IEnumerable<ArticoliDto>>> GetAll()
        {
            try
            {
                var famAssort = await articoliRepository.GetAllFamAssort()!;

                var famAssortDto = mapper.Map<IEnumerable<FamAssortDto>>(famAssort);

                return Ok(famAssortDto);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}

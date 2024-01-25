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
    public class IvaController : ControllerBase
    {
        private readonly IArticoliRepository articoliRepository;
        private readonly IMapper mapper;

        public IvaController(IArticoliRepository articoliRepository, IMapper mapper)
        {
            this.articoliRepository = articoliRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IvaDto>))]
        public async Task<ActionResult<IEnumerable<IvaDto>>> GetAll()
        {
            try
            {
                var ivas = await articoliRepository.GetAllIva()!;

                var iavsDto = mapper.Map<IEnumerable<IvaDto>>(ivas);

                return Ok(iavsDto);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message, null, StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
    }
}

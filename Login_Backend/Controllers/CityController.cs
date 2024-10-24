using App.Core.App.City.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Login_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CityController(IMediator IMediator)
        {
            _mediator = IMediator;
        }

        [HttpGet("[action]/{stateId}")]
        public async Task<IActionResult> GetAllCitiesByStateId(int stateId)
        {
            return Ok(await _mediator.Send(new GetAllCityByStateIdQuery { StateId = stateId }));
        }
    }
}

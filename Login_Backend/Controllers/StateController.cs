using App.Core.App.State;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Login_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IMediator _mediator;
        public StateController(IMediator IMediator)
        {
            _mediator = IMediator;
        }

        [HttpGet("[action]/{countryId}")]
        public async Task<IActionResult> GetAllStateByCountryId(int countryId)
        {
            return Ok(await _mediator.Send(new GetStateByCountryIdQuery { CountryId = countryId }));
        }
    }
}

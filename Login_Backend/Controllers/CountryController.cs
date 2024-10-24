using App.Core.App.Country.Query;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Login_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CountryController(IMediator IMediator)
        {
            _mediator = IMediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllCountries()
        {
            return Ok(await _mediator.Send(new GetAllCountryQuery()));
        }
    }
}

using App.Core.App.Patient.Command;
using App.Core.Models.Patient;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Login_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PatientController(IMediator IMediator)
        {
            _mediator = IMediator;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreatePatient(CreatePatientDto createPatientDto)
        {
            return Ok(await _mediator.Send(new CreatePatientCommand { CreatePatientDto = createPatientDto }));
        }
    }
}

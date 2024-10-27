using App.Core.App.Patient.Command;
using App.Core.App.Patient.Query;
using App.Core.Models.Patient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Login_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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

        [HttpGet("[action]/agentId/{agentId}/pageSize/{pageSize}/pageNumber/{pageNum}")]
        public async Task<IActionResult> GetPatientByAgentIdInPage(int agentId, int pageSize, int pageNum)
        {
            return Ok(await _mediator.Send(new GetAllPatientByAgentQuery { UserId = agentId, PageNumber = pageNum, PageSize = pageSize }));
        }


        [HttpDelete("[action]/patientId/{patientId}")]
        public async Task<IActionResult> DeletePatientById(string patientId)
        {
            return Ok(await _mediator.Send(new DeletePatientCommand { PatientId = patientId }));
        }

        [HttpGet("[action]/patientId/{patientId}")]
        public async Task<IActionResult> GetPatientById(string patientId)
        {
            return Ok(await _mediator.Send(new GetPatientByIdQuery { PatientId = patientId }));
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePatient(PatientDto patient)
        {
            return Ok(await _mediator.Send(new UpdatePatientCommand { Patient = patient }));
        }
    }
}

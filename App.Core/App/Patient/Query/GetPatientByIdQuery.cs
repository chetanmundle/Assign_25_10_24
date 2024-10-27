using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.Patient;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.Patient.Query
{
    public class GetPatientByIdQuery : IRequest<ResponseDto>
    {
        public string PatientId { get; set; }
    }

    internal class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GetPatientByIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(GetPatientByIdQuery request, CancellationToken cancellationToken)
        {
            var patientId = request.PatientId ?? throw new BadRequest("Id is Null");

            var patient = await _appDbContext.Set<Domain.Entities.Patient>()
                               .FirstOrDefaultAsync(p => p.PatientId == patientId, cancellationToken: cancellationToken) ??
                               throw new NotFoundException("Patient with this Id Not Found");

            return new ResponseDto
            {
                Status = 200,
                Message = "Successfully Fetched",
                Data = patient.Adapt<PatientDto>()
            };
        }
    }
}

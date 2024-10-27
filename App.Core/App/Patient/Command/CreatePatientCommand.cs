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

namespace App.Core.App.Patient.Command
{
    public class CreatePatientCommand : IRequest<ResponseDto>
    {
        public CreatePatientDto CreatePatientDto { get; set; }
    }

    public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public CreatePatientCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(CreatePatientCommand request, CancellationToken cancellationToken)
        {
            var patientDto = request.CreatePatientDto ??
                throw new BadRequest("Pationt Dto is Null");

            var validator = new CreatePatientDtoValidator();
            var val = validator.Validate(patientDto);

            if(!val.IsValid)
            {
                var errorMessage = val.Errors[0].ErrorMessage;
                throw new BadRequest(errorMessage);
            }

            var user = await _appDbContext.Set<Domain.Entities.User>()
                             .FirstOrDefaultAsync(u => u.UserId == patientDto.CreatedBy,
                                                       cancellationToken: cancellationToken) ??
                             throw new NotFoundException("User Not Found for UserId");

            var userName = user.UserName;
            string beforeAt = userName.Split('@')[0];

            user.PatientCreated = user.PatientCreated + 1;

            var patient = patientDto.Adapt<Domain.Entities.Patient>();

            // Creating Primary key
            //PadLEft is the method which insert the 0 to left until the length not full i.e. 5
            patient.PatientId = user.AgentId + user.PatientCreated.ToString().PadLeft(5, '0');
            patient.IsDeleted = false;


            await _appDbContext.Set<Domain.Entities.Patient>()
                  .AddAsync(patient, cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            var result = patient.Adapt<PatientDto>();

            return new ResponseDto
            {
                Status = 200,
                Message = "Patient Created Successfully",
                Data = result
            }; 
        }
    }
}

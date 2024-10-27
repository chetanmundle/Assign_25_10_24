using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.Patient;
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
    public class UpdatePatientCommand : IRequest<ResponseDto>
    {
        public PatientDto Patient { get; set; }
    }

    internal class UpdatePatientCommandHadler : IRequestHandler<UpdatePatientCommand, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public UpdatePatientCommandHadler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
        {
            var patientDto = request.Patient ?? throw new BadRequest("Body is null");

            var patient = await _appDbContext.Set<Domain.Entities.Patient>()
                                .FirstOrDefaultAsync(p => p.PatientId == patientDto.PatientId, cancellationToken: cancellationToken)
                                ?? throw new NotFoundException("Patient Not Found For this Id");
        
            patient.FirstName = patientDto.FirstName;
            patient.LastName = patientDto.LastName;
            patient.DateOfBirth = patientDto.DateOfBirth;
            patient.Gender = patientDto.Gender;
            patient.ContactNumber = patientDto.ContactNumber;   
            patient.Email = patientDto.Email;
            patient.Address = patientDto.Address;
            patient.BloodGroup = patientDto.BloodGroup;
            patient.CountryId = patientDto.CountryId;
            patient.StateId = patientDto.StateId;
            patient.CityId = patientDto.CityId;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new ResponseDto
            {
                Status = 200,
                Message = "Patient Updated Successfully"
            };

        
        
        }
    }
}

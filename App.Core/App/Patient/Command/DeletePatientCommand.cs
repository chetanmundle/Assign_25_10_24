using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
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
    public class DeletePatientCommand : IRequest<ResponseDto>
    {
        public string PatientId {  get; set; }
    }

    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public DeletePatientCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(DeletePatientCommand request, CancellationToken cancellationToken)
        {
            var patientId = request.PatientId;

            var patient = await _appDbContext.Set<Domain.Entities.Patient>()
                                .FirstOrDefaultAsync(p =>  p.PatientId == patientId,
                                                     cancellationToken: cancellationToken) ??
                                 throw new NotFoundException("Patient With this id Not Found");

            patient.IsDeleted = true;
            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new ResponseDto
            {
                Status = 200,
                Message = "Patient Deleted Successfully"
            };
        }
    }
}

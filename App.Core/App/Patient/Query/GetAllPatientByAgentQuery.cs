using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using Domain.Entities;
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
    public class GetAllPatientByAgentQuery : IRequest<ResponseDto>
    {
        public int UserId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    internal class GetAllPatientByAgentQueryHandler : IRequestHandler<GetAllPatientByAgentQuery, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllPatientByAgentQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(GetAllPatientByAgentQuery request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var PageNumber = request.PageNumber;
            var PageSize = request.PageSize;

            var listOfPatients = await (from p in _appDbContext.Set<Domain.Entities.Patient>()
                                        join country in _appDbContext.Set<Domain.Entities.Country>()
                                        on p.CountryId equals country.CountryId
                                        join state in _appDbContext.Set<Domain.Entities.State>()
                                        on p.StateId equals state.StateId
                                        join city in _appDbContext.Set<Domain.Entities.City>()
                                        on p.CityId equals city.CityId
                                        where p.IsDeleted == false && p.CreatedBy == userId
                                        select new
                                        {
                                            p.PatientId,
                                            p.FirstName,
                                            p.DateOfBirth,
                                            p.Email,
                                            country.CountryName,
                                            state.StateName,
                                            city.CityName,
                                        })
                                        .Skip((PageNumber - 1) * PageSize)
                                      .Take(PageSize)
                                      .ToListAsync(cancellationToken: cancellationToken);

          
            if (listOfPatients.Count <= 0) throw new NotFoundException("Not Patient Found");

            return new ResponseDto
            {
                Status = 200,
                Message = "Fetch Succesfully",
                Data = listOfPatients
            };


        }
    } 

    
}

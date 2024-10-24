using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.City;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.City.Query
{
    public class GetAllCityByStateIdQuery : IRequest<ResponseDto>
    {
        public int StateId { get; set; }
    }
    internal class GetAllCityByStateIdQueryHandler : IRequestHandler<GetAllCityByStateIdQuery, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllCityByStateIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ResponseDto> Handle(GetAllCityByStateIdQuery request, CancellationToken cancellationToken)
        {
            var stateId = request.StateId;

            var cityList = await _appDbContext.Set<Domain.Entities.City>()
                            .Where(c => c.StateId == stateId)
                            .ToListAsync(cancellationToken);

            if(cityList.Count <= 0)
            {
                //var sList = await _appDbContext.Set<Domain.Entities.State>()
                //            .Where(c => c.StateId == stateId)
                //            .ToListAsync(cancellationToken);

                throw new NotFoundException("No City Found");
            }

            return new ResponseDto
            {
                Status = 200,
                Message="City Fetch Successfully",
                Data = cityList.Adapt<List<CityDto>>()
            };


        }
    }

}

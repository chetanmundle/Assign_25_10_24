using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.Country;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.Country.Query
{
    public class GetAllCountryQuery : IRequest<ResponseDto>
    {
    }

    internal class GetAllCountryQueryHandler : IRequestHandler<GetAllCountryQuery, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GetAllCountryQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(GetAllCountryQuery request, CancellationToken cancellationToken)
        {
            var countries = await _appDbContext.Set<Domain.Entities.Country>()
                                  .AsNoTracking()
                                  .ToListAsync(cancellationToken) ;

            if(countries.Count <= 0 ) throw new NotFoundException("No Country Found");

            return new ResponseDto
            {
                Status = 200,
                Message = "Country Fetch Successfully",
                Data = countries.Adapt<List<CountryDto>>()
            };
        }
    }
}

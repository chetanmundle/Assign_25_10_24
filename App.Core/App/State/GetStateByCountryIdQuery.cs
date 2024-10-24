using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.State;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.State
{
    public class GetStateByCountryIdQuery : IRequest<ResponseDto>
    {
        public int CountryId { get; set; }
    }
    internal class GetStateByCountryIdQueryHandler : IRequestHandler<GetStateByCountryIdQuery, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public GetStateByCountryIdQueryHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(GetStateByCountryIdQuery request, CancellationToken cancellationToken)
        {
            var countryId = request.CountryId;

            var stateList = await _appDbContext.Set<Domain.Entities.State>()
                                  .Where(s => s.CountryId == countryId)
                                  .ToListAsync(cancellationToken);

            if (stateList.Count <= 0) throw new NotFoundException("No State Found");

            return new ResponseDto
            {
                Status = 200,
                Message = "Data Get Successfully",
                Data = stateList.Adapt<List<StateDto>>(),
            };
        }
    }
}

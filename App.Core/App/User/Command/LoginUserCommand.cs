using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.User;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace App.Core.App.User.Command
{
    public class LoginUserCommand : IRequest<ResponseDto>
    {
        public LoginDto LoginDto { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public LoginUserCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
           var LoginDtoModel = request.LoginDto ?? throw new BadRequest("Null Object");

            var user = await _appDbContext.Set<Domain.Entities.User>()
                             .FirstOrDefaultAsync(u => u.UserName == LoginDtoModel.UserName &&
                                                       u.Password == LoginDtoModel.Password,
                                                       cancellationToken: cancellationToken)
                             ?? throw new NotFoundException("User With this Credentials Not Found");

            //if(user == null)
            //{
            //    return new ResponseDto
            //    {
            //        Status = 404,
            //        Message = "User Not Found"
            //    };
            //}

            return new ResponseDto
            {
                Status = 200,
                Message = "User Successfully Logged In",
                Data = user.Adapt<UserWithoutPassDto>(),
            };

        }
    }
}

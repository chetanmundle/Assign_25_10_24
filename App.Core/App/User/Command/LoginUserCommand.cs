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
    public class LoginUserCommand : IRequest<UserLoginResponse>
    {
        public LoginDto LoginDto { get; set; }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserLoginResponse>
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IJwtService _jwtService;
        private readonly IEncryptionService _encryptionService;

        public LoginUserCommandHandler(IAppDbContext appDbContext, IJwtService jwtService,IEncryptionService encryptionService)
        {
            _appDbContext = appDbContext;
            _jwtService = jwtService;
            _encryptionService = encryptionService;
        }

        public async Task<UserLoginResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
           var LoginDtoModel = request.LoginDto ?? throw new BadRequest("Null Object");

            var user = await _appDbContext.Set<Domain.Entities.User>()
                             .FirstOrDefaultAsync(u => u.UserName == LoginDtoModel.UserName ,
                                                       cancellationToken: cancellationToken)
                             ?? throw new NotFoundException("UserId is  Not Valid");

            if (!string.Equals(LoginDtoModel.Password, _encryptionService.DecryptData(user.Password)))
                throw new NotFoundException("Password is Wrong");

            var accessToken = await _jwtService.Authenticate(user.UserId, user.UserName);


            return new UserLoginResponse
            {
                Status = 200,
                Message = "User Login Successfully",
                access_token = accessToken,
                Data = user.Adapt<UserWithoutPassDto>(),
            };

        }
    }
}

using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.User;
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
    public class ForgetPasswordUserCommand : IRequest<ResponseDto>
    {
        public ForgetPasswordDto ForgetPasswordDto { get; set; }
    }

    public class ForgetPasswordUserCommandHandler : IRequestHandler<ForgetPasswordUserCommand, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public ForgetPasswordUserCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(ForgetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            var forgetPassDto = request.ForgetPasswordDto ??
                throw new BadRequest("Forgetpassword Dto is null");

            if(!string.Equals( forgetPassDto.NewPassword, forgetPassDto.ConfirmNewPassword))
                throw new BadRequest("New Password and ConfirmNewPassword is Not Match");

            var user = await _appDbContext.Set<Domain.Entities.User>()
                             .FirstOrDefaultAsync(u => u.UserName == forgetPassDto.UserName &&
                                                       u.Password == forgetPassDto.OldPassword,
                                                      cancellationToken: cancellationToken) ??
                             throw new NotFoundException("User With this UserName and Password not Exist");

            if(string.Equals(forgetPassDto.NewPassword,user.Password) ||
                string.Equals(forgetPassDto.NewPassword, user.LastFirstPass) ||
                string.Equals(forgetPassDto.NewPassword, user.LastSecondPass) ||
                string.Equals(forgetPassDto.NewPassword, user.LastThirdPass))
            {
                throw new BadRequest("New Password is Same with Current or Last three Passwords!");
            }

            user.LastThirdPass = user.LastSecondPass;
            user.LastSecondPass = user.LastFirstPass;
            user.LastFirstPass = user.Password;
            user.Password = forgetPassDto.NewPassword;

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new ResponseDto
            {
                Status = 200,
                Message = "Password Updated Successfully",
            };
        }
    }
}

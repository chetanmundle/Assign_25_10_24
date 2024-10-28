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
        private readonly IEncryptionService _encryptionService;

        public ForgetPasswordUserCommandHandler(IAppDbContext appDbContext, IEncryptionService encryptionService)
        {
            _appDbContext = appDbContext;
            _encryptionService = encryptionService;
        }

        public async Task<ResponseDto> Handle(ForgetPasswordUserCommand request, CancellationToken cancellationToken)
        {
            var forgetPassDto = request.ForgetPasswordDto ??
                throw new BadRequest("Forgetpassword Dto is null");
            //Console.WriteLine($"Dto pass : {forgetPassDto.NewPassword}");

            if(!string.Equals( forgetPassDto.NewPassword, forgetPassDto.ConfirmNewPassword))
                throw new BadRequest("New Password and ConfirmNewPassword is Not Match");

            var user = await _appDbContext.Set<Domain.Entities.User>()
                             .FirstOrDefaultAsync(u => u.UserName == forgetPassDto.UserName,
                                                      cancellationToken: cancellationToken) ??
                             throw new NotFoundException("User With this UserName not Exist");

            //Console.WriteLine($"DB PAss : {_encryptionService.DecryptData(user.Password)}");
            if (!string.Equals(forgetPassDto.OldPassword, _encryptionService.DecryptData(user.Password)))
                throw new NotFoundException("Wrong Password");




            if(string.Equals(forgetPassDto.NewPassword, _encryptionService.DecryptData(user.Password)) ||
                string.Equals(forgetPassDto.NewPassword, user.LastFirstPass != null ? _encryptionService.DecryptData(user.LastFirstPass) : "") ||
                string.Equals(forgetPassDto.NewPassword, user.LastSecondPass != null ? _encryptionService.DecryptData(user.LastSecondPass) : "") ||
                string.Equals(forgetPassDto.NewPassword, user.LastThirdPass != null ? _encryptionService.DecryptData(user.LastThirdPass) : ""))
            {
                throw new BadRequest("New Password is Same with Current or Last three Passwords!");
            }

            user.LastThirdPass = user.LastSecondPass;
            user.LastSecondPass = user.LastFirstPass;
            user.LastFirstPass = user.Password;
            user.Password = _encryptionService.EncryptData(forgetPassDto.NewPassword);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new ResponseDto
            {
                Status = 200,
                Message = "Password Updated Successfully",
            };
        }
    }
}

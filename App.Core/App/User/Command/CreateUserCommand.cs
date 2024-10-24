using App.Core.Common;
using App.Core.Common.Exceptions;
using App.Core.Interfaces;
using App.Core.Models.User;
using Domain.Entities;
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
    public class CreateUserCommand : IRequest<ResponseDto>
    {
        public UserDto UserDto { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseDto>
    {
        private readonly IAppDbContext _appDbContext;

        public CreateUserCommandHandler(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var userModel = request.UserDto ?? throw new BadRequest("User Model is Null");

            var validator = new UserDtoValidator();

            var val = validator.Validate(userModel);

            if (!val.IsValid)
            {
                var errorMessage = val.Errors[0].ErrorMessage;
                throw new BadRequest(errorMessage);
            }

            var isExist = await _appDbContext.Set<Domain.Entities.User>()
                                .AnyAsync(u => u.UserName == userModel.UserName, cancellationToken: cancellationToken);

            if(isExist) throw new ConflictException("User Already Exist");

            var user = userModel.Adapt<Domain.Entities.User>();
            user.PatientCreated = 0;

            await _appDbContext.Set<Domain.Entities.User>().AddAsync(user, cancellationToken);

            await _appDbContext.SaveChangesAsync(cancellationToken);

            return new ResponseDto
            {
                Status = 200,
                Message = "User Created Successfully",
                Data = user.Adapt<UserWithoutPassDto>(),
            };
        }
    }
}

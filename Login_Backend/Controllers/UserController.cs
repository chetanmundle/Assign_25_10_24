using App.Core.App.User.Command;
using App.Core.Models.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Login_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator IMediator)
        {
            _mediator = IMediator;
        }

        // Api for register user
        [HttpPost("[action]")]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            return Ok(await _mediator.Send(new CreateUserCommand { UserDto = userDto }));
        }

        // Api for login
        [HttpPost("[action]")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            return Ok(await _mediator.Send(new LoginUserCommand { LoginDto = loginDto }));
        }

        // Api for Forgot password
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdatePassword(ForgetPasswordDto passwordDto)
        {
            return Ok(await _mediator.Send(
                new ForgetPasswordUserCommand { ForgetPasswordDto = passwordDto }));
        }
    }
}

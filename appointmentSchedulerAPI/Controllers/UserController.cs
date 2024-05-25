using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appointmentSchedulerAPI.Contracts;
using appointmentSchedulerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace appointmentSchedulerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            var isAuthenticatedUser = await _userService.AuthenticateAsync(userObj);
            if (!isAuthenticatedUser)
            {
                return NotFound(new { Message = "User not found!" });
            }

            return Ok(new { Message = "Login success!" });
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegisterAsync([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            await _userService.UserRegisterAsync(userObj);
            return Ok(new { Message = "User registerd!" });
        }
    }
}
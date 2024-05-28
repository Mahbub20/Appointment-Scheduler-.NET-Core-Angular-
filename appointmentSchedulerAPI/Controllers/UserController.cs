using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appointmentSchedulerAPI.Contracts;
using appointmentSchedulerAPI.Models;
using Microsoft.AspNetCore.Authorization;
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
            var authenticationResponse = await _userService.AuthenticateAsync(userObj);
            if (!authenticationResponse.isSuccess)
            {
                return BadRequest(new { Message = authenticationResponse.message });
            }

            return Ok(new { Token = authenticationResponse.token, Message = authenticationResponse.message });
        }

        [HttpPost("register")]
        public async Task<IActionResult> UserRegisterAsync([FromBody] User userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            var registerResponse = await _userService.UserRegisterAsync(userObj);
            if(!registerResponse.isSuccess){
                return BadRequest(new { Message = registerResponse.message });
            }
            else
            return Ok(new { Message = registerResponse.message });
        }

        [Authorize]
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }
    }
}
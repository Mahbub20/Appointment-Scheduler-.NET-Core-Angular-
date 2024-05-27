using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appointmentSchedulerAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace appointmentSchedulerAPI.Contracts
{
    public interface IUserService
    {
        Task<dynamic> AuthenticateAsync([FromBody] User userObj);
        Task<dynamic> UserRegisterAsync([FromBody] User userObj);
    }
}
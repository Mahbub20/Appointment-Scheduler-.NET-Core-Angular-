using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using appointmentSchedulerAPI.Contexts;
using appointmentSchedulerAPI.Contracts;
using appointmentSchedulerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace appointmentSchedulerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appContext;

        public UserService(AppDbContext appContext)
        {
            _appContext = appContext;
        }

        public async Task<bool> AuthenticateAsync([FromBody] User userObj)
        {
            try
            {
                var user = await _appContext.Users.FirstOrDefaultAsync(x => x.UserName == userObj.UserName && x.Password == userObj.Password);
                if (user == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task UserRegisterAsync([FromBody] User userObj)
        {
            try
            {
                await _appContext.Users.AddAsync(userObj);
                await _appContext.SaveChangesAsync();
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using appointmentSchedulerAPI.Contexts;
using appointmentSchedulerAPI.Contracts;
using appointmentSchedulerAPI.Helpers;
using appointmentSchedulerAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;


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

        public async Task<dynamic> UserRegisterAsync([FromBody] User userObj)
        {
            try
            {
                var errorMsg = "";
                //check user name is exist
                if (await CheckUserNameExistAsync(userObj.UserName))
                {
                    errorMsg = "User name is already exist!";
                    return new {isSuccess = false, message = errorMsg};
                }
                //check email is exist
                if (await CheckEmailExistAsync(userObj.Email))
                {
                    errorMsg = "Email is already exist!";
                    return new {isSuccess = false, message = errorMsg};
                }
                //check password strength
                errorMsg = CheckPasswordStrength(userObj.Password);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return new {isSuccess = false, message = errorMsg};
                }
                else
                {
                    userObj.Password = PasswordHasher.HashPassword(userObj.Password);
                    userObj.Role = "User";
                    userObj.Token = "";
                    await _appContext.Users.AddAsync(userObj);
                    await _appContext.SaveChangesAsync();
                    return new {isSuccess = true, message = "User registered!"};
                }

            }
            catch (System.Exception)
            {

                throw;
            }
        }

        private async Task<bool> CheckUserNameExistAsync(string userName)
        {
            return await _appContext.Users.AnyAsync(x => x.UserName == userName);
        }

        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _appContext.Users.AnyAsync(x => x.Email == email);
        }

        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
            {
                sb.Append("Minimum password length should be 8!" + Environment.NewLine);
            }

            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                sb.Append("Password should be alpha numeric!" + Environment.NewLine);
            }

            if (!Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
            {
                sb.Append("Password should have special characters!" + Environment.NewLine);
            }
            return sb.ToString();
        }
    }


}
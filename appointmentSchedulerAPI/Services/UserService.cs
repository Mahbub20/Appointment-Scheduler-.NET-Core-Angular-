using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using appointmentSchedulerAPI.Contexts;
using appointmentSchedulerAPI.Contracts;
using appointmentSchedulerAPI.Helpers;
using appointmentSchedulerAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<dynamic> AuthenticateAsync([FromBody] User userObj)
        {
            try
            {
                var user = await _appContext.Users.FirstOrDefaultAsync(x => x.UserName == userObj.UserName);
                if (user == null)
                {
                    return new { isSuccess = false, message = "User not found!" };
                }
                else
                {
                    if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
                    {
                        return new { isSuccess = false, message = "Password is incorrect!" };
                    }
                    else
                    {
                        var jwtToken = CreateJwtToken(user);
                        return new { isSuccess = true, token = jwtToken, message = "Login success!" };
                    }
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
                    return new { isSuccess = false, message = errorMsg };
                }
                //check email is exist
                if (await CheckEmailExistAsync(userObj.Email))
                {
                    errorMsg = "Email is already exist!";
                    return new { isSuccess = false, message = errorMsg };
                }
                //check password strength
                errorMsg = CheckPasswordStrength(userObj.Password);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    return new { isSuccess = false, message = errorMsg };
                }
                else
                {
                    userObj.Password = PasswordHasher.HashPassword(userObj.Password);
                    userObj.Role = "User";
                    userObj.Token = "";
                    await _appContext.Users.AddAsync(userObj);
                    await _appContext.SaveChangesAsync();
                    return new { isSuccess = true, message = "User registered!" };
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

        private string CreateJwtToken(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }


}
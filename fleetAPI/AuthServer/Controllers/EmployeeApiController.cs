using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;

using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using AuthServer.Models;
using AuthServer.Areas.Identity.Data;

namespace SecurityWeb.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class EmployeeApiController : ControllerBase
    {
        private readonly SignInManager<AuthServerUser> _signInManager;
        private readonly UserManager<AuthServerUser> _userManager;
        private readonly AuthServerContext _context;

        public EmployeeApiController(AuthServerContext context, SignInManager<AuthServerUser> signInManager, UserManager<AuthServerUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> tryreq()
        {
            return Ok("{ \"Error\": \"get request\"}");
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authenticate([FromBody] LoginModel.InputModel loginModel)
        {
            var result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: true);
            // return null if user not found
            if (result.Succeeded)
            {
                // authentication successful so generate jwt token
                AuthServerUser usr = await _userManager.FindByNameAsync(loginModel.Email);
                dynamic userDetail = await GetUserInfo(usr.Id.ToString());
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                byte[] key = System.Text.Encoding.ASCII.GetBytes("thisisrandomstring");
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usr.Id.ToString()),
                        new Claim(ClaimTypes.GivenName, userDetail.employee.FirstName),
                        new Claim(ClaimTypes.Role, userDetail.employee.Role),
                        new Claim(ClaimTypes.PrimarySid, usr.carrierID+""),
                    }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                

                
                return Ok("{ \"token\": \""+ tokenHandler.WriteToken(token).ToString() + "\"}");
            }
            if (result.IsLockedOut)
            {
                return Ok("{ \"Error\": \"higher the number of attempts\"}");
            }
            else
            {
                return Ok("{ \"Error\": \"unable to login\"}");
            }

            
        }

        [HttpPost("GetUserInfo")]
        public async Task<object> GetUserInfo([FromHeader] string userid)
        {
            var employee = await _context.EmployeeInfo.FirstOrDefaultAsync(x => x.UserId == userid);
            var user = _userManager.FindByIdAsync(userid).Result;
            employee.Email = user.Email;
            return new { employee, user };
        }



        [HttpPost("PostUserInfo")]
        public async Task<IActionResult> PostUserInfo([FromBody] EmployeeInfo employeeInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = await _context.EmployeeInfo.FirstOrDefaultAsync(x => x.UserId == employeeInfo.UserId);
            employee.FirstName = employeeInfo.FirstName;
            employee.LastName = employeeInfo.LastName;
            employee.Address = employeeInfo.Address;
            employee.Role = employeeInfo.Role;
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                int count = await _context.SaveChangesAsync();
                if (count > 0)
                {
                    return Ok("{ \"Success\": \"true\"}");
                }
                else
                {
                    return Ok("{ \"Success\": \"false\"}");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (employee == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
        }
    }
}
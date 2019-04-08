//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;


//namespace SecurityWeb.Services
//{
//    public interface IUserService
//    {
//        Task<IActionResult> Authenticate(string username, string password, out User);
        
//    }

//    public class UserService:IUserService
//    {
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly UserManager<IdentityUser> _userManager;
//        private Task<IdentityUser> GetCurrentUserAsync()
//        {
//            return _userManager.GetUserAsync(User);
//        }

//        public async Task<IActionResult> Authenticate(string username, string password)
//        {
//            var result = await _signInManager.PasswordSignInAsync(username, password, true, lockoutOnFailure: true);
//            IdentityUser usr = await GetCurrentUserAsync();
//            // return null if user not found
//            if (result.Succeeded)
//            {
//                // authentication successful so generate jwt token
//                var tokenHandler = new JwtSecurityTokenHandler();
//                var key = System.Text.Encoding.ASCII.GetBytes(_appSettings.Secret);
//                var tokenDescriptor = new SecurityTokenDescriptor
//                {
//                    Subject = new ClaimsIdentity(new Claim[]
//                    {
//                    new Claim(ClaimTypes.Name, "")
//                    }),
//                    Expires = DateTime.UtcNow.AddDays(7),
//                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//                };
//                var token = tokenHandler.CreateToken(tokenDescriptor);
//                user.Token = tokenHandler.WriteToken(token);

//                // remove password before returning
//                User.Password = null;
//            }
//            if (result.IsLockedOut)
//            {
                
//            }
//            else
//            {
                
//            }
//        }
//    }
//}

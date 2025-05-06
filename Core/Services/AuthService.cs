using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager,IOptions<JwtOptions> options) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
           var user= await userManager.FindByEmailAsync(loginDto.Email);
           if (user is null) throw new UnAuthorizedException();

           var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
           if(!flag) throw new UnAuthorizedException();

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await GenerateTokenAsync(user)
            };

        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user=new AppUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber
            };

            var result= await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                throw new ValidationException(errors);
            }

            return new UserResultDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateTokenAsync(user)
            };
        }

        private async Task<string> GenerateTokenAsync(AppUser user)
        {
            var jwtOptions=options.Value;
            var authClaims =new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Email,user.Email),
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
                signingCredentials: new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.BusinessLayer.Services
{
    internal class JwtService : IJwtService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;
        protected readonly UserManager<BaseUser> userManager;

        public JwtService(IUnitOfWork unitOfWork, IConfiguration configuration, UserManager<BaseUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.configuration = configuration;
            this.userManager = userManager;
        }

        public async Task<LoginResponseDTO> Authenticate(LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null)
                throw new Exception("Invalid Login");
            if (string.IsNullOrWhiteSpace(loginRequestDTO.Username) || (string.IsNullOrWhiteSpace(loginRequestDTO.Password)))
                throw new Exception("Invalid Login");
            BaseUser? user = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user == null)
                throw new Exception("User email doesnt exist");
            var valid = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!valid)
                throw new Exception("Incorrect password");
            var issuer = configuration["JwtConfig:Issuer"];
            var audience = configuration["JwtConfig:Audience"];
            var key = configuration["JwtConfig:Key"];
            int tokenValidityMins = configuration.GetValue<int>("JwtConfig:TokenValidityInMinutes");
            var expiryTime = DateTime.UtcNow.AddMinutes(tokenValidityMins);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new System.Security.Claims.Claim(ClaimTypes.Email, loginRequestDTO.Username),
                    new System.Security.Claims.Claim("Id", user.Id.ToString()),
                    //new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName)

                }),
                NotBefore = DateTime.UtcNow.AddSeconds(-1), // Start 1 second in the past (optional, but safer)

                Expires = expiryTime,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return new LoginResponseDTO
            {
                Username = user.UserName,
                AccessToken = accessToken,
                Expiration = expiryTime
            };
        }
    }
}
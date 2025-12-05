using Bookify.BusinessLayer.Contracts;
using Bookify.BusinessLayer.DTOs.BaseUserDTOs;
using Bookify.DAL.Entities;
using Bookify.DAL.Repositories;
using Bookify.DAL.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
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
                ExceptionFactory.LoginFailedException();
            if (string.IsNullOrWhiteSpace(loginRequestDTO.Username) || (string.IsNullOrWhiteSpace(loginRequestDTO.Password)))
                ExceptionFactory.LoginFailedException();
            BaseUser? user = await userManager.FindByEmailAsync(loginRequestDTO.Username);
            if (user == null)
                ExceptionFactory.IncorrectEmailException();
            var valid = await userManager.CheckPasswordAsync(user, loginRequestDTO.Password);
            if (!valid)
                ExceptionFactory.UnauthorizedException("Invalid credentials, please try again.");

            var issuer = configuration["JwtConfig:Issuer"];
            var audience = configuration["JwtConfig:Audience"];
            var key = configuration["JwtConfig:Key"];
            int tokenValidityMins = configuration.GetValue<int>("JwtConfig:TokenValidityInMinutes");
            var expiryTime = DateTime.UtcNow.AddMinutes(tokenValidityMins);

            var roles = await userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id)
            };

            // add each role claim from Identity
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow.AddSeconds(-1), // Start 1 second in the past (optional, but safer)

                Expires = expiryTime,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JsonWebTokenHandler();
            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            return new LoginResponseDTO
            {
                Username = user.UserName,
                AccessToken = accessToken,
                Expiration = expiryTime,
                Role= roles.FirstOrDefault() ?? string.Empty
            };
        }
    }
}
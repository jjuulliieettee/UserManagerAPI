using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserManagerAPI.Configs;
using UserManagerAPI.Dtos;
using UserManagerAPI.Exceptions;
using UserManagerAPI.Models;
using UserManagerAPI.Repositories.Interfaces;
using UserManagerAPI.Services.Interfaces;

namespace UserManagerAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly AuthConfigsManager _authConfigsManager;
        public AuthService( IUserRepository userRepository, AuthConfigsManager authConfigsManager )
        {
            _userRepository = userRepository;
            _authConfigsManager = authConfigsManager;
        }

        public LoginResponseDto Login( string username, string password )
        {
            ClaimsIdentity identity = GetIdentity( username, password );
            
            DateTime now = DateTime.UtcNow;

            JwtSecurityToken jwt = new JwtSecurityToken(
                    issuer: _authConfigsManager.GetIssuer(),
                    audience: _authConfigsManager.GetAudience(),
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add( TimeSpan.FromMinutes( _authConfigsManager.GetLifetime() ) ),
                    signingCredentials: new SigningCredentials( _authConfigsManager.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256 ) );
            string encodedJwt = new JwtSecurityTokenHandler().WriteToken( jwt );

            return new LoginResponseDto
            {
                AccessToken = encodedJwt,
                Username = identity.Name
            };
        }

        private ClaimsIdentity GetIdentity( string username, string password )
        {
            User user = _userRepository.GetByUsername( username );
            if (user != null)
            {
                if (PasswordService.VerifyHashedPassword( user.Password, password ))
                {
                    List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username)
                };
                    ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity( claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType );
                    return claimsIdentity;
                }
            }
            throw new AuthException();
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UserManagerAPI.Configs
{
    public class AuthConfigsManager
    {
        private readonly IConfiguration _configuration;

        public AuthConfigsManager( IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public string GetIssuer()
        {
            return _configuration.GetSection( "Jwt" ).GetValue<string>( "issuer" );
        }

        public string GetAudience()
        {
            return _configuration.GetSection( "Jwt" ).GetValue<string>( "audience" );
        }

        public string GetKey()
        {
            return _configuration.GetSection( "Jwt" ).GetValue<string>( "key" );
        }

        public int GetLifetime()
        {
            return _configuration.GetSection( "Jwt" ).GetValue<int>( "lifetime" );
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey( Encoding.ASCII.GetBytes( GetKey() ) );
        }
    }
}

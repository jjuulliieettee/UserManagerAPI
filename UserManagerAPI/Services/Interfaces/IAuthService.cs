using UserManagerAPI.Dtos;

namespace UserManagerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        LoginResponseDto Login( string username, string password );
    }
}

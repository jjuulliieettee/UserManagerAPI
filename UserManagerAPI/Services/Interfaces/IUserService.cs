using System.Collections.Generic;
using UserManagerAPI.Dtos;

namespace UserManagerAPI.Services.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserReadDto> GetAll();
        UserReadDto GetById( int id );
        UserReadDto Create( UserCreateDto userDto );
        UserReadDto Update( UserUpdateDto userDto );
        void Delete( int id );
    }
}

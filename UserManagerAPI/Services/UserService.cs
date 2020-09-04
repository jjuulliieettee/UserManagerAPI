using AutoMapper;
using System.Collections.Generic;
using UserManagerAPI.Dtos;
using UserManagerAPI.Exceptions;
using UserManagerAPI.Models;
using UserManagerAPI.Repositories.Interfaces;
using UserManagerAPI.Services.Interfaces;

namespace UserManagerAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper )
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public UserReadDto Create( UserCreateDto userDto )
        {
            if(_userRepository.GetByUsername(userDto.Username) != null)
            {
                throw new UsernameExistsException();
            }
            userDto.Password = PasswordService.HashPassword( userDto.Password );
            User user = _mapper.Map<User>( userDto );

            return _mapper.Map<UserReadDto>( _userRepository.Create( user ) );
        }

        public void Delete( int id )
        {
            User user = _userRepository.GetById( id );
            if (user == null)
            {
                throw new UserNotFoundException();
            }
            _userRepository.Delete( user );
        }

        public IEnumerable<UserReadDto> GetAll()
        {
            return _mapper.Map<IEnumerable<UserReadDto>>( _userRepository.GetAll() );
        }

        public UserReadDto GetById( int id )
        {
            User user = _userRepository.GetById( id );
            if(user == null)
            {
                throw new UserNotFoundException();
            }
            return _mapper.Map<UserReadDto>( user );
        }

        public UserReadDto Update( UserUpdateDto userDto )
        {
            User user = _userRepository.GetById( userDto.Id );
            if (user == null)
            {
                throw new UserNotFoundException();
            }

            User userInDb = _userRepository.GetByUsername( userDto.Username );
            if (userInDb != null && userInDb.Id != userDto.Id)
            {
                throw new UsernameExistsException();
            }
                        
            if(userDto.Password != null)
            {
                userDto.Password = PasswordService.HashPassword( userDto.Password );
            }

            user = _mapper.Map<User>( userDto );
            return _mapper.Map<UserReadDto>( _userRepository.Update( user ) );
        }
    }
}

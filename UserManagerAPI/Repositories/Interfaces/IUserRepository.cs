using System.Collections.Generic;
using UserManagerAPI.Models;

namespace UserManagerAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {        
        IEnumerable<User> GetAll();
        User GetById( int id );
        User GetByUsername( string username );
        User Create( User user );
        User Update( User user );
        void Delete( User user );
    }
}

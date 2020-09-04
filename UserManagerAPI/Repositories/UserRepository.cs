using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using UserManagerAPI.Data;
using UserManagerAPI.Models;
using UserManagerAPI.Repositories.Interfaces;

namespace UserManagerAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _context;
        public UserRepository( UserContext context )
        {
            _context = context;
        }

        public User Create( User user )
        {
            _context.Users.Add( user );
            _context.SaveChanges();

            return user;
        }

        public void Delete( User user )
        {
            _context.Users.Remove( user );
            _context.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById( int id )
        {
            return _context.Users.AsNoTracking().FirstOrDefault( us => us.Id == id );
        }

        public User GetByUsername( string username )
        {
            return _context.Users.AsNoTracking().FirstOrDefault( us => us.Username == username );
        }

        public User Update( User user )
        {
            _context.Entry( user ).State = EntityState.Modified;
            if(user.Password == null)
            {
                _context.Entry( user ).Property( u => u.Password ).IsModified = false;
            }
            _context.SaveChanges();

            return user;
        }
    }
}

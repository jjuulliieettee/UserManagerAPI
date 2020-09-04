using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagerAPI.Exceptions
{
    public class AuthException : ApiException
    {
        public AuthException() : base("Invalid credentials!", 400 ) { }
    }
}

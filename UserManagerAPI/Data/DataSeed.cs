using System.Collections.Generic;
using System.Linq;
using UserManagerAPI.Models;
using UserManagerAPI.Services;

namespace UserManagerAPI.Data
{
    public class DataSeed
    {
        public static void SeedUsers( UserContext context )
        {
            if (!context.Users.Any())
            {
                User user1 =
                new User
                {
                    Username = "petya",
                    Name = "Petro Boyko",
                    Password = PasswordService.HashPassword( "qwe123**" )
                };
                User user2 =
                new User
                {
                    Username = "vasya",
                    Name = "Vasyl Kit",
                    Password = PasswordService.HashPassword( "qwe123**" )
                };
                User user3 =
                new User
                {
                    Username = "batwoman",
                    Name = "Kate Cane",
                    Password = PasswordService.HashPassword( "qwe123**" )
                };

                context.AddRange( new List<User>() { user1, user2, user3 } );

                context.SaveChanges();
            }
        }
    }
}

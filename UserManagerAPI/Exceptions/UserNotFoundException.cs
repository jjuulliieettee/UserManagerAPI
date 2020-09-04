namespace UserManagerAPI.Exceptions
{
    public class UserNotFoundException : ApiException
    {
        public UserNotFoundException() : base ("User not found!" ) { }
    }
}

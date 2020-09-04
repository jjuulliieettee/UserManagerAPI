namespace UserManagerAPI.Exceptions
{
    public class UsernameExistsException : ApiException
    {
        public UsernameExistsException() : base("This username already exists!", 400 ) { }
    }
}

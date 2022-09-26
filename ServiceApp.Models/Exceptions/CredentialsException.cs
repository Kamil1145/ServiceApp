namespace ServiceApp.API.Exceptions
{
    public class CredentialsException : Exception
    {
        public override string Message { get; }

        public CredentialsException()
        {
            
        }
        public CredentialsException(string message)
        {
            Message = message;
        }

    }
}

namespace ServiceApp.API.Exceptions
{
    public class JiraException : Exception
    {
        public override string Message { get; }

        public JiraException()
        {
            
        }
        public JiraException(string message)
        {
            Message = message;
        }

    }
}

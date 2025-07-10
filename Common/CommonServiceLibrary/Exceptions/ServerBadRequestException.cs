namespace CommonServiceLibrary.Exceptions
{
    public class ServerBadRequestException : Exception
    {
        public string Details { get; set; }
        public ServerBadRequestException(string request, string details) : base(request)
        {
            Details = details;
        }
    }
}

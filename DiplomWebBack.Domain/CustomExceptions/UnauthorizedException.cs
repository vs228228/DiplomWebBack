namespace DiplomWebBack.Domain.CustomExceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException() : base() { }

        public UnauthorizedException(string message) : base(message) { }
    }
}

namespace DiplomWebBack.Domain.CustomExceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message)
            : base(message) { }

        public NotFoundException() 
            : base() { }
    }

}

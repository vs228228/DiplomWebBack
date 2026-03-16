using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.CustomExceptions
{
    public class BadRequestException : ApplicationException
    {
        public string[]? Arguments { get; }

        public BadRequestException()
            : base() { }

        public BadRequestException(string message)
            : base(message) { }

        public BadRequestException(string message, params string[] arguments)
            : this(message)
        {
            Arguments = arguments;
        }
    }
}

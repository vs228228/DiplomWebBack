using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.CustomExceptions
{
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException() : base() { }

        public UnauthorizedException(string message) : base(message) { }
    }
}

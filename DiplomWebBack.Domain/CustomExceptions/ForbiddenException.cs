using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.CustomExceptions
{
    public sealed class ForbiddenException : ApplicationException
    {
        public ForbiddenException()
            : base() { }

        public ForbiddenException(string message)
            : base(message) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.DTOs.Tags.Requests
{
    public sealed record LoginRequestDto(string Login, string Password);
}

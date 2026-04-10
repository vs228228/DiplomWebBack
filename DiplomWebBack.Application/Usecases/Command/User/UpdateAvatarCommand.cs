using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Application.Usecases.Command.User
{
    public sealed record UploadAvatarCommand(Guid UserId, IFormFile File) : IRequest<Unit>;
}

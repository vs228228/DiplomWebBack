using DiplomWebBack.Domain.Enums;

namespace DiplomWebBack.Application.DTOs.User.Request
{
    public class UserProjectShortRequest
    {
        public Guid Id { get; set; }
        public ProjectRole Role { get; set; }
    }
}

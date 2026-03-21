using DiplomWebBack.Domain.Enums;

namespace DiplomWebBack.Application.DTOs.User.Request
{
    public record UserProjectCreateRequestDto(Guid Id, ProjectRole Role);
}

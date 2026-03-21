using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Domain.Entities.m2m;
using Mapster;

namespace DiplomWebBack.Application.MapRules
{
    public class UserProjectResponseDtoRules
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserToProject, UserProjectResponseDto>()
                .Map(dest => dest.ProjectRole, src => src.Role)
                .Map(dest => dest.JoinedAt, src => src.JoinedAt)
                .Map(dest => dest.Avatar, src => src.User.Avatar)
                .Map(dest => dest.Email, src => src.User.Email)
                .Map(dest => dest.Name, src => src.User.Name)
                .Map(dest => dest.Patronymic, src => src.User.Patronymic)
                .Map(dest => dest.Surname, src => src.User.Surname)
                .Map(dest => dest.Position, src => src.User.Position);
        }
    }
}

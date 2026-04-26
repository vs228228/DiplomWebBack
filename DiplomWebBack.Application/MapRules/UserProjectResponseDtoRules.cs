using DiplomWebBack.Application.DTOs.Project;
using DiplomWebBack.Application.DTOs.Project.Response;
using DiplomWebBack.Application.DTOs.User.Response;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.m2m;
using Mapster;

namespace DiplomWebBack.Application.MapRules
{
    public class UserProjectResponseDtoRules : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserToProject, UserProjectResponseDto>()
                .Map(dest => dest.Id, src => src.UserId)
                .Map(dest => dest.ProjectRole, src => src.Role)
                .Map(dest => dest.JoinedAt, src => src.JoinedAt)
                .Map(dest => dest.Avatar, src => src.User.Avatar)
                .Map(dest => dest.Email, src => src.User.Email)
                .Map(dest => dest.Name, src => src.User.Name)
                .Map(dest => dest.Patronymic, src => src.User.Patronymic)
                .Map(dest => dest.Surname, src => src.User.Surname)
                .Map(dest => dest.Position, src => src.User.Position);

            config.NewConfig<TagToProject, Tag>()
                .Map(dest => dest.Id, src => src.TagId)
                .Map(dest => dest.Title, src => src.Tag.Title);

            config.NewConfig<TagToProject, ProjectTagResponseDto>()
                .Map(dest => dest.Id, src => src.TagId)
                .Map(dest => dest.Title, src => src.Tag.Title)
                .Map(dest => dest.Weight, src => src.Weight)
                .Map(dest => dest.Year, src => src.Year);

            config.NewConfig<Project, ProjectResponseDto>()
                .Map(dest => dest.CreatedBy, src => src.CreatedBy)
                .Map(dest => dest.Users, src => src.UserToProjects)
                .Map(dest => dest.Tags, src => src.ProjectTags);
        }


    }
}

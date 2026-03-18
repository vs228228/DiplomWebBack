using DiplomWebBack.Application.DTOs.UserActivator.Response;
using DiplomWebBack.Domain.Entities;
using Mapster;
using System.Xml.Serialization;

namespace DiplomWebBack.Application.MapRules
{
    public class UserActivatorRule
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UserActivator, UserActivatorResponse>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.UserId, src => src.User.Id)
                .Map(dest => dest.UserName, src => src.User != null ? src.User.Name : null)
                .Map(dest => dest.UserSurname, src => src.User != null ? src.User.Surname : null)
                .Map(dest => dest.UserPatronymic, src => src.User != null ? src.User.Patronymic : null)
                .Map(dest => dest.UserEmail, src => src.User != null ? src.User.Email : null);
        }
    }
}

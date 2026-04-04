using DiplomWebBack.Domain.Entities.m2m;
using DiplomWebBack.Domain.Enums;

namespace DiplomWebBack.Application.DTOs.User.Request
{
    public sealed record UpdateUserRequestDto(string Name, string Surname, string? Patronymic, string PhoneNumber, string Position);
}
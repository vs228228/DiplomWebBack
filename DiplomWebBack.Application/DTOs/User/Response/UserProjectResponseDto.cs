using DiplomWebBack.Domain.Enums;

namespace DiplomWebBack.Application.DTOs.User.Response
{
    public class UserProjectResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic {  get; set; }
        public string Email { get; set; }
        public ProjectRole ProjectRole { get; set; }
        public string Position { get; set; }
        public string Avatar {  get; set; }
        public DateTimeOffset JoinedAt { get; set; }
    }
}

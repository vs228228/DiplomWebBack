using DiplomWebBack.Domain.Enums;

namespace DiplomWebBack.Application.DTOs.User.Response
{
    public record class UserProfileDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public UserRole Role { get; set; }
        public string? Avatar { get; set; }
    }
}

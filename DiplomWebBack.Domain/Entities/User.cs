using DiplomWebBack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Domain.Entities
{
    /// <summary>
    /// Ещё нет миграции и юзер не робит, к тому же ещё нет в датасетах и нет репозитория
    /// Добавится ещё компания(сущность) + проект(сущность)
    /// Аватарку храним на сервере. Возможно стоит рассмотреть вариант хранить её как сжатый вариант в бд, но тут уже хз хз
    /// </summary>
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Patronymic { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string Position { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;
        public string? RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiryTime { get; set; }
        public UserRole Role { get; set; } = UserRole.Employee;
        public string? Avatar { get; set; }
    }
}

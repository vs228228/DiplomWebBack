using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Infrastructure.Services;

namespace DiplomWebBack.Infrastructure.Context;
public static class SeedData
    {
    public static async Task SeedDataAsync(AppDbContext context)
    {
        await SeedTagsAsync(context);
        await SeedUsersAsync(context);
    }

    private static async Task SeedTagsAsync(AppDbContext context)
    {
        if (context.Tags.Any())
            return;

        var tags = new List<Tag>
        {
            new Tag { Id = Guid.NewGuid(), Title = "Backend" },
            new Tag { Id = Guid.NewGuid(), Title = "Frontend" },
            new Tag { Id = Guid.NewGuid(), Title = "Database" },
            new Tag { Id = Guid.NewGuid(), Title = "DevOps" },
            new Tag { Id = Guid.NewGuid(), Title = "IvanaBanana" }
        };

        await context.Tags.AddRangeAsync(tags);
        await context.SaveChangesAsync();
    }

    private static async Task SeedUsersAsync(AppDbContext context)
    {
        if (context.User.Any()) return;

        var users = new List<User>
        {
            new User  { Id = Guid.NewGuid(), Avatar = null, CreatedAt = DateTime.UtcNow, Email = "admin@mail.com", Name = "Admin", Surname = "Admin", IsActive = true, IsDelete = false,
            Patronymic = "Admin", PhoneNumber = "+375123456789", Position = "Admin", Role = Domain.Enums.UserRole.Admin, PasswordHash = new PasswordHasher().HashPassword("Admin")},
            new User  { Id = Guid.NewGuid(), Avatar = null, CreatedAt = DateTime.UtcNow, Email = "neadmin@mail.com", Name = "neAdmin", Surname = "neAdmin",  IsActive = true, IsDelete = false,
            Patronymic = "neAdmin", PhoneNumber = "+375123456744", Position = "neAdmin", Role = Domain.Enums.UserRole.Employee, PasswordHash = new PasswordHasher().HashPassword("neAdmin")},
            new User  { Id = Guid.NewGuid(), Avatar = null, CreatedAt = DateTime.UtcNow, Email = "hr@mail.com", Name = "hr", Surname = "hr",  IsActive = true, IsDelete = false,
            Patronymic = "hr", PhoneNumber = "+375987654321", Position = "hr", Role = Domain.Enums.UserRole.Manager, PasswordHash = new PasswordHasher().HashPassword("hr")},
        };
        await context.User.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

}

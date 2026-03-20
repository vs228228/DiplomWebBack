using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DiplomWebBack.Infrastructure.Context;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        optionsBuilder.UseNpgsql(
            "Host=localhost;Port=5432;Database=TempDb;Username=password123;Password=123");

        return new AppDbContext(optionsBuilder.Options);
    }
}
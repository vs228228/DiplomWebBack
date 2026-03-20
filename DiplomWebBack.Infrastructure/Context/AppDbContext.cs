using Microsoft.EntityFrameworkCore;
using DiplomWebBack.Domain.Entities;
using DiplomWebBack.Domain.Entities.m2m;

namespace DiplomWebBack.Infrastructure.Context;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<User> User => Set<User>();
    public DbSet<UserActivator> Activators => Set<UserActivator>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<UserToProject> UserToProjects => Set<UserToProject>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);
    }
}
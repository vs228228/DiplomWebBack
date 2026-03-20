using DiplomWebBack.Domain.Entities.m2m;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiplomWebBack.Infrastructure.Configs.m2m
{
    public class UserToProjectConfiguration : IEntityTypeConfiguration<UserToProject>
    {
        public void Configure(EntityTypeBuilder<UserToProject> builder)
        {
            builder.HasKey(utp => new { utp.UserId, utp.ProjectId });

            builder.HasOne(utp => utp.User)
                .WithMany(u => u.UserToProjects)
                .HasForeignKey(utp => utp.UserId);

            builder.HasOne(utp => utp.Project)
                .WithMany(p => p.UserToProjects)
                .HasForeignKey(utp => utp.ProjectId);

            builder.Property(utp => utp.Role)
                .IsRequired();
        }
    }
}

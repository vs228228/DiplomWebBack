using DiplomWebBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DiplomWebBack.Infrastructure.Configs
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.CreatedAt)
                .IsRequired();

            builder.HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Tags)
                .WithMany();

            builder.HasMany(p => p.UserToProjects)
                .WithOne(utp => utp.Project)
                .HasForeignKey(utp => utp.ProjectId);
        }
    }
}

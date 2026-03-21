using DiplomWebBack.Domain.Entities.m2m;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Infrastructure.Configs.m2m
{
    public class TagToProjectConfiguration : IEntityTypeConfiguration<TagToProject>
    {
        public void Configure(EntityTypeBuilder<TagToProject> builder)
        {
            builder.HasKey(tp => new { tp.ProjectId, tp.TagId });

            builder.HasOne(tp => tp.Project)
                .WithMany(p => p.ProjectTags)
                .HasForeignKey(tp => tp.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tp => tp.Tag)
                .WithMany()
                .HasForeignKey(tp => tp.TagId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

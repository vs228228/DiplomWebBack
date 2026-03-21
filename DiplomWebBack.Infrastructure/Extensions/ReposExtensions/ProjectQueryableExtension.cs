using DiplomWebBack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Infrastructure.Extensions.ReposExtensions
{
    public static class ProjectQueryableExtension
    {
        public static IQueryable<Project> IncludeTags(
        this IQueryable<Project> source,
        bool includeTags)
        {
            return includeTags ? source.Include(p => p.ProjectTags).ThenInclude(pt => pt.Tag) : source;
        }

        public static IQueryable<Project> IncludeCreator(
        this IQueryable<Project> source,
        bool includeCreator)
        {
            return includeCreator ? source.Include(p => p.CreatedBy) : source;
        }

        public static IQueryable<Project> IncludeEmployees(
        this IQueryable<Project> source,
        bool includeEmployees)
        {
            return includeEmployees ? source.Include(p => p.UserToProjects).ThenInclude(up => up.User) : source;
        }
    }
}

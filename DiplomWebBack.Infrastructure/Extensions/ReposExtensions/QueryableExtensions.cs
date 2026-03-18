using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiplomWebBack.Infrastructure.Extensions.ReposExtensions
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Включает или отключает отслеживание изменений для запроса EF Core
        /// </summary>
        public static IQueryable<T> TrackChanges<T>(this IQueryable<T> query, bool trackChanges) where T : class
        {
            return trackChanges ? query : query.AsNoTracking();
        }

        public static IQueryable<TSource> OptionalWhere<TSource>(
        this IQueryable<TSource> source,
        bool when,
        Expression<Func<TSource, bool>> predicate)
        {
            return when ? source.Where(predicate) : source;
        }
    }
}

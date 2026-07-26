using System.Linq.Expressions;

namespace Extellect.Nuvo
{
    public static class QueryableExtensions
    {
        /// <summary>
        /// Orders a query.
        /// </summary>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, List<(Expression<Func<T, IComparable>>, bool)> orderByExpressions)
        {
            if (orderByExpressions != null && orderByExpressions.Count > 0)
            {
                var (orderByExpression, ascending) = orderByExpressions[0];
                var orderedQuery = ascending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);
                for (var i = 1; i < orderByExpressions.Count; i++)
                {
                    (orderByExpression, ascending) = orderByExpressions[i];
                    orderedQuery = ascending ? orderedQuery.ThenBy(orderByExpression) : orderedQuery.ThenByDescending(orderByExpression);
                }
                query = orderedQuery;
            }

            return query;
        }
    }
}

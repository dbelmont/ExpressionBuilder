using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using System;
using System.Linq.Expressions;

namespace ExpressionBuilder.Builders
{
    /// <summary>
    /// Static class that enables creating LINQ expression trees.
    /// </summary>
    public static class GroupBuilder
    {
        /// <summary>
        /// Combines zero or more LINQ Expressions into a single LINQ Expression.
        /// Return true if no filters are supplied.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="connector"></param>
        /// <param name="filters">The LINQ expressions to combine.</param>
        /// <returns>Single combined LINQ expression.</returns>
        public static Expression<Func<TClass, bool>> StartGroup<TClass>(Connector connector, params Expression<Func<TClass, bool>>[] filters) where TClass : class
        {
            if (filters == null)
            {
                return (c) => true;
            }
            FilterBuilder _filterBuilder = new FilterBuilder();
            if (filters.Length == 1)
            {
                return filters[0];
            }
            Expression<Func<TClass, bool>> predicate = null;
            if (connector == Connector.And)
            {
                predicate = c => true;
                foreach (var filter in filters)
                {
                    predicate = predicate.And(filter);
                }
            }
            else if (connector == Connector.Or)
            {
                predicate = c => false;
                foreach (var filter in filters)
                {
                    predicate = predicate.Or(filter);
                }
            }
            return predicate;
        }

        /// <summary>
        /// Converts a LINQ expression to a lambda delegate.
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <param name="filter">The expression to compile</param>
        /// <returns>The function that can be used inside LINQ functions (e.g. Where, Any, ...).</returns>
        public static Func<TClass, bool> GetFilter<TClass>(Expression<Func<TClass, bool>> filter) where TClass : class
        {
            if (filter == null)
            {
                return (c) => true;
            }
            return filter.Compile();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Builders
{
    public class BuilderDefinitions
    {
        readonly MethodInfo containsMethod = typeof(string).GetMethod("Contains");
        readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
        readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        public readonly Dictionary<Operation, Func<Expression, Expression, Expression>> Expressions;

        public BuilderDefinitions()
        {
            Expressions = new Dictionary<Operation, Func<Expression, Expression, Expression>>
            {
                { Operation.Equals, Expression.Equal },
                { Operation.NotEquals, Expression.NotEqual },
                { Operation.GreaterThan, Expression.GreaterThan },
                { Operation.GreaterThanOrEquals, Expression.GreaterThanOrEqual },
                { Operation.LessThan, Expression.LessThan },
                { Operation.LessThanOrEquals, Expression.LessThanOrEqual },
                { Operation.Contains, Contains },
                { Operation.StartsWith, (member, constant) => Expression.Call(member, startsWithMethod, constant) },
                { Operation.EndsWith, (member, constant) => Expression.Call(member, endsWithMethod, constant) }
            };
        }

        private Expression Contains(Expression member, Expression expression)
        {
            MethodCallExpression contains = null;
            if (expression is ConstantExpression constant && constant.Value is IList && constant.Value.GetType().IsGenericType)
            {
                var type = constant.Value.GetType();
                var containsInfo = type.GetMethod("Contains", new[] { type.GetGenericArguments()[0] });
                contains = Expression.Call(constant, containsInfo, member);
            }

            return contains ?? Expression.Call(member, containsMethod, expression); ;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Builder.Generic;

namespace ExpressionBuilder.Builder
{
	public static class Builder
	{
		static MethodInfo containsMethod = typeof(string).GetMethod("Contains");
		static MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);
        static MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);
        static MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new [] { typeof(string) });
        static MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new [] { typeof(string) });
		
        static readonly Dictionary<Operation, Func<Expression, Expression, Expression>> Expressions;
        
		static Builder()
		{
			Expressions = new Dictionary<Operation, Func<Expression, Expression, Expression>>();
			Expressions.Add(Operation.Equals, Expression.Equal);
			Expressions.Add(Operation.NotEquals, Expression.NotEqual);
			Expressions.Add(Operation.GreaterThan, Expression.GreaterThan);
			Expressions.Add(Operation.GreaterThanOrEquals, Expression.GreaterThanOrEqual);
			Expressions.Add(Operation.LessThan, Expression.LessThan);
			Expressions.Add(Operation.LessThanOrEquals, Expression.LessThanOrEqual);
			Expressions.Add(Operation.Contains, Contains);
            Expressions.Add(Operation.StartsWith, (member, constant) => Expression.Call(member, startsWithMethod, constant));
            Expressions.Add(Operation.EndsWith, (member, constant) => Expression.Call(member, endsWithMethod, constant));
		}
		
		public static Expression<Func<T, bool>> GetExpression<T>(IFilter<T> filter) where T : class
        {
            var param = Expression.Parameter(typeof(T), "x");
            Expression expression = Expression.Constant(true);
            var connector = FilterStatementConnector.And;
            foreach (var statement in filter.Statements)
            {
                Expression expr = null;
                if (IsList(statement))
                    expr = ProcessListStatement(param, statement);
                else
                    expr = GetExpression(param, statement);

                expression = CombineExpressions(expression, expr, connector);
                connector = statement.Connector;
            }
            return Expression.Lambda<Func<T, bool>>(expression, param);
        }
		
        static bool IsList(IFilterStatement statement)
        {
            return statement.PropertyName.Contains("[") && statement.PropertyName.Contains("]");
        }

        static Expression CombineExpressions(Expression expr1, Expression expr2, FilterStatementConnector connector)
        {
            return connector == FilterStatementConnector.And ? Expression.AndAlso(expr1, expr2) : Expression.OrElse(expr1, expr2);
        }

        static Expression ProcessListStatement(ParameterExpression param, IFilterStatement statement)
        {
            var basePropertyName = statement.PropertyName.Substring(0, statement.PropertyName.IndexOf("["));
            var propertyName = statement.PropertyName.Replace(basePropertyName, "").Replace("[", "").Replace("]", "");

            var type = param.Type.GetProperty(basePropertyName).PropertyType.GetGenericArguments()[0];
            ParameterExpression listItemParam = Expression.Parameter(type, "i");
            var lambda = Expression.Lambda(GetExpression(listItemParam, statement, propertyName), listItemParam);
            var member = GetMemberExpression(param, basePropertyName);
            var tipoEnumerable = typeof(Enumerable);
            var anyInfo = tipoEnumerable.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2);
            anyInfo = anyInfo.MakeGenericMethod(type);
            return Expression.Call(anyInfo, member, lambda);
        }
        
        static Expression GetExpression(ParameterExpression param, IFilterStatement statement, string propertyName = null)
        {
            Expression member = GetMemberExpression(param, propertyName ?? statement.PropertyName);
            Expression constant = Expression.Constant(statement.Value);

            if (statement.Value is string)
            {
            	var trimMemberCall = Expression.Call(member, trimMethod);
            	member = Expression.Call(trimMemberCall, toLowerMethod);
            	var trimConstantCall = Expression.Call(constant, trimMethod);
            	constant = Expression.Call(trimConstantCall, toLowerMethod);
            }
            
            return Expressions[statement.Operation].Invoke(member, constant);
        }

        static Expression Contains(Expression member, Expression expression)
        {
        	if (expression is ConstantExpression) {
        		var constant = (ConstantExpression)expression;
	        	if (constant.Value is IList && constant.Value.GetType().IsGenericType)
	            {
	                var type = constant.Value.GetType();
	                var containsInfo = type.GetMethod("Contains", new [] { type.GetGenericArguments()[0] });
	                var contains = Expression.Call(constant, containsInfo, member);
	                return contains;
	            }
        	}

            return Expression.Call(member, containsMethod, expression);
        }

        static MemberExpression GetMemberExpression(Expression param, string propertyName)
        {
        	if (propertyName.Contains("."))
        	{
        		int index = propertyName.IndexOf(".");
        		var subParam = Expression.Property(param, propertyName.Substring(0, index));
        		return GetMemberExpression(subParam, propertyName.Substring(index + 1));
        	}
        	
        	return Expression.Property(param, propertyName);
        }
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Interfaces.Generics;

namespace ExpressionBuilder.Builders
{
	public class FilterBuilder
	{
		readonly MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);
        readonly MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);        
        
        readonly BuilderHelper helper;
        readonly BuilderDefinitions definitions;

        public FilterBuilder(BuilderHelper helper, BuilderDefinitions definitions)
		{
            this.definitions = definitions;
            this.helper = helper;
		}
		
		public Expression<Func<T, bool>> GetExpression<T>(IFilter filter) where T : class
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
		
        bool IsList(IFilterStatement statement)
        {
            return statement.PropertyName.Contains("[") && statement.PropertyName.Contains("]");
        }

        Expression CombineExpressions(Expression expr1, Expression expr2, FilterStatementConnector connector)
        {
            return connector == FilterStatementConnector.And ? Expression.AndAlso(expr1, expr2) : Expression.OrElse(expr1, expr2);
        }

        Expression ProcessListStatement(ParameterExpression param, IFilterStatement statement)
        {
            var basePropertyName = statement.PropertyName.Substring(0, statement.PropertyName.IndexOf("["));
            var propertyName = statement.PropertyName.Replace(basePropertyName, "").Replace("[", "").Replace("]", "");

            var type = param.Type.GetProperty(basePropertyName).PropertyType.GetGenericArguments()[0];
            ParameterExpression listItemParam = Expression.Parameter(type, "i");
            var lambda = Expression.Lambda(GetExpression(listItemParam, statement, propertyName), listItemParam);
            var member = helper.GetMemberExpression(param, basePropertyName);
            var enumerableType = typeof(Enumerable);
            var anyInfo = enumerableType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2);
            anyInfo = anyInfo.MakeGenericMethod(type);
            return Expression.Call(anyInfo, member, lambda);
        }
        
        Expression GetExpression(ParameterExpression param, IFilterStatement statement, string propertyName = null)
        {
            Expression member = helper.GetMemberExpression(param, propertyName ?? statement.PropertyName);
            Expression constant = Expression.Constant(statement.Value);

            if (statement.Value is string)
            {
            	var trimMemberCall = Expression.Call(member, trimMethod);
            	member = Expression.Call(trimMemberCall, toLowerMethod);
            	var trimConstantCall = Expression.Call(constant, trimMethod);
            	constant = Expression.Call(trimConstantCall, toLowerMethod);
            }
            
            return definitions.Expressions[statement.Operation].Invoke(member, constant);
        }
	}
}
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Builders
{
	public class BuilderHelper
	{
        public readonly MethodInfo trimMethod = typeof(string).GetMethod("Trim", new Type[0]);
        public readonly MethodInfo toLowerMethod = typeof(string).GetMethod("ToLower", new Type[0]);

        public Expression GetMemberExpression(Expression param, string propertyName)
        {
        	if (propertyName.Contains("."))
        	{
        		int index = propertyName.IndexOf(".");
        		var subParam = Expression.Property(param, propertyName.Substring(0, index));
        		return GetMemberExpression(subParam, propertyName.Substring(index + 1));
        	}
        	
        	Expression member = Expression.Property(param, propertyName);

            if (member.Type == typeof(string))
            {
                var trimMemberCall = Expression.Call(member, trimMethod);
                member = Expression.Call(trimMemberCall, toLowerMethod);
            }

            return member;
        }
	}
}
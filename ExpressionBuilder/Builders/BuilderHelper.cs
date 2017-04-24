using System;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Builders
{
	public class BuilderHelper
	{
		public MemberExpression GetMemberExpression(Expression param, string propertyName)
        {
        	if (propertyName.Contains("."))
        	{
        		int index = propertyName.IndexOf(".");
        		var subParam = Expression.Property(param, propertyName.Substring(0, index));
        		return GetMemberExpression(subParam, propertyName.Substring(index + 1));
        	}
        	
        	return Expression.Property(param, propertyName);
        }
		
		public MethodInfo MethodOf<T>(Expression<Func<T>> method)
	    {
	        MethodCallExpression mce = (MethodCallExpression) method.Body;
	        var mi = mce.Method;
	        return mi;
	    }
	}
}

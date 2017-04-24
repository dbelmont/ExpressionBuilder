using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ExpressionBuilder.Generics;

namespace ExpressionBuilder.Builders
{
	public class SortBuilder
	{
		readonly MethodInfo orderByMethod;
	    readonly MethodInfo orderByDescendingMethod;
	    readonly MethodInfo thenByMethod;
	    readonly MethodInfo thenByDescendingMethod;
	    readonly BuilderHelper helper;
		
	    public SortBuilder(BuilderHelper helper)
	    {
	    	this.helper = helper;
	    	orderByMethod = this.helper
	    					.MethodOf(() => Enumerable.OrderBy(default(IEnumerable<object>), default(Func<object, object>)))
			            	.GetGenericMethodDefinition();
	    	orderByDescendingMethod = this.helper
        					.MethodOf(() => Enumerable.OrderByDescending(default(IEnumerable<object>), default(Func<object, object>)))
			            	.GetGenericMethodDefinition();
			
	    	thenByMethod = this.helper
			        		.MethodOf(() => Enumerable.ThenBy(default(IOrderedEnumerable<object>), default(Func<object, object>)))
			            	.GetGenericMethodDefinition();
	
	    	thenByDescendingMethod = this.helper
			        		.MethodOf(() => Enumerable.ThenByDescending(default(IOrderedEnumerable<object>), default(Func<object, object>)))
			            	.GetGenericMethodDefinition();
	    }
	    
	    public IEnumerable<TType> BuildOrderBys<TType>(IEnumerable<TType> source, List<SortElement<TType>> properties) where TType : class
	    {
	        if (properties == null || properties.Count == 0) return source;
	
	        var typeOfT = typeof(TType);
	
	        Type t = typeOfT;
	
	        IOrderedEnumerable<TType> result = null;
	        var thenBy = false;
	
	        foreach (var item in properties
	            .Select(prop => new {PropertyInfo = t.GetProperty(prop.PropertyName), prop.Direction}))
	        {
	            var oExpr = Expression.Parameter(typeOfT, "o");
	            var propertyInfo = item.PropertyInfo;
	            var propertyType = propertyInfo.PropertyType;
	            var isAscending = item.Direction == SortDirection.Ascending;
	
	            if (thenBy)
	            {
	                var prevExpr = Expression.Parameter(typeof (IOrderedEnumerable<TType>), "prevExpr");
	                var expr1 = Expression.Lambda<Func<IOrderedEnumerable<TType>, IOrderedEnumerable<TType>>>(
	                    Expression.Call(
	                        (isAscending ? thenByMethod : thenByDescendingMethod).MakeGenericMethod(typeOfT, propertyType),
	                        prevExpr,
	                        Expression.Lambda(
	                            typeof (Func<,>).MakeGenericType(typeOfT, propertyType),
	                            Expression.MakeMemberAccess(oExpr, propertyInfo),
	                            oExpr)
	                        ),
	                    prevExpr)
	                    .Compile();
	
	                result = expr1(result);
	            }
	            else
	            {
	                var prevExpr = Expression.Parameter(typeof (IEnumerable<TType>), "prevExpr");
	                var expr1 = Expression.Lambda<Func<IEnumerable<TType>, IOrderedEnumerable<TType>>>(
	                    Expression.Call(
	                        (isAscending ? orderByMethod : orderByDescendingMethod).MakeGenericMethod(typeOfT, propertyType),
	                        prevExpr,
	                        Expression.Lambda(
	                            typeof (Func<,>).MakeGenericType(typeOfT, propertyType),
	                            Expression.MakeMemberAccess(oExpr, propertyInfo),
	                            oExpr)
	                        ),
	                    prevExpr)
	                    .Compile();
	
	                result = expr1(source);
	                thenBy = true;
	            }
	        }
	        return result;
	    }
	}
}

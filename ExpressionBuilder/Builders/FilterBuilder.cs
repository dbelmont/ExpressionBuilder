using ExpressionBuilder.Common;
using ExpressionBuilder.Exceptions;
using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Builders
{
    internal class FilterBuilder
    {
        internal FilterBuilder()
        {
        }

        public Expression<Func<T, bool>> GetExpression<T>(IFilter filter) where T : class
        {
            var param = Expression.Parameter(typeof(T), "x");
            Expression expression = null;
            var connector = Connector.And;
            foreach (var statementGroup in filter.Statements)
            {
                var statementGroupConnector = Connector.And;
                Expression partialExpr = GetPartialExpression(param, ref statementGroupConnector, statementGroup);

                expression = expression == null ? partialExpr : CombineExpressions(expression, partialExpr, connector);
                connector = statementGroupConnector;
            }

            expression = expression ?? Expression.Constant(true);

            return Expression.Lambda<Func<T, bool>>(expression, param);
        }

        private Expression GetPartialExpression(ParameterExpression param, ref Connector connector, IEnumerable<IFilterStatement> statementGroup)
        {
            Expression expression = null;
            foreach (var statement in statementGroup)
            {
                Expression expr = null;
                if (IsList(statement))
                {
                    expr = ProcessListStatement(param, statement);
                }
                else
                {
                    expr = GetExpression(param, statement);
                }

                expression = expression == null ? expr : CombineExpressions(expression, expr, connector);
                connector = statement.Connector;
            }

            return expression;
        }

        private bool IsList(IFilterStatement statement)
        {
            return statement.PropertyId.Contains("[") && statement.PropertyId.Contains("]");
        }

        private Expression CombineExpressions(Expression expr1, Expression expr2, Connector connector)
        {
            return connector == Connector.And ? Expression.AndAlso(expr1, expr2) : Expression.OrElse(expr1, expr2);
        }

        private Expression ProcessListStatement(ParameterExpression param, IFilterStatement statement)
        {
            var basePropertyName = statement.PropertyId.Substring(0, statement.PropertyId.IndexOf("["));
            var propertyName = statement.PropertyId.Substring(statement.PropertyId.IndexOf("[") + 1).Replace("]", string.Empty);

            var type = param.Type.GetProperty(basePropertyName).PropertyType.GetGenericArguments()[0];
            ParameterExpression listItemParam = Expression.Parameter(type, "i");
            var lambda = Expression.Lambda(GetExpression(listItemParam, statement, propertyName), listItemParam);
            var member = param.GetMemberExpression(basePropertyName);
            var enumerableType = typeof(Enumerable);
            var anyInfo = enumerableType.GetMethods(BindingFlags.Static | BindingFlags.Public).First(m => m.Name == "Any" && m.GetParameters().Count() == 2);
            anyInfo = anyInfo.MakeGenericMethod(type);
            return Expression.Call(anyInfo, member, lambda);
        }

        private Expression GetExpression(ParameterExpression param, IFilterStatement statement, string propertyName = null)
        {
            Expression resultExpr = null;
            var memberName = propertyName ?? statement.PropertyId;
            MemberExpression member = param.GetMemberExpression(memberName);

            if (Nullable.GetUnderlyingType(member.Type) != null && statement.Value != null)
            {
                resultExpr = Expression.Property(member, "HasValue");
                member = Expression.Property(member, "Value");
            }

            var constant1 = Expression.Constant(statement.Value);
            var constant2 = Expression.Constant(statement.Value2);

            CheckPropertyValueMismatch(member, constant1);

            var safeStringExpression = statement.Operation.GetExpression(member, constant1, constant2);
            resultExpr = resultExpr != null ? Expression.AndAlso(resultExpr, safeStringExpression) : safeStringExpression;
            resultExpr = GetSafePropertyMember(param, memberName, resultExpr);

            if (statement.Operation.ExpectNullValues && memberName.Contains("."))
            {
                resultExpr = Expression.OrElse(CheckIfParentIsNull(param, memberName), resultExpr);
            }

            return resultExpr;
        }

        private void CheckPropertyValueMismatch(MemberExpression member, ConstantExpression constant1)
        {
            var memberType = member.Member.MemberType == MemberTypes.Property ? (member.Member as PropertyInfo).PropertyType : (member.Member as FieldInfo).FieldType;

            var constant1Type = GetConstantType(constant1);
            var nullableType = constant1Type != null ? Nullable.GetUnderlyingType(constant1Type) : null;

            var constantValueIsNotNull = constant1.Value != null;
            var memberAndConstantTypeDoNotMatch = nullableType == null && memberType != constant1Type;
            var memberAndNullableUnderlyingTypeDoNotMatch = nullableType != null && memberType != nullableType;

            if (constantValueIsNotNull && (memberAndConstantTypeDoNotMatch || memberAndNullableUnderlyingTypeDoNotMatch))
            {
                throw new PropertyValueTypeMismatchException(member.Member.Name, memberType.Name, constant1.Type.Name);
            }
        }

        private Type GetConstantType(ConstantExpression constant)
        {
            if (constant != null && constant.Value != null && constant.Value.IsGenericList())
            {
                return constant.Value.GetType().GenericTypeArguments[0];
            }

            return constant != null && constant.Value != null ? constant.Value.GetType() : null;
        }

        public Expression GetSafePropertyMember(ParameterExpression param, string memberName, Expression expr)
        {
            if (!memberName.Contains("."))
            {
                return expr;
            }

            var index = memberName.LastIndexOf(".", StringComparison.InvariantCulture);
            var parentName = memberName.Substring(0, index);
            var subParam = param.GetMemberExpression(parentName);
            var resultExpr = Expression.AndAlso(Expression.NotEqual(subParam, Expression.Constant(null)), expr);
            return GetSafePropertyMember(param, parentName, resultExpr);
        }

        protected Expression CheckIfParentIsNull(ParameterExpression param, string memberName)
        {
            var parentMember = GetParentMember(param, memberName);
            return Expression.Equal(parentMember, Expression.Constant(null));
        }

        private MemberExpression GetParentMember(ParameterExpression param, string memberName)
        {
            var parentName = memberName.Substring(0, memberName.IndexOf("."));
            return param.GetMemberExpression(parentName);
        }
    }
}
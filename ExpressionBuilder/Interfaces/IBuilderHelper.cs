using System.Linq.Expressions;

namespace ExpressionBuilder.Interfaces
{
    internal interface IBuilderHelper
    {
        MemberExpression GetMemberExpression(Expression param, string propertyName);
    }
}
using System.Linq.Expressions;

namespace ExpressionBuilder.Interfaces
{
    internal interface IBuilderHelper
    {
        Expression GetMemberExpression(Expression param, string propertyName);
    }
}
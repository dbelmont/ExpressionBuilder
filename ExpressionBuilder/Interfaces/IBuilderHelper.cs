using System.Linq.Expressions;

namespace ExpressionBuilder.Interfaces
{
    public interface IBuilderHelper
    {
        Expression GetMemberExpression(Expression param, string propertyName);
    }
}
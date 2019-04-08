using ExpressionBuilder.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing a string "Contains" method call.
    /// </summary>
    public class Contains : OperationBase
    {
        private readonly MethodInfo stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        /// <inheritdoc />
        public Contains()
            : base("Contains", 1, TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            Expression constant = constant1.TrimToLower();

            return Expression.Call(member.TrimToLower(), stringContainsMethod, constant)
                   .AddNullCheck(member);
        }
    }
}
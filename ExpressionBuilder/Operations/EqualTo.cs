using ExpressionBuilder.Common;
using System.Linq.Expressions;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Operation representing an equality comparison.
    /// </summary>
    public class EqualTo : OperationBase
    {
        /// <inheritdoc />
        public EqualTo()
            : base("EqualTo", 1, TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text) { }

        /// <inheritdoc />
        public override Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            Expression constant = constant1;

            if (member.Type == typeof(string))
            {
                constant = constant1.TrimToLower();

                return Expression.Equal(member.TrimToLower(), constant)
                       .AddNullCheck(member);
            }

            return Expression.Equal(member, constant);
        }
    }
}
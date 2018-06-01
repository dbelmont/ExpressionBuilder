using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using System.Linq.Expressions;

namespace ExpressionBuilder.Test.CustomOperations
{
    public class EqualTo : IOperation
    {
        public string Name { get { return "EqualTo"; } }

        public TypeGroup TypeGroup { get { return TypeGroup.Default | TypeGroup.Boolean | TypeGroup.Date | TypeGroup.Number | TypeGroup.Text; } }

        public int NumberOfValues { get { return 1; } }

        public bool Active { get; set; }

        public bool SupportsLists { get { return false; } }

        public bool ExpectNullValues { get { return false; } }

        public EqualTo()
        {
            Active = true;
        }

        public Expression GetExpression(MemberExpression member, ConstantExpression constant1, ConstantExpression constant2)
        {
            return Expression.Equal(member, constant1);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
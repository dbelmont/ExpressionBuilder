using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using System;
using System.Linq.Expressions;

namespace ExpressionBuilder.Test.CustomOperations
{
    public class ThisDay : IOperation
    {
        public string Name { get { return "ThisDay"; } }

        public TypeGroup TypeGroup { get { return TypeGroup.Date; } }

        public int NumberOfValues { get { return 0; } }

        public bool Active { get; set; }

        public bool SupportsLists { get { return false; } }

        public bool ExpectNullValues { get { return false; } }

        public ThisDay()
        {
            Active = true;
        }

        public Expression GetExpression(MemberExpression member, ConstantExpression value1, ConstantExpression value2)
        {
            var today = DateTime.Today;
            var constantDay = Expression.Constant(today.Day);
            var constantMonth = Expression.Constant(today.Month);

            if (Nullable.GetUnderlyingType(member.Type) != null)
            {
                var memberValue = Expression.Property(member, "Value");
                var dayMemberValue = Expression.Property(memberValue, "Day");
                var monthMemberValue = Expression.Property(memberValue, "Month");
                return Expression.AndAlso(
                    Expression.Equal(dayMemberValue, constantDay),
                    Expression.Equal(monthMemberValue, constantMonth)
                    )
                    .AddNullCheck(member);
            }

            var dayMember = Expression.Property(member, "Day");
            var monthMember = Expression.Property(member, "Month");
            return Expression.AndAlso(
                Expression.Equal(dayMember, constantDay),
                Expression.Equal(monthMember, constantMonth)
                );
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
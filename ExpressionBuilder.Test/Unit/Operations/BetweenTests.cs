using ExpressionBuilder.Operations;
using ExpressionBuilder.Test.Models;
using ExpressionBuilder.Test.Unit.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionBuilder.Test.Unit.Operations
{
    [TestFixture]
    public class BetweenTests
    {
        private TestData TestData { get; set; }

        public BetweenTests()
        {
            TestData = new TestData();
        }

        [TestCase(TestName = "'Between' operation - Get expression")]
        public void GetExpressionTest()
        {
            var propertyName = "Salary";
            var operation = new Between();
            var param = Expression.Parameter(typeof(Person), "x");
            var member = Expression.Property(param, propertyName);
            var constant1 = Expression.Constant(4000D);
            var constant2 = Expression.Constant(5000D);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);

            //Testing the operation structure
            expression.Left.Should().BeAnExpressionCheckingIf(propertyName, ExpressionType.GreaterThanOrEqual, 4000D);

            Assert.That(expression.NodeType, Is.EqualTo(ExpressionType.AndAlso));

            expression.Right.Should().BeAnExpressionCheckingIf(propertyName, ExpressionType.LessThanOrEqual, 5000D);

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solution = TestData.People.Where(x => x.Salary >= 4000 && x.Salary <= 5000);
            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Checking if two operations are equal (failure: comparing with different type)")]
        public void EqualsDifferentType_Failure()
        {
            var operation = new Between();
            var notOperation = "notOperation";

            operation.Equals(notOperation).Should().BeFalse();
        }
    }
}
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
    public class NotEqualToTests
    {
        private TestData TestData { get; set; }

        public NotEqualToTests()
        {
            TestData = new TestData();
        }

        [TestCase("Name", " John doe ", TestName = "'NotEqualTo' operation - Get expression (string value)")]
        [TestCase("Salary", 3500D, TestName = "'NotEqualTo' operation - Get expression (Failure: string property with integer value)")]
        public void GetExpression(string propertyName, object value)
        {
            var operation = new NotEqualTo();
            var param = Expression.Parameter(typeof(Person), "x");
            var member = Expression.Property(param, propertyName);
            var constant1 = Expression.Constant(value);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

            //Testing the operation structure
            if (value.GetType() == typeof(string))
            {
                expression.Left.Should().BeNullChecking(propertyName);
                expression.NodeType.Should().Be(ExpressionType.AndAlso);
                expression.Right.Should().BeAStringExpressionCheckingIf(propertyName, ExpressionType.NotEqual, value);
            }
            else
            {
                expression.Should().BeAnExpressionCheckingIf(propertyName, ExpressionType.NotEqual, value);
            }

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solutionMethod = (Func<Person, bool>)GetType().GetMethod(propertyName).Invoke(this, new[] { value });
            var solution = TestData.People.Where(solutionMethod);
            Assert.That(people, Is.EquivalentTo(solution));
        }

        public Func<Person, bool> Name(string value)
        {
            return x => x.Name.Trim().ToLower() != value.Trim().ToLower();
        }

        public Func<Person, bool> Salary(double value)
        {
            return x => x.Salary != value;
        }
    }
}
using ExpressionBuilder.Interfaces;
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
    public class IsNullOrWhiteSpaceOrNotOperationsTests
    {
        private TestData TestData { get; set; }

        public IsNullOrWhiteSpaceOrNotOperationsTests()
        {
            TestData = new TestData();
        }

        [TestCase(TestName = "'IsNullOrWhiteSpace' operation - Get expression")]
        public void GetExpressionTestNullWhiteSpace()
        {
            var propertyName = "Country";
            string value = string.Empty;
            var operation = new IsNullOrWhiteSpace();
            var param = Expression.Parameter(typeof(Person), "x");
            var parent = Expression.Property(param, "Birth");
            var member = Expression.Property(parent, "Country");
            var constant1 = Expression.Constant(4000D);
            var constant2 = Expression.Constant(5000D);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);

            //Testing the operation structure
            expression.Left.Should().BeNullChecking(propertyName, true);
            expression.NodeType.Should().Be(ExpressionType.OrElse);
            var isEmpty = (BinaryExpression)expression.Right;
            isEmpty.Left.Should().BeNullChecking(propertyName);
            isEmpty.NodeType.Should().Be(ExpressionType.AndAlso);
            isEmpty.Right.Should().BeAStringExpressionCheckingIf(propertyName, ExpressionType.Equal, value, false);

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solution = TestData.People.Where(x => x.Birth == null || (x.Birth.Country == null || (x.Birth.Country != null && x.Birth.Country.Trim().ToLower() == string.Empty)));
            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "'IsNotNullNorWhiteSpace' operation - Get expression")]
        public void GetExpressionTestNotNullNorWhiteSpace()
        {
            var propertyName = "Country";
            string value = string.Empty;
            var operation = new IsNotNullNorWhiteSpace();
            var param = Expression.Parameter(typeof(Person), "x");
            var parent = Expression.Property(param, "Birth");
            var member = Expression.Property(parent, "Country");
            var constant1 = Expression.Constant(4000D);
            var constant2 = Expression.Constant(5000D);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, constant2);

            //Testing the operation structure
            expression.Left.Should().BeNullChecking(propertyName, false);
            expression.NodeType.Should().Be(ExpressionType.AndAlso);
            expression.Right.Should().BeAStringExpressionCheckingIf(propertyName, ExpressionType.NotEqual, value, false);

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solution = TestData.People.Where(x => x.Birth != null && x.Birth.Country != null && x.Birth.Country.Trim().ToLower() != string.Empty);
            Assert.That(people, Is.EquivalentTo(solution));
        }
    }
}
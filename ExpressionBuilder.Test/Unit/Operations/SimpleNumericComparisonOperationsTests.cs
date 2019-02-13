using ExpressionBuilder.Interfaces;
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
    public class SimpleNumericComparisonOperationsTests
    {
        private TestData TestData { get; set; }

        public SimpleNumericComparisonOperationsTests()
        {
            TestData = new TestData();
        }

        [TestCase(ExpressionType.GreaterThan, "GreaterThan", 4000D, TestName = "'GreaterThan' operation - Get expression")]
        [TestCase(ExpressionType.GreaterThanOrEqual, "GreaterThanOrEqualTo", 4000D, TestName = "'GreaterThanOrEqualTo' operation - Get expression")]
        [TestCase(ExpressionType.LessThan, "LessThan", 4000D, TestName = "'LessThan' operation - Get expression")]
        [TestCase(ExpressionType.LessThanOrEqual, "LessThanOrEqualTo", 4000D, TestName = "'LessThanOrEqualTo' operation - Get expression")]
        public void GetExpressionTest(ExpressionType method, string methodName, double value)
        {
            var propertyName = "Salary";
            var type = typeof(IFilter).Assembly.Types()
                .Single(t => t.FullName == "ExpressionBuilder.Operations." + methodName);
            var operation = (IOperation)Activator.CreateInstance(type);
            var param = Expression.Parameter(typeof(Person), "x");
            var member = Expression.Property(param, propertyName);
            var constant1 = Expression.Constant(value);

            BinaryExpression expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

            //Testing the operation structure
            expression.Should().BeAnExpressionCheckingIf(propertyName, method, value);

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solutionMethod = (Func<Person, bool>)GetType().GetMethod(method.ToString()).Invoke(this, new object[] { value });
            var solution = TestData.People.Where(solutionMethod);
            Assert.That(people, Is.EquivalentTo(solution));
        }

        public Func<Person, bool> GreaterThan(double value)
        {
            return x => x.Salary > value;
        }

        public Func<Person, bool> GreaterThanOrEqual(double value)
        {
            return x => x.Salary >= value;
        }

        public Func<Person, bool> LessThan(double value)
        {
            return x => x.Salary < value;
        }

        public Func<Person, bool> LessThanOrEqual(double value)
        {
            return x => x.Salary <= value;
        }
    }
}
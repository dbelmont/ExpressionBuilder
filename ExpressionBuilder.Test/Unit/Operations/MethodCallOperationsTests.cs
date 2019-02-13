using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Test.Models;
using ExpressionBuilder.Test.Unit.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Test.Unit.Operations
{
    [TestFixture]
    public class MethodCallOperationsTests
    {
        private TestData TestData { get; set; }

        public MethodCallOperationsTests()
        {
            TestData = new TestData();
        }

        [TestCase("Contains", "  Doe ", TestName = "'Contains' operation - Get expression")]
        [TestCase("EndsWith", "  Doe ", TestName = "'EndsWith' operation - Get expression")]
        [TestCase("StartsWith", " John ", TestName = "'StartsWith' operation - Get expression")]
        public void GetExpressionTest(string methodName, string value)
        {
            var propertyName = "Name";
            var type = typeof(IFilter).Assembly.Types()
                .Single(t => t.FullName == "ExpressionBuilder.Operations." + methodName);
            var operation = (IOperation)Activator.CreateInstance(type);
            var param = Expression.Parameter(typeof(Person), "x");
            var member = Expression.Property(param, propertyName);
            var constant1 = Expression.Constant(value);

            var expression = (BinaryExpression)operation.GetExpression(member, constant1, null);

            //Testing the operation structure
            expression.Left.Should().BeNullChecking(propertyName);
            expression.NodeType.Should().Be(ExpressionType.AndAlso);

            var contains = (expression.Right as MethodCallExpression);
            contains.Method.Should().BeAssignableTo<MethodInfo>();
            var method = contains.Method as MethodInfo;
            method.Name.Should().Be(methodName);

            var property = contains.Object.ExtractTrimToLowerProperty(propertyName);
            property.Member.Name.Should().Be(propertyName);

            var constant = contains.Arguments.First().ExtractTrimToLowerConstant();
            constant.Value.Should().Be(value);

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solutionMethod = (Func<Person, bool>)GetType().GetMethod(methodName).Invoke(this, new object[] { value });
            var solution = TestData.People.Where(solutionMethod);
            people.Should().BeEquivalentTo(solution);
        }

        public Func<Person, bool> Contains(string value)
        {
            return x => x.Name.Trim().ToLower().Contains(value.Trim().ToLower());
        }

        public Func<Person, bool> EndsWith(string value)
        {
            return x => x.Name.Trim().ToLower().EndsWith(value.Trim().ToLower());
        }

        public Func<Person, bool> StartsWith(string value)
        {
            return x => x.Name.Trim().ToLower().StartsWith(value.Trim().ToLower());
        }
    }
}
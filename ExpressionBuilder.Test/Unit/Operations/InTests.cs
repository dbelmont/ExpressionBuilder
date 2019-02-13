using ExpressionBuilder.Test.Models;
using ExpressionBuilder.Test.Unit.Helpers;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionBuilder.Test.Unit.Operations
{
    [TestFixture]
    public class InTests
    {
        private TestData TestData { get; set; }

        public InTests()
        {
            TestData = new TestData();
        }

        [TestCase(TestName = "'In' operation - Get expression")]
        public void GetExpressionTest()
        {
            var value = new List<string> { "USA", "AUS" };
            var operation = new ExpressionBuilder.Operations.In();
            var param = Expression.Parameter(typeof(Person), "x");
            var parent = Expression.Property(param, "Birth");
            var member = Expression.Property(parent, "Country");
            var constant1 = Expression.Constant(value);

            var expression = (MethodCallExpression)operation.GetExpression(member, constant1, null);

            //Testing the operation structure
            expression.Method.Should().BeAssignableTo<MethodInfo>();
            var method = expression.Method as MethodInfo;
            method.Name.Should().Be("Contains");

            var property = expression.Arguments.First() as MemberExpression;
            property.Member.Name.Should().Be("Country");

            var constant = (ConstantExpression)expression.Object;
            constant.Value.Should().Be(value);

            //Testing the operation execution
            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solution = TestData.People.Where(x => value.Contains(x.Birth.Country));
            people.Should().BeEquivalentTo(solution);
        }

        [TestCase(TestName = "'In' operation - should deal nicely with a list of nullable objects")]
        public void ShouldDealNicelyWithListOfNullables()
        {
            var value = new List<long?> { 123, null };
            var operation = new ExpressionBuilder.Operations.In();
            var param = Expression.Parameter(typeof(Person), "x");
            var member = Expression.Property(param, "EmployeeReferenceNumber");
            var constant1 = Expression.Constant(value);
            var expression = (MethodCallExpression)operation.GetExpression(member, constant1, null);

            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solution = TestData.People.Where(x => value.Contains(x.EmployeeReferenceNumber));
            people.Should().BeEquivalentTo(solution);
        }

        [TestCase(TestName = "'In' operation - should deal nicely with a list of nullable objects against a non-nullable member")]
        public void ShouldDealNicelyWithListOfNullablesAgainstNonnullableMember()
        {
            var value = new List<long?> { 123, null };
            var operation = new ExpressionBuilder.Operations.In();
            var param = Expression.Parameter(typeof(Person), "x");
            var parent = Expression.Property(param, "EmployeeReferenceNumber");
            var member = Expression.Property(parent, "Value");
            var constant1 = Expression.Constant(value);
            var expression = (MethodCallExpression)operation.GetExpression(member, constant1, null);

            var lambda = Expression.Lambda<Func<Person, bool>>(expression, param);
            var people = TestData.People.Where(lambda.Compile());
            var solution = TestData.People.Where(x => value.Contains(x.EmployeeReferenceNumber));
            people.Should().BeEquivalentTo(solution);
        }

        [TestCase(TestName = "'In' operation - Get expression (Failure: non list constant)")]
        public void GetExpressionWithNonListConstant_Failure()
        {
            var value = "USA";
            var operation = new ExpressionBuilder.Operations.In();
            var param = Expression.Parameter(typeof(Person), "x");
            var parent = Expression.Property(param, "Birth");
            var member = Expression.Property(parent, "Country");
            var constant1 = Expression.Constant(value);

            var ex = Assert.Throws<ArgumentException>(() => operation.GetExpression(member, constant1, null));
            ex.Message.Should().Be("The 'In' operation only supports lists as parameters.");
        }

        [TestCase(TestName = "'In' operation - Get expression (Failure: non generic list constant)")]
        public void GetExpressionWithNonGenericListConstant_Failure()
        {
            var value = new System.Collections.ArrayList { "USA", "UAS" };
            var operation = new ExpressionBuilder.Operations.In();
            var param = Expression.Parameter(typeof(Person), "x");
            var parent = Expression.Property(param, "Birth");
            var member = Expression.Property(parent, "Country");
            var constant1 = Expression.Constant(value);

            var ex = Assert.Throws<ArgumentException>(() => operation.GetExpression(member, constant1, null));
            ex.Message.Should().Be("The 'In' operation only supports lists as parameters.");
        }
    }
}
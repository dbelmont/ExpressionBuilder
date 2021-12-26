using ExpressionBuilder.Builders;
using ExpressionBuilder.Common;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Operations;
using ExpressionBuilder.Test.Models;
using ExpressionBuilder.Test.Unit.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionBuilder.Test.Integration
{
    [TestFixture]
    public class GroupBuilderTest
    {
        private List<Person> _people;

        public List<Person> People
        {
            get
            {
                if (_people == null)
                {
                    _people = new TestData().People;
                }

                return _people;
            }
        }

        [TestCase(TestName = "Passing null filter should return true")]
        public void GroupWithEmptyFilter()
        {
            var expr1 = GroupBuilder.GetFilter<Person>(null);
            Assert.AreEqual(expr1.Invoke(null), true);
            var expr2 = GroupBuilder.GetFilter<Person>(new Filter<Person>());
            Assert.AreEqual(expr2.Invoke(null), true);
        }

        [TestCase(TestName = "Passing null array should return true")]
        public void GroupWithEmptyFilterArray()
        {
            var andExpression = GroupBuilder.GetFilter(GroupBuilder.Group<Person>(Common.Connector.And, null));
            var orExpression = GroupBuilder.GetFilter(GroupBuilder.Group<Person>(Common.Connector.Or, null));
            Assert.AreEqual(andExpression.Invoke(null), true);
            Assert.AreEqual(orExpression.Invoke(null), true);
            Assert.AreEqual(andExpression.Invoke(null), orExpression.Invoke(null));
        }

        [TestCase(TestName = "1 passed filter should return that filter's expression")]
        public void GroupWithSingleElementFilterArray()
        {
            var filter = new Filter<Person>();
            filter.By("Birth.Country", Operation.IsEmpty, null, (object)null, Connector.And);
            var andExpression = GroupBuilder.GetFilter(GroupBuilder.Group<Person>(Common.Connector.And, filter));
            var orExpression = GroupBuilder.GetFilter(GroupBuilder.Group<Person>(Common.Connector.Or, filter));
            var people1 = People.Where(andExpression);
            var solution = People.Where(filter);
            Assert.That(people1, Is.EquivalentTo(solution));
            var people2 = People.Where(orExpression);
            Assert.That(people2, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "And/Or with 2 parameters")]
        public void GroupWith2Parameters()
        {
            var f1 = new Filter<Person>();
            f1.By("Birth.Date", Operation.IsNotNull);
            var f2 = new Filter<Person>();
            f2.By("Birth.Date", Operation.GreaterThan, new DateTime(1980, 1, 1));
            var orExpression = Connector.Or.Group<Person>(f1, f2).GetFilter();
            var orPeople = People.Where(orExpression);
            var orFilter = new Filter<Person>();
            orFilter.By("Birth.Date", Operation.IsNotNull).Or.By("Birth.Date", Operation.GreaterThan, new DateTime(1980, 1, 1));
            var orSolution = People.Where(orFilter);
            Assert.That(orPeople, Is.EquivalentTo(orSolution));

            var andExpression = Connector.And.Group<Person>(f1, f2).GetFilter();
            var andPeople = People.Where(andExpression);
            var andFilter = new Filter<Person>();
            andFilter.By("Birth.Date", Operation.IsNotNull).And.By("Birth.Date", Operation.GreaterThan, new DateTime(1980, 1, 1));
            var andSolution = People.Where(andFilter);
            Assert.That(andPeople, Is.EquivalentTo(andSolution));
        }

        [TestCase(TestName = "Group equal to builder using complex expressions (fluent interface)", Category = "ComplexExpressions")]
        public void GroupUsingComplexExpressionsFluentInterface()
        {
            var f1 = new Filter<Person>();
            var f2 = new Filter<Person>();
            var f3 = new Filter<Person>();
            var f4 = new Filter<Person>();
            f1.By("Birth.Country", Operation.EqualTo, "USA");
            f2.By("Name", Operation.DoesNotContain, "doe");
            f3.By("Name", Operation.EndsWith, "Doe");
            f4.By("Birth.Country", Operation.IsNullOrWhiteSpace);
            var orExpression = Connector.Or.Group(Connector.And.Group<Person>(f1, f2), Connector.And.Group<Person>(f3, f4)).GetFilter();
            var people = People.Where(orExpression);
            var filter = new Filter<Person>();
            filter.By("Birth.Country", Operation.EqualTo, "USA").And.By("Name", Operation.DoesNotContain, "doe")
                .Or
                .Group.By("Name", Operation.EndsWith, "Doe").And.By("Birth.Country", Operation.IsNullOrWhiteSpace);
            var solution = People.Where(filter);
            Assert.That(people, Is.EquivalentTo(solution));
        }
    }
}

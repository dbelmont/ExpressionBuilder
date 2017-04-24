using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Extensions;
using ExpressionBuilder.Test.Models;
using NUnit.Framework;

namespace ExpressionBuilder.Test
{
	[TestFixture]
	public class BuilderTest
	{
		List<Person> _people;
		
		public List<Person> People
		{
			get
			{
				_people = new List<Person>();
				_people.Add(new Person { Name = "John Doe", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1979, 2, 28), Country = "USA" } });
				_people.Add(new Person { Name = "Jane Doe", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1985, 9, 5), Country = "CYM" } });
				_people.Add(new Person { Name = "Wade Wilson", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1973, 10, 9), Country = "USA" } });
				_people.Add(new Person { Name = "Jessica Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "USA" } });
				_people.Add(new Person { Name = "Jane Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "USA" } });
				_people.Add(new Person { Name = "Fulano Silva", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1983, 5, 10), Country = "BRA" } });
				
				var id = 1;
				foreach (var person in _people)
				{
					person.Id = id++;
					var email = person.Name.ToLower().Replace(" ", ".") + "@email.com";
					person.Contacts.Add(new Contact{ Type = ContactType.Email, Value = email, Comments = person.Name + "'s email" });
				}
				
				return _people;
			}
		}
		
		[TestCase(TestName="Build expression from an empty filter: should return all records")]
        public void BuilderWithEmptyFilter()
        {
        	var filter = new Filter<Person>();
            var people = People.Where(filter);
            Assert.That(people.Count(), Is.EqualTo(6));
        }
        
		[TestCase(TestName="Build expression from an empty filter (but using implict cast): should return all records")]
        public void BuilderWithEmptyFilterUsingImplicitCast()
        {
        	var filter = new Filter<Person>();
            var people = People.Where(filter);
            Assert.That(people.Count(), Is.EqualTo(6));
        }
        
        [TestCase(TestName="Build expression from a filter with simple statements")]
        public void BuilderWithSimpleFilterStatements()
        {
        	var filter = new Filter<Person>();
        	filter.By("Name", Operation.EndsWith, "Doe").Or.By("Gender", Operation.Equals, PersonGender.Female);
            var people = People.Where(filter);
            Assert.That(people.Count(), Is.EqualTo(4));
        }
        
        [TestCase(TestName="Build expression from a filter with property chain filter statements")]
        public void BuilderWithPropertyChainFilterStatements()
        {
        	var filter = new Filter<Person>();
        	filter.By("Birth.Country", Operation.Equals, "USA", FilterStatementConnector.Or);
        	filter.By("Birth.Date", Operation.LessThanOrEquals, new DateTime(1980, 1, 1), FilterStatementConnector.Or);
        	filter.By("Name", Operation.Contains, "Doe");
            var people = People.Where(filter);
            Assert.That(people.Count(), Is.EqualTo(5));
        }
        
        [TestCase(TestName="Build expression from a filter with property list filter statements")]
        public void BuilderWithPropertyListFilterStatements()
        {
        	var filter = new Filter<Person>();
        	filter.By("Contacts[Type]", Operation.Equals, ContactType.Email).And.By("Contacts[Value]", Operation.StartsWith, "jane");
            var people = People.Where(filter);
            Assert.That(people.Count(), Is.EqualTo(2));
        }
        
        [TestCase(TestName="Build expression from a filter statement with a list of values")]
        public void BuilderWithFilterStatementWithListOfValues()
        {
        	var filter = new Filter<Person>();
        	filter.By("Id", Operation.Contains, new []{ 1, 2, 4, 5 });
            var people = People.Where(filter);
            Assert.That(people.Count(), Is.EqualTo(4));
        }
        
        [TestCase(TestName="Build expression with an empty 'order by'")]
        public void BuilderWithEmptyOrderBy()
        {
        	var sortExpression = new SortExpression<Person>();
        	var firstPersonId = People.First().Id;
        	People.OrderBy(sortExpression);
        	Assert.That(People.First().Id, Is.EqualTo(firstPersonId));
        }
        
        [TestCase(TestName="Build expression ordering by only one property")]
        public void BuilderOrderingByOneProperty()
        {
        	var sortExpression = new SortExpression<Person>();
        	sortExpression.By("Name");
        	var sortedPeople = People.OrderBy(sortExpression);
        	Assert.That(sortedPeople.First().Name, Is.EqualTo("Fulano Silva"));
        	Assert.That(sortedPeople.Last().Name, Is.EqualTo("Wade Wilson"));
        }
        
        [TestCase(TestName="Build expression ordering by two properties")]
        public void BuilderOrderingByTwoProperties()
        {
        	var sortExpression = new SortExpression<Person>();
        	sortExpression.By("Gender").By("Name", SortDirection.Descending);
        	var sortedPeople = People.OrderBy(sortExpression);
        	Assert.That(sortedPeople.First().Name, Is.EqualTo("Wade Wilson"));
        	Assert.That(sortedPeople.Last().Name, Is.EqualTo("Jane Doe"));
        }
	}
}
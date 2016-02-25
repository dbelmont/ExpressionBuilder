using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionBuilder.Builder;
using ExpressionBuilder.Builder.Generic;
using ExpressionBuilder.Models;
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
        	var expr = filter.BuildExpression();
            var people = People.Where(expr.Compile());
            Assert.That(people.Count(), Is.EqualTo(6));
        }
        
        [TestCase(TestName="Build expression from a filter with simple statements")]
        public void BuilderWithSimpleFilterStatements()
        {
        	var filter = new Filter<Person>();
        	filter.By("Name", Operation.EndsWith, "Doe").Or.By("Gender", Operation.Equals, PersonGender.Female);
        	var expr = filter.BuildExpression();
            var people = People.Where(expr.Compile());
            Assert.That(people.Count(), Is.EqualTo(4));
        }
        
        [TestCase(TestName="Build expression from a filter with property chain filter statements")]
        public void BuilderWithPropertyChainFilterStatements()
        {
        	var filter = new Filter<Person>();
        	filter.By("Birth.Country", Operation.Equals, "USA", FilterStatementConnector.Or);
        	filter.By("Birth.Date", Operation.LessThanOrEquals, new DateTime(1980, 1, 1), FilterStatementConnector.Or);
        	filter.By("Name", Operation.Contains, "Doe");
        	var expr = filter.BuildExpression();
            var people = People.Where(expr.Compile());
            Assert.That(people.Count(), Is.EqualTo(5));
        }
        
        [TestCase(TestName="Build expression from a filter with property list filter statements")]
        public void BuilderWithPropertyListFilterStatements()
        {
        	var filter = new Filter<Person>();
        	filter.By("Contacts[Type]", Operation.Equals, ContactType.Email).And.By("Contacts[Value]", Operation.StartsWith, "jane");
        	var expr = filter.BuildExpression();
            var people = People.Where(expr.Compile());
            Assert.That(people.Count(), Is.EqualTo(2));
        }
        
        [TestCase(TestName="Build expression from a filter statement with a list of values")]
        public void BuilderWithFilterStatementWithListOfValues()
        {
        	var filter = new Filter<Person>();
        	filter.By("Id", Operation.Contains, new []{ 1, 2, 4, 5 });
        	var expr = filter.BuildExpression();
            var people = People.Where(expr.Compile());
            Assert.That(people.Count(), Is.EqualTo(4));
        }
	}
}
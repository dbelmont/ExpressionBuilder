using System;
using System.Linq;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Test.Models;
using NUnit.Framework;

namespace ExpressionBuilder.Test
{
	[TestFixture]
	public class FilterTest
	{
		[TestCase(TestName="Should be able to add statements to a filter")]
		public void FilterShouldAddStatement()
		{
			var filter = new Filter<Person>();
			filter.By("Name", Operation.Contains, "John");
			Assert.That(filter.Statements.Count(), Is.EqualTo(1));
			Assert.That(filter.Statements.First().PropertyName, Is.EqualTo("Name"));
			Assert.That(filter.Statements.First().Operation, Is.EqualTo(Operation.Contains));
			Assert.That(filter.Statements.First().Value, Is.EqualTo("John"));
			Assert.That(filter.Statements.First().Connector, Is.EqualTo(FilterStatementConnector.And));
		}
		
		[TestCase(TestName="Should be able to remove all statements of a filter")]
		public void FilterShouldRemoveStatement()
		{
			var filter = new Filter<Person>();
			Assert.That(filter.Statements.Count(), Is.EqualTo(0));
			
			filter.By("Name", Operation.Contains, "John").Or.By("Birth.Country", Operation.Equals, "USA");
			Assert.That(filter.Statements.Count(), Is.EqualTo(2));
			
			filter.Clear();
			Assert.That(filter.Statements.Count(), Is.EqualTo(0));
		}
		
		[TestCase(TestName="Only the 'Contains' operation should support arrays as parameters")]
		public void OnlyContainsOperationShouldSupportArraysAsParameters()
		{
			var filter = new Filter<Person>();
			Assert.Throws<ArgumentException>(() => filter.By("Id", Operation.Equals, new []{ 1, 2, 3, 4 }), "Only 'Operacao.Contains' supports arrays as parameters.");
		}
		
		[TestCase(TestName="Should be able to 'read' a filter as a string")]
		public void FilterToString()
		{
			var filter = new Filter<Person>();
			filter.By("Name", Operation.Contains, "John").Or.By("Birth.Country", Operation.Equals, "USA");
			Assert.That(filter.ToString(), Is.EqualTo("Name Contains John Or Birth.Country Equals USA"));
		}
	}
}

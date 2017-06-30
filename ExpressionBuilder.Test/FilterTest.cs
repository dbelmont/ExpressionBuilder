using System;
using System.Linq;
using ExpressionBuilder.Common;
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
			Assert.That(filter.Statements.First().PropertyId, Is.EqualTo("Name"));
			Assert.That(filter.Statements.First().Operation, Is.EqualTo(Operation.Contains));
			Assert.That(filter.Statements.First().Value, Is.EqualTo("John"));
			Assert.That(filter.Statements.First().Connector, Is.EqualTo(FilterStatementConnector.And));
		}
		
		[TestCase(TestName="Should be able to remove all statements of a filter")]
		public void FilterShouldRemoveStatement()
		{
			var filter = new Filter<Person>();
			Assert.That(filter.Statements.Count(), Is.EqualTo(0));
			
			filter.By("Name", Operation.Contains, "John").Or.By("Birth.Country", Operation.EqualTo, "USA");
			Assert.That(filter.Statements.Count(), Is.EqualTo(2));
			
			filter.Clear();
			Assert.That(filter.Statements.Count(), Is.EqualTo(0));
		}
		
		[TestCase(TestName="Only the 'Contains' and the 'In' operations should support arrays as parameters")]
		public void OnlyContainsOperationShouldSupportArraysAsParameters()
		{
			var filter = new Filter<Person>();
			Assert.Throws<ArgumentException>(() => filter.By("Id", Operation.EqualTo, new []{ 1, 2, 3, 4 }), "Only 'Operacao.Contains' and 'Operacao.In' support arrays as parameters.");
		}
		
		[TestCase(TestName="Should be able to 'read' a double-valued filter as a string")]
		public void DoubleValuedFilterToString()
		{
			var filter = new Filter<Person>();
			filter.By("Id", Operation.Between, 1, 3).Or.By("Birth.Country", Operation.EqualTo, "USA");
			Assert.That(filter.ToString(), Is.EqualTo("Id Between 1 And 3 Or Birth.Country EqualTo USA"));
		}

		[TestCase(TestName="Should be able to 'read' a single-valued filter as a string")]
		public void SingleValuedFilterToString()
		{
			var filter = new Filter<Person>();
			filter.By("Name", Operation.Contains, "John").Or.By("Birth.Country", Operation.EqualTo, "USA");
			Assert.That(filter.ToString(), Is.EqualTo("Name Contains John Or Birth.Country EqualTo USA"));
		}

		[TestCase(TestName="Should be able to 'read' a no-valued filter as a string")]
		public void NoValuedFilterToString()
		{
			var filter = new Filter<Person>();
			filter.By("Name", Operation.IsNotNull).Or.By("Birth.Country", Operation.EqualTo, "USA");
			Assert.That(filter.ToString(), Is.EqualTo("Name IsNotNull Or Birth.Country EqualTo USA"));
		}
	}
}

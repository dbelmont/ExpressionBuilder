using System;
using System.Linq;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Test.Models;
using NUnit.Framework;

namespace ExpressionBuilder.Test
{
	[TestFixture]
	public class OrderByTest
	{
		[TestCase(TestName="Should be able to add an element to a sort expression")]
		public void OrderByShouldAddElement()
		{
			var order = new SortExpression<Person>();
			order.By("Name", SortDirection.Ascending)
				.By("Birth.Date", SortDirection.Descending);
			Assert.That(order.Elements.Count(), Is.EqualTo(2));
			Assert.That(order.Elements.First().PropertyName, Is.EqualTo("Name"));
			Assert.That(order.Elements.Last().Direction, Is.EqualTo(SortDirection.Descending));
		}
		
		[TestCase(TestName="Should be able to remove all elements of a 'order by'")]
		public void OrderByShouldRemoveStatement()
		{
			var order = new SortExpression<Person>();
			Assert.That(order.Elements.Count(), Is.EqualTo(0));
			
			order.By("Name", SortDirection.Ascending).By("Birth.Country", SortDirection.Descending);
			Assert.That(order.Elements.Count(), Is.EqualTo(2));
			
			order.Clear();
			Assert.That(order.Elements.Count(), Is.EqualTo(0));
		}
	}
}
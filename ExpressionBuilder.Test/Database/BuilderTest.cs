#if (NETSTANDARD2_0 || NETSTANDARD2_1 || NETSTANDARD2_2 || NETCOREAPP2_0 || NETCOREAPP2_1 || NETCOREAPP2_2)

using ExpressionBuilder.Test.NetCore.Database;

#else

using DbContext;

#endif

using ExpressionBuilder.Common;
using ExpressionBuilder.Generics;
using ExpressionBuilder.Operations;
using NUnit.Framework;
using System;
using System.Linq;

namespace ExpressionBuilder.Test.Database
{
    [TestFixture(Category = "Database")]
    public class BuilderTest
    {
        private readonly DbDataContext db = new DbDataContext();

        [TestCase(TestName = "Filter without statements")]
        public void FilterWithoutStatements()
        {
            var filter = new Filter<Products>();
            var products = db.Products.Where(filter);
            var solution = db.Products.Where(p => true);

            Assert.That(products, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter with simple statement")]
        public void FilterWithSimpleStatement()
        {
            var filter = new Filter<Products>();
            filter.By("Discontinued", Operation.EqualTo, true);
            var products = db.Products.Where(filter);
            var solution = db.Products.Where(p => p.Discontinued);

            Assert.That(products, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter casting the value to object")]
        public void FilterCastingTheValueToObject()
        {
            var filter = new Filter<Products>();
            filter.By("UnitPrice", Operation.GreaterThan, (object)new Decimal(100));
            var products = db.Products.Where(filter);
            var solution = db.Products.Where(p => p.UnitPrice > 100);
            Assert.That(products, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter with property chain filter statements")]
        public void FilterWithPropertyChainFilterStatements()
        {
            var filter = new Filter<Products>();
            filter.By("Categories.CategoryName", Operation.EqualTo, "Beverages", default(string), Connector.Or);
            filter.By("ProductID", Operation.In, new[] { 1, 2, 4, 5 });
            var products = db.Products.Where(filter);
            var solution = db.Products.Where(p => (p.Categories != null && p.Categories.CategoryName != null && p.Categories.CategoryName.Trim().ToLower().Equals("beverages")) ||
                                                  new[] { 1, 2, 4, 5 }.Contains(p.ProductID));
            Assert.That(products, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter with property list filter statements")]
        public void FilterWithPropertyListFilterStatements()
        {
            var filter = new Filter<Products>();
            filter.By("OrderDetails[Discount]", Operation.GreaterThan, 0F).And.By("Categories.CategoryName", Operation.StartsWith, " Con ");
            var people = db.Products.Where(filter);
            var solution = db.Products.Where(p => p.OrderDetails != null && p.OrderDetails.Any(o => o.Discount > 0F) &&
                                             (p.Categories != null && p.Categories.CategoryName != null && p.Categories.CategoryName.Trim().ToLower().StartsWith("con")));
            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter statement with a list of values")]
        public void FilterWithFilterStatementWithListOfValues()
        {
            var filter = new Filter<Products>();
            filter.By("ProductID", Operation.In, new[] { 1, 2, 4, 5 });
            var people = db.Products.Where(filter);
            var solution = db.Products.Where(p => new[] { 1, 2, 4, 5 }.Contains(p.ProductID));
            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter with a single filter statement using a between operation")]
        public void BuilderWithSingleFilterStatementWithBetween()
        {
            var filter = new Filter<Products>();
            filter.By("ProductID", Operation.Between, 2, 5);
            var people = db.Products.Where(filter);
            var solution = db.Products.Where(p => p.ProductID >= 2 && p.ProductID <= 5);
            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter using 'IsNull' operator")]
        public void BuilderUsingIsNullOperation()
        {
            var filter = new Filter<Orders>();
            filter.By("ShipRegion", Operation.IsNull);
            var people = db.Orders.Where(filter);
            var solution = db.Orders.Where(p => p.ShipRegion == null);
            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter using 'IsNotNull' operator on an inner property of a list of values")]
        public void BuilderUsingIsNotNullOperationOnAnInnerProperty()
        {
            var filter = new Filter<Products>();
            filter.By("OrderDetails[Orders.ShipRegion]", Operation.IsNotNull);
            var people = db.Products.Where(filter);
            var solution = db.Products.Where(p => p.OrderDetails != null && p.OrderDetails.Any(o => o.Orders.ShipRegion != null));
            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter using complex expressions (fluent interface)", Category = "ComplexExpressions")]
        public void BuilderUsingComplexExpressionsFluentInterface()
        {
            var filter = new Filter<Products>();
            filter.By("SupplierID", Operation.EqualTo, 1)
                .And
                .Group.By("CategoryID", Operation.EqualTo, 1).Or.By("CategoryID", Operation.EqualTo, 2);
            var people = db.Products.Where(filter);
            var solution = db.Products.Where(p => p.SupplierID == 1 && (p.CategoryID == 1 || p.CategoryID == 2));

            Assert.That(people, Is.EquivalentTo(solution));
        }

        [TestCase(TestName = "Filter using complex expressions", Category = "ComplexExpressions")]
        public void BuilderUsingComplexExpressions()
        {
            var filter = new Filter<Products>();
            filter.By("SupplierID", Operation.EqualTo, 1);
            filter.StartGroup();
            filter.By("CategoryID", Operation.EqualTo, 1, Connector.Or);
            filter.By("CategoryID", Operation.EqualTo, 2);
            var people = db.Products.Where(filter);
            var solution = db.Products.Where(p => p.SupplierID == 1 && (p.CategoryID == 1 || p.CategoryID == 2));

            Assert.That(people, Is.EquivalentTo(solution));
        }
    }
}
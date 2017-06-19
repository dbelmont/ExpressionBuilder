using ExpressionBuilder.Builders;
using ExpressionBuilder.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Test
{
    [TestFixture]
    public class HelperTests
    {
        [TestCase(typeof(int), TestName ="Get supported operations for int")]
        [TestCase(typeof(uint), TestName ="Get supported operations for number uint")]
        [TestCase(typeof(byte), TestName = "Get supported operations for number byte")]
        [TestCase(typeof(sbyte), TestName = "Get supported operations for number sbyte")]
        [TestCase(typeof(short), TestName = "Get supported operations for number short")]
        [TestCase(typeof(ushort), TestName = "Get supported operations for number ushort")]
        [TestCase(typeof(long), TestName = "Get supported operations for number long")]
        [TestCase(typeof(ulong), TestName = "Get supported operations for number ulong")]
        [TestCase(typeof(Single), TestName = "Get supported operations for number Single")]
        [TestCase(typeof(double), TestName = "Get supported operations for number double")]
        [TestCase(typeof(decimal), TestName = "Get supported operations for number decimal")]
        public void SupportedOperationsForNumbers(Type numberType)
        {
            var definitions = new OperationHelper();
            var numberOperations = new List<Operation> { Operation.Equals, Operation.NotEquals, Operation.GreaterThan, Operation.GreaterThanOrEquals, Operation.LessThan, Operation.LessThanOrEquals };
            var operations = definitions.GetSupportedOperations(numberType);
            Assert.That(operations, Is.EqualTo(numberOperations));
        }

        [TestCase(typeof(string), TestName = "Get supported operations for string")]
        [TestCase(typeof(char), TestName = "Get supported operations for char")]
        public void SupportedOperationsForText(Type textType)
        {
            var definitions = new OperationHelper();
            var textOperations = new List<Operation> { Operation.Equals, Operation.Contains, Operation.EndsWith, Operation.NotEquals, Operation.StartsWith };
            var operations = definitions.GetSupportedOperations(textType);
            Assert.That(operations, Is.EqualTo(textOperations));
        }
        
        [TestCase(TestName = "Get supported operations for dates")]
        public void SupportedOperationsForDates()
        {
            var definitions = new OperationHelper();
            var dateOperations = new List<Operation> { Operation.Equals, Operation.NotEquals, Operation.GreaterThan, Operation.GreaterThanOrEquals, Operation.LessThan, Operation.LessThanOrEquals };
            var operations = definitions.GetSupportedOperations(typeof(DateTime));
            Assert.That(operations, Is.EqualTo(dateOperations));
        }
        
        [TestCase(TestName = "Get supported operations for bool")]
        public void SupportedOperationsForBool()
        {
            var definitions = new OperationHelper();
            var booleanOperations = new List<Operation> { Operation.Equals, Operation.NotEquals };
            var operations = definitions.GetSupportedOperations(typeof(bool));
            Assert.That(operations, Is.EqualTo(booleanOperations));
        }
    }
}
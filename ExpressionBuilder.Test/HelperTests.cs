using ExpressionBuilder.Common;
using ExpressionBuilder.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Test
{
    [TestFixture]
    public class HelperTests
    {
        [TestCase(typeof(int), TestName ="Get supported operations for number int")]
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
        [TestCase(typeof(int[]), TestName = "Get supported operations for an array of int")]
        public void SupportedOperationsForNumbers(Type numberType)
        {
            var definitions = new OperationHelper();
            var numberOperations = new List<Operation> { Operation.EqualTo, Operation.NotEqualTo, Operation.GreaterThan, Operation.GreaterThanOrEqualTo,
                                                         Operation.LessThan, Operation.LessThanOrEqualTo, Operation.Between };

            if (numberType.IsArray)
            {
                numberOperations.Add(Operation.In);
            }

            var operations = definitions.SupportedOperations(numberType);
            Assert.That(operations, Is.EquivalentTo(numberOperations));
        }

        [TestCase(typeof(string), TestName = "Get supported operations for string")]
        [TestCase(typeof(char), TestName = "Get supported operations for char")]
        [TestCase(typeof(string[]), TestName = "Get supported operations for an array string")]
        public void SupportedOperationsForText(Type textType)
        {
            var definitions = new OperationHelper();
            var textOperations = new List<Operation> { Operation.EqualTo, Operation.Contains, Operation.EndsWith, Operation.NotEqualTo, Operation.StartsWith,
                                                       Operation.IsEmpty, Operation.IsNotEmpty, Operation.IsNotNull, Operation.IsNotNullNorWhiteSpace, Operation.IsNull,
                                                       Operation.IsNullOrWhiteSpace };

            if (textType.IsArray)
            {
                textOperations.Add(Operation.In);
            }

            var operations = definitions.SupportedOperations(textType);
            Assert.That(operations, Is.EquivalentTo(textOperations));
        }
        
        [TestCase(TestName = "Get supported operations for dates")]
        public void SupportedOperationsForDates()
        {
            var definitions = new OperationHelper();
            var dateOperations = new List<Operation> { Operation.Between, Operation.EqualTo, Operation.NotEqualTo, Operation.GreaterThan, Operation.GreaterThanOrEqualTo,
                                                       Operation.LessThan, Operation.LessThanOrEqualTo };
            var operations = definitions.SupportedOperations(typeof(DateTime));
            Assert.That(operations, Is.EquivalentTo(dateOperations));
        }
        
        [TestCase(TestName = "Get supported operations for bool")]
        public void SupportedOperationsForBool()
        {
            var definitions = new OperationHelper();
            var booleanOperations = new List<Operation> { Operation.EqualTo, Operation.NotEqualTo };
            var operations = definitions.SupportedOperations(typeof(bool));
            Assert.That(operations, Is.EquivalentTo(booleanOperations));
        }

        [TestCase(TestName = "Get supported operations for nullable types")]
        public void SupportedOperationsForNullable()
        {
            var definitions = new OperationHelper();
            var nullableOperations = new List<Operation> { Operation.IsNotNull, Operation.IsNull };
            var numberOperations = new List<Operation> { Operation.EqualTo, Operation.NotEqualTo, Operation.GreaterThan, Operation.GreaterThanOrEqualTo,
                                                         Operation.LessThan, Operation.LessThanOrEqualTo, Operation.Between };
            nullableOperations.AddRange(numberOperations);
            var operations = definitions.SupportedOperations(typeof(int?));
            Assert.That(operations, Is.EquivalentTo(nullableOperations));
        }
    }
}
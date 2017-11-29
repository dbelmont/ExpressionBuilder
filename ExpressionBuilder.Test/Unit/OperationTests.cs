using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ExpressionBuilder.Common;
using ExpressionBuilder.Resources;
using NUnit.Framework;

namespace ExpressionBuilder.Test.Unit
{
    [TestFixture]
    public class OperationTests
    {
        List<Operation> operations = new List<Operation> { Operation.EqualTo, Operation.Contains, Operation.StartsWith, Operation.EndsWith, Operation.NotEqualTo, Operation.GreaterThan, Operation.GreaterThanOrEqualTo, Operation.LessThan,
            Operation.LessThanOrEqualTo, Operation.Between, Operation.IsNull, Operation.IsEmpty, Operation.IsNullOrWhiteSpace, Operation.IsNotNull, Operation.IsNotEmpty, Operation.IsNotNullNorWhiteSpace, Operation.In, Operation.DoesNotContain };

        List<string> operationsNames = new List<string> {
            "Ends with", "Equal to", "Greater than", "Greater than or equals", "Is empty", "Is not empty", "Is not null", "Is not null nor whitespace", "Is null", "Is null or whitespace", "Less than", "Less than or equals",
            "Not equal to", "Starts with", "Does not contain", "Between", "Contains", "In"
        };

        List<string> operationsNamesptBR = new List<string> {
            "entre", "contem", "termina com", "igual", "maior que", "maior que ou igual", "em", "é vazio", "não é vazio", "não é nulo", "não é nulo nem vazio", "é nulo", "é nulo ou vazio", "menor que","menor que ou igual", "diferente",
            "começa com", "não contem"
        };

        [TestCase("", TestName = "Should load operation description from resource file")]
        [TestCase("pt-BR", TestName = "Should load operation description from resource file")]
        public void ShouldLoadOperationDescriptionFromResourceFile(string cultureName)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            if (cultureName == "pt-BR")
                Assert.That(operations.Select(o => o.GetDescription(Resources.Operations.ResourceManager)).OrderBy(o => o), Is.EquivalentTo(operationsNamesptBR.OrderBy(o => o)));
            else
                Assert.That(operations.Select(o => o.GetDescription(Resources.Operations.ResourceManager)).OrderBy(o => o), Is.EquivalentTo(operationsNames.OrderBy(o => o)));
        }
    }
}
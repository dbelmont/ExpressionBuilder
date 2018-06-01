using ExpressionBuilder.Helpers;
using ExpressionBuilder.Resources;
using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ExpressionBuilder.Test.Integration
{
    [TestFixture]
    public class OperationTests
    {
        private readonly List<string> operationsNames = new List<string> {
            "Ends with", "Equal to", "Greater than", "Greater than or equals", "Is empty", "Is not empty", "Is not null", "Is not null nor whitespace", "Is null", "Is null or whitespace", "Less than", "Less than or equals",
            "Not equal to", "Starts with", "Does not contain", "Between", "Contains", "In"
        };

        private readonly List<string> operationsNamesptBR = new List<string> {
            "entre", "contem", "termina com", "igual", "maior que", "maior que ou igual", "em", "é vazio", "não é vazio", "não é nulo", "não é nulo nem vazio", "é nulo", "é nulo ou vazio", "menor que","menor que ou igual", "diferente",
            "começa com", "não contem"
        };

        [TestCase("", TestName = "Should load operation description from resource file")]
        [TestCase("pt-BR", TestName = "Should load operation description from resource file")]
        public void ShouldLoadOperationDescriptionFromResourceFile(string cultureName)
        {
            var operations = new OperationHelper().Operations;
            var culture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            if (cultureName == "pt-BR")
            {
                Assert.That(
                    operations.Select(o => o.GetDescription(Resources.Operations.ResourceManager)).OrderBy(o => o),
                    Is.EquivalentTo(operationsNamesptBR.OrderBy(o => o)));
            }
            else
            {
                Assert.That(
                    operations.Select(o => o.GetDescription(Resources.Operations.ResourceManager)).OrderBy(o => o),
                    Is.EquivalentTo(operationsNames.OrderBy(o => o)));
            }
        }
    }
}
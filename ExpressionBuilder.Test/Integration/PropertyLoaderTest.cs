using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Resources;
using ExpressionBuilder.Test.Models;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace ExpressionBuilder.Test.Integration
{
    [TestFixture]
    public class PropertyLoaderTest
    {
        private readonly List<string> propertyIds = new List<string>
        {
            "Id", "Name", "Gender", "Salary", "Birth.Date", "Birth.DateOffset", "Birth.Age", "Birth.Country", "Contacts[Type]", "Contacts[Value]", "Contacts[Comments]", "Employer.Name", "Employer.Industry", "EmployeeReferenceNumber"
        };

        private readonly List<string> propertyNames = new List<string>
        {
            "Id", "Name", "Gender", "Salary", "Date of Birth", "DateOffset","Age", "Country of Birth", "Contact's Type", "Contact's Value", "Contact's Comments", "Employer's Name", "Employer's Industry", "EmployeeReferenceNumber"
        };

        private readonly List<string> propertyNamesptBr = new List<string>
        {
            "Id", "Nome", "Sexo", "Salário", "Data de nascimento", "DateOffset", "Idade", "País de origem", "Tipo de contato", "Valor do contato", "Comentários do contato", "Nome do empregador", "Indústria do empregador", "EmployeeReferenceNumber"
        };

#if NETCOREAPP2_0
        [TestCase("", TestName = "Loading properties' info", Ignore = "Having some trouble making this work properly")]
        [TestCase("pt-BR", TestName = "Loading properties' info [Portuguese]", Ignore = "Having some trouble making this work properly")]
#else

        [TestCase("", TestName = "Loading properties' info")]
        [TestCase("pt-BR", TestName = "Loading properties' info [Portuguese]")]
#endif
        public void PropertyLoaderLoadProperties(string cultureName)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            IPropertyCollection loader = new PropertyCollection(typeof(Person), Resources.Person.ResourceManager);
            var properties = loader.ToList();
            var ids = properties.Select(p => p.Id);
            var names = properties.Select(p => p.Name);

            Assert.That(ids, Is.EquivalentTo(propertyIds));

            if (cultureName == "pt-BR")
            {
                Assert.That(names, Is.EquivalentTo(propertyNamesptBr));
            }
            else
            {
                Assert.That(names, Is.EquivalentTo(propertyNames));
            }
        }

#if NETCOREAPP2_0
        [TestCase(TestName = "The string representation of a property should be its name followed by its id", Ignore = "Having some trouble making this work properly")]
#else

        [TestCase(TestName = "The string representation of a property should be its name followed by its id")]
#endif
        public void PropertyToString()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(string.Empty);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            IPropertyCollection loader = new PropertyCollection(typeof(Person));
            var properties = loader.LoadProperties(Resources.Person.ResourceManager);
            foreach (var property in properties)
            {
                property.ToString().Should().Be(string.Format("{0} ({1})", property.Name, property.Id));
            }
        }

        [TestCase(TestName = "Checking the loading of classes' properties and fields")]
        public void LoadingPropertiesAndFields()
        {
            IPropertyCollection properties = new PropertyCollection(typeof(Person));
            var id = properties.ToList().Single(p => p.Id == "Id");
            var name = properties.ToList().Single(p => p.Id == "Name");
            id.MemberType.Should().Be(typeof(int));
            name.MemberType.Should().Be(typeof(string));
        }

        [TestCase(TestName = "Checking if all properties and fields were loaded")]
        public void LoadingAllPropertiesAndFields()
        {
            var properties = new PropertyCollection(typeof(Person)).ToList();
            properties.Count.Should().Be(propertyIds.Count);
            properties.Select(p => p.Id).Should().BeEquivalentTo(propertyIds);
        }
    }
}
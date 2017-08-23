# Expression Builder
In short words, this library basically provides you with a simple way to create lambda expressions to filter lists and database queries by delivering an easy-to-use fluent interface that enables creation, storage and transmission of those filters. What could be used to from helping rendering WebApi requests parameters into expressions, to creating advanced search screens with the capability to save and re-run those filters.  If you would like more details on how it works, please, check out the article [Build Lambda Expression Dinamically](https://www.codeproject.com/Articles/1079028/Build-Lambda-Expressions-Dynamically).

# How to use it
Let us imagine we have classes like this...
```CSharp
public enum PersonGender
{
    Male,
    Female
}

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public PersonGender Gender { get; set; }
    public BirthData Birth { get; set; }
    public List<Contact> Contacts { get; private set; }
    public Company Employer { get; set; }

    public class BirthData
    {
        public DateTime Date { get; set; }
        public string Country { get; set; }
    }

    public class Company {
        public string Name { get; set; }
        public string Industry { get; set; }
    }
}

public enum ContactType
{
    Telephone,
    Email
}

public class Contact
{
    public ContactType Type { get; set; }
    public string Value { get; set; }
    public string Comments { get; set; }
}
```

Now, what about being able query a list of `Person` in a way like this:
```CSharp
var filter = new Filter<Person>();
filter.By("Id", Operation.Between, 2, 4,  FilterStatementConnector.And);
filter.By("Contacts[Value]", Operation.EndsWith, "@email.com", default(string), FilterStatementConnector.And);
filter.By("Birth.Country", Operation.IsNotNull, default(string), default(string),  FilterStatementConnector.Or);
filter.By("Name", Operation.Contains, " John");
var people = People.Where(filter);

//or like this...

var filter = new Filter<Person>();
filter.By("Id", Operation.Between, 2, 4)
      .And.By("Birth.Country", Operation.IsNotNull)
      .And.By("Contacts[Value]", Operation.EndsWith, "@email.com")
      .Or.By("Name", Operation.Contains, " John ");
var people = People.Where(filter);
```
So that would generate an expression like this:
```CSharp
People.Where(p => (p.Id >= 2 && p.Id <= 4)
             && (p.Birth != null && p.Birth.Country != null)
             && (p.Contacts != null && p.Contacts.Any(c => c.Value.Trim().ToLower().EndsWith("@email.com")))
             || (p.Name != null  && p.Name.Trim().ToLower().Contains("john")));
```

## Conventions
The convention around the properties names is, probably, the heart of this project. It defines the way in which the properties are addressed, how to reference a property, or the property of a property, or even the property of an item in a list property of the type being filtered.

How to address a property:
1. *Any value property of the type being filtered:* just mention its name (e.g. `Id`, `Name`, `Gender`, etc.)
2. *Any value property of a reference property of the type being filtered:* use the "dot notation" (e.g. `Birth.Date`, `Company.Name`, etc.)
3. *Any value property of an item in a list property:* mention the name of the list property followed by the name of its property between brackets (e.g. `Contacts[Type]`, `Contacts[Value]`)

## Supported types/operations
The operations are grouped together into logical type groups to simplify the association of a type with an operation:
* Default
  * EqualTo
  * NotEqualTo
* Text
  * Contains
  * EndsWith
  * EqualTo
  * IsEmpty
  * IsNotEmpty
  * IsNotNull
  * IsNotNullNorWhiteSpace
  * IsNull
  * IsNullOrWhiteSpace
  * NotEqualTo
  * StartsWith
* Number
  * Between
  * EqualTo
  * GreaterThan
  * GreaterThanOrEqualTo
  * LessThan
  * LessThanOrEqualTo
  * NotEqualTo
* Boolean
  * EqualTo
  * NotEqualTo
* Date
  * Between
  * EqualTo
  * GreaterThan
  * GreaterThanOrEqualTo
  * LessThan
  * LessThanOrEqualTo
  * NotEqualTo

This way, when a type is associated with a type group, that type will "inherit" the list of supported operations from its group.

While compiling the filter into a lambda expression, the expression builder will validate if the selected operation is supported by the property's type and throw an exception if it's not. To overcome situations in which you would like to add support to a specific type, you may configure it by adding the following to your config file:
```Xml
<configuration>
  ...
  <configSections>
    <section name="ExpressionBuilder" type="ExpressionBuilder.Configuration.ExpressionBuilderConfig, ExpressionBuilder" />
  </configSections>

  ...

  <ExpressionBuilder>
    <SupportedTypes>
      <add typeGroup="Date" type="System.DateTimeOffset" />
    </SupportedTypes>
  </ExpressionBuilder>
  ...
</configuration>
```

## Globalization support
You just need to perform some easy steps to add globalization support to the UI:
1. Add a resource file to the project, naming it after the type you'll create your filter to (e.g. `Person.resx`);
2. Add one entry for each property you'd like to globalize following the conventions (previously mentioned), but replacing the dots (`.`) and the brackets (`[`, `]`) by underscores (`_`);
3. You can globalize the operations on a similar way as well by adding a resources file named `Operations.resx`;
4. For the properties, you'll instantiate a `PropertyCollection` : `new PropertyCollection(typeof(Person), Resources.Person.ResourceManager)`. That will give you a collection of objects with three members:
  * `Id`: The conventionalised property identifier (previously mentioned)
  * `Name`: The resources file matching value for the property id
  * `Info`: The `PropertyInfo` object for the property
5. And for the operations, you have an extension method: `Operation.GreaterThanOrEqualTo.GetDescription(Resources.Operations.ResourceManager)`.

#### Note on globalization
Any property or operation not mentioned at the resources files will be replaced by its conventionalised property identifier.

# License
Copyright 2017 David Belmont

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at [LICENSE](LICENSE)

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

Icon by [Alina Oleynik](https://thenounproject.com/dorxela), Ukraine

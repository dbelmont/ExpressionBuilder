using ExpressionBuilder.Test.Models;
using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Test.Unit.Helpers
{
    public class TestData
    {
        public List<Person> People { get; private set; }

        public TestData()
        {
            var company = new Person.Company { Name = "Back to the future", Industry = "Time Traveling Agency" };

            People = new List<Person>
            {
                new Person { Name = "John Doe", Gender = PersonGender.Male, Salary=4565, Birth = new Person.BirthData { Date = new DateTime(1979, 2, 28) }, Employer = company },
                new Person { Name = "Jane Doe", Gender = PersonGender.Female, Salary=4973, Birth = new Person.BirthData { Date = new DateTime(1985, 9, 5), Country = " " } },
                new Person { Name = "Wade Wilson", Gender = PersonGender.Male, Salary=3579, Birth = new Person.BirthData { Date = new DateTime(1973, 10, 9), Country = "USA" } },
                new Person { Name = "Jessica Jones", Gender = PersonGender.Female, Salary=5000, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "usa" } },
                new Person { Name = "Jane Jones", Gender = PersonGender.Female, Salary=3500, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "AUS" } },
                new Person { Name = "Fulano Silva", Gender = PersonGender.Male, Salary=3322, Birth = new Person.BirthData { Date = new DateTime(1983, 5, 10), Country = "BRA" }, Employer = company },
                new Person { Name = "John Hancock", Gender = PersonGender.Male, Employer = company }
            };
            var id = 1;
            foreach (var person in People)
            {
                person.Id = id++;

                if (id <= 5)
                {
                    var email = person.Name.ToLower().Replace(" ", ".") + "@email.com";
                    person.Contacts.Add(new Contact { Type = ContactType.Email, Value = email, Comments = person.Name + "'s email" });
                }
            }
        }
    }
}
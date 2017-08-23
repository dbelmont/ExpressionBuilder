using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Test.Models
{
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
		
		public Person()
		{
			Contacts = new List<Contact>();
            Birth = new BirthData();
		}

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = GetType().ToString().GetHashCode();
                hash = (hash * 16777619) ^ Name.GetHashCode();
                hash = (hash * 16777619) ^ Gender.GetHashCode();

                if (Birth.Date != null)
                {
                    hash = (hash * 16777619) ^ Birth.Date.GetHashCode();
                }

                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }

        public class BirthData
		{
			public DateTime? Date { get; set; }
			public string Country { get; set; }
            public DateTimeOffset? DateOffset
            {
                get
                {
                    return Date.HasValue ? new DateTimeOffset?(Date.Value) : new DateTimeOffset?();
                }
            }
			
			public override string ToString()
			{
				return string.Format("Born at {0} in {1}", Date.Value.ToShortDateString(), Country);
			}

		}

        public class Company {
            public string Name { get; set; }
            public string Industry { get; set; }
        }
	}
}
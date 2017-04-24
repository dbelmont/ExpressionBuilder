using System;

namespace ExpressionBuilder.Test.Models
{
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
}

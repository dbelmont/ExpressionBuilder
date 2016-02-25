using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ExpressionBuilder.Builder.Generic;
using ExpressionBuilder.Controls;
using ExpressionBuilder.Models;
using ExpressionBuilder.Builder;

namespace ExpressionBuilder
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		List<Person> _people;
		
		public List<Person> People
		{
			get
			{
				_people = new List<Person>();
				_people.Add(new Person { Name = "John Doe", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1979, 2, 28), Country = "USA" } });
				_people.Add(new Person { Name = "Jane Doe", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1985, 9, 5), Country = "CYM" } });
				_people.Add(new Person { Name = "Wade Wilson", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1973, 10, 9), Country = "USA" } });
				_people.Add(new Person { Name = "Jessica Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "USA" } });
				_people.Add(new Person { Name = "Jane Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "USA" } });
				_people.Add(new Person { Name = "Fulano Silva", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1983, 5, 10), Country = "BRA" } });
				
				var id = 1;
				foreach (var person in _people)
				{
					person.Id = id++;
					var email = person.Name.ToLower().Replace(" ", ".") + "@email.com";
					person.Contacts.Add(new Contact{ Type = ContactType.Email, Value = email, Comments = person.Name + "'s email" });
				}
				
				return _people;
			}
		}
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			AddFilter();
		}
		
		protected void AddFilter()
		{
			var control = new ucFilter();
			control.TypeName = "ExpressionBuilder.Models.Person";
			control.OnAdd += UcFilterOnAdd;
			control.OnRemove += UcFilterOnRemove;
			control.Name = "filter" + pnFilters.Controls.Count;
			control.Top = pnFilters.Controls.Count * control.Height;
			
			pnFilters.Controls.Add(control);
			grid.DataSource = People;
		}
		
		void UcFilterOnAdd(object sender, EventArgs e)
		{
			AddFilter();
		}
		
		void UcFilterOnRemove(object sender, EventArgs e)
		{
			var filter = (sender as Button).Parent;
			if (pnFilters.Controls.Count > 1)
			{
				pnFilters.Controls.Remove(filter);
			}
			
			for (var i = 0; i < pnFilters.Controls.Count; i++)
			{
				var control = pnFilters.Controls[i];
				control.Top = i * control.Height;
			}
		}
		
		void ExecuteFilterF5ToolStripMenuItemClick(object sender, EventArgs e)
		{
			var filter = new Filter<Person>();
			foreach (var control in pnFilters.Controls)
			{
				var ufilter = (ucFilter)control;
				filter.By(ufilter.PropertyName, ufilter.Operation, ufilter.Value, ufilter.Conector);
			}
			
			grid.DataSource = People.Where(filter.BuildExpression().Compile()).ToList();
		}
	}
}
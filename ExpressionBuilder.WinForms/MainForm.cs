using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExpressionBuilder.WinForms.Controls;
using ExpressionBuilder.Generics;
using ExpressionBuilder.WinForms.Models;

namespace ExpressionBuilder.WinForms
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
                var company = new Person.Company { Name = "Back to the future", Industry = "Time Traveling Agency" };

                _people = new List<Person>();
                _people.Add(new Person { Name = "John Doe", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1979, 2, 28), Country = "USA" }, Employer = company });
                _people.Add(new Person { Name = "Jane Doe", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1985, 9, 5), Country = " " } });
                _people.Add(new Person { Name = "Wade Wilson", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1973, 10, 9), Country = "USA" } });
                _people.Add(new Person { Name = "Jessica Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "USA" } });
                _people.Add(new Person { Name = "Jane Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "USA" } });
                _people.Add(new Person { Name = "Fulano Silva", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1983, 5, 10), Country = "BRA" }, Employer = company });
                _people.Add(new Person { Name = "John Hancock", Gender = PersonGender.Male, Employer = company });

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
			
			AddFilter();
            grid.DataSource = People;
        }
		
		protected void AddFilter()
		{
			var control = new UcFilter();
			control.TypeName = "ExpressionBuilder.WinForms.Models.Person";
			control.OnAdd += UcFilterOnAdd;
			control.OnRemove += UcFilterOnRemove;
			control.Name = "filter" + pnFilters.Controls.Count;
			control.Top = pnFilters.Controls.Count * control.Height;
			
			pnFilters.Controls.Add(control);
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
				var ufilter = (UcFilter)control;
                if (!ufilter.ValidateChildren())
                {
                    break;
                }

				filter.By(ufilter.PropertyId, ufilter.Operation, ufilter.Value, ufilter.Value2, ufilter.Conector);
			}
			
			grid.DataSource = People.Where(filter).ToList();
		}
	}
}
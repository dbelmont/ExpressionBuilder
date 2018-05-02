using ExpressionBuilder.Generics;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.WinForms.Controls;
using ExpressionBuilder.WinForms.CustomOperations;
using ExpressionBuilder.WinForms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExpressionBuilder.WinForms
{
    /// <summary>
    /// Description of MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        private List<Person> _people;

        public List<Person> People
        {
            get
            {
                var company = new Person.Company { Name = "Back to the future", Industry = "Time Traveling Agency" };

                _people = new List<Person>();
                _people.Add(new Person { Name = "John Doe", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1979, 2, 28) }, Employer = company });
                _people.Add(new Person { Name = "Jane Doe", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1985, 9, 5), Country = " " } });
                _people.Add(new Person { Name = "Wade Wilson", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = new DateTime(1973, 10, 9), Country = "USA" } });
                _people.Add(new Person { Name = "Jessica Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "usa" } });
                _people.Add(new Person { Name = "Jane Jones", Gender = PersonGender.Female, Birth = new Person.BirthData { Date = new DateTime(1980, 12, 20), Country = "USA" } });
                _people.Add(new Person { Name = "Fulano Silva", Gender = PersonGender.Male, Birth = new Person.BirthData { Date = DateTime.Now.AddYears(-35), Country = "BRA" }, Employer = company });
                _people.Add(new Person { Name = "John Hancock", Gender = PersonGender.Male, Employer = company });

                var id = 1;
                foreach (var person in _people)
                {
                    person.Id = id++;
                    var email = person.Name.ToLower().Replace(" ", ".") + "@email.com";
                    person.Contacts.Add(new Contact { Type = ContactType.Email, Value = email, Comments = person.Name + "'s email" });
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

            Operations.Operation.LoadOperations(new List<IOperation> { new ThisDay(), new EqualTo() }, true);
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

        private void UcFilterOnAdd(object sender, EventArgs e)
        {
            AddFilter();
        }

        private void UcFilterOnRemove(object sender, EventArgs e)
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

        private void ExecuteFilterF5ToolStripMenuItemClick(object sender, EventArgs e)
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

        private void MainForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show("This WinForms application is meant to be just a USAGE EXAMPLE of the ExpressionBuilder project.\r\n\r\n" +
                            "Keep in mind that the operation 'This day' has been added as an example on how to implement your own custom operations, " +
                            "and the operation 'Equal to' has been modified in this WinForms project to be case-sensitive, " +
                            "as an example on how to overwrite default operations.\r\n\r\n" +
                            "Have fun!",
                            "Expression Builder - Example project");
        }
    }
}
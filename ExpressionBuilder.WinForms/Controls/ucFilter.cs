using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Helpers;
using ExpressionBuilder.Resources;
using ExpressionBuilder.Interfaces;

namespace ExpressionBuilder.WinForms.Controls
{
	/// <summary>
	/// Description of ucFilter.
	/// </summary>
	public partial class ucFilter : UserControl
	{
		string _typeName = "ExpressionBuilder.Models.Person";
        IPropertyCollection _properties;
		
		[Category("Data")]
		public string TypeName
		{
			get { return _typeName ?? "ExpressionBuilder.WinForms.Models.Person"; }
			set { _typeName = value; }
		}
		
		public string PropertyId
		{
			get { return (cbProperties.SelectedItem as Property).Id; }
		}
		
		public Operation Operation
		{
			get { return (Operation)Enum.Parse(typeof(Operation), (cbOperations.SelectedItem as dynamic).Id); }
		}
		
		public object Value
		{
			get
			{
                var numberOfValues = new OperationHelper().GetNumberOfValuesAcceptable(Operation);
                var defaultValue = _properties[PropertyId].Info.PropertyType.IsValueType ? Activator.CreateInstance(_properties[PropertyId].Info.PropertyType) : null;
                return numberOfValues > 0 ? GetValue("ctrlValue") : defaultValue;
			}
		}

		public object Value2
		{
			get
			{
                var numberOfValues = new OperationHelper().GetNumberOfValuesAcceptable(Operation);
                var defaultValue = _properties[PropertyId].Info.PropertyType.IsValueType ? Activator.CreateInstance(_properties[PropertyId].Info.PropertyType) : null;
                return numberOfValues == 2 ? GetValue("ctrlValue2") : defaultValue;
			}
		}
		
        private object GetValue(string ctrlName)
        {
            var ctrl = Controls[ctrlName];

            if (ctrl != null)
            {
                var property = _properties[PropertyId];
                if (property.Info.PropertyType == typeof(string)) return ctrl.Text;
                if (property.Info.PropertyType == typeof(DateTime)) return (ctrl as DateTimePicker).Value;
                if (property.Info.PropertyType == typeof(int)) return Convert.ToInt32((ctrl as NumericUpDown).Value);
                if (property.Info.PropertyType == typeof(bool)) return Boolean.Parse(ctrl.Text);
                if (property.Info.PropertyType.IsEnum) return Enum.ToObject(property.Info.PropertyType, (ctrl as DomainUpDown).SelectedItem);
            }

			return null;
        }

        public override bool ValidateChildren()
        {
            var isValid = base.ValidateChildren();

            if (String.IsNullOrWhiteSpace(cbOperations.Text))
            {
                MessageBox.Show("Please, select an operation");
                isValid = false;
            }

            return isValid;
        }

        public FilterStatementConnector Conector
		{
			get { return (FilterStatementConnector)Enum.Parse(typeof(FilterStatementConnector), cbConector.Text); }
		}
		
		public ucFilter()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		
		public event EventHandler OnAdd;
		public event EventHandler OnRemove;
		
		void UcFilterLoad(object sender, EventArgs e)
		{
			LoadProperties();
            LoadOperations();
			cbProperties.SelectedIndex = 0;
			cbConector.SelectedIndex = 0;
		}
		
		public void LoadProperties()
		{
			var type = Type.GetType(TypeName, true);
			LoadProperties(type);
            cbProperties.ValueMember = "Id";
            cbProperties.DisplayMember = "Name";
			cbProperties.DataSource = _properties;
			cbProperties.SelectedIndexChanged += cbProperties_SelectedIndexChanged;
		}

        private void LoadOperations()
        {
            LoadValueControls();

            var type = _properties[PropertyId].Info.PropertyType;
            var supportedOperations = new OperationHelper()
                                        .GetSupportedOperations(type)
                                        .Select(o => new {
                                            Id = o.ToString(),
                                            Name = o.GetDescription(Resources.Operations.ResourceManager)
                                        })
                                        .ToArray();

            cbOperations.ValueMember = "Id";
            cbOperations.DisplayMember = "Name";
            cbOperations.DataSource = supportedOperations;
        }

		void cbProperties_SelectedIndexChanged(object sender, EventArgs e)
		{
            LoadOperations();
        }
		
		void LoadValueControls()
		{
            Controls.RemoveByKey("ctrlValue");
            Controls.RemoveByKey("ctrlValue2");

            var ctrl = CreateNewControl();
			ctrl.Name = "ctrlValue";
			ctrl.Size = new Size(300, 20);
			ctrl.Location = new Point(422, 6);
			Controls.Add(ctrl);

            var ctrl2 = CreateNewControl();
			ctrl2.Name = "ctrlValue2";
			ctrl2.Size = new Size(300, 20);
			ctrl2.Location = new Point(727, 6);
			Controls.Add(ctrl2);
		}
		
        Control CreateNewControl()
        {
            var info = _properties[PropertyId].Info;
            Control ctrl = null;
            if (info.PropertyType.IsEnum || info.PropertyType == typeof(bool))
            {
                ctrl = new DomainUpDown();
                if (info.PropertyType == typeof(bool))
                {
                    (ctrl as DomainUpDown).Items.AddRange(new[] { true, false });
                }
                else
                {
                    (ctrl as DomainUpDown).Items.AddRange(Enum.GetValues(info.PropertyType));
                }
                (ctrl as DomainUpDown).SelectedItem = (ctrl as DomainUpDown).Items[0];
                (ctrl as DomainUpDown).ReadOnly = true;

            }

            if (info.PropertyType == typeof(string))
            {
                ctrl = new TextBox();
            }

            if (info.PropertyType == typeof(DateTime))
            {
                ctrl = new DateTimePicker();
            }

            if (new[] { typeof(int), typeof(double), typeof(float), typeof(decimal) }.Contains(info.PropertyType))
            {
                ctrl = new NumericUpDown();
                (ctrl as NumericUpDown).Value = 0;
            }

            if (ctrl == null) throw new Exception("Type not supported");

            return ctrl;
        }

		public IPropertyCollection LoadProperties(Type type)
		{
            return _properties = new PropertyCollection(type, Resources.Person.ResourceManager);

        }
		
		void BtnAddClick(object sender, EventArgs e)
		{
			OnAdd(sender, e);
		}
		
		void BtnRemoveClick(object sender, EventArgs e)
		{
			OnRemove(sender, e);
		}

        private void cbOperations_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlVisibility("ctrlValue", true);
            SetControlVisibility("ctrlValue2", true);

            var numberOfValues = new OperationHelper().GetNumberOfValuesAcceptable(Operation);
            switch (numberOfValues)
            {
                case 0:
                    SetControlVisibility("ctrlValue", false);
                    SetControlVisibility("ctrlValue2", false);
                    break;
                case 1:
                    SetControlVisibility("ctrlValue2", false);
                    break;
            }
        }

        private void SetControlVisibility(string controlName, bool visible)
        {
            var ctrl = Controls.Find(controlName, false).FirstOrDefault();
            
            if (ctrl != null)
            {
                ctrl.Visible = visible;
            }
        }
    }
}
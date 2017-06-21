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

namespace ExpressionBuilder.WinForms.Controls
{
	/// <summary>
	/// Description of ucFilter.
	/// </summary>
	public partial class ucFilter : UserControl
	{
		string _typeName = "ExpressionBuilder.Models.Person";
		Dictionary<string, PropertyInfo> _properties = new Dictionary<string, PropertyInfo>();
		
		[Category("Data")]
		public string TypeName
		{
			get { return _typeName ?? "ExpressionBuilder.WinForms.Models.Person"; }
			set { _typeName = value; }
		}
		
		public string PropertyName
		{
			get { return cbProperties.Text; }
		}
		
		public Operation Operation
		{
			get { return (Operation)Enum.Parse(typeof(Operation), cbOperations.Text); }
		}
		
		public object Value
		{
			get
			{
                return GetValue("ctrlValue");
			}
		}

		public object Value2
		{
			get
			{
                return GetValue("ctrlValue2");
			}
		}
		
        private object GetValue(string ctrlName)
        {
            var ctrl = Controls[ctrlName];

            if (ctrl != null)
            {
                var property = _properties[PropertyName];
                if (property.PropertyType == typeof(string)) return ctrl.Text;
                if (property.PropertyType == typeof(DateTime)) return (ctrl as DateTimePicker).Value;
                if (property.PropertyType == typeof(int)) return Convert.ToInt32((ctrl as NumericUpDown).Value);
                if (property.PropertyType == typeof(bool)) return Boolean.Parse(ctrl.Text);
                if (property.PropertyType.IsEnum) return Enum.ToObject(property.PropertyType, (ctrl as DomainUpDown).SelectedItem);
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
			cbProperties.SelectedIndex = 0;
			cbOperations.SelectedIndex = 0;
			cbConector.SelectedIndex = 0;
		}
		
		public void LoadProperties()
		{
			var type = Type.GetType(TypeName, true);
			_properties = LoadProperties(type);
			cbProperties.Items.AddRange(_properties.Keys.ToArray());
			cbProperties.SelectedIndexChanged += cbProperties_SelectedIndexChanged;
		}

		void cbProperties_SelectedIndexChanged(object sender, EventArgs e)
		{
			Controls.RemoveByKey("ctrlValue");
			Controls.RemoveByKey("ctrlValue2");
		}
		
		void LoadValueControls()
		{
            var ctrl = CreateNewControl();
			ctrl.Name = "ctrlValue";
			ctrl.Size = new Size(300, 20);
			ctrl.Location = new Point(400, 6);
			Controls.Add(ctrl);

            var ctrl2 = CreateNewControl();
			ctrl2.Name = "ctrlValue2";
			ctrl2.Size = new Size(300, 20);
			ctrl2.Location = new Point(705, 6);
			Controls.Add(ctrl2);
		}
		
        Control CreateNewControl()
        {
            var propertyKey = cbProperties.Text;
            var info = _properties[propertyKey];
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

		public Dictionary<string, PropertyInfo> LoadProperties(Type type)
		{
			var list = new Dictionary<string, PropertyInfo>();
			var properties = type.GetProperties();
			foreach (var property in properties)
			{
				if (property.PropertyType.IsValueType || property.PropertyType == typeof(String))
				{
					list.Add(property.Name, property);
					continue;
				}
				
				if (property.PropertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
				{
					var props = LoadProperties(property.PropertyType.GetGenericArguments()[0]);
					foreach (var info in props)
					{
						list.Add(property.Name + "[" + info.Key + "]", info.Value);
					}
					continue;
				}
				
				if (!property.PropertyType.IsValueType)
				{
					var props = LoadProperties(property.PropertyType);
					foreach (var info in props)
					{
						list.Add(property.Name + "." + info.Key, info.Value);
					}
					continue;
				}
			}
			
			return list;
		}
		
		void BtnAddClick(object sender, EventArgs e)
		{
			OnAdd(sender, e);
		}
		
		void BtnRemoveClick(object sender, EventArgs e)
		{
			OnRemove(sender, e);
		}

        private void cbProperties_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            var type = _properties[PropertyName].PropertyType;
            var supportedOperations = new OperationHelper()
                                        .GetSupportedOperations(type)
                                        .Select(o => o.ToString())
                                        .ToArray();
            cbOperations.Items.Clear();
            cbOperations.Items.AddRange(supportedOperations);
        }

        private void cbOperations_SelectedIndexChanged(object sender, EventArgs e)
        {
            Controls.RemoveByKey("ctrlValue");
            Controls.RemoveByKey("ctrlValue2");
            LoadValueControls();

            var operation = (Operation)Enum.Parse(typeof(Operation), cbOperations.Text);
            var numberOfValues = new OperationHelper().GetNumberOfValuesAcceptable(operation);
            
            switch (numberOfValues)
            {
                case 0:
                    Controls.RemoveByKey("ctrlValue");
                    Controls.RemoveByKey("ctrlValue2");
                    break;
                case 1:
                    Controls.RemoveByKey("ctrlValue2");
                    break;
            }
        }
    }
}
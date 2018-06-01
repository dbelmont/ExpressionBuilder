using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ExpressionBuilder.Common;
using ExpressionBuilder.Helpers;
using ExpressionBuilder.Resources;
using ExpressionBuilder.Interfaces;

namespace ExpressionBuilder.WinForms.Controls
{
    /// <summary>
    /// Description of UcFilter.
    /// </summary>
    public partial class UcFilter : UserControl
    {
        private string _typeName = "ExpressionBuilder.Models.Person";
        private IPropertyCollection _properties;
        private OperationHelper operationHelper = new OperationHelper();

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

        public IOperation Operation
        {
            get { return operationHelper.GetOperationByName((cbOperations.SelectedItem as dynamic).Id); }
        }

        public object Value
        {
            get
            {
                var numberOfValues = Operation.NumberOfValues;
                return numberOfValues > 0 ? GetValue("ctrlValue") : null;
            }
        }

        public object Value2
        {
            get
            {
                var numberOfValues = Operation.NumberOfValues;
                return numberOfValues == 2 ? GetValue("ctrlValue2") : null;
            }
        }

        private object GetValue(string ctrlName)
        {
            var ctrl = Controls[ctrlName];

            if (ctrl != null)
            {
                var property = _properties[PropertyId];
                var type = Nullable.GetUnderlyingType(property.MemberType) ?? property.MemberType;
                if (type == typeof(string)) return ctrl.Text;
                if (type == typeof(DateTime)) return (ctrl as DateTimePicker).Value;
                if (type == typeof(int)) return Convert.ToInt32((ctrl as NumericUpDown).Value);
                if (type == typeof(bool)) return Boolean.Parse(ctrl.Text);
                if (type.IsEnum) return Enum.ToObject(property.MemberType, (ctrl as DomainUpDown).SelectedItem);
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

        public Connector Conector
        {
            get { return (Connector)Enum.Parse(typeof(Connector), cbConector.Text); }
        }

        public UcFilter()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();
        }

        public event EventHandler OnAdd;

        public event EventHandler OnRemove;

        private void UcFilterLoad(object sender, EventArgs e)
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
            cbProperties.DataSource = _properties.ToList();
            cbProperties.SelectedIndexChanged += cbProperties_SelectedIndexChanged;
        }

        private void LoadOperations()
        {
            LoadValueControls();

            var type = _properties[PropertyId].MemberType;
            var supportedOperations = new OperationHelper()
                                        .SupportedOperations(type)
                                        .Select(o => new
                                        {
                                            Id = o.ToString(),
                                            Name = o.GetDescription(Resources.Operations.ResourceManager)
                                        })
                                        .ToArray();

            cbOperations.ValueMember = "Id";
            cbOperations.DisplayMember = "Name";
            cbOperations.DataSource = supportedOperations;
        }

        private void cbProperties_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOperations();
        }

        private void LoadValueControls()
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

        private Control CreateNewControl()
        {
            var memberType = _properties[PropertyId].MemberType;
            var underlyingNullableType = Nullable.GetUnderlyingType(memberType);
            var type = underlyingNullableType ?? memberType;
            Control ctrl = null;
            if (type.IsEnum || type == typeof(bool))
            {
                ctrl = new DomainUpDown();
                if (type == typeof(bool))
                {
                    (ctrl as DomainUpDown).Items.AddRange(new[] { true, false });
                }
                else
                {
                    (ctrl as DomainUpDown).Items.AddRange(Enum.GetValues(type));
                }
                (ctrl as DomainUpDown).SelectedItem = (ctrl as DomainUpDown).Items[0];
                (ctrl as DomainUpDown).ReadOnly = true;
            }

            if (type == typeof(string))
            {
                ctrl = new TextBox();
            }

            if (type == typeof(DateTime))
            {
                ctrl = new DateTimePicker();
            }

            if (new[] { typeof(int), typeof(double), typeof(float), typeof(decimal) }.Contains(type))
            {
                ctrl = new NumericUpDown();
                (ctrl as NumericUpDown).Value = 0;
            }

            if (ctrl == null) throw new Exception("Type not supported");

            return ctrl;
        }

        public IPropertyCollection LoadProperties(Type type)
        {
            _properties = new PropertyCollection(type);
            _properties.LoadProperties(Resources.Person.ResourceManager);
            return _properties;
        }

        private void BtnAddClick(object sender, EventArgs e)
        {
            OnAdd(sender, e);
        }

        private void BtnRemoveClick(object sender, EventArgs e)
        {
            OnRemove(sender, e);
        }

        private void cbOperations_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlVisibility("ctrlValue", true);
            SetControlVisibility("ctrlValue2", true);

            var numberOfValues = Operation.NumberOfValues;
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
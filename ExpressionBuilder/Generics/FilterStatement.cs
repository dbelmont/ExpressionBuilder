using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Helpers;
using ExpressionBuilder.Exceptions;

namespace ExpressionBuilder.Generics
{
    [Serializable]
    public class FilterStatement<TPropertyType> : IFilterStatement, IXmlSerializable
	{
        public FilterStatementConnector Connector { get; set; }
        public string PropertyName { get; set; }
        public Operation Operation { get; set; }
        public object Value { get; set; }
        public object Value2 { get; set; }
		
		public FilterStatement(string propertyName, Operation operation, TPropertyType value, TPropertyType value2 = default(TPropertyType), FilterStatementConnector connector = FilterStatementConnector.And)
		{
			PropertyName = propertyName;
			Connector = connector;
			Operation = operation;
			if (typeof(TPropertyType).IsArray)
			{
				if (operation != Operation.Contains && operation != Operation.In) throw new ArgumentException("Only 'Operacao.Contains' and 'Operacao.In' support arrays as parameters.");

				var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(typeof(TPropertyType).GetElementType());
                Value = value != null ? Activator.CreateInstance(constructedListType, value) : null;
                Value2 = value2 != null ? Activator.CreateInstance(constructedListType, value2) : null;
            }
			else
			{
				Value = value;
                Value2 = value2;
			}

            Validate();
		}
		
        public FilterStatement()
        {

        }

        public void Validate()
        {
            var helper = new OperationHelper();            
            ValidateNumberOfValues(helper);
            ValidateSupportedOperations(helper);
        }

        private void ValidateNumberOfValues(OperationHelper helper)
        {
            var numberOfValues = helper.GetNumberOfValuesAcceptable(Operation);
            var failsForSingleValue = numberOfValues == 1 && Value2 != null && !Value2.Equals(default(TPropertyType));
            var failsForNoValueAtAll = numberOfValues == 0 && Value != null && Value2 != null && (!Value.Equals(default(TPropertyType)) || !Value2.Equals(default(TPropertyType)));

            if (failsForSingleValue || failsForNoValueAtAll)
            {
                throw new WrongNumberOfValuesException(Operation);
            }
        }

        private void ValidateSupportedOperations(OperationHelper helper)
        {
            var supportedOperations = helper.GetSupportedOperations(typeof(TPropertyType));

            if (!supportedOperations.Contains(Operation))
            {
                throw new UnsupportedOperationException(Operation, typeof(TPropertyType).Name);
            }
        }

		public override string ToString()
        {
            var operationHelper = new OperationHelper();

            switch (operationHelper.GetNumberOfValuesAcceptable(Operation))
            {
                case 0:
                    return string.Format("{0} {1}", PropertyName, Operation);
                case 2:
                    return string.Format("{0} {1} {2} And {3}", PropertyName, Operation, Value, Value2);
                default:
                    return string.Format("{0} {1} {2}", PropertyName, Operation, Value);
            }
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            PropertyName = reader.ReadElementContentAsString();
            Operation = (Operation)Enum.Parse(typeof(Operation), reader.ReadElementContentAsString());
            if (typeof(TPropertyType).IsEnum)
            {
                Value = Enum.Parse(typeof(TPropertyType), reader.ReadElementContentAsString());
            }
            else
            {
                Value = Convert.ChangeType(reader.ReadElementContentAsString(), typeof(TPropertyType));
            }

            Connector = (FilterStatementConnector)Enum.Parse(typeof(FilterStatementConnector), reader.ReadElementContentAsString());
        }

        public void WriteXml(XmlWriter writer)
        {
            var type = Value.GetType();
            var serializer = new XmlSerializer(type);
            writer.WriteAttributeString("Type", type.AssemblyQualifiedName);
            writer.WriteElementString("PropertyName", PropertyName);
            writer.WriteElementString("Operation", Operation.ToString("d"));
            writer.WriteElementString("Value", Value.ToString());
            writer.WriteElementString("Connector", Connector.ToString("d"));
        }
    }
}
using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using ExpressionBuilder.Builders;

namespace ExpressionBuilder.Generics
{
    [Serializable]
    public class FilterStatement<TPropertyType> : IFilterStatement, IXmlSerializable
	{
        public FilterStatementConnector Connector { get; set; }
        public string PropertyName { get; set; }
        public Operation Operation { get; set; }
        public object Value { get; set; }
		
		public FilterStatement(string propertyName, Operation operation, TPropertyType value, FilterStatementConnector connector = FilterStatementConnector.And)
		{
			PropertyName = propertyName;
			Connector = connector;
			Operation = operation;
			if (typeof(TPropertyType).IsArray)
			{
				if (operation != Operation.Contains) throw new ArgumentException("Only 'Operacao.Contains' supports arrays as parameters.");
				var listType = typeof(List<>);
                var constructedListType = listType.MakeGenericType(typeof(TPropertyType).GetElementType());
                Value = Activator.CreateInstance(constructedListType, value);
			}
			else
			{
				Value = value;
			}
		}
		
        public FilterStatement()
        {

        }

		public override string ToString()
        {
            return string.Format("{0} {1} {2}", PropertyName, Operation, Value);
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
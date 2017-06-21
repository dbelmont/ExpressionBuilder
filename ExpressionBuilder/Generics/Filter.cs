using ExpressionBuilder.Builders;
using ExpressionBuilder.Interfaces;
using ExpressionBuilder.Interfaces.Generics;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using ExpressionBuilder.Helpers;

namespace ExpressionBuilder.Generics
{
    [Serializable]
    public class Filter<TClass> : IFilter, IXmlSerializable where TClass : class
	{
		private List<IFilterStatement> _statements;
        
        public IEnumerable<IFilterStatement> Statements
		{
			get
			{
				return _statements.ToArray();
			}
		}
		
		public Filter()
		{
			_statements = new List<IFilterStatement>();
		}
        
		public IFilterStatementConnection By(string propertyName, Operation operation, FilterStatementConnector connector = FilterStatementConnector.And)
        {
            return By<string>(propertyName, operation, null, null, connector);
        }

		public IFilterStatementConnection By<TPropertyType>(string propertyName, Operation operation, TPropertyType value, TPropertyType value2 = default(TPropertyType), FilterStatementConnector connector = FilterStatementConnector.And)
		{
			IFilterStatement statement = new FilterStatement<TPropertyType>(propertyName, operation, value, value2, connector);
			_statements.Add(statement);
			return new FilterStatementConnection<TClass>(this, statement);
		}	
		
		public void Clear()
		{
			_statements.Clear();
		}
		
		public static implicit operator Func<TClass, bool>(Filter<TClass> filter)
		{
			var builder = new FilterBuilder(new BuilderHelper());
			return builder.GetExpression<TClass>(filter).Compile();
		}

        public override string ToString()
		{
			var result = "";
			FilterStatementConnector lastConector = FilterStatementConnector.And;
			foreach (var statement in _statements)
			{
				if (!string.IsNullOrWhiteSpace(result)) result += " " + lastConector + " ";
				result += statement.ToString();
				lastConector = statement.Connector;
			}
			
			return result.Trim();
		}

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name.StartsWith("FilterStatementOf"))
                {
                    var type = reader.GetAttribute("Type");
                    var filterType = typeof(FilterStatement<>).MakeGenericType(Type.GetType(type));
                    var serializer = new XmlSerializer(filterType);
                    var statement = (IFilterStatement)serializer.Deserialize(reader);
                    _statements.Add(statement);
                }
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", typeof(TClass).AssemblyQualifiedName);
            writer.WriteStartElement("Statements");
            foreach (var statement in _statements)
            {
                var serializer = new XmlSerializer(statement.GetType());
                serializer.Serialize(writer, statement);
            }

            writer.WriteEndElement();
        }
    }
}

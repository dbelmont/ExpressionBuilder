using ExpressionBuilder.Builders;
using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace ExpressionBuilder.Generics
{
    /// <summary>
    /// Aggregates <see cref="FilterStatement{TPropertyType}" /> and build them into a LINQ expression.
    /// </summary>
    /// <typeparam name="TClass"></typeparam>
    [Serializable]
    public class Filter<TClass> : IFilter, IXmlSerializable where TClass : class
    {
        private readonly List<List<IFilterStatement>> _statements;

        public IFilter Group
        {
            get
            {
                StartGroup();
                return this;
            }
        }

        /// <summary>
        /// List of <see cref="IFilterStatement" /> groups that will be combined and built into a LINQ expression.
        /// </summary>
        public IEnumerable<IEnumerable<IFilterStatement>> Statements
        {
            get
            {
                return _statements.ToArray();
            }
        }

        private List<IFilterStatement> CurrentStatementGroup
        {
            get
            {
                return _statements.Last();
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="Filter{TClass}" />
        /// </summary>
		public Filter()
        {
            _statements = new List<List<IFilterStatement>> { new List<IFilterStatement>() };
        }

        /// <summary>
        /// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// (To be used by <see cref="IOperation" /> that need no values)
        /// </summary>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="connector"></param>
        /// <returns></returns>
        public IFilterStatementConnection By(string propertyId, IOperation operation, Connector connector)
        {
            return By<string>(propertyId, operation, null, null, connector);
        }

        /// <summary>
        /// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// (To be used by <see cref="IOperation" /> that need no values)
        /// </summary>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <returns></returns>
        public IFilterStatementConnection By(string propertyId, IOperation operation)
        {
            return By<string>(propertyId, operation, null, null, Connector.And);
        }

        /// <summary>
        /// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
		public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value)
        {
            return By(propertyId, operation, value, default(TPropertyType));
        }

        /// <summary>
        /// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <param name="connector"></param>
        /// <returns></returns>
		public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value, Connector connector)
        {
            return By(propertyId, operation, value, default(TPropertyType), connector);
        }

        /// <summary>
        /// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
		public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value, TPropertyType value2)
        {
            return By(propertyId, operation, value, value2, Connector.And);
        }

        /// <summary>
        /// Adds a new <see cref="FilterStatement{TPropertyType}" /> to the <see cref="Filter{TClass}" />.
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="propertyId">Property identifier conventionalized by for the Expression Builder.</param>
        /// <param name="operation">Operation to be used.</param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <param name="connector"></param>
        /// <returns></returns>
		public IFilterStatementConnection By<TPropertyType>(string propertyId, IOperation operation, TPropertyType value, TPropertyType value2, Connector connector)
        {
            IFilterStatement statement = new FilterStatement<TPropertyType>(propertyId, operation, value, value2, connector);
            CurrentStatementGroup.Add(statement);
            return new FilterStatementConnection(this, statement);
        }

        /// <summary>
        /// Starts a new group denoting that every subsequent filter statement should be grouped together (as if using a parenthesis).
        /// </summary>
        public void StartGroup()
        {
            if (CurrentStatementGroup.Any())
            {
                _statements.Add(new List<IFilterStatement>());
            }
        }

        /// <summary>
        /// Removes all <see cref="FilterStatement{TPropertyType}" />, leaving the <see cref="Filter{TClass}" /> empty.
        /// </summary>
        public void Clear()
        {
            _statements.Clear();
            _statements.Add(new List<IFilterStatement>());
        }

        /// <summary>
        /// Implicitly converts a <see cref="Filter{TClass}" /> into a <see cref="Func{TClass, TResult}" />.
        /// </summary>
        /// <param name="filter"></param>
        public static implicit operator Func<TClass, bool>(Filter<TClass> filter)
        {
            var builder = new FilterBuilder();
            return builder.GetExpression<TClass>(filter).Compile();
        }

        /// <summary>
        /// Implicitly converts a <see cref="Filter{TClass}" /> into a <see cref="System.Linq.Expressions.Expression{Func{TClass, TResult}}" />.
        /// </summary>
        /// <param name="filter"></param>
        public static implicit operator System.Linq.Expressions.Expression<Func<TClass, bool>>(Filter<TClass> filter)
        {
            var builder = new FilterBuilder();
            return builder.GetExpression<TClass>(filter);
        }

        /// <summary>
        /// String representation of <see cref="Filter{TClass}" />.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new System.Text.StringBuilder();
            Connector lastConector = Connector.And;

            foreach (var statementGroup in _statements)
            {
                if (_statements.Count() > 1)
                {
                    result.Append("(");
                }

                var groupResult = new System.Text.StringBuilder();
                foreach (var statement in statementGroup)
                {
                    if (groupResult.Length > 0)
                    {
                        groupResult.Append(" " + lastConector + " ");
                    }

                    groupResult.Append(statement);
                    lastConector = statement.Connector;
                }

                result.Append(groupResult.ToString().Trim());
                if (_statements.Count() > 1)
                {
                    result.Append(")");
                }
            }

            return result.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        ///  Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The System.Xml.XmlReader stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.Name.Equals("StatementsGroup") && reader.IsStartElement())
                {
                    StartGroup();
                }

                if (reader.Name.StartsWith("FilterStatementOf"))
                {
                    var type = reader.GetAttribute("Type");
                    var filterType = typeof(FilterStatement<>).MakeGenericType(Type.GetType(type));
                    var serializer = new XmlSerializer(filterType);
                    var statement = (IFilterStatement)serializer.Deserialize(reader);
                    CurrentStatementGroup.Add(statement);
                }
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The System.Xml.XmlWriter stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("Type", typeof(TClass).AssemblyQualifiedName);
            writer.WriteStartElement("Statements");
            foreach (var statementsGroup in _statements)
            {
                writer.WriteStartElement("StatementsGroup");
                foreach (var statement in statementsGroup)
                {
                    var serializer = new XmlSerializer(statement.GetType());
                    serializer.Serialize(writer, statement);
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
using System;
using System.Reflection;

namespace ExpressionBuilder.Resources
{
    public class Property
    {
        public string Id { get; private set; }
        public string Name { get; set; }
        public PropertyInfo Info { get; private set; }

        public Property(string id, string name, PropertyInfo info)
        {
            Id = id;
            Name = name;
            Info = info;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}
using ExpressionBuilder.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilder.Helpers
{
    public class OperationHelper
    {
        public readonly Dictionary<TypeGroup, List<Type>> TypeGroups;
        public readonly Dictionary<TypeGroup, List<Operation>> SupportedOperations;

        public OperationHelper()
        {
            TypeGroups = new Dictionary<TypeGroup, List<Type>>
            {
                { TypeGroup.Text, new List<Type> { typeof(string), typeof(char) } },
                { TypeGroup.Number, new List<Type> { typeof(int), typeof(uint), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(Single), typeof(double), typeof(decimal) } },
                { TypeGroup.Boolean, new List<Type> { typeof(bool) } },
                { TypeGroup.Date, new List<Type> { typeof(DateTime) } }
            };

            SupportedOperations = new Dictionary<TypeGroup, List<Operation>>
            {
                { TypeGroup.Text, new List<Operation>{ Operation.Equals, Operation.Contains, Operation.EndsWith, Operation.NotEquals, Operation.StartsWith } },
                { TypeGroup.Number, new List<Operation>{ Operation.Equals, Operation.NotEquals, Operation.GreaterThan, Operation.GreaterThanOrEquals, Operation.LessThan, Operation.LessThanOrEquals } },
                { TypeGroup.Boolean, new List<Operation>{ Operation.Equals, Operation.NotEquals } },
                { TypeGroup.Date, new List<Operation>{ Operation.Equals, Operation.NotEquals, Operation.GreaterThan, Operation.GreaterThanOrEquals, Operation.LessThan, Operation.LessThanOrEquals } },
                { TypeGroup.Default, new List<Operation>{ Operation.Equals, Operation.NotEquals } }
            };
        }

        public List<Operation> GetSupportedOperations(Type type)
        {
            var typeGroup = TypeGroups.FirstOrDefault(i => i.Value.Contains(type)).Key;
            return SupportedOperations[typeGroup];
        }
    }
}

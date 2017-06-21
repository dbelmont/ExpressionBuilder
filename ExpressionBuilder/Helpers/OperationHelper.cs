using ExpressionBuilder.Attributes;
using ExpressionBuilder.Builders;
using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilder.Helpers
{
    public class OperationHelper : IOperationHelper
    {
        public readonly Dictionary<TypeGroup, List<Type>> TypeGroups;

        public OperationHelper()
        {
            TypeGroups = new Dictionary<TypeGroup, List<Type>>
            {
                { TypeGroup.Text, new List<Type> { typeof(string), typeof(char) } },
                { TypeGroup.Number, new List<Type> { typeof(int), typeof(uint), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(Single), typeof(double), typeof(decimal) } },
                { TypeGroup.Boolean, new List<Type> { typeof(bool) } },
                { TypeGroup.Date, new List<Type> { typeof(DateTime) } },
                { TypeGroup.Nullable, new List<Type> { typeof(Nullable<>) } }
            };
        }

        public List<Operation> GetSupportedOperations(Type type)
        {
            var supportedOperations = ExtractSupportedOperationsFromAttribute(type);
            
            if (type.IsArray)
            {
                //The 'In' operation is supported by all types, as long as it's an array...
                supportedOperations.Add(Operation.In);
            }

            var underlyingNullableType = Nullable.GetUnderlyingType(type);
            if(underlyingNullableType != null)
            {
                var underlyingNullableTypeOperations = GetSupportedOperations(underlyingNullableType);
                supportedOperations.AddRange(underlyingNullableTypeOperations);
            }

            return supportedOperations;
        }

        private List<Operation> ExtractSupportedOperationsFromAttribute(Type type)
        {
            var typeName = type.Name;
            if (type.IsArray)
            {
                typeName = type.GetElementType().Name;
            }
            
            var typeGroup = TypeGroups.FirstOrDefault(i => i.Value.Any(v => v.Name == typeName)).Key;
            var fieldInfo = typeGroup.GetType().GetField(typeGroup.ToString());
            var attrs = fieldInfo.GetCustomAttributes(false);
            var attr = attrs.FirstOrDefault(a => a is SupportedOperationsAttribute);
            return (attr as SupportedOperationsAttribute).SupportedOperations;
        }

        public int GetNumberOfValuesAcceptable(Operation operation)
        {
            var fieldInfo = operation.GetType().GetField(operation.ToString());
            var attrs = fieldInfo.GetCustomAttributes(false);
            var attr = attrs.FirstOrDefault(a => a is NumberOfValuesAttribute);
            return (attr as NumberOfValuesAttribute).NumberOfValues;
        }
    }
}
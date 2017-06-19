using ExpressionBuilder.Attributes;
using ExpressionBuilder.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilder.Helpers
{
    public class OperationHelper
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

            if (fieldInfo == null)
            {
                return new List<Operation>();
            }

            var attrs = fieldInfo.GetCustomAttributes(false);
            if (attrs == null || !attrs.Any())
            {
                return new List<Operation>();
            }

            var attr = attrs.FirstOrDefault(a => a is SupportedOperationsAttribute);
            if (attr == null)
            {
                return new List<Operation>();
            }

            return (attr as SupportedOperationsAttribute).SupportedOperations;
        }

        public int GetNumberOfValuesAcceptable(Operation operation)
        {
            var fieldInfo = operation.GetType().GetField(operation.ToString());

            if (fieldInfo == null)
            {
                return 0;
            }

            var attrs = fieldInfo.GetCustomAttributes(false);
            if (attrs == null || !attrs.Any())
            {
                return 0;
            }

            var attr = attrs.FirstOrDefault(a => a is NumberOfValuesAttribute);
            if (attr == null)
            {
                return 0;
            }

            return (attr as NumberOfValuesAttribute).NumberOfValues;
        }
    }
}
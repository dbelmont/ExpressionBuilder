using ExpressionBuilder.Attributes;
using ExpressionBuilder.Common;
using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilder.Helpers
{
    /// <summary>
    /// Useful methods regarding <seealso cref="Operation"></seealso>.
    /// </summary>
    public class OperationHelper : IOperationHelper
    {
        readonly Dictionary<TypeGroup, List<Type>> TypeGroups;

        /// <summary>
        /// Instantiates a new OperationHelper.
        /// </summary>
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

        /// <summary>
        /// Retrieves a list of <see cref="Operation"></see> supported by a type.
        /// </summary>
        /// <param name="type">Type for which supported operations should be retrieved.</param>
        /// <returns></returns>
        public List<Operation> SupportedOperations(Type type)
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
                var underlyingNullableTypeOperations = SupportedOperations(underlyingNullableType);
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

        /// <summary>
        /// Retrieves the exactly number of values acceptable by a specific operation.
        /// </summary>
        /// <param name="operation"><see cref="Operation"></see> for which the number of values acceptable should be verified.</param>
        /// <returns></returns>
        public int NumberOfValuesAcceptable(Operation operation)
        {
            var fieldInfo = operation.GetType().GetField(operation.ToString());
            var attrs = fieldInfo.GetCustomAttributes(false);
            var attr = attrs.FirstOrDefault(a => a is NumberOfValuesAttribute);
            return (attr as NumberOfValuesAttribute).NumberOfValues;
        }
    }
}
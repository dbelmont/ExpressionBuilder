using ExpressionBuilder.Attributes;
using ExpressionBuilder.Common;
using ExpressionBuilder.Configuration;
using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;

namespace ExpressionBuilder.Helpers
{
    /// <summary>
    /// Useful methods regarding <seealso cref="Operation"></seealso>.
    /// </summary>
    public class OperationHelper : IOperationHelper
    {
        private readonly Dictionary<TypeGroup, HashSet<Type>> TypeGroups;

        public List<IOperation> Operations
        {
            get
            {
                var cache = new MemoryCache("ExpressionBuilder");
                var operationsCache = cache["Operations"];
                if (operationsCache == null)
                {
                    var @interface = typeof(IOperation);
                    operationsCache = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => @interface.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                        .Select(t => (IOperation)Activator.CreateInstance(t))
                        .ToList();

                    cache.Add("Operations", operationsCache, DateTimeOffset.Now.AddHours(2));
                }

                return (List<IOperation>)operationsCache;
            }
        }

        /// <summary>
        /// Instantiates a new OperationHelper.
        /// </summary>
        public OperationHelper()
        {
            TypeGroups = new Dictionary<TypeGroup, HashSet<Type>>
            {
                { TypeGroup.Text, new HashSet<Type> { typeof(string), typeof(char) } },
                { TypeGroup.Number, new HashSet<Type> { typeof(int), typeof(uint), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort), typeof(long), typeof(ulong), typeof(Single), typeof(double), typeof(decimal) } },
                { TypeGroup.Boolean, new HashSet<Type> { typeof(bool) } },
                { TypeGroup.Date, new HashSet<Type> { typeof(DateTime) } },
                { TypeGroup.Nullable, new HashSet<Type> { typeof(Nullable<>), typeof(string) } }
            };
        }

        /// <summary>
        /// Retrieves a list of <see cref="Operation"></see> supported by a type.
        /// </summary>
        /// <param name="type">Type for which supported operations should be retrieved.</param>
        /// <returns></returns>
        public List<Operation> SupportedOperations(Type type)
        {
            GetCustomSupportedTypes();
            var supportedOperations = GetSupportedOperations(type);
            return supportedOperations.Select(o => (Operation)Enum.Parse(typeof(Operation), o.Name)).ToList();
        }

        private HashSet<IOperation> GetSupportedOperations(Type type)
        {
            var underlyingNullableType = Nullable.GetUnderlyingType(type);
            var typeName = (underlyingNullableType ?? type).Name;

            var supportedOperations = new List<IOperation>();
            if (type.IsArray)
            {
                typeName = type.GetElementType().Name;
                supportedOperations.AddRange(Operations.Where(o => o.SupportsLists));
            }

            var typeGroup = TypeGroups.FirstOrDefault(i => i.Value.Any(v => v.Name == typeName)).Key;
            supportedOperations.AddRange(Operations.Where(o => o.TypeGroup.HasFlag(typeGroup) && !o.SupportsLists));

            if (underlyingNullableType != null)
            {
                supportedOperations.AddRange(Operations.Where(o => o.TypeGroup.HasFlag(TypeGroup.Nullable)));
            }

            return new HashSet<IOperation>(supportedOperations);
        }

        private void GetCustomSupportedTypes()
        {
            var configSection = ConfigurationManager.GetSection(ExpressionBuilderConfig.SectionName) as ExpressionBuilderConfig;
            if (configSection == null)
            {
                return;
            }

            foreach (ExpressionBuilderConfig.SupportedTypeElement supportedType in configSection.SupportedTypes)
            {
                Type type = Type.GetType(supportedType.Type, false, true);
                if (type != null)
                {
                    TypeGroups[supportedType.TypeGroup].Add(type);
                }
            }
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
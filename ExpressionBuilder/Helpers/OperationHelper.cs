using ExpressionBuilder.Common;
using ExpressionBuilder.Configuration;
using ExpressionBuilder.Exceptions;
using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionBuilder.Helpers
{
    /// <summary>
    /// Useful methods regarding <seealso cref="IOperation"></seealso>.
    /// </summary>
    public class OperationHelper : IOperationHelper
    {
        private static HashSet<IOperation> _operations;

        private readonly Settings _settings;

        private readonly Dictionary<TypeGroup, HashSet<Type>> TypeGroups;

        static OperationHelper()
        {
            LoadDefaultOperations();
        }

        /// <summary>
        /// Loads the default operations overwriting any previous changes to the <see cref="Operations"></see> list.
        /// </summary>
        public static void LoadDefaultOperations()
        {
            var @interface = typeof(IOperation);
            var operationsFound = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.DefinedTypes.Any(t => t.Namespace == "ExpressionBuilder.Operations"))
                .SelectMany(s => s.GetTypes())
                .Where(p => @interface.IsAssignableFrom(p) && p.IsClass && !p.IsAbstract)
                .Select(t => (IOperation)Activator.CreateInstance(t));
            _operations = new HashSet<IOperation>(operationsFound, new OperationEqualityComparer());
        }

        /// <summary>
        /// List of all operations loaded so far.
        /// </summary>
        public IEnumerable<IOperation> Operations { get { return _operations.ToArray(); } }

        /// <summary>
        /// Instantiates a new OperationHelper.
        /// </summary>
        public OperationHelper()
        {
            _settings = new Settings();
            Settings.LoadSettings(_settings);
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
        /// Retrieves a list of <see cref="IOperation"></see> supported by a type.
        /// </summary>
        /// <param name="type">Type for which supported operations should be retrieved.</param>
        /// <returns></returns>
        public HashSet<IOperation> SupportedOperations(Type type)
        {
            GetCustomSupportedTypes();
            return GetSupportedOperations(type);
        }

        private void GetCustomSupportedTypes()
        {
            foreach (var supportedType in _settings.SupportedTypes)
            {
                if (supportedType.Type != null)
                {
                    TypeGroups[supportedType.TypeGroup].Add(supportedType.Type);
                }
            }
        }

        private HashSet<IOperation> GetSupportedOperations(Type type)
        {
            var underlyingNullableType = Nullable.GetUnderlyingType(type);
            var typeName = (underlyingNullableType ?? type).Name;

            var supportedOperations = new List<IOperation>();
            if (type.IsArray)
            {
                typeName = type.GetElementType().Name;
                supportedOperations.AddRange(Operations.Where(o => o.SupportsLists && o.Active));
            }

            var typeGroup = TypeGroup.Default;
            if (TypeGroups.Any(i => i.Value.Any(v => v.Name == typeName)))
            {
                typeGroup = TypeGroups.FirstOrDefault(i => i.Value.Any(v => v.Name == typeName)).Key;
            }

            supportedOperations.AddRange(Operations.Where(o => o.TypeGroup.HasFlag(typeGroup) && !o.SupportsLists && o.Active));

            if (underlyingNullableType != null)
            {
                supportedOperations.AddRange(Operations.Where(o => o.TypeGroup.HasFlag(TypeGroup.Nullable) && !o.SupportsLists && o.Active));
            }

            return new HashSet<IOperation>(supportedOperations);
        }

        /// <summary>
        /// Instantiates an IOperation given its name.
        /// </summary>
        /// <param name="operationName">Name of the operation to be instantiated.</param>
        /// <returns></returns>
        public IOperation GetOperationByName(string operationName)
        {
            var operation = Operations.SingleOrDefault(o => o.Name == operationName && o.Active);

            if (operation == null)
            {
                throw new OperationNotFoundException(operationName);
            }

            return operation;
        }

        /// <summary>
        /// Loads a list of custom operations into the <see cref="Operations"></see> list.
        /// </summary>
        /// <param name="operations">List of operations to load.</param>
        public void LoadOperations(List<IOperation> operations)
        {
            LoadOperations(operations, false);
        }

        /// <summary>
        /// Loads a list of custom operations into the <see cref="Operations"></see> list.
        /// </summary>
        /// <param name="operations">List of operations to load.</param>
        /// <param name="overloadExisting">Specifies that any matching pre-existing operations should be replaced by the ones from the list. (Useful to overwrite the default operations)</param>
        public void LoadOperations(List<IOperation> operations, bool overloadExisting)
        {
            foreach (var operation in operations)
            {
                DeactivateOperation(operation.Name, overloadExisting);
                _operations.Add(operation);
            }
        }

        private void DeactivateOperation(string operationName, bool overloadExisting)
        {
            if (!overloadExisting)
            {
                return;
            }

            var op = _operations.FirstOrDefault(o => string.Compare(o.Name, operationName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (op != null)
            {
                op.Active = false;
            }
        }
    }
}
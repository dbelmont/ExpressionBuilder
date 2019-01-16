using ExpressionBuilder.Helpers;
using ExpressionBuilder.Interfaces;
using System.Collections.Generic;

namespace ExpressionBuilder.Operations
{
    /// <summary>
    /// Exposes the default operations supported by the <seealso cref="Builders.FilterBuilder" />.
    /// </summary>
    public static class Operation
    {
        private static OperationHelper _operationHelper;

        static Operation()
        {
            _operationHelper = new OperationHelper();
        }

        /// <summary>
        /// Operation representing a range comparison.
        /// </summary>
        public static IOperation Between { get { return new Between(); } }

        /// <summary>
        /// Operation representing a string "Contains" method call.
        /// </summary>
        public static IOperation Contains { get { return new Contains(); } }

        /// <summary>
        /// Operation that checks for the non-existence of a substring within another string.
        /// </summary>
        public static IOperation DoesNotContain { get { return new DoesNotContain(); } }

        /// <summary>
        /// Operation representing a string "EndsWith" method call.
        /// </summary>
        public static IOperation EndsWith { get { return new EndsWith(); } }

        /// <summary>
        /// Operation representing an equality comparison.
        /// </summary>
        public static IOperation EqualTo { get { return new EqualTo(); } }

        /// <summary>
        /// Operation representing an "greater than" comparison.
        /// </summary>
        public static IOperation GreaterThan { get { return new GreaterThan(); } }

        /// <summary>
        /// Operation representing an "greater than or equal" comparison.
        /// </summary>
        public static IOperation GreaterThanOrEqualTo { get { return new GreaterThanOrEqualTo(); } }

        /// <summary>
        /// Operation representing a list "Contains" method call.
        /// </summary>
        public static IOperation In { get { return new In(); } }

        /// <summary>
        /// Operation representing a check for an empty string.
        /// </summary>
        public static IOperation IsEmpty { get { return new IsEmpty(); } }

        /// <summary>
        /// Operation representing a check for a non-empty string.
        /// </summary>
        public static IOperation IsNotEmpty { get { return new IsNotEmpty(); } }

        /// <summary>
        /// Operation representing a "not-null" check.
        /// </summary>
        public static IOperation IsNotNull { get { return new IsNotNull(); } }

        /// <summary>
        /// Operation representing a "not null nor whitespace" check.
        /// </summary>
        public static IOperation IsNotNullNorWhiteSpace { get { return new IsNotNullNorWhiteSpace(); } }

        /// <summary>
        /// Operation representing a null check.
        /// </summary>
        public static IOperation IsNull { get { return new IsNull(); } }

        /// <summary>
        /// Operation representing a "null or whitespace" check.
        /// </summary>
        public static IOperation IsNullOrWhiteSpace { get { return new IsNullOrWhiteSpace(); } }

        /// <summary>
        /// Operation representing an "less than" comparison.
        /// </summary>
        public static IOperation LessThan { get { return new LessThan(); } }

        /// <summary>
        /// Operation representing an "less than or equal" comparison.
        /// </summary>
        public static IOperation LessThanOrEqualTo { get { return new LessThanOrEqualTo(); } }

        /// <summary>
        /// Operation representing an inequality comparison.
        /// </summary>
        public static IOperation NotEqualTo { get { return new NotEqualTo(); } }

        /// <summary>
        /// Operation representing a string "StartsWith" method call.
        /// </summary>
        public static IOperation StartsWith { get { return new StartsWith(); } }

        /// <summary>
        /// Operation representing the inverse of a list "Contains" method call.
        /// </summary>
        public static IOperation NotIn { get { return new NotIn(); } }

        /// <summary>
        /// Instantiates an IOperation given its name.
        /// </summary>
        /// <param name="operationName">Name of the operation to be instantiated.</param>
        /// <returns></returns>
        public static IOperation ByName(string operationName)
        {
            return _operationHelper.GetOperationByName(operationName);
        }

        /// <summary>
        /// Loads a list of custom operations into the <see cref="Operations"></see> list.
        /// </summary>
        /// <param name="operations">List of operations to load.</param>
        /// <param name="overloadExisting">Specifies that any matching pre-existing operations should be replaced by the ones from the list. (Useful to overwrite the default operations)</param>
        public static void LoadOperations(List<IOperation> operations, bool overloadExisting = false)
        {
            _operationHelper.LoadOperations(operations, overloadExisting);
        }
    }
}
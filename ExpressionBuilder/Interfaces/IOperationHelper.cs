using System;
using System.Collections.Generic;
using ExpressionBuilder.Operations;

namespace ExpressionBuilder.Interfaces
{
    /// <summary>
    /// Useful methods regarding <seealso cref="IOperation"></seealso>.
    /// </summary>
    public interface IOperationHelper
    {
        /// <summary>
        /// List of all operations loaded so far.
        /// </summary>
        IEnumerable<IOperation> Operations { get; }

        /// <summary>
        /// Retrieves a list of <see cref="Operation"></see> supported by a type.
        /// </summary>
        /// <param name="type">Type for which supported operations should be retrieved.</param>
        /// <returns></returns>
        HashSet<IOperation> SupportedOperations(Type type);

        /// <summary>
        /// Instantiates an IOperation given its name.
        /// </summary>
        /// <param name="operationName">Name of the operation to be instantiated.</param>
        /// <returns></returns>
        IOperation GetOperationByName(string operationName);

        /// <summary>
        /// Loads a list of custom operations into the <see cref="Operations"></see> list.
        /// </summary>
        /// <param name="operations">List of operations to load.</param>
        void LoadOperations(List<IOperation> operations);

        /// <summary>
        /// Loads a list of custom operations into the <see cref="Operations"></see> list.
        /// </summary>
        /// <param name="operations">List of operations to load.</param>
        /// <param name="overloadExisting">Specifies that any matching pre-existing operations should be replaced by the ones from the list. (Useful to overwrite the default operations)</param>
        void LoadOperations(List<IOperation> operations, bool overloadExisting);
    }
}
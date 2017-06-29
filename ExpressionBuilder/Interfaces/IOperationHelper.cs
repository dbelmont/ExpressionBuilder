using ExpressionBuilder.Common;
using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Interfaces
{
    /// <summary>
    /// Useful methods regarding <seealso cref="Operation"></seealso>.
    /// </summary>
    public interface IOperationHelper
    {
        /// <summary>
        /// Retrieves a list of <see cref="Operation"></see> supported by a type.
        /// </summary>
        /// <param name="type">Type for which supported operations should be retrieved.</param>
        /// <returns></returns>
        List<Operation> SupportedOperations(Type type);
        /// <summary>
        /// Retrieves the exactly number of values acceptable by a specific operation.
        /// </summary>
        /// <param name="operation"><see cref="Operation"></see> for which the number of values acceptable should be verified.</param>
        /// <returns></returns>
        int NumberOfValuesAcceptable(Operation operation);
    }
}
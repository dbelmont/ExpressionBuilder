using System;

namespace ExpressionBuilder.Exceptions
{
    /// <summary>
    /// Represents an attempt to instantiate an operation that was not loaded.
    /// </summary>
    [Serializable]
    public class OperationNotFoundException : Exception
    {
        /// <summary>
        /// Name of the operation that was intended to be instantiated.
        /// </summary>
        public string OperationName { get; }

        /// <inheritdoc />
        public override string Message
        {
            get
            {
                return string.Format("Sorry, the operation '{0}' was not found.", OperationName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationNotFoundException" /> class.
        /// </summary>
        /// <param name="operationName">Name of the operation that was intended to be instantiated.</param>
        public OperationNotFoundException(string operationName)
        {
            OperationName = operationName;
        }
    }
}
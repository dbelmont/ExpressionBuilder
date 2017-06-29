using ExpressionBuilder.Common;
using ExpressionBuilder.Helpers;
using System;

namespace ExpressionBuilder.Exceptions
{
    /// <summary>
    /// Represents an attempt to use an operation providing the wrong number of values.
    /// </summary>
    public class WrongNumberOfValuesException : Exception
    {
        /// <summary>
        /// Gets the <see cref="Operation" /> attempted to be used.
        /// </summary>
        public Operation Operation { get; private set; }

        /// <summary>
        /// Gets the number of values acceptable by this <see cref="Operation" />.
        /// </summary>
        public int NumberOfValuesAcceptable { get; private set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("The operation '{0}' admits exactly '{1}' values (not more neither less than this).", Operation, NumberOfValuesAcceptable);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WrongNumberOfValuesException" /> class.
        /// </summary>
        /// <param name="operation">Operation used.</param>
        public WrongNumberOfValuesException(Operation operation) : base()
        {
            Operation = operation;
            NumberOfValuesAcceptable = new OperationHelper().NumberOfValuesAcceptable(operation);
        }
    }
}
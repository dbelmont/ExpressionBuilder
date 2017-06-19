using ExpressionBuilder.Builders;
using ExpressionBuilder.Helpers;
using System;

namespace ExpressionBuilder.Exceptions
{
    public class WrongNumberOfValuesException : Exception
    {
        public Operation Operation { get; private set; }

        public int NumberOfValuesAcceptable { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format("The operation '{0}' admits exactly '{1}' values (not more neither less than this).", Operation, NumberOfValuesAcceptable);
            }
        }

        public WrongNumberOfValuesException(Operation operation) : base()
        {
            Operation = operation;
            NumberOfValuesAcceptable = new OperationHelper().GetNumberOfValuesAcceptable(operation);
        }
    }
}
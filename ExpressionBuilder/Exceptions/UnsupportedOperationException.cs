using ExpressionBuilder.Builders;
using System;

namespace ExpressionBuilder.Exceptions
{
    public class UnsupportedOperationException : Exception
    {
        public Operation Operation { get; private set; }

        public string TypeName { get; private set; }

        public override string Message
        {
            get
            {
                return string.Format("The type '{0}' does not have support for the operation '{1}'.", TypeName, Operation);
            }
        }

        public UnsupportedOperationException(Operation operation, String typeName) : base()
        {
            Operation = operation;
            TypeName = typeName;
        }
    }
}
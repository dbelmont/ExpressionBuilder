using System;

namespace ExpressionBuilder.Exceptions
{
    /// <summary>
    /// Represents an attempt to set a property's value with an object of a different type from the property's type.
    /// </summary>
    [Serializable]
    public class PropertyValueTypeMismatchException : Exception
    {
        /// <summary>
        /// Name of the property or fields.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Type of the property or field.
        /// </summary>
        public string MemberType { get; }

        /// <summary>
        /// Type of the constant which value tried to be attributed to the property or field.
        /// </summary>
        public string ConstantType { get; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return string.Format("The type of the member '{0}' ({1}) is different from the type of one of the constants ({2})", MemberName, MemberType, ConstantType);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueTypeMismatchException" /> class.
        /// </summary>
        /// <param name="memberName">Property or field name.</param>
        /// <param name="memberType">Property or field type.</param>
        /// <param name="constantType">Type of the constant which value tried to be attributed to the property or field.</param>
        public PropertyValueTypeMismatchException(string memberName, string memberType, string constantType)
        {
            MemberName = memberName;
            MemberType = memberType;
            ConstantType = constantType;
        }
    }
}
using System.Reflection;

namespace ExpressionBuilder.Resources
{
    /// <summary>
    /// Provides information on the property to the Expression Builder.
    /// </summary>
    public class Property
    {
        /// <summary>
        /// Property identifier conventionalized by the Expression Builder.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Property name obtained from a ResourceManager, or the property's original name (in the absence of a ResourceManager correspondent value).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Property metadata.
        /// </summary>
        public MemberInfo Info { get; private set; }

        public System.Type MemberType
        {
            get
            {
                return Info.MemberType == MemberTypes.Property ? (Info as PropertyInfo).PropertyType : (Info as FieldInfo).FieldType;
            }
        }

        internal Property(string id, string name, MemberInfo info)
        {
            Id = id;
            Name = name;
            Info = info;
        }

        /// <summary>
        /// String representation of <see cref="Property" />.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, Id);
        }
    }
}
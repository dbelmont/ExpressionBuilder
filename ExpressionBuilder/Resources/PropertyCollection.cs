using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Collections;
using System.Linq;

namespace ExpressionBuilder.Resources
{
    /// <summary>
    /// Collection of <see cref="Property" />.
    /// </summary>
    public class PropertyCollection : IPropertyCollection
    {
        /// <summary>
        /// Type from which the properties are loaded.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// ResourceManager which the properties descriptions should be gotten from.
        /// </summary>
        public ResourceManager ResourceManager { get; private set; }
        
        private List<Property> Properties { get; set; }
        
        /// <summary>
        /// Gets the number of <see cref="Property" /> contained in the <see cref="PropertyCollection" />.
        /// </summary>
        public int Count => Properties.Count();

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot => throw new NotImplementedException();

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized => throw new NotImplementedException();
        
        /// <summary>
        /// Retrieves a property based on its Id.
        /// </summary>
        /// <param name="propertyId">Property conventionalized <see cref="Property.Id" />.</param>
        /// <returns></returns>
        public Property this[string propertyId] => Properties.FirstOrDefault(p => p.Id.Equals(propertyId));

        /// <summary>
        /// Instantiates a new <see cref="PropertyCollection" />.
        /// </summary>
        /// <param name="type"></param>
        public PropertyCollection(Type type)
        {
            Type = type;
        }

        /// <summary>
        /// Instantiates a new <see cref="PropertyCollection" />.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="resourceManager"></param>
        public PropertyCollection(Type type, ResourceManager resourceManager) : this(type)
        {
            LoadProperties(resourceManager);
        }

        /// <summary>
        /// Loads the properties names from the specified ResourceManager.
        /// </summary>
        /// <param name="resourceManager"></param>
        /// <returns></returns>
        public List<Property> LoadProperties(ResourceManager resourceManager)
        {
            ResourceManager = resourceManager;
            var resourceSet = resourceManager.GetResourceSet(System.Threading.Thread.CurrentThread.CurrentCulture, true, false);
            var properties = LoadProperties(Type);
            foreach (Property property in properties)
            {
                property.Name = resourceManager.GetString(GetPropertyResourceName(property.Id)) ?? property.Name;
            }

            return Properties = properties;
        }

        private string GetPropertyResourceName(string propertyConventionName)
        {
            return propertyConventionName
                                        .Replace("[", "_")
                                        .Replace("]", "_")
                                        .Replace(".", "_");
        }

        private List<Property> LoadProperties(Type type)
        {
            var list = new List<Property>();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsValueType || property.PropertyType == typeof(String))
                {
                    list.Add(new Property(property.Name, property.Name, property));
                    continue;
                }

                if (property.PropertyType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                {
                    var props = LoadProperties(property.PropertyType.GetGenericArguments()[0]);
                    foreach (var info in props)
                    {
                        list.Add(new Property(property.Name + "[" + info.Id + "]", info.Name, info.Info));
                    }
                    continue;
                }

                if (!property.PropertyType.IsValueType)
                {
                    var props = LoadProperties(property.PropertyType);
                    foreach (var info in props)
                    {
                        list.Add(new Property(property.Name + "." + info.Id, info.Name, info.Info));
                    }
                    continue;
                }
            }

            return list;
        }

        /// <summary>
        /// Copies the elements of the <see cref="PropertyCollection" /> to an System.Array,
        /// starting at a particular System.Array index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional System.Array that is the destination of the elements copied
        /// from System.Collections.ICollection. The System.Array must have zero-based indexing.
        /// </param>
        /// <param name="index">The zero-based index in array at which copying begins.</param>
        public void CopyTo(Array array, int index)
        {
            Properties.CopyTo((Property[])array, index);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            return Properties.GetEnumerator();
        }

        /// <summary>
        /// Converts the collection into a list.
        /// </summary>
        /// <returns></returns>
        public IList<Property> ToList()
        {
            Property[] properties = new Property[Properties.Count];
            CopyTo(properties, 0);
            return properties;
        }
    }
}
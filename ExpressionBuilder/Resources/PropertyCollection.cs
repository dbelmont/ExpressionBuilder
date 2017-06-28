using ExpressionBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Collections;
using System.Linq;

namespace ExpressionBuilder.Resources
{
    public class PropertyCollection : IPropertyCollection
    {
        public Type Type { get; private set; }

        public ResourceManager ResourceManager { get; private set; }

        public List<Property> Properties { get; private set; }

        public bool IsReadOnly => true;

        public bool IsFixedSize => false;

        public int Count => Properties.Count();

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object this[int index] { get => Properties[index]; set => throw new NotImplementedException(); }

        public Property this[string propertyId] => Properties.FirstOrDefault(p => p.Id.Equals(propertyId));

        public PropertyCollection(Type type)
        {
            Type = type;
        }

        public PropertyCollection(Type type, ResourceManager resourceManager) : this(type)
        {
            LoadProperties(resourceManager);
        }

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

        public string GetPropertyResourceName(string propertyConventionName)
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

        public int Add(object value)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            return Properties.Contains(value);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(object value)
        {
            return Properties.IndexOf((Property)value);
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            Properties.CopyTo((Property[])array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return Properties.GetEnumerator();
        }
    }
}
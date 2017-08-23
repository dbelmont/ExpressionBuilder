using ExpressionBuilder.Common;
using System;
using System.Configuration;

namespace ExpressionBuilder.Configuration
{
    internal class ExpressionBuilderConfig : ConfigurationSection
    {
        public const string SectionName = "ExpressionBuilder";

        private const string SupportedTypeCollectionName = "SupportedTypes";

        [ConfigurationProperty(SupportedTypeCollectionName)]
        
        public SupportedTypesElementConfiguration SupportedTypes { get { return (SupportedTypesElementConfiguration)base[SupportedTypeCollectionName]; } }

        public class SupportedTypeElement : ConfigurationElement
        {
            [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
            public string Type
            {
                get
                {
                    return (string)base["type"];
                }
            }

            [ConfigurationProperty("typeGroup", IsRequired = true, IsKey = false)]
            public TypeGroup TypeGroup
            {
                get
                {
                    return (TypeGroup)base["typeGroup"];
                }
            }
        }

        [ConfigurationCollection(typeof(SupportedTypesElementConfiguration), AddItemName = "add")]
        public class SupportedTypesElementConfiguration : ConfigurationElementCollection
        {
            protected override ConfigurationElement CreateNewElement()
            {
                return new SupportedTypeElement();
            }

            protected override object GetElementKey(ConfigurationElement element)
            {
                if (element == null)
                    throw new ArgumentNullException("element");

                return ((SupportedTypeElement)element).Type;
            }
        }
    }
}
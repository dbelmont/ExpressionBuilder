using ExpressionBuilder.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;

namespace ExpressionBuilder.Interfaces
{
    public interface IPropertyCollection : IList
    {
        Type Type { get; }
        ResourceManager ResourceManager { get; }
        Property this[string propertyId] { get; }

        List<Property> LoadProperties(ResourceManager resourceManager);
    }
}
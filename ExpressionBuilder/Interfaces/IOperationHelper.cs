using ExpressionBuilder.Builders;
using System;
using System.Collections.Generic;

namespace ExpressionBuilder.Interfaces
{
    public interface IOperationHelper
    {
        List<Operation> GetSupportedOperations(Type type);
        int GetNumberOfValuesAcceptable(Operation operation);
    }
}
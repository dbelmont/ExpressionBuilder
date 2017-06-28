using ExpressionBuilder.Builders;
using System.Resources;

namespace ExpressionBuilder.Resources
{
    public static class OperationGlobalizationExtensionMethods
    {
        public static string GetDescription(this Operation operation, ResourceManager resourceManager)
        {
            return resourceManager.GetString(operation.ToString()) ?? operation.ToString();
        }
    }
}
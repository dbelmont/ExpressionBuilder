using ExpressionBuilder.Common;
using System.Resources;

namespace ExpressionBuilder.Resources
{
    /// <summary>
    /// Extension methods for <see cref="Operation" />.
    /// </summary>
    public static class OperationGlobalizationExtensionMethods
    {
        /// <summary>
        /// Retrieves the description for an operation from the specified resource manager.
        /// If it's not possible to find a key that matches the operation value, then
        /// the operation value itself will be return as the description.
        /// </summary>
        /// <param name="operation">Operation which description should be returned.</param>
        /// <param name="resourceManager">ResourceManager where the description can be found.</param>
        /// <returns></returns>
        public static string GetDescription(this Operation operation, ResourceManager resourceManager)
        {
            return resourceManager.GetString(operation.ToString()) ?? operation.ToString();
        }
    }
}
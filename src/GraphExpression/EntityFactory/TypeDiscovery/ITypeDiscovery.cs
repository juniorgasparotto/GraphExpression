using System;

namespace GraphExpression
{
    /// <summary>
    /// Interface that should be used in searching for a type for an entity
    /// </summary>
    public interface ITypeDiscovery
    {
        /// <summary>
        /// Verify if can discovery type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Return TRUE if can discovery</returns>
        bool CanDiscovery(Entity item);

        /// <summary>
        /// Returns the desired Type
        /// </summary>
        /// <param name="item">The item that contains the information to get type</param>
        /// <returns>Returns the desired Type</returns>
        Type GetEntityType(Entity item);
    }
}
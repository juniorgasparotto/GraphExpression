using System.ComponentModel;

namespace GraphExpression
{
    /// <summary>
    /// Class default used to creating any primitive type
    /// </summary>
    public class PrimitiveValueLoader : IValueLoader
    {
        /// <summary>
        /// Verify if can load instance value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return TRUE if can load</returns>
        public bool CanLoad(Entity item)
        {
            return item.IsPrimitive;
        }

        /// <summary>
        /// Return value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return object instance</returns>
        public object GetValue(Entity item)
        {
            if (item.ValueRaw == null)
                return null;

            return TypeDescriptor.GetConverter(item.Type).ConvertFromInvariantString(item.ValueRaw);
        }
    }
}
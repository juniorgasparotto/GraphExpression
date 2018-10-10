namespace GraphExpression
{
    /// <summary>
    /// Interface that should be used when creating an instance
    /// </summary>
    public interface IValueLoader
    {
        /// <summary>
        /// Verify if can load instance value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return TRUE if can load</returns>
        bool CanLoad(Entity item);

        /// <summary>
        /// Return value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return object instance</returns>
        object GetValue(Entity item);
    }
}
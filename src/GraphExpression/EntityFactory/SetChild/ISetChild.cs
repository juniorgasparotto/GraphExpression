namespace GraphExpression
{
    /// <summary>
    /// Interface that should be used to assign a child to the parent
    /// </summary>
    public interface ISetChild 
    {
        /// <summary>
        /// Verify if can set child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        /// <returns>Return TRUE if can set</returns>
        bool CanSet(Entity item, Entity child);

        /// <summary>
        /// Set a child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        void SetChild(Entity item, Entity child);
    }
}
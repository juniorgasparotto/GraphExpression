namespace GraphExpression
{
    /// <summary>
    /// Represents the type of browsing in the search for "siblings"
    /// </summary>
    public enum SiblingDirection
    {
        /// <summary>
        /// Start search from the first brother; First child of the parent (current.Parent.Childrens()[0])
        /// </summary>
        Start,

        /// <summary>
        /// Start search from next entity 
        /// </summary>
        Next,

        /// <summary>
        /// Start search from previous entity
        /// </summary>
        Previous
    }
}

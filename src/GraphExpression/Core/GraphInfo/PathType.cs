namespace GraphExpression
{
    /// <summary>
    /// Path type
    /// </summary>
    public enum PathType
    {
        /// <summary>
        /// Not repeat and not circular 
        /// </summary>
        Simple,

        /// <summary>
        /// Repeat first and last (circular) 
        /// </summary>
        Circuit,

        /// <summary>
        /// Repeat in middle (A,B,B) 
        /// </summary>
        Circle
    }
}
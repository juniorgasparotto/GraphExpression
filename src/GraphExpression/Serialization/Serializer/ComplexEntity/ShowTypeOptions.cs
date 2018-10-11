namespace GraphExpression.Serialization
{
    /// <summary>
    /// Determines how the types of entities will be displayed in serialization
    /// </summary>
    public enum ShowTypeOptions
    {
        /// <summary>
        /// Does not display type
        /// </summary>
        None,

        /// <summary>
        /// Displays the type for the root entity only
        /// </summary>
        TypeNameOnlyInRoot,

        /// <summary>
        /// Displays the type name
        /// </summary>
        TypeName,

        /// <summary>
        /// Displays the full name of the type
        /// </summary>
        FullTypeName
    }
}

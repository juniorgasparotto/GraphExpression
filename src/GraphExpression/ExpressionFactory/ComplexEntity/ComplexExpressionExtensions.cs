namespace GraphExpression
{
    /// <summary>
    /// Extension to add methods of creating complex expressions on any object.
    /// </summary>
    public static class ComplexExpressionExtensions
    {
        /// <summary>
        /// Creates a complex expression, that is, where entities can have different types. By default, the expression is inferred with the object type.
        /// </summary>
        /// <param name="entityRoot">Root entity of expression. All entity paths will be traversed via reflection and each entity in that path will be an item in the expression.</param>
        /// <param name="factory">Enter this parameter to customize the build of the expression</param>
        /// <param name="deep">Determines whether the expression constructed will be a deep one, that is, if it will navigate objects that have already been navigated, except for cyclic references.</param>
        /// <returns>Return a complex expression</returns>
        public static Expression<object> AsExpression(this object entityRoot, ComplexExpressionFactory factory = null, bool deep = false)
        {
            factory = factory ?? new ComplexExpressionFactory();
            return factory.Build(entityRoot, deep);
        }
    }
}

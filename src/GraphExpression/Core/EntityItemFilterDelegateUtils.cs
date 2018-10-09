namespace GraphExpression
{
    /// <summary>
    /// Delegate to any filter/stop action
    /// </summary>
    /// <typeparam name="T">Real entity type</typeparam>
    /// <param name="item">Item to filter</param>
    /// <returns>Return TRUE if find</returns>
    public delegate bool EntityItemFilterDelegate<T>(EntityItem<T> item);

    /// <summary>
    /// Delegate to any filter/stop action
    /// </summary>
    /// <typeparam name="T">Real entity type</typeparam>
    /// <param name="item">Item to filter</param>
    /// <param name="depth">Depth information</param>
    /// <returns>Return TRUE if find</returns>
    public delegate bool EntityItemFilterDelegate2<T>(EntityItem<T> item, int depth);

    /// <summary>
    /// Utils for works with a different delegates in EntityItem search
    /// </summary>
    /// <typeparam name="T">Real entity type</typeparam>
    public static class EntityItemFilterDelegateUtils<T>
    {
        /// <summary>
        /// Convert delegate 'EntityItemFilterDelegate' to 'EntityItemFilterDelegate2'
        /// </summary>
        /// <param name="del">Delegate to convert</param>
        /// <returns>A delegate of type 'EntityItemFilterDelegate2'</returns>
        public static EntityItemFilterDelegate2<T> ConvertToMajorDelegate(EntityItemFilterDelegate<T> del)
        {
            if (del == null)
                return null;

            return (e, _) => del(e);
        }
    }
}
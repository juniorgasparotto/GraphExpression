namespace GraphExpression
{
    public delegate bool EntityItemFilterDelegate<T>(EntityItem<T> items);
    public delegate bool EntityItemFilterDelegate2<T>(EntityItem<T> items, int depth);

    public static class EntityItemFilterDelegateUtils<T>
    {
        public static EntityItemFilterDelegate2<T> ConvertToMajorDelegate(EntityItemFilterDelegate<T> del)
        {
            if (del == null)
                return null;

            return (e, _) => del(e);
        }
    }
}
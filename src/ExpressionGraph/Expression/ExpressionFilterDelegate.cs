using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    public delegate bool ExpressionFilterDelegate<T>(ExpressionItem<T> items);
    public delegate bool ExpressionFilterDelegate2<T>(ExpressionItem<T> items, int depth);

    public static class ExpressionFilterDelegateUtils<T>
    {
        public static ExpressionFilterDelegate2<T> ConvertToMajorDelegate(ExpressionFilterDelegate<T> del)
        {
            if (del == null)
                return null;

            return (e, _) => del(e);
        }
    }
}
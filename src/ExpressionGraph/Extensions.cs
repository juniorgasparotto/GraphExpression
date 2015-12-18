using ExpressionGraph.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    /// <summary>
    /// Extentions for enums.
    /// </summary>
    public static class Extensions
    {
        public static ReflectionTree AsReflection(this object obj)
        {
            var query = new ReflectionTree(obj);
            return query;
        }

        public static IEnumerable<object> Objects(this Expression<ReflectInstance> expression)
        {
            foreach (var r in expression)
                yield return r.Entity.Object;
        }
    }
}

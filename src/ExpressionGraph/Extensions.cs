using ExpressionGraph.Reflection;
using System;
using System.Collections.Generic;

namespace ExpressionGraph
{
    /// <summary>
    /// Extentions for enums.
    /// </summary>
    public static class Extensions
    {
        public static Expression<ReflectedInstance> AsExpression(this object obj, Action<ReflectionTree> config = null)
        {
            var builder = new ReflectionTree(obj);
            config?.Invoke(builder);
            return builder.AsExpression();
        }

        public static IEnumerable<object> Objects(this IEnumerable<ReflectedInstance> instanceReflecteds)
        {
            foreach (var r in instanceReflecteds)
                yield return r.Object;
        }

        //public static IEnumerable<object> Objects(this Expression<InstanceReflected> expression)
        //{
        //    foreach (var r in expression.ToEntities())
        //        yield return r.Entity.Object;
        //}

        //public static IEnumerable<InstanceReflected> ReflectTree(this Expression<InstanceReflected> expression)
        //{
        //    foreach (var r in expression.ToEntities())
        //        yield return r.Entity;
        //}
    }
}

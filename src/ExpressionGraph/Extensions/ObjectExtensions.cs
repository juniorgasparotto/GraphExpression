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
    public static class ObjectExtensions
    {
        public static InstanceTree AsReflection(this object obj)
        {
            var query = new InstanceTree(obj);
            return query;
        }
    }
}

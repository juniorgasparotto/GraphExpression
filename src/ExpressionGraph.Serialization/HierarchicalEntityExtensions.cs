using NCalc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionGraph
{
    public static class HierarchicalEntityExtensions
    {       
        public static HierarchicalEntity GetByIdentity(this IEnumerable<HierarchicalEntity> source, object id)
        {
            return source.FirstOrDefault(f => f.IdentityIsEquals(id));
        }
    }
}

using ExpressionGraph.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    public class ExpressionUtils
    {
        #region From "String"

        public static IEnumerable<HierarchicalEntity> FromString(params string[] expressions)
        {
            var converter = new ExpressionSerializer();
            return converter.FromString(expressions);
        }

        public static IEnumerable<HierarchicalEntity> FromString(List<HierarchicalEntity> paramsOfExpressions, params string[] expressions)
        {
            var converter = new ExpressionSerializer();
            return converter.FromString(paramsOfExpressions, expressions);
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public class ExpressionItemCloseParenthesis<T> : ExpressionItem<T>
    {
        internal ExpressionItemCloseParenthesis(int level, int levelInExpression, int index)
            : base(default(T), level, levelInExpression, index)
        {
        }

        public override string ToString()
        {
            return ")";
        }
    }
}

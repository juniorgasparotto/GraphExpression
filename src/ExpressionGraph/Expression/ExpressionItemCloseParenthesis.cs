using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    public class ExpressionItemCloseParenthesis<T> : ExpressionItem<T>
    {
        internal ExpressionItemCloseParenthesis(int level, int levelInExpression, int index, int indexSameLevel)
            : base(default(T), -1, level, levelInExpression, index, indexSameLevel)
        {
        }

        public override string ToString()
        {
            return ")";
        }
    }
}

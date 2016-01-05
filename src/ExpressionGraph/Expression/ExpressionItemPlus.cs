using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    public class ExpressionItemPlus<T> : ExpressionItem<T>
    {
        internal ExpressionItemPlus(int level, int levelInExpression, int index)
            : base(default(T), -1, level, levelInExpression, index)
        {
        }

        public override string ToString()
        {
            return " + ";
        }
    }
}

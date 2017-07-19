using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQuery
{
    public class ExpressionItemPlus<T> : ExpressionItem<T>
    {
        internal ExpressionItemPlus(int level, int levelInExpression, int index, int indexSameLevel)
            : base(default(T), -1, level, levelInExpression, index, indexSameLevel)
        {
        }

        public override string ToString()
        {
            return " + ";
        }
    }
}

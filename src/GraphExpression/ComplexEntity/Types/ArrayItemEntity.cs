using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class ArrayItemEntity : ComplexEntity
    {
        public int[] Key { get; private set; }

        public ArrayItemEntity(Expression<object> expression, int key, object value)
            : base(expression)
        {
            this.Key = new int[] { key };
            this.Entity = value;
        }

        public ArrayItemEntity(Expression<object> expression, int[] key, object value)
            : base(expression)
        {
            this.Key = key;
            this.Entity = value;
        }
    }
}

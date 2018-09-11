using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class CollectionItemEntity : ComplexEntity
    {
        public long Key { get; private set; }

        public CollectionItemEntity(Expression<object> expression, long key, object value) 
            : base(expression)
        {
            this.Key = key;
            this.Entity = value;
        }
    }
}

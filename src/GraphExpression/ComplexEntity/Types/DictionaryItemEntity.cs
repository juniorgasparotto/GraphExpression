using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class DictionaryItemEntity : ComplexEntity
    {
        public object Key { get; private set; }

        public DictionaryItemEntity(Expression<object> expression, object key, object value) 
            : base(expression)
        {
            this.Key = key;
            this.Entity = value;
        }
    }
}

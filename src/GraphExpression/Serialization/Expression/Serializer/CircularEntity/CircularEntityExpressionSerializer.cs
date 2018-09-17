using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class CircularEntityExpressionSerializer<T> : ExpressionSerializerBase<T>
    {
        private readonly Expression<T> expression;

        public Func<T, object> EntityNameCallback { get; set; }

        public CircularEntityExpressionSerializer(Expression<T> expression, Func<T, object> entityNameCallback)
            : base(expression)
        {
            this.expression = expression;
            this.EntityNameCallback = entityNameCallback;
        }

        public override string SerializeItem(EntityItem<T> item)
        {
            string value;

            if (item.Entity == null)
                value = "null";
            else
                value = ValueFormatter.Format(item.Entity.GetType(), EntityNameCallback(item.Entity), true);

            var encloseStart = EncloseItem ? "{" : null;
            var encloseEnd = EncloseItem ? "}" : null;

            return $"{encloseStart}{value}{encloseEnd}";
        }
    }
}

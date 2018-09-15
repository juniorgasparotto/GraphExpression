using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class SerializationAsExpression<T> : ISerialization<T>
    {
        private readonly Expression<T> expression;

        public bool EncloseParenthesisInRoot { get; set; }
        public Func<EntityItem<T>, string> SerializeItem { get; set; }

        public SerializationAsExpression(Expression<T> expression)
        {
            this.expression = expression;
            this.SerializeItem = SerializeItemInternal;
        }

        public virtual string Serialize()
        {
            var output = "";
            expression.IterationAll
                (
                    itemBeginGroupExpression =>
                    {
                        if (itemBeginGroupExpression.IsRoot)
                        {
                            if (EncloseParenthesisInRoot)
                                output = "(";

                            output += itemBeginGroupExpression.ToString();
                        }
                        else
                        {
                            output += " + ";
                            if (itemBeginGroupExpression.IsFirstInParent)
                                output += "(";

                            output += itemBeginGroupExpression.ToString();
                        }
                    },
                    itemEndGroupExpression =>
                    {
                        if (!itemEndGroupExpression.IsRoot || (itemEndGroupExpression.IsRoot && EncloseParenthesisInRoot))
                            output += ")";
                    }
                );

            return output;
        }

        private string SerializeItemInternal(EntityItem<T> item)
        {
            return item.Entity.ToString();
        }
    }
}

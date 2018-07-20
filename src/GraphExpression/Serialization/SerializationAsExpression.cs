using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class SerializationAsExpression<T> : ISerialization<T>
    {
        private readonly Expression<T> expression;

        public bool EncloseParenthesisInRoot { get; set; }
        public Func<EntityItem<T>, string> SerializeItem { get; }

        public SerializationAsExpression(Expression<T> expression, Func<EntityItem<T>, string> serializeItem = null)
        {
            this.expression = expression;
            this.SerializeItem = serializeItem ?? (i => i.Entity?.ToString());
        }

        public virtual string Serialize()
        {
            var parenthesisToClose = new Stack<EntityItem<T>>();
            var output = "";
            foreach (var item in expression)
            {
                var next = item.Next;
                var isFirstInParenthesis = item.IsFirstInParent;
                var isLastInParenthesis = item.IsLastInParent;

                if (!item.IsRoot) output += " + ";

                if ((!item.IsRoot && isFirstInParenthesis) || (item.IsRoot && EncloseParenthesisInRoot == true))
                {
                    output += "(";
                    parenthesisToClose.Push(item);
                }

                output += SerializeItem(item);

                if (isLastInParenthesis)
                {
                    int countToClose;

                    if (next == null)
                        countToClose = parenthesisToClose.Count;
                    else
                        countToClose = item.Level - next.Level;

                    for (var i = countToClose; i > 0; i--)
                    {
                        parenthesisToClose.Pop();
                        output += ")";
                    }
                }
            }

            return output;
        }
    }
}

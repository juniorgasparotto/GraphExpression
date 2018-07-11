using System;
using System.Collections.Generic;

namespace GraphExpression.Serialization
{
    public class SerializationAsExpression<T>
    {
        private readonly Expression<T> expression;

        public bool EncloseParenthesisInRoot { get; set; }
        public Func<EntityItem<T>, string> SerializeItemCallback { get; set; }

        public SerializationAsExpression(Expression<T> expression)
        {
            this.expression = expression;
            this.SerializeItemCallback = SerializeItem;
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

                output += SerializeItemCallback(item);

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

        private string SerializeItem(EntityItem<T> item)
        {
            return item.Entity?.ToString();
        }
    }
}

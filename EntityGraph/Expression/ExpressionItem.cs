using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public class ExpressionItem<T>
    {
        public T Entity { get; private set; }
        public int Level { get; private set; }
        public int Index { get; private set; }

        public int LevelInExpression { get; private set; }
        public ExpressionItem<T> PreviousInExpression { get; internal set; }
        public ExpressionItem<T> NextInExpression { get; internal set; }

        public ExpressionItem<T> Root { get; internal set; }
        public ExpressionItem<T> Parent { get; internal set; }
        
        public ExpressionItem<T> Next
        {
            get 
            {
                var next = this.NextInExpression;

                while (next != null && next.GetType() != typeof(ExpressionItem<T>))
                    next = next.NextInExpression;

                return next;
            } 
        }

        public ExpressionItem<T> Previous
        {
            get
            {
                var previous = this.PreviousInExpression;

                while (previous != null && previous.GetType() != typeof(ExpressionItem<T>))
                    previous = previous.PreviousInExpression;

                return previous;
            }
        }

        internal ExpressionItem(T entity, int level, int levelInExpression, int index)
        {
            this.Entity = entity;
            this.Level = level;
            this.LevelInExpression = levelInExpression;
            this.Index = index;
        }

        public override string ToString()
        {
            return this.Entity.ToString();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public class ExpressionItem<T>
    {
        //public PathItem<T> PathItem { get; private set; }
        public T Entity { get; private set; }
        public int LevelInExpression { get; private set; }
        public int Level { get; private set; }
        public int Index { get; private set; }

        public ExpressionItem<T> Root { get; internal set; }
        public ExpressionItem<T> Parent { get; internal set; }
        public ExpressionItem<T> Previous { get; internal set; }
        public ExpressionItem<T> Next { get; internal set; }
        
        public ExpressionItem<T> Next2
        {
            get 
            {
                var next = this.Next;

                while (next != null && next.GetType() != typeof(ExpressionItem<T>))
                    next = next.Next;

                return next;
            } 
        }

        public ExpressionItem<T> Previous2
        {
            get
            {
                var previous = this.Previous;

                while (previous != null && previous.GetType() != typeof(ExpressionItem<T>))
                    previous = previous.Next;

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

    public class ExpressionItemPlus<T> : ExpressionItem<T>
    {
        internal ExpressionItemPlus(int level, int levelInExpression, int index)
            : base(default(T), level, levelInExpression, index)
        {
        }

        public override string ToString()
        {
            return " + ";
        }
    }

    public class ExpressionItemOpenParenthesis<T> : ExpressionItem<T>
    {
        internal ExpressionItemOpenParenthesis(int level, int levelInExpression, int index)
            : base(default(T), level, levelInExpression, index)
        {
        }

        public override string ToString()
        {
            return "(";
        }
    }

    public class ExpressionItemCloseParenthesis<T> : ExpressionItem<T>
    {
        internal ExpressionItemCloseParenthesis(int level, int levelInExpression, int index)
            : base(default(T), level, levelInExpression, index)
        {
        }

        public override string ToString()
        {
            return ")";
        }
    }
}

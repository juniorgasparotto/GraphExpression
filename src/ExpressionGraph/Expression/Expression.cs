using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    public class Expression<T> : IEnumerable<ExpressionItem<T>>
    {
        private static Type TypeOpenParenthesis = typeof(ExpressionItemOpenParenthesis<T>);
        private static Type TypeCloseParenthesis = typeof(ExpressionItemCloseParenthesis<T>);
        private static Type TypePlus = typeof(ExpressionItemPlus<T>);
        private Func<T, string> toStringCallBack;
        private bool isLastOpenParenthesis = false;

        private List<ExpressionItem<T>> items;
        private ExpressionItem<T> currentParent;
        private ExpressionItem<T> lastItem;

        private int levelInExpression = 1;
        private int level = 1;

        public bool EnableParenthesis { get; private set; }
        public bool EnablePlus { get; private set; }

        public ExpressionItem<T> this[int i]
        {
            get
            {
                return items[i];
            }
        }

        public int Count
        { 
            get
            {
                return this.items.Count;
            }
        }

        internal Expression(bool enableParenthesis, bool enablePlus, Func<T, string> toStringCallBack = null)
        {
            this.items = new List<ExpressionItem<T>>();
            this.toStringCallBack = toStringCallBack;
            this.EnableParenthesis = enableParenthesis;
            this.EnablePlus = enablePlus;
        }

        internal void AddItem(T item)
        {
            //var lastItemIsOpenParenthesis = this.lastItem != null && this.lastItem.GetType() == TypeOpenParenthesis;
            //var lastItemIsOpenParenthesis = this.isOpenParenthesis;

            var lastItemIsPlus = this.lastItem != null && this.lastItem.GetType() == TypePlus;

            if (this.EnablePlus && this.items.Count > 0 && !this.isLastOpenParenthesis && !lastItemIsPlus)
            {
                var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count);
                this.items.Add(plus);

                plus.PrevInExpression = this.lastItem;
                plus.Root = this.items[0];
                plus.Parent = this.currentParent;

                this.lastItem.NextInExpression = plus;
                this.lastItem = plus;
            }

            var current = new ExpressionItem<T>(item, this.level, this.levelInExpression, this.items.Count);
            current.ToStringCallBack = this.toStringCallBack;

            this.items.Add(current);

            current.PrevInExpression = this.lastItem;
            current.Root = this.items[0];
            current.Parent = this.currentParent;

            if (this.lastItem != null)
                this.lastItem.NextInExpression = current;

            this.lastItem = current;

            // if exists only root or last item, in init function, is a open parentheisis
            if (this.items.Count == 1 || this.isLastOpenParenthesis)
            { 
                this.level++;
                this.currentParent = current;
            }

            this.isLastOpenParenthesis = false;
        }

        internal void OpenParenthesis()
        {
            this.isLastOpenParenthesis = true;

            if (this.items.Count > 0)
            {
                if (this.EnablePlus)
                { 
                    var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count);
                    this.items.Add(plus);

                    plus.PrevInExpression = this.lastItem;
                    plus.Root = this.items[0];
                    plus.Parent = this.currentParent;

                    this.lastItem.NextInExpression = plus;
                    this.lastItem = plus;
                }

                this.levelInExpression++;
            }

            if (this.EnableParenthesis)
            { 
                var current = new ExpressionItemOpenParenthesis<T>(this.level, this.levelInExpression, this.items.Count);
                this.items.Add(current);

                current.PrevInExpression = lastItem;
                current.Root = this.items[0];
                current.Parent = this.currentParent;

                if (this.lastItem != null)
                    this.lastItem.NextInExpression = current;

                this.lastItem = current;
            }
        }

        internal void CloseParenthesis()
        {
            this.level--;
            this.currentParent = this.currentParent.Parent;

            if (this.EnableParenthesis)
            {
                var current = new ExpressionItemCloseParenthesis<T>(this.level, this.levelInExpression, this.items.Count);
                this.items.Add(current);

                current.PrevInExpression = lastItem;
                current.Root = this.items[0];
                current.Parent = this.currentParent;

                if (this.lastItem != null)
                    this.lastItem.NextInExpression = current;

                lastItem = current;
            }

            this.levelInExpression--;
        }

        public int IndexOf(ExpressionItem<T> item)
        {
            return this.items.IndexOf(item);
        }

        public IEnumerator<ExpressionItem<T>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        #region Overrides

        public override string ToString()
        {
            var output = "";

            this.items.FirstOrDefault().IterationAll
                (
                    itemWhenStart =>
                    {
                        if (itemWhenStart.IsRoot())
                        {
                            output = itemWhenStart.ToString();
                        }
                        else
                        {
                            output += " + ";
                            if (itemWhenStart.HasChildren())
                                output += "(";

                            output += itemWhenStart.ToString();
                        }
                    },
                    itemWhenEnd =>
                    {
                        if (!itemWhenEnd.IsRoot())
                            output += ")";
                    }
                );

            return output;
        }

        #endregion
    }
}
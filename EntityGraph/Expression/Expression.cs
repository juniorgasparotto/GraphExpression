using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public class Expression<T> : IEnumerable<ExpressionItem<T>>
    {
        private static Type TypeOpenParenthesis = typeof(ExpressionItemOpenParenthesis<T>);
        private static Type TypeCloseParenthesis = typeof(ExpressionItemCloseParenthesis<T>);
        private static Type TypePlus = typeof(ExpressionItemPlus<T>);
        private Func<T, string> toStringCallBack;

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

        public void AddItem(T item)
        {
            var lastItemIsOpenParenthesis = this.lastItem != null && this.lastItem.GetType() == TypeOpenParenthesis;
            var lastItemIsPlus = this.lastItem != null && this.lastItem.GetType() == TypePlus;

            if (this.EnablePlus && this.items.Count > 0 && !lastItemIsOpenParenthesis && !lastItemIsPlus)
            {
                var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count);
                this.items.Add(plus);

                plus.PreviousInExpression = this.lastItem;
                plus.Root = this.items[0];
                plus.Parent = this.currentParent;

                this.lastItem.NextInExpression = plus;
                this.lastItem = plus;
            }

            var current = new ExpressionItem<T>(item, this.level, this.levelInExpression, this.items.Count);
            current.ToStringCallBack = this.toStringCallBack;
            this.items.Add(current);

            current.PreviousInExpression = this.lastItem;
            current.Root = this.items[0];
            current.Parent = this.currentParent;

            if (this.lastItem != null)
                this.lastItem.NextInExpression = current;

            this.lastItem = current;

            // if exists only root or last item, in init function, is a open parentheisis
            if (this.items.Count == 1 || lastItemIsOpenParenthesis)
            { 
                this.level++;
                this.currentParent = current;
            }
        }

        public void OpenParenthesis()
        {
            if (this.items.Count > 0)
            {
                if (this.EnablePlus)
                { 
                    var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count);
                    this.items.Add(plus);

                    plus.PreviousInExpression = this.lastItem;
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

                current.PreviousInExpression = lastItem;
                current.Root = this.items[0];
                current.Parent = this.currentParent;

                if (this.lastItem != null)
                    this.lastItem.NextInExpression = current;

                lastItem = current;
            }
        }

        public void CloseParenthesis()
        {
            this.level--;
            this.currentParent = this.currentParent.Parent;

            if (this.EnableParenthesis)
            {
                var current = new ExpressionItemCloseParenthesis<T>(this.level, this.levelInExpression, this.items.Count);
                this.items.Add(current);

                current.PreviousInExpression = lastItem;
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
            var str = "";
            ExpressionItem<T> last = null;
            foreach (var item in items)
            {
                if (!this.EnableParenthesis && last != null && item.Level < last.Level)
                    str += ")";

                if (!this.EnablePlus 
                    && last != null
                    && last.GetType() != typeof(ExpressionItemOpenParenthesis<T>)
                    && item.GetType() != typeof(ExpressionItemCloseParenthesis<T>)
                   )
                { 
                    str += " + ";
                }

                if (!this.EnableParenthesis
                    && last != null
                    && item.Level > last.Level
                   )
                {
                    if (item.GetType() != typeof(ExpressionItemPlus<T>))
                    { 
                        str += "(";
                        str += item.ToString();
                    }
                    else
                    {
                        str += item.ToString();
                        str += "(";
                    }
                }
                else
                {
                    str += item.ToString();
                }

                last = item;
            }

            return str;
        }

        public string ToDebug()
        {
            var str = "";
            foreach (var i in items)
                str += i.ToString().Trim() + " ";

            str += "\r\n";
            foreach (var i in items)
                str += i.LevelInExpression.ToString() + " ";

            str += "\r\n";
            foreach (var i in items)
                str += i.Level.ToString() + " ";

            str += "\r\n";
            foreach (var i in items)
                str += (i.Parent == null) ? "- " : i.Parent.ToString() + " ";

            str += "\r\n";
            foreach (var i in items)
                str += i.Root.ToString() + " ";

            return str;
        }

        #endregion
    }
}
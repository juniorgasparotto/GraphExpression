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
        private Func<ExpressionItem<T>, string> toStringCallBack;
        private bool isLastOpenParenthesis = false;

        private Dictionary<T, long> itemsIds;
        private List<ExpressionItem<T>> items;
        private ExpressionItem<T> currentParent;
        private ExpressionItem<T> lastItem;

        private int levelInExpression = 1;
        private int level = 1;
        private long maxOfDifferentItems;
        
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

        public long CountOfDifferentEntities
        {
            get;
            private set;
        }

        internal Expression(bool enableParenthesis, bool enablePlus, Func<ExpressionItem<T>, string> toStringCallBack = null, long maxOfDifferentItems = 0)
        {
            this.items = new List<ExpressionItem<T>>();
            this.itemsIds = new Dictionary<T, long>();
            this.toStringCallBack = toStringCallBack;
            this.EnableParenthesis = enableParenthesis;
            this.EnablePlus = enablePlus;
            this.maxOfDifferentItems = maxOfDifferentItems;
        }

        internal void AddItem(T item, int index)
        {
            //var lastItemIsOpenParenthesis = this.lastItem != null && this.lastItem.GetType() == TypeOpenParenthesis;
            //var lastItemIsOpenParenthesis = this.isOpenParenthesis;
            var lastItemIsPlus = this.lastItem != null && this.lastItem.GetType() == TypePlus;

            if (this.EnablePlus && this.items.Count > 0 && !this.isLastOpenParenthesis && !lastItemIsPlus)
            {
                var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count, -1);
                this.items.Add(plus);

                plus.PrevInExpression = this.lastItem;
                plus.Root = this.items[0];
                plus.Parent = this.currentParent;

                this.lastItem.NextInExpression = plus;
                this.lastItem = plus;
            }

            long id;
            if (!itemsIds.ContainsKey(item))
            {
                if (this.maxOfDifferentItems > 0 && this.CountOfDifferentEntities == this.maxOfDifferentItems)
                    throw new Exception("Number of loaded items exceeded the limit of '" + this.maxOfDifferentItems + "'.");

                id = this.CountOfDifferentEntities++;
            }
            else
            {
                id = itemsIds[item];
            }

            var current = new ExpressionItem<T>(item, id, this.level, this.levelInExpression, this.items.Count, index);
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
                    var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count, -1);
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
                var current = new ExpressionItemOpenParenthesis<T>(this.level, this.levelInExpression, this.items.Count, -1);
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
                var current = new ExpressionItemCloseParenthesis<T>(this.level, this.levelInExpression, this.items.Count, -1);
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

        #region Iterations

        public void IterationAll(Action<ExpressionItem<T>> beginCodeBlockCallback, Action<ExpressionItem<T>> endCodeBlockCallback)
        {
            var typeOfItem = typeof(ExpressionItem<T>);
            var remainingItemsToClose = new Stack<ExpressionItem<T>>();

            // FROM:  A + ( B + C ) + J - Complete
            // FROM:  A   ( B   C )   J - Without "+"
            // FROM:  A +   B + C   + J - Without "(" and ")"
            // TO:    A     B   C     J - Result
            //        1 2 2 2 3 3 2 2 2 - Levels

            var current = this.items.FirstOrDefault();
            while (current != null)
            {
                // ignore "+" or "(" or ")"
                if (current.GetType() != typeOfItem)
                {
                    current = current.NextInExpression;
                    continue;
                }

                // start iteration
                beginCodeBlockCallback(current);

                if (current.HasChildren())
                    remainingItemsToClose.Push(current);

                if (current.IsLastInParenthesis())
                {
                    var next = current.Next;
                    int countToClose;

                    if (next == null)
                    {
                        // NEXT is null: Close all that are opening because the expression came to an end 
                        //               and don't exist next to get the base diff.
                        // ** The action is fired in item 'E' because it is last in your parenthesis group
                        // * A + (B + (C + (D + E)))
                        // * 1    2    3    4   5 => Levels
                        // *      ^    ^    ^   * => 3 items was opening when current is 'E'
                        // 
                        // ** Close 3 parenthesis when current is "E", the same amount that was opening.
                        countToClose = remainingItemsToClose.Count;
                    }
                    else
                    {
                        // Otherwise: close all pending using with base the next item.
                        // Sample 1
                        // ** The action is fired in item 'D' because it is last in your parenthesis group
                        // *     A + (B + (C + (D + E))) + J
                        // *     1    2    3    4   5      2      => Levels
                        // ** Calc diff:           [5  -   2 = 3] => Close 3 parenthesis when current is "E"
                        // Sample 2
                        // ** 1: The action is fired in item 'D' because it is last in your parenthesis group
                        // ** 2: The action is fired in item 'O' because it is last in your parenthesis group
                        // *     A + (B + (C + D) + O) + J
                        // *     1    2    3   4    3    2      => Levels
                        // ** 1: Calc diff:   [4  - 3      = 1] => Close 1 parenthesis when current is "D"
                        // ** 2: Calc diff:        [3  - 2 = 1] => Close 1 parenthesis when current is "O"
                        countToClose = current.Level - next.Level;
                    }

                    // end
                    for (var iClose = countToClose; iClose > 0; iClose--)
                        endCodeBlockCallback(remainingItemsToClose.Pop());
                }

                current = current.NextInExpression;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            var output = "";

            this.IterationAll
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
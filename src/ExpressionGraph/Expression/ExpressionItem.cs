using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph
{
    public class ExpressionItem<T>
    {
        public long Id { get; private set; }
        public int Index { get; private set; }
        public T Entity { get; private set; }
        public int Level { get; private set; }
        public int LevelInExpression { get; private set; }
        public ExpressionItem<T> PrevInExpression { get; internal set; }
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

        public ExpressionItem<T> Prev
        {
            get
            {
                var previous = this.PrevInExpression;

                while (previous != null && previous.GetType() != typeof(ExpressionItem<T>))
                    previous = previous.PrevInExpression;

                return previous;
            }
        }

        internal Func<ExpressionItem<T>, string> ToStringCallBack { get; set; }

        internal ExpressionItem(T entity, long id, int level, int levelInExpression, int index)
        {
            this.Entity = entity;
            this.Id = id;
            this.Level = level;
            this.LevelInExpression = levelInExpression;
            this.Index = index;
        }

        #region Checks

        public bool IsRoot()
        {
            var last = this.Prev;
            if (last == null)
                return true;

            return false;
        }

        public bool HasChildren()
        {
            var next = this.Next;
            if (next != null && this.Level < next.Level)
                return true;

            return false;
        }

        public bool IsLast()
        {
            var next = this.Next;
            if (next == null)
                return true;

            return false;
        }

        public bool IsLastInParenthesis()
        {
            var next = this.Next;
            if (next == null || this.Level > next.Level)
                return true;

            return false;
        }

        #endregion

        #region Ancestors

        public IEnumerable<ExpressionItem<T>> Ancestors(Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            if (depthStart <= 0)
                throw new ArgumentException("The 'depthStart' parameter can not be lower than 1.");

            if (depthEnd <= 0)
                throw new ArgumentException("The 'depthEnd' parameter can not be lower than 1.");

            if (depthStart > depthEnd)
                throw new ArgumentException("The 'depthStart' parameter can not be greater than the 'depthEnd' parameter.");

            var ancestor = this.Parent;

            while (ancestor != null)
            {
                var depth = this.Level - ancestor.Level;

                if (!depthStart.HasValue || !depthEnd.HasValue || (depth >= depthStart && depth <= depthEnd))
                {
                    var filterResult = (filter == null || filter(ancestor, depth));
                    var stopResult = (stop != null && stop(ancestor, depth));

                    if (filterResult)
                        yield return ancestor;

                    if (stopResult)
                        break;
                }

                ancestor = ancestor.Parent;
            }
        }

        #endregion

        #region Descendants

        public IEnumerable<ExpressionItem<T>> Descendants(Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            if (depthStart <= 0)
                throw new ArgumentException("The 'depthStart' parameter can not be lower than 1.");

            if (depthEnd <= 0)
                throw new ArgumentException("The 'depthEnd' parameter can not be lower than 1.");

            if (depthStart > depthEnd)
                throw new ArgumentException("The 'depthStart' parameter can not be greater than the 'depthEnd' parameter.");

            var descendant = this.Next;

            while (descendant != null && this.Level < descendant.Level)
            {
                var depth = descendant.Level - this.Level;

                if (!depthStart.HasValue || !depthEnd.HasValue || (depth >= depthStart && depth <= depthEnd))
                {
                    var filterResult = (filter == null || filter(descendant, depth));
                    var stopResult = (stop != null && stop(descendant, depth));

                    if (filterResult)
                        yield return descendant;

                    if (stopResult)
                        break;
                }

                descendant = descendant.Next;
            }
        }

        #endregion 

        #region Next or previous

        private enum SiblingDirection
        {
            Next,
            Previous
        }
        
        private IEnumerable<ExpressionItem<T>> NextsOrPrevious(SiblingDirection direction, Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            ExpressionItem<T> item;
            if (direction == SiblingDirection.Previous)
                item = this.Prev;
            else
                item = this.Next;

            var position = 1;
            while (item != null && this.Level <= item.Level)
            {
                var depth = Math.Abs(item.Level - this.Level);
                if (depth == 0)
                {
                    if (!positionStart.HasValue || !positionEnd.HasValue || (position >= positionStart && position <= positionEnd))
                    {
                        var filterResult = (filter == null || filter(item, position));
                        var stopResult = (stop != null && stop(item, position));

                        if (filterResult)
                            yield return item;

                        if (stopResult)
                            break;
                    }

                    position++;
                }

                if (direction == SiblingDirection.Previous)
                    item = item.Prev;
                else
                    item = item.Next;
            }
        }

        public IEnumerable<ExpressionItem<T>> Nexts(Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            return NextsOrPrevious(SiblingDirection.Next, filter, stop, positionStart, positionEnd);
        }

        public IEnumerable<ExpressionItem<T>> Previous(Func<ExpressionItem<T>, int, bool> filter = null, Func<ExpressionItem<T>, int, bool> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            return NextsOrPrevious(SiblingDirection.Previous, filter, stop, positionStart, positionEnd);
        }

        #endregion

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

            var current = this;
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
            if (ToStringCallBack != null)
                return ToStringCallBack(this);

            return this.Entity.ToString();
        }

        #endregion
    }
}
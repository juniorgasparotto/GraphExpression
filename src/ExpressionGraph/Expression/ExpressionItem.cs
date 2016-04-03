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

        internal ExpressionItem(T entity, long id, int level, int levelInExpression, int index, int indexSameLevel)
        {
            this.Entity = entity;
            this.Id = id;
            this.Level = level;
            this.LevelInExpression = levelInExpression;
            this.Index = index;
            this.IndexSameLevel = indexSameLevel;
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


        #region Overrides

        public override string ToString()
        {
            if (ToStringCallBack != null)
                return ToStringCallBack(this);

            return this.Entity.ToString();
        }

        #endregion

        public int IndexSameLevel { get; set; }
    }
}
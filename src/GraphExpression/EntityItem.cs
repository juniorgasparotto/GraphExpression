using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphExpression
{
    [DebuggerDisplay("{ToString()}")]
    public class EntityItem<T>
    {
        private readonly Expression<T> expression;

        public EntityItem(Expression<T> expression)
        {
            this.expression = expression;
        }

        public int Index { get; set; }
        public int IndexAtLevel { get; set; }
        public int Level { get; set; }
        public int LevelAtExpression { get; set; }

        public EntityItem<T> Previous { get => expression.ElementAtOrDefault(Index - 1); }
        public T Entity { get; set; }
        public EntityItem<T> Next { get => expression.ElementAtOrDefault(Index + 1); }

        public EntityItem<T> Parent
        {
            get
            {
                var previous = this.Previous;
                while(previous != null)
                {
                    if (previous.Level < this.Level)
                        return previous;
                    previous = previous.Previous;
                }
                return null;
            }
        }

        public bool IsRoot { get => Index == 0; }
        public bool IsLast { get => Next == null; }
        public bool IsFirstInParent { get => Next != null && Level < Next.Level; }
        public bool IsLastInParent { get => Next == null || Level > Next.Level; }

        #region Ancestors

        public IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            return Ancestors(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        public IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
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
        
        public IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            if (depthStart <= 0)
                throw new ArgumentException("The 'depthStart' parameter can not be lower than 1.");

            if (depthEnd <= 0)
                throw new ArgumentException("The 'depthEnd' parameter can not be lower than 1.");

            if (depthStart > depthEnd)
                throw new ArgumentException("The 'depthStart' parameter can not be greater than the 'depthEnd' parameter.");

            // A + (B + C + (D + B)) + (F + (I + (B + C + (D + B))))
            //                   ^                             ^
            //     ^^^
            // If the occurence not has children, then find the first 
            // ocurrence and use this to continue.
            EntityItem<T> reference = this;
            if (!IsFirstInParent)
                reference = expression.Find(Entity).First();

            var descendant = reference.Next;

            while (descendant != null && reference.Level < descendant.Level)
            {
                var depth = descendant.Level - reference.Level;

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

        public IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            return Descendants(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        public IEnumerable<EntityItem<T>> Descendants(int depthStart, int depthEnd)
        {
            return Descendants((EntityItemFilterDelegate2<T>)null, null, depthStart, depthEnd);
        }

        public IEnumerable<EntityItem<T>> Descendants(int depthEnd)
        {
            return Descendants(1, depthEnd);
        }

        #region DescendantsUntil

        public IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Descendants(filter, stop);
        }

        public IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            return DescendantsUntil(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        #endregion

        #region Children

        public IEnumerable<EntityItem<T>> Children()
        {
            return Descendants(1);
        }

        #endregion

        #endregion 

        #region Siblings

        public enum SiblingDirection
        {
            Next,
            Previous
        }

        public IEnumerable<EntityItem<T>> Siblings(SiblingDirection direction, EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, int? positionStart = null, int? positionEnd = null)
        {
            EntityItem<T> item;
            if (direction == SiblingDirection.Previous)
                item = this.Previous;
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
                    item = item.Previous;
                else
                    item = item.Next;
            }
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            if (expression.ToStringCallBack != null)
                return expression.ToStringCallBack(this);

            return this.Entity?.ToString();
        }

        #endregion
    }
}

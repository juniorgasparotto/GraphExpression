using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public partial class EntityItem<T>
    {
        #region Fields and properties 

        private readonly Expression<T> expression;
        private EntityItem<T> previous;
        private EntityItem<T> next; 
        private EntityItem<T> parent;

        public int Index { get; set; }
        public int IndexAtLevel { get; set; }
        public int Level { get; set; }
        public int LevelAtExpression { get; set; }

        public EntityItem<T> Previous
        {
            get
            {
                if (previous == null)
                    previous = expression.ElementAtOrDefault(Index - 1);
                return previous;
            }
            set => previous = value;
        }

        public T Entity { get; set; }

        public EntityItem<T> Next
        {
            get
            {
                if (next == null)
                    next = expression.ElementAtOrDefault(Index + 1);
                return next;
            }
            set => next = value;
        }

        public EntityItem<T> Parent
        {
            get
            {
                if (parent == null)
                {
                    var previous = this.Previous;
                    while (previous != null)
                    {
                        if (previous.Level < this.Level)
                        {
                            parent = previous;
                            break;
                        }

                        previous = previous.Previous;
                    }
                }

                return parent;
            }
            set => parent = value;
        }

        public bool IsRoot { get => Index == 0; }
        public bool IsLast { get => Next == null; }
        public bool IsFirstInParent { get => IsRoot || (Next != null && Level < Next.Level); }
        public bool IsLastInParent { get => Next == null || Level > Next.Level; }

        #region Graph info
        public Vertex<T> Vertex { get; set; }
        public Edge<T> Edge { get; set; }
        public Path<T> Path { get; set; }
        //public object ParentIterationRef { get; internal set; }
        #endregion

        #endregion

        public EntityItem(Expression<T> expression)
        {
            this.expression = expression;
        }

        public bool AreEntityEquals(EntityItem<T> compare)
        {
            if (compare == null)
                return false;

            return Entity?.Equals(compare.Entity) == true;
        }

        #region Ancestors

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

        public IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            return Ancestors(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        public IEnumerable<EntityItem<T>> Ancestors(int depthStart, int depthEnd)
        {
            return Ancestors((EntityItemFilterDelegate2<T>)null, null, depthStart, depthEnd);
        }

        public IEnumerable<EntityItem<T>> Ancestors(int depthEnd)
        {
            return Ancestors(1, depthEnd);
        }

        #region AncestorsUntil

        public IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Ancestors(filter, stop);
        }

        public IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            return AncestorsUntil(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        #endregion

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

        public IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, SiblingDirection direction = SiblingDirection.Both, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            EntityItem<T> item;
            var refLevel = Level;

            if (direction == SiblingDirection.Both)
            {
                // GET NEXT FROM THE PARENT D (A is parent and B is first child):
                // ( A + B + C + D )
                //               ^
                //   *   ^
                // item = B
                item = this.Parent?.Next;
                direction = SiblingDirection.Next;
            }
            else if (direction == SiblingDirection.Previous)
                item = Previous;
            else
                item = Next;

            var position = 1;
            while (item != null && refLevel <= item.Level)
            {
                var depth = Math.Abs(item.Level - refLevel);
                if (depth == 0)
                {
                    // The current element can not be returned as its own sibling
                    if (item != this)
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
                    }

                    position++;
                }

                if (direction == SiblingDirection.Previous)
                    item = item.Previous;
                else
                    item = item.Next;
            }
        }

        public IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Both, int? positionStart = null, int? positionEnd = null)
        {
            return Siblings(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), direction, positionStart, positionEnd);
        }

        public IEnumerable<EntityItem<T>> Siblings(int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Both)
        {
            return Siblings((EntityItemFilterDelegate2<T>)null, null, direction, positionStart, positionEnd);
        }

        public IEnumerable<EntityItem<T>> Siblings(int positionEnd, SiblingDirection direction = SiblingDirection.Both)
        {
            return Siblings(1, positionEnd, direction);
        }

        #region SiblingsUntil

        public IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Both)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Siblings(filter, stop, direction);
        }

        public IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Both)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Siblings(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), direction);
        }

        #endregion

        #endregion

        #region Overrides

        public override string ToString()
        {
            if (expression.DefaultSerializer?.SerializeItem(this) != null)
                return expression.DefaultSerializer?.SerializeItem(this);

            return this.Entity?.ToString();
        }

        #endregion
    }
}

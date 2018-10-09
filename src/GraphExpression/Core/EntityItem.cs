using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Represents an instance of an entity within an expression of graphs.
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public partial class EntityItem<T>
    {
        #region Fields and properties 

        private readonly Expression<T> expression;
        private EntityItem<T> previous;
        private EntityItem<T> next; 
        private EntityItem<T> parent;

        /// <summary>
        /// Index of the occurrence of the entity in the expression 
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Index of the occurrence of the entity within its level
        /// </summary>
        public int IndexAtLevel { get; set; }

        /// <summary>
        /// Entity level within the expression hierarchy
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Level of entity in expression
        /// </summary>
        public int LevelAtExpression { get; set; }

        /// <summary>
        /// Returns the previous occurrence of this entity.
        /// </summary>
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

        /// <summary>
        /// Real entity
        /// </summary>
        public T Entity { get; set; }

        /// <summary>
        /// Returns the next occurrence of this entity.
        /// </summary>
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

        /// <summary>
        /// Returns the parent entity of the current entity.
        /// </summary>
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

        /// <summary>
        /// Returns TRUE if the entity is the root.
        /// </summary>
        public bool IsRoot { get => Index == 0; }

        /// <summary>
        /// Returns TRUE if the entity is the last one in the expression
        /// </summary>
        public bool IsLast { get => Next == null; }

        /// <summary>
        /// Returns TRUE if the entity is the first within its expression group, ie a parent entity.
        /// </summary>
        public bool IsFirstInParent { get => IsRoot || (Next != null && Level < Next.Level); }

        /// <summary>
        /// Returns TRUE if the entity is the last one within its expression group.
        /// </summary>
        public bool IsLastInParent { get => Next == null || Level > Next.Level; }

        #region Graph info

        /// <summary>
        /// Contains information about the vertex within the graph.
        /// </summary>
        public Vertex<T> Vertex { get; set; }

        /// <summary>
        /// Contains information about the edge within the graph.
        /// </summary>
        public Edge<T> Edge { get; set; }

        /// <summary>
        /// Contains information about the path to this occurrence
        /// </summary>
        public Path<T> Path { get; set; }

        //public object ParentIterationRef { get; internal set; }

        #endregion

        #endregion

        /// <summary>
        /// Creates an instance based on an expression
        /// </summary>
        /// <param name="expression">Represents the expression of this entity</param>
        public EntityItem(Expression<T> expression)
        {
            this.expression = expression;
        }

        /// <summary>
        /// Checks whether two occurrences contain the same final entity.
        /// </summary>
        /// <param name="compare">Occurrence to be verified</param>
        /// <returns>Returns TRUE if they are equal</returns>
        public bool AreEntityEquals(EntityItem<T> compare)
        {
            if (compare == null)
                return false;

            return Entity?.Equals(compare.Entity) == true;
        }

        #region Ancestors

        /// <summary>
        /// Returns the ancestors of this occurrence within the hierarchy.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the parent of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
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

        /// <summary>
        /// Returns the ancestors of this occurrence within the hierarchy.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the parent of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
        public IEnumerable<EntityItem<T>> Ancestors(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            return Ancestors(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        /// <summary>
        /// Returns the ancestors of this occurrence within the hierarchy.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the parent of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
        public IEnumerable<EntityItem<T>> Ancestors(int depthStart, int depthEnd)
        {
            return Ancestors((EntityItemFilterDelegate2<T>)null, null, depthStart, depthEnd);
        }

        /// <summary>
        /// Returns the ancestors of this occurrence within the hierarchy.
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the parent and grandfather of the current occurrence.</param>
        /// <returns>Returns a list of ancestors</returns>
        public IEnumerable<EntityItem<T>> Ancestors(int depthEnd)
        {
            return Ancestors(1, depthEnd);
        }

        #region AncestorsUntil

        /// <summary>
        /// Returns the ancestors of this occurrence within the hierarchy until the "stop" action returns TRUE
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of ancestors</returns>
        public IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Ancestors(filter, stop);
        }

        /// <summary>
        /// Returns the ancestors of this occurrence within the hierarchy until the "stop" action returns TRUE
        /// An ancestor is a parent, grandparent, great-grandparent, and so on.
        /// </summary>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of ancestors</returns>
        public IEnumerable<EntityItem<T>> AncestorsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            return AncestorsUntil(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        #endregion

        #endregion

        #region Descendants

        /// <summary>
        /// Returns descendants of this occurrence within the hierarchy
        /// A descendant is a child, grandchild, great-grandchild, and so on.
        /// </summary>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the child of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
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

        /// <summary>
        /// Returns descendants of this occurrence within the hierarchy
        /// A descendant is a child, grandchild, great-grandchild, and so on.
        /// </summary>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the child of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
        public IEnumerable<EntityItem<T>> Descendants(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, int? depthStart = null, int? depthEnd = null)
        {
            return Descendants(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), depthStart, depthEnd);
        }

        /// <summary>
        /// Returns descendants of this occurrence within the hierarchy
        /// A descendant is a child, grandchild, great-grandchild, and so on.
        /// </summary>
        /// <param name="depthStart">Determines the initial depth. Example: depthStart = 1 returns the child of the current instance.</param>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
        public IEnumerable<EntityItem<T>> Descendants(int depthStart, int depthEnd)
        {
            return Descendants((EntityItemFilterDelegate2<T>)null, null, depthStart, depthEnd);
        }

        /// <summary>
        /// Returns descendants of this occurrence within the hierarchy
        /// A descendant is a child, grandchild, great-grandchild, and so on.
        /// </summary>
        /// <param name="depthEnd">Determines the final depth. Example: depthStart = 1; depthEnd = 2; returns the child and grandchild of the current occurrence.</param>
        /// <returns>Returns a list of descendants</returns>
        public IEnumerable<EntityItem<T>> Descendants(int depthEnd)
        {
            return Descendants(1, depthEnd);
        }

        #region DescendantsUntil

        /// <summary>
        /// Returns descendants of this occurrence within the hierarchy until the "stop" action returns TRUE
        /// A descendant is a child, grandchild, great-grandchild, and so on.
        /// </summary>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of descendants</returns>
        public IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Descendants(filter, stop);
        }

        /// <summary>
        /// Returns descendants of this occurrence within the hierarchy until the "stop" action returns TRUE
        /// A descendant is a child, grandchild, great-grandchild, and so on.
        /// </summary>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <returns>Returns a list of descendants</returns>
        public IEnumerable<EntityItem<T>> DescendantsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null)
        {
            return DescendantsUntil(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter));
        }

        #endregion

        #region Children

        /// <summary>
        /// Returns the children of this entity
        /// </summary>
        /// <returns>Returns the children of this entity</returns>
        public IEnumerable<EntityItem<T>> Children()
        {
            return Descendants(1);
        }

        #endregion

        #endregion

        #region Siblings

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <param name="positionStart">Determines start position</param>
        /// <param name="positionEnd">Determines end position</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate2<T> filter = null, EntityItemFilterDelegate2<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
        {
            if (positionStart <= 0)
                throw new ArgumentException("The 'positionStart' parameter can not be lower than 1.");

            if (positionEnd <= 0)
                throw new ArgumentException("The 'positionEnd' parameter can not be lower than 1.");

            if (positionStart > positionEnd)
                throw new ArgumentException("The 'positionStart' parameter can not be greater than the 'depthEnd' parameter.");

            EntityItem<T> item;
            var refLevel = Level;

            if (direction == SiblingDirection.Start)
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

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <param name="positionStart">Determines start position</param>
        /// <param name="positionEnd">Determines end position</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public IEnumerable<EntityItem<T>> Siblings(EntityItemFilterDelegate<T> filter, EntityItemFilterDelegate<T> stop = null, SiblingDirection direction = SiblingDirection.Start, int? positionStart = null, int? positionEnd = null)
        {
            return Siblings(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), direction, positionStart, positionEnd);
        }

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <param name="positionStart">Determines start position</param>
        /// <param name="positionEnd">Determines end position</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public IEnumerable<EntityItem<T>> Siblings(int positionStart, int positionEnd, SiblingDirection direction = SiblingDirection.Start)
        {
            return Siblings((EntityItemFilterDelegate2<T>)null, null, direction, positionStart, positionEnd);
        }

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction.
        /// </summary>
        /// <param name="positionEnd">Determines end position</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public IEnumerable<EntityItem<T>> Siblings(int positionEnd, SiblingDirection direction = SiblingDirection.Start)
        {
            return Siblings(1, positionEnd, direction);
        }

        #region SiblingsUntil

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction and until the "stop" action returns TRUE.
        /// </summary>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate2<T> stop, EntityItemFilterDelegate2<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Siblings(filter, stop, direction);
        }

        /// <summary>
        /// Returns a list of the siblings of this entity according to the selected direction and until the "stop" action returns TRUE.
        /// </summary>
        /// <param name="stop">Action to stop the search when it returns TRUE.</param>
        /// <param name="filter">Action to filter, you can remove what you do not need</param>
        /// <param name="direction">Determines the direction of the search: Right, Left, or Start from the first brother</param>
        /// <returns>Returns a list of siblings of this entity</returns>
        public IEnumerable<EntityItem<T>> SiblingsUntil(EntityItemFilterDelegate<T> stop, EntityItemFilterDelegate<T> filter = null, SiblingDirection direction = SiblingDirection.Start)
        {
            if (stop == null)
                throw new ArgumentNullException("stop");

            return Siblings(EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(filter), EntityItemFilterDelegateUtils<T>.ConvertToMajorDelegate(stop), direction);
        }

        #endregion

        #endregion

        #region Overrides

        /// <summary>
        /// Serialize this entity to string using the instance: "expression.DefaultSerializer"
        /// </summary>
        /// <returns>Entity serialized</returns>
        public override string ToString()
        {
            var output = expression.DefaultSerializer?.SerializeItem(this);
            if (output != null)
                return output;

            return this.Entity?.ToString();
        }

        #endregion
    }
}

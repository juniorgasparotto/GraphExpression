using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class Expression<T> : List<EntityItem<T>>
    {
        public static bool EnableNonRecursiveAlgorithm = true;

        private readonly Func<Expression<T>, EntityItem<T>, IEnumerable<EntityItem<T>>> getChildrenCallback;
        public bool Deep { get; }
        public ISerialization<T> DefaultSerializer { get; set; }

        public Expression()
        {
        }

        public Expression(Func<Expression<T>, EntityItem<T>> getRootCallback, Func<Expression<T>, EntityItem<T>, IEnumerable<EntityItem<T>>> childrenCallback, bool deep = false)
        {
            this.Deep = deep;
            this.getChildrenCallback = childrenCallback;

            var itemRoot = getRootCallback?.Invoke(this);
            if (itemRoot != null)
            {
                if (EnableNonRecursiveAlgorithm)
                    BuildNonRecursive(itemRoot);
                else
                    Build(itemRoot, getChildrenCallback(this, itemRoot));
            }
        }

        public Expression(T root, Func<T, IEnumerable<T>> childrenCallback, bool deep = false)
        {
            this.Deep = deep;
            this.getChildrenCallback = (expr, e) =>
            {
                return CreateEntityItems(childrenCallback(e.Entity));
            };

            if (root != null)
            {
                var itemRoot = new EntityItem<T>(this) { Entity = root };
                if (EnableNonRecursiveAlgorithm)
                    BuildNonRecursive(itemRoot);
                else
                    Build(itemRoot, getChildrenCallback(this, itemRoot));
            }
        }

        public void IterationAll(Action<EntityItem<T>> beginGroupExpressionCallback, Action<EntityItem<T>> endGroupExpressionCallback)
        {
            var typeOfItem = typeof(EntityItem<T>);
            var remainingItemsToClose = new Stack<EntityItem<T>>();

            // FROM:  A + ( B + C ) + J - Complete
            // FROM:  A   ( B   C )   J - Without "+"
            // FROM:  A +   B + C   + J - Without "(" and ")"
            // TO:    A     B   C     J - Result
            //        1 2 2 2 3 3 2 2 2 - Levels

            var current = this.FirstOrDefault();
            while (current != null)
            {
                var isFirstInParenthesis = current.IsFirstInParent;
                var isLastInParenthesis = current.IsLastInParent;

                // start iteration
                beginGroupExpressionCallback(current);

                if (isFirstInParenthesis)
                    remainingItemsToClose.Push(current);

                if (isLastInParenthesis)
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
                        endGroupExpressionCallback(remainingItemsToClose.Pop());
                }

                current = current.Next;
            }
        }

        #region recursive

        private void Build(EntityItem<T> parent, IEnumerable<EntityItem<T>> children, int level = 1)
        {
            // only when is root entity
            if (Count == 0)
            {
                PopulateEntityItem(parent, null, null, 0, 0, level, level);
                Add(parent);
            }

            var indexLevel = 0;
            var parentItem = this.Last();

            level++;            
            foreach (var child in children)
            {
                var previous = this.Last();
                previous.Next = child;

                PopulateEntityItem(child, parent, previous, Count, indexLevel++, 0, level);

                Add(child);

                // if:   IS 'deep' and the entity already declareted in expression, don't build the children of item.
                // else: if current entity exists in ancestors (to INFINITE LOOP), don't build the children of item.
                var continueBuild = true;
                if (Deep)
                    continueBuild = !HasAncestorEqualsTo(child);
                else
                    continueBuild = !IsEntityDeclared(child);

                var grandchildren = getChildrenCallback(this, child);
                if (continueBuild && grandchildren.Any())
                {
                    child.LevelAtExpression = parentItem.LevelAtExpression + 1;
                    Build(child, grandchildren, level);
                }
                else
                {
                    child.LevelAtExpression = parentItem.LevelAtExpression;
                }
            }
        }

        #endregion

        #region non-recursive

        private class Iteration
        {
            public IEnumerator<EntityItem<T>> Enumerator { get; set; }
            public EntityItem<T> EntityRootOfTheIterationForDebug { get; set; }
            public int Level { get; set; }
            public Iteration IterationParent { get; set; }
            public int IndexAtLevel { get; set; }

            public override string ToString()
            {
                if (EntityRootOfTheIterationForDebug == null)
                    return "";

                return EntityRootOfTheIterationForDebug.ToString();
            }
        }

        private void BuildNonRecursive(EntityItem<T> root)
        {
            var rootEnumerator = new EntityItem<T>[] { root }.Cast<EntityItem<T>>().GetEnumerator();
            var iteration = new Iteration()
            {
                Enumerator = rootEnumerator,
                Level = 1,
                IndexAtLevel = 0
            };

            var iterations = new List<Iteration>
            {
                iteration
            };

            while (true)
            {
                while (iteration.Enumerator.MoveNext())
                {
                    var entityItem = iteration.Enumerator.Current;
                    var parent = iteration.IterationParent?.Enumerator.Current;

                    bool exists;
                    if (Deep)
                        exists = HasAncestorEqualsTo(parent, entityItem.Entity);
                    else
                        exists = IsEntityDeclared(entityItem);

                    IEnumerable<EntityItem<T>> children = null;

                    if (!exists)
                        children = getChildrenCallback(this, entityItem);

                    var hasChildren = children != null && children.Count() > 0;

                    PopulateEntityItem(entityItem, parent, this.LastOrDefault(), Count, iteration.IndexAtLevel++, 0, iteration.Level);

                    if (hasChildren)
                    {
                        entityItem.LevelAtExpression = iteration.Level;

                        iteration = new Iteration()
                        {
                            Enumerator = children.GetEnumerator(),
                            Level = iteration.Level + 1,
                            EntityRootOfTheIterationForDebug = iteration.Enumerator.Current,
                            IterationParent = iteration,
                            IndexAtLevel = 0
                        };

                        iterations.Add(iteration);
                    }
                    else
                    {
                        entityItem.LevelAtExpression = iteration.Level - 1;
                    }

                    this.Add(entityItem);
                }

                // Remove iteration because is empty
                iterations.Remove(iteration);

                if (iterations.Count == 0)
                    break;

                iteration = iterations.LastOrDefault();
            }
        }

        #endregion

        #region Auxs

        private IEnumerable<EntityItem<T>> CreateEntityItems(IEnumerable<T> children)
        {
            foreach (var child in children)
            {
                yield return new EntityItem<T>(this)
                {
                    Entity = child
                };
            }
        }

        private void PopulateEntityItem(EntityItem<T> entityItem, EntityItem<T> parent, EntityItem<T> previous, int index, int indexAtLevel, int levelAtExpression, int level)
        {
            entityItem.Index = index;
            entityItem.Parent = parent;
            entityItem.Previous = previous;
            entityItem.IndexAtLevel = indexAtLevel;
            entityItem.LevelAtExpression = levelAtExpression;
            entityItem.Level = level;
        }

        private bool HasAncestorEqualsTo(EntityItem<T> entityItem)
        {
            var ancestor = entityItem.Parent;
            while (ancestor != null)
            {
                if (entityItem.Entity.Equals(ancestor.Entity))
                    return true;

                ancestor = ancestor.Parent;
            }

            return false;
        }

        private bool HasAncestorEqualsTo(EntityItem<T> parentRef, T entity)
        {
            bool exists = parentRef != null && parentRef.Entity != null && parentRef.Entity.Equals(entity);
            if (!exists && parentRef != null)
            {
                var ancestor = parentRef.Parent;
                while (ancestor != null)
                {
                    if (entity.Equals(ancestor.Entity))
                        return true;

                    ancestor = ancestor.Parent;
                }
            }

            return exists;
        }

        private bool IsEntityDeclared(EntityItem<T> entityItem)
        {
            return this.Any(e => e != entityItem && e.Entity?.Equals(entityItem.Entity) == true);
        }

        #endregion

        //public string ToMatrixAsString()
        //{
        //    var s = "";
        //    s += "Index    | Entity  | Level    | Level Index     | LevelAtExpression \r\n";

        //    foreach (var i in this)
        //    {
        //        s += $"{i.Index.ToString("00")}       ";
        //        s += $"| {i.Entity.Name}       ";
        //        s += $"| {i.Level.ToString("00")}       ";
        //        s += $"| {i.IndexAtLevel.ToString("00")}              ";
        //        s += $"| {i.LevelAtExpression.ToString("00")} \r\n";
        //    }
        //    return s;
        //}

    }
}
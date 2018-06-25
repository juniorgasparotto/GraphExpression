using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class Expression<T> : List<EntityItem<T>>
    {
        private readonly Func<T, IEnumerable<T>> childrenCallback;
        public bool Deep { get; }
        public Func<EntityItem<T>, string> ToStringCallBack { get; set; }

        public Expression()
        {
        }

        public Expression(T root, Func<T, IEnumerable<T>> childrenCallback, bool deep = false)
        {
            this.Deep = deep;
            this.childrenCallback = childrenCallback;

            if (root != null)
                Build(root);
        }

        private void Build(T parent, int level = 1)
        {
            // only when is root entity
            if (Count == 0)
            {
                var rootItem = new EntityItem<T>(this)
                {
                    Entity = parent,
                    Index = 0,
                    IndexAtLevel = 0,
                    LevelAtExpression = level,
                    Level = level
                };

                Add(rootItem);
            }

            var indexLevel = 0;
            var parentItem = this.Last();

            level++;
            var children = childrenCallback(parent);
            foreach (var child in children)
            {
                var previous = this.Last();
                var childItem = new EntityItem<T>(this)
                {
                    Entity = child,
                    Index = Count,
                    IndexAtLevel = indexLevel++,
                    Level = level,
                };

                Add(childItem);

                // if:   IS 'deep' and the entity already declareted in expression, don't build the children of item.
                // else: if current entity exists in ancestors (to INFINITE LOOP), don't build the children of item.
                var continueBuild = true;
                if (Deep)
                    continueBuild = !HasAncestorEqualsTo(childItem);
                else
                    continueBuild = !IsEntityDeclared(childItem);

                if (continueBuild && childrenCallback(child).Any())
                {
                    childItem.LevelAtExpression = parentItem.LevelAtExpression + 1;
                    Build(child, level);
                }
                else
                {
                    childItem.LevelAtExpression = parentItem.LevelAtExpression;
                }
            }
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

        private bool IsEntityDeclared(EntityItem<T> entityItem)
        {
            return this.Any(e => e != entityItem && e.Entity.Equals(entityItem.Entity));
        }

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

        public string ToExpressionAsString(bool encloseParenthesisInRoot = false)
        {
            var parenthesisToClose = new Stack<EntityItem<T>>();
            var output = "";
            foreach (var item in this)
            {
                var next = item.Next;
                var isFirstInParenthesis = item.IsFirstInParent;
                var isLastInParenthesis = item.IsLastInParent;

                if (!item.IsRoot) output += " + ";

                if ((!item.IsRoot && isFirstInParenthesis) || (item.IsRoot && encloseParenthesisInRoot))
                {
                    output += "(";
                    parenthesisToClose.Push(item);
                }

                output += item.Entity.ToString();

                if (isLastInParenthesis)
                {
                    int countToClose;

                    if (next == null)
                        countToClose = parenthesisToClose.Count;
                    else
                        countToClose = item.Level - next.Level;

                    for (var i = countToClose; i > 0; i--)
                    {
                        parenthesisToClose.Pop();
                        output += ")";
                    }
                }
            }

            return output;
        }
    }
}
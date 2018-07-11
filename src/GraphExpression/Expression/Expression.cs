using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class Expression<T> : List<EntityItem<T>>
    {
        private readonly Func<T, IEnumerable<T>> childrenCallback;
        public bool Deep { get; }
        public SerializationAsExpression<T> Serializer { get; set; }

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
                var rootItem = GetEntityItem(parent, 0, 0, level, level);
                Add(rootItem);
            }

            var indexLevel = 0;
            var parentItem = this.Last();

            level++;
            var children = childrenCallback(parent);
            foreach (var child in children)
            {
                var previous = this.Last();
                var childItem = GetEntityItem(child, Count, indexLevel++, 0, level);

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

        private EntityItem<T> GetEntityItem(T entity, int index, int indexAtLevel, int levelAtExpression, int level)
        {
            EntityItem<T> item;

            // merge situation
            if (typeof(T) == typeof(EntityItem<>))
                item = entity as EntityItem<T>;
            else
                item = new EntityItem<T>(this);

            item.Entity = entity;
            item.Index = index;
            item.IndexAtLevel = indexAtLevel;
            item.LevelAtExpression = levelAtExpression;
            item.Level = level;
            return item;
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
    }
}
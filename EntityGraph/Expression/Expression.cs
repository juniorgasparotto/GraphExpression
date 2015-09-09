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

        private List<ExpressionItem<T>> items;
        private ExpressionItem<T> currentParent;
        private ExpressionItem<T> lastItem;

        private int levelInExpression = 1;
        private int level = 1;

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

        internal Expression()
        {
            this.items = new List<ExpressionItem<T>>();
        }

        //public IEnumerable<T> AncestorsOf(T entity)
        //{
        //    var hashSet = new HashSet<T>();

        //    var itemsFound = this.items.Where(f => f.Entity != null && f.Entity.Equals(entity)).ToList();
        //    foreach (var itemFound in itemsFound)
        //    {
        //        for (var i = itemFound.Index - 1; i > 0; i--)
        //        {
        //            // Identifies an ancestor when the left neighbor is a closed parenthesis 
        //            // AND his level is less than or equal to the item found. This is to prevent the situation:
        //            // A+(B+(C+D)+E)
        //            // 1122233333222
        //            //       ^ <- E : The ancestor is "C" if don't exists "this.items[i].Level <= itemFound.Level"
        //            if (this.items[i].GetType() == typeof(ExpressionItemOpenParenthesis<T>) && this.items[i].LevelInExpression <= itemFound.LevelInExpression)
        //            {
        //                var openParenthesis = this.items[i];
        //                var parent = this.items[i + 1];

        //                if (parent.Index != itemFound.Index)
        //                    hashSet.Add(parent.Entity);

        //                if (openParenthesis.LevelInExpression == 2)
        //                    break;
        //            }
        //        }

        //        ExpressionItem<T> root = this.items[0] ;

        //        // Add root when 'itemFound' the highest index than 'root'
        //        if (itemFound.Index > root.Index)
        //            hashSet.Add(root.Entity);
        //    }

        //    return hashSet;
        //}

        //public IEnumerable<T> DescendantsOf(T entity, int depth = 0)
        //{
        //    var hashSet = new HashSet<T>();

        //    var itemFound = this.items.FirstOrDefault(f =>
        //    {
        //        if (f.Entity != null && f.Entity.Equals(entity))
        //        {
        //            // is root or has children
        //            if (f.Previous == null || f.Previous.GetType() == typeof(ExpressionItemOpenParenthesis<T>))
        //                return true;
        //        }

        //        return false;
        //    }
        //    );

        //    if (itemFound != null)
        //    {
        //        for (var i = itemFound.Index + 1; i < this.items.Count; i++)
        //        {
        //            var curItem = this.items[i];
        //            if (curItem.GetType() == typeof(ExpressionItem<T>) && (depth == 0 || itemFound.Level >= curItem.Level - depth))
        //                hashSet.Add(curItem.Entity);
        //            else if (curItem.LevelInExpression == itemFound.LevelInExpression && curItem.GetType() == typeof(ExpressionItemCloseParenthesis<T>))
        //                break;
        //        }
        //    }

        //    return hashSet;
        //}

        //public IEnumerable<T> ParentsOf(T entity)
        //{
        //    var hashSet = new HashSet<T>();

        //    var itemsFound = this.items.Where(f => f.Entity != null && f.Entity.Equals(entity)).ToList();
        //    foreach (var itemFound in itemsFound)
        //    {
        //        for (var i = itemFound.Index - 1; i > 0; i--)
        //        {
        //            if (this.items[i].GetType() == typeof(ExpressionItemOpenParenthesis<T>) && this.items[i].LevelInExpression <= itemFound.LevelInExpression)
        //            {
        //                var openParenthesis = this.items[i];
        //                var parent = this.items[i + 1];

        //                if (parent.Index != itemFound.Index)
        //                    hashSet.Add(parent.Entity);

        //                if (openParenthesis.LevelInExpression == 2 || openParenthesis.Level == itemFound.Level)
        //                    break;
        //            }
        //        }

        //        ExpressionItem<T> root = this.items[0];

        //        // Add root when 'itemFound' the highest index than 'root'
        //        if (itemFound.Index > root.Index)
        //            hashSet.Add(root.Entity);
        //    }

        //    return hashSet;
        //}

        //public IEnumerable<T> ChildrenOf(T entity)
        //{
        //    return this.DescendantsOf(entity, 1);
        //}

        public void AddItem(T item)
        {
            var lastItemIsOpenParenthesis = this.lastItem != null && this.lastItem.GetType() == TypeOpenParenthesis;

            if (this.items.Count > 0 && !lastItemIsOpenParenthesis)
            {
                var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count);
                this.items.Add(plus);

                plus.Previous = this.lastItem;
                plus.Root = this.items[0];
                plus.Parent = this.currentParent;

                this.lastItem.Next = plus;
                this.lastItem = plus;
            }

            var current = new ExpressionItem<T>(item, this.level, this.levelInExpression, this.items.Count);
            this.items.Add(current);

            current.Previous = this.lastItem;
            current.Root = this.items[0];
            current.Parent = this.currentParent;

            if (this.lastItem != null)
                this.lastItem.Next = current;

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
                var plus = new ExpressionItemPlus<T>(this.level, this.levelInExpression, this.items.Count);
                this.items.Add(plus);

                plus.Previous = this.lastItem;
                plus.Root = this.items[0];
                plus.Parent = this.currentParent;

                this.lastItem.Next = plus;
                this.lastItem = plus;
                
                this.levelInExpression++;
            }

            var current = new ExpressionItemOpenParenthesis<T>(this.level, this.levelInExpression, this.items.Count);
            this.items.Add(current);

            current.Previous = lastItem;
            current.Root = this.items[0];
            current.Parent = this.currentParent;

            if (this.lastItem != null)
                this.lastItem.Next = current;

            lastItem = current;
        }

        public void CloseParenthesis()
        {
            this.level--;
            this.currentParent = this.currentParent.Parent;

            var current = new ExpressionItemCloseParenthesis<T>(this.level, this.levelInExpression, this.items.Count);
            this.items.Add(current);

            current.Previous = lastItem;
            current.Root = this.items[0];
            current.Parent = this.currentParent;

            if (this.lastItem != null)
                this.lastItem.Next = current;

            lastItem = current;
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
            foreach (var i in items)
                str += i.ToString();
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

        #region Temp

        //public static bool operator ==(Path<T> a, Path<T> b)
        //{
        //    return Equals(a, b);
        //}

        //public static bool operator !=(Path<T> a, Path<T> b)
        //{
        //    return !Equals(a, b);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
        //        return false;

        //    var converted = (Path<T>)obj;
        //    return (this.Identity == converted.Identity);
        //}

        //public override int GetHashCode()
        //{
        //    return 0;
        //}

        #endregion
    }
}
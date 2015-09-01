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
        private ExpressionItem<T> lastItem;
        private int levelInExpression = 1;

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

        public IEnumerable<T> AncestorsOf(T entity, int depth = 0)
        {
            var hashSet = new HashSet<T>();

            var itemsFound = this.items.Where(f => f.Entity != null && f.Entity.Equals(entity)).ToList();
            foreach (var itemFound in itemsFound)
            {
                var depthReal = (itemFound.LevelInExpression - depth);
                for (var i = itemFound.Index - 1; i > 0; i--)
                {
                    // Identifies an ancestor when the left neighbor is a closed parenthesis 
                    // AND his level is less than or equal to the item found. This is to prevent the situation:
                    // A+(B+(C+D)+E)
                    // 1122233333222
                    //       ^ <- E : The ancestor is "C" if don't exists "this.items[i].Level <= itemFound.Level"
                    if (this.items[i].GetType() == typeof(ExpressionItemOpenParenthesis<T>) && this.items[i].LevelInExpression <= itemFound.LevelInExpression)
                    {
                        var currentItem = this.items[i];
                        var nextItemForAdd = this.items[i + 1];

                        if (nextItemForAdd.Index != itemFound.Index)
                            hashSet.Add(nextItemForAdd.Entity);

                        if (currentItem.LevelInExpression == 2 || (depth > 0 && currentItem.LevelInExpression > depthReal))
                            break;
                    }
                }

                ExpressionItem<T> root;
                if (this.items[0].GetType() == typeof(ExpressionItemOpenParenthesis<T>))
                    root = this.items[1];
                else
                    root = this.items[0];

                // Add root when 'itemFound' the highest index than 'root'
                if (itemFound.Index > root.Index)
                    hashSet.Add(root.Entity);
            }

            // If found, add root with last ancestors
            if (itemsFound.Count > 0)
            {
                
            }

            return hashSet;
        }

        public IEnumerable<T> DescendantsOf(T entity, int depth = 0)
        {
            var hashSet = new HashSet<T>();

            var itemFound = this.items.FirstOrDefault(f =>
            {
                if (f.Entity != null && f.Entity.Equals(entity))
                {
                    // is root or has children
                    if (f.Previous == null || f.Previous.GetType() == typeof(ExpressionItemOpenParenthesis<T>))
                        return true;
                }

                return false;
            }
            );

            if (itemFound != null)
            {
                var depthReal = (itemFound.LevelInExpression + depth) - 1;
                for (var i = itemFound.Index + 1; i < this.items.Count; i++)
                {
                    var curItem = this.items[i];
                    if (curItem.GetType() == typeof(ExpressionItem<T>))
                        hashSet.Add(curItem.Entity);
                    else if (curItem.LevelInExpression == itemFound.LevelInExpression && curItem.GetType() == typeof(ExpressionItemCloseParenthesis<T>))
                        break;
                    else if (depth > 0 && curItem.LevelInExpression > depthReal)
                        break;
                }
            }

            return hashSet;
        }

        public void AddItem(T item)
        {
            var level = this.levelInExpression;
            var lastItemIsOpenParenthesis = this.lastItem != null && this.lastItem.GetType() == TypeOpenParenthesis;

            if (this.items.Count > 0 && !lastItemIsOpenParenthesis)
            {
                var plus = new ExpressionItemPlus<T>(level, this.levelInExpression, this.items.Count);
                this.items.Add(plus);

                plus.Previous = this.lastItem;
                this.lastItem.Next = plus;
                this.lastItem = plus;
            }
            else if (lastItemIsOpenParenthesis)
            {
                level--;
            }

            var currentToken = new ExpressionItem<T>(item, level, this.levelInExpression, this.items.Count);
            this.items.Add(currentToken);

            currentToken.Previous = this.lastItem;

            if (this.lastItem != null)
                this.lastItem.Next = currentToken;

            this.lastItem = currentToken;
        }

        public void OpenParenthesis()
        {
            if (this.items.Count > 0)
            {
                var plus = new ExpressionItemPlus<T>(this.levelInExpression, this.levelInExpression, this.items.Count);
                this.items.Add(plus);

                plus.Previous = this.lastItem;
                this.lastItem.Next = plus;
                this.lastItem = plus;
                
                // If "(" is first token, can't count level
                this.levelInExpression++;
            }

            var currentToken = new ExpressionItemOpenParenthesis<T>(this.levelInExpression - 1, this.levelInExpression, this.items.Count);
            this.items.Add(currentToken);

            currentToken.Previous = lastItem;

            if (this.lastItem != null)
                this.lastItem.Next = currentToken;

            lastItem = currentToken;
        }

        public void CloseParenthesis()
        {
            var currentToken = new ExpressionItemCloseParenthesis<T>(this.levelInExpression - 1, this.levelInExpression, this.items.Count);
            this.items.Add(currentToken);

            currentToken.Previous = lastItem;

            if (this.lastItem != null)
                this.lastItem.Next = currentToken;

            lastItem = currentToken;
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
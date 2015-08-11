using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public class GraphExpression<T> : IEnumerable<IGraphExpressionPosition>
    {
        private List<IGraphExpressionPosition> tokens;

        public IGraphExpressionPosition this[int i]
        {
            get
            {
                return tokens[i];
            }
        }

        public int Count 
        { 
            get
            {
                return this.tokens.Count;
            }
        }

        internal GraphExpression()
        {
            this.tokens = new List<IGraphExpressionPosition>();
        }

        internal void Add(IGraphExpressionPosition item)
        {
            this.tokens.Add(item);
        }

        public IEnumerator<IGraphExpressionPosition> GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return tokens.GetEnumerator();
        }

        #region Overrides

        public override string ToString()
        {
            var str = "";
            foreach (var i in tokens)
                str += i.ToString();
            return str;
        }

        public string ToDebug()
        {
            var str = "";
            foreach (var i in tokens)
                str += i.ToString().Trim() + " ";

            str += "\r\n";
            foreach (var i in tokens)
                str += i.LevelInExpression.ToString() + " ";
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
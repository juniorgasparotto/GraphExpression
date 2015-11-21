using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph.Graph
{
    public class Path<T> : IEnumerable<PathItem<T>>
    {
        private List<PathItem<T>> pathItems;

        public PathItem<T> this[int i]
        {
            get
            {
                return pathItems[i];
            }
        }

        public string Identity { get; private set; }
        public PathType PathType { get; private set; }

        internal Path()
        {
            this.pathItems = new List<PathItem<T>>();
        }

        public bool ContainsPath(Path<T> pathTest)
        {
            if (this.Identity.Contains(pathTest.Identity))
                return true;
            return false;
        }

        internal IEnumerable<PathItem<T>> GetPrevious(Iteration<T> limit)
        {
            foreach (var pathItem in this.pathItems)
                if (pathItem.Iteration == limit)
                    break;
                else
                    yield return pathItem;
        }


        internal void SetType()
        {
            if (this.pathItems.First().Edge.Target == this.pathItems.Last().Edge.Target)
                this.PathType = PathType.Circuit;
            else
            {
                PathItem<T> last = null;
                foreach (var current in pathItems)
                {
                    if (last != null && current.Edge.Target == last.Edge.Target)
                    {
                        this.PathType = PathType.Circle;
                        break;
                    }
                    else
                    {
                        this.PathType = PathType.Simple;
                    }

                    last = current;
                }
            }
        }

        internal void Add(PathItem<T> item)
        {   
            this.Identity += (string.IsNullOrWhiteSpace(this.Identity) ? "" : ".") + "[" + item.Edge.Target.ToString() + "]";
            this.pathItems.Add(item);
        }

        public IEnumerator<PathItem<T>> GetEnumerator()
        {
            return pathItems.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return pathItems.GetEnumerator();
        }

        #region Overrides

        public static bool operator ==(Path<T> a, Path<T> b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(Path<T> a, Path<T> b)
        {
            return !Equals(a, b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
                return false;

            var converted = (Path<T>)obj;
            return (this.Identity == converted.Identity);
        }

        public override int GetHashCode()
        {
            return 0;
        }

        public string ToString(bool showEdge = false)
        {
            if (showEdge)
            {
                var output = "";
                foreach (var item in pathItems)
                    output += (output == "") ? item.ToString() : "." + item.ToString();
                return output;
            }
            else
            {
                return this.Identity;
            }
        }

        public override string ToString()
        {
            return this.ToString(false);
        }

        #endregion
    }
}
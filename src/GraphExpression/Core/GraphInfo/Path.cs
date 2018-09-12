using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class Path<T> : IEnumerable<PathItem<T>>
    {
        private List<PathItem<T>> pathItems;

        public string Identity { get; private set; }
        public PathType PathType { get; private set; }

        public PathItem<T> this[int i]
        {
            get
            {
                return pathItems[i];
            }
        }

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

        public bool AreEquals(Path<T> obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            return (this.Identity == obj.Identity);
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
            if (this.pathItems.First().Edge.Target?.AreEquals(this.pathItems.Last().Edge.Target) == true)
            {
                this.PathType = PathType.Circuit;
            }
            else
            {
                PathItem<T> last = null;
                foreach (var current in pathItems)
                {
                    if (last != null && current.Edge.Target?.AreEquals(last.Edge.Target) == true)
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
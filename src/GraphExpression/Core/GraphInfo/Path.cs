using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class Path<T>
    {
        private List<PathItem<T>> pathItems;

        public string Identity { get; private set; }
        public PathType PathType { get; private set; }

        public IReadOnlyList<PathItem<T>> Items
        {
            get => pathItems.AsReadOnly();
        }

        public Path()
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

        //public IEnumerable<PathItem<T>> GetPrevious(Iteration<T> limit)
        //{
        //    foreach (var pathItem in this.pathItems)
        //        if (pathItem.ParentIterationRef == limit)
        //            break;
        //        else
        //            yield return pathItem;
        //}

        public void SetType()
        {
            if (this.pathItems.First().Item?.AreEntityEquals(this.pathItems.Last().Item) == true)
            {
                this.PathType = PathType.Circuit;
            }
            else
            {
                PathItem<T> last = null;
                foreach (var current in pathItems)
                {
                    if (last != null && current.Item?.AreEntityEquals(last.Item) == true)
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

        public void Add(PathItem<T> item)
        {   
            this.Identity += (string.IsNullOrWhiteSpace(this.Identity) ? "" : ".") + "[" + item.VertexId + "]";
            item.Identity = this.Identity;
            this.pathItems.Add(item);
        }

        #region Overrides

        public string ToString(bool showEntityDesc)
        {
            if (showEntityDesc)
            {
                var output = "";
                foreach (var item in pathItems)
                {
                    var desc = $"[{item.Item?.ToString()}]";
                    output += (output == "") ? desc : "." + desc;
                }
                return output;
            }
            else
            {
                return this.Identity;
            }
        }

        public override string ToString()
        {
            return this.ToString(true);
        }

        #endregion
    }
}
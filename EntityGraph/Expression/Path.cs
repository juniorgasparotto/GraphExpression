//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;

//namespace EntityGraph
//{
//    private class Path<T> : IEnumerable<PathItem<T>>
//    {
//        private List<PathItem<T>> pathItems;

//        public PathItem<T> this[int i]
//        {
//            get
//            {
//                return pathItems[i];
//            }
//        }

//        public string Identity { get; private set; }
//        public PathType PathType { get; private set; }

//        public Path()
//        {
//            this.pathItems = new List<PathItem<T>>();
//        }

//        public IEnumerable<PathItem<T>> GetPrevious(Iteration<T> limit)
//        {
//            foreach (var pathItem in this.pathItems)
//                if (pathItem.Iteration == limit)
//                    break;
//                else
//                    yield return pathItem;
//        }

//        public void Add(PathItem<T> item)
//        {
//            this.Identity += (string.IsNullOrWhiteSpace(this.Identity) ? "" : ".") + "[" + item.Edge.Target.ToString() + "]";
//            this.pathItems.Add(item);
//        }

//        public IEnumerator<PathItem<T>> GetEnumerator()
//        {
//            return pathItems.GetEnumerator();
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return pathItems.GetEnumerator();
//        }

//        #region Overrides

//        public static bool operator ==(Path<T> a, Path<T> b)
//        {
//            return Equals(a, b);
//        }

//        public static bool operator !=(Path<T> a, Path<T> b)
//        {
//            return !Equals(a, b);
//        }

//        public override bool Equals(object obj)
//        {
//            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
//                return false;

//            var converted = (Path<T>)obj;
//            return (this.Identity == converted.Identity);
//        }

//        public override int GetHashCode()
//        {
//            return 0;
//        }

//        public string ToString(bool showEdge = false)
//        {
//            if (showEdge)
//            {
//                var output = "";
//                foreach (var item in pathItems)
//                    output += (output == "") ? item.ToString() : "." + item.ToString();
//                return output;
//            }
//            else
//            {
//                return this.Identity;
//            }
//        }

//        public override string ToString()
//        {
//            return this.ToString(false);
//        }

//        #endregion
//    }

//}
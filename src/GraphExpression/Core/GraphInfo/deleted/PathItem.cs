//namespace GraphExpression
//{
//    public class PathItem<T>
//    {
//        private Path<T> path;

//        public int Level { get; private set; }
//        public EntityItem<T> Item { get; private set; }
//        public object ParentIterationRef { get; private set; }
//        public long VertexId { get; set; }
//        public string Identity { get; set; }

//        public PathItem(Path<T> path, long vertexId, object parentIterationRef, EntityItem<T> item, int level)
//        {
//            this.path = path;
//            this.VertexId = vertexId;
//            this.Item = item;
//            this.Level = level;
//            this.ParentIterationRef = parentIterationRef;
//        }

//        #region Overrides

//        public string ToString(bool showEntityDesc)
//        {
//            if (showEntityDesc)
//            {
//                var output = "";
//                foreach (var pathItem in path.Items)
//                {
//                    var desc = $"[{pathItem.ToString()}]";
//                    output += (output == "") ? desc : "." + desc;

//                    if (pathItem == this.Item)
//                        break; ;
//                }
//                return output;
//            }
//            else
//            {
//                return this.Identity;
//            }
//        }
                
//        public override string ToString()
//        {
//            return ToString(true);
//        }

//        #endregion
//    }
//}
namespace GraphExpression
{
    public class PathItem<T>
    {
        private Path<T> path;

        public int Level { get; private set; }
        public Edge<T> Edge { get; private set; }
        public object ParentIterationRef { get; private set; }
        public string Identity { get; set; }

        public PathItem(Path<T> path, object parentIterationRef, Edge<T> edge, int level)
        {
            this.path = path;
            this.Edge = edge;
            this.Level = level;
            this.ParentIterationRef = parentIterationRef;
        }

        #region Overrides

        public string ToString(bool showEntityDesc)
        {
            if (showEntityDesc)
            {
                var output = "";
                foreach (var item in path.Items)
                {
                    var desc = $"[{item.Edge.Target?.ToString()}]";
                    output += (output == "") ? desc : "." + desc;

                    if (item == this)
                        break; ;
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
            return ToString(true);
        }

        #endregion

    }
}
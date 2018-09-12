namespace GraphExpression
{
    public class PathItem<T>
    {
        public int Level { get; private set; }
        public Edge<T> Edge { get; private set; }
        internal object Iteration { get; private set; }

        internal PathItem(object Iteration, Edge<T> edge, int level)
        {
            this.Edge = edge;
            this.Level = level;
            this.Iteration = Iteration;
        }

        public override string ToString()
        {
            return "[" + this.Edge.ToString() + "]";
        }
    }
}
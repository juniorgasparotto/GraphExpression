namespace GraphExpression
{
    public class Edge<T>
    {
        private bool? isLoop = false;

        public decimal Weight { get; set; }
        public Vertex<T> Source { get; internal set; }
        public Vertex<T> Target { get; internal set; }

        // Laço em português
        public bool IsLoop
        {
            get
            {
                if (isLoop != null)
                    isLoop = this.Source?.AreEquals(this.Target) == true;

                return isLoop.Value;
            }
        }

        internal Edge(Vertex<T> source = null, Vertex<T> target = null, decimal weight = 0)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        // Anti-paralelo em português
        public bool IsAntiparallel(Edge<T> compare)
        {
            if (this.Source?.AreEquals(compare.Target) == true && this.Target?.AreEquals(compare.Source) == true)
                return true;
            return false;
        }

        public bool AreEquals(Edge<T> obj)
        {
            if (ReferenceEquals(obj, null))
                return false;

            return (this.Source == obj.Source && this.Target == obj.Target);
        }

        #region Overrides

        public override string ToString()
        {
            return (Source == null ? "" : Source.ToString()) + ", " + (Target == null ? "" : Target.ToString());
        }

        #endregion
    }
}
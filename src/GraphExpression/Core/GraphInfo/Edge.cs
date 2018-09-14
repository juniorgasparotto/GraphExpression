namespace GraphExpression
{
    public class Edge<T>
    {
        private bool? isLoop = false;

        public decimal Weight { get; set; }
        public EntityItem<T> Source { get; internal set; }
        public EntityItem<T> Target { get; internal set; }

        // Laço em português
        public bool IsLoop
        {
            get
            {
                if (isLoop != null)
                    isLoop = this.Source?.AreEntityEquals(this.Target) == true;

                return isLoop.Value;
            }
        }

        public Edge(EntityItem<T> source = null, EntityItem<T> target = null, decimal weight = 0)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        // Anti-paralelo em português
        public bool IsAntiparallel(Edge<T> compare)
        {
            if (this.Source?.AreEntityEquals(compare.Target) == true && this.Target?.AreEntityEquals(compare.Source) == true)
                return true;
            return false;
        }

        //public bool AreEquals(Edge<T> obj)
        //{
        //    if (ReferenceEquals(obj, null))
        //        return false;

        //    return (this.Source.AreEntityEquals(obj.Source) && this.Target.AreEntityEquals(obj.Target));
        //}

        #region Overrides

        public override string ToString()
        {
            return (Source == null ? "" : Source.ToString()) + ", " + (Target == null ? "" : Target.ToString());
        }

        #endregion
    }
}
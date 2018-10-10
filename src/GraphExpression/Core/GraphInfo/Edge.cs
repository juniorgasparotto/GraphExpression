namespace GraphExpression
{
    /// <summary>
    /// Representing relations or connections between two entities
    /// </summary>
    /// <typeparam name="T">Type of real entity</typeparam>
    public class Edge<T>
    {
        private bool? isLoop = false;

        /// <summary>
        /// Stores the weight of each entity
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// Parent entity of Target
        /// </summary>
        public EntityItem<T> Source { get; internal set; }

        /// <summary>
        /// Entity child of Source
        /// </summary>
        public EntityItem<T> Target { get; internal set; }

        /// <summary>
        /// Checks if the entity is its parent, if it is, then it is looping.
        /// </summary>
        public bool IsLoop
        {
            get
            {
                if (isLoop != null)
                    isLoop = this.Source?.AreEntityEquals(this.Target) == true;

                return isLoop.Value;
            }
        }

        /// <summary>
        /// Creates a new edge
        /// </summary>
        /// <param name="source">Entity source (Parent)</param>
        /// <param name="target">Entity target (Child)</param>
        /// <param name="weight">Determines the weight of this edge</param>
        public Edge(EntityItem<T> source = null, EntityItem<T> target = null, decimal weight = 0)
        {
            this.Source = source;
            this.Target = target;
            this.Weight = weight;
        }

        /// <summary>
        /// Check if two edge are IsAntiparallel:
        ///  A + B
        ///  B + A
        /// </summary>
        /// <param name="compare">Edge to compare</param>
        /// <returns>Return TRUE if is antiparallel</returns>
        public bool IsAntiparallel(Edge<T> compare)
        {
            if (this.Source?.AreEntityEquals(compare.Target) == true && this.Target?.AreEntityEquals(compare.Source) == true)
                return true;
            return false;
        }

        #region Overrides

        /// <summary>
        /// Edge to string
        /// </summary>
        /// <returns>Edge to string</returns>
        public override string ToString()
        {
            return (Source == null ? "" : Source.ToString()) + ", " + (Target == null ? "" : Target.ToString());
        }

        #endregion
    }
}
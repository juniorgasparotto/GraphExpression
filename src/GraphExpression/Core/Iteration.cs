using System.Collections.Generic;

namespace GraphExpression
{
    /// <summary>
    /// Represents an iteration within a recursion simulated by WHILE
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Iteration<T>
    {
        /// <summary>
        /// Represents the enumerator of this iteration
        /// </summary>
        public IEnumerator<EntityItem<T>> Enumerator { get; set; }

        /// <summary>
        /// Root entity of this iteration for debugging.
        /// </summary>
        public EntityItem<T> EntityRootOfTheIterationForDebug { get; set; }

        /// <summary>
        /// Level of iteration
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Iteration parent
        /// </summary>
        public Iteration<T> IterationParent { get; set; }

        /// <summary>
        /// Index at level of the iteration
        /// </summary>
        public int IndexAtLevel { get; set; }

        /// <summary>
        /// Entity to string for debugging.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (EntityRootOfTheIterationForDebug == null)
                return "";

            return EntityRootOfTheIterationForDebug.ToString();
        }
    }
}
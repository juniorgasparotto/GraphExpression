using System.Collections.Generic;

namespace GraphExpression
{
    public class Iteration<T>
    {
        public IEnumerator<EntityItem<T>> Enumerator { get; set; }
        public EntityItem<T> EntityRootOfTheIterationForDebug { get; set; }
        public int Level { get; set; }
        public Iteration<T> IterationParent { get; set; }
        public int IndexAtLevel { get; set; }

        public override string ToString()
        {
            if (EntityRootOfTheIterationForDebug == null)
                return "";

            return EntityRootOfTheIterationForDebug.ToString();
        }
    }
}
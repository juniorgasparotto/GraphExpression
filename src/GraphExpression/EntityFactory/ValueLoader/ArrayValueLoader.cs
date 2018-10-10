using GraphExpression.Utils;
using System;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    /// Class default used to creating an array
    /// </summary>
    public class ArrayValueLoader : IValueLoader
    {
        /// <summary>
        /// Verify if can load instance value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return TRUE if can load</returns>
        public bool CanLoad(Entity item)
        {
            return item.Type.IsArray;
        }

        /// <summary>
        /// Return value
        /// </summary>
        /// <param name="item">The item that contains the information to get value</param>
        /// <returns>Return object instance</returns>
        public object GetValue(Entity item)
        {
            var entityType = item.Type;
            var lastItem = item.Children.LastOrDefault();
            object[] indexes;

            if (lastItem != null)
            {
                var longs = ReflectionUtils.GetArrayIndexesByString(lastItem.Name);
                indexes = new object[longs.Length];
                // 1) array needs a INT index, long throw exceptions
                // 2) Need add + 1 because the seralize contain the position and
                //    the array constructors needs the lenght from each dimentions
                //    [0] -> pos = 0 | and | lenght = 1
                for (var i = 0; i < longs.Length; i++)
                    indexes[i] = (int)longs[i] + 1;
            }
            else
            {
                indexes = new object[] { 0 };
            }

            return Activator.CreateInstance(entityType, indexes);
        }
    }
}
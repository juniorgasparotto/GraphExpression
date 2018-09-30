using GraphExpression.Utils;
using System;
using System.Linq;

namespace GraphExpression.Serialization
{
    public class ArrayGetEntity : IGetEntity
    {
        public bool CanGetEntity(ItemDeserializer item)
        {
            return item.EntityType.IsArray;
        }

        public object GetEntity(ItemDeserializer item)
        {
            var entityType = item.EntityType;
            var lastItem = item.Children.LastOrDefault();
            object[] indexes;

            if (lastItem != null)
            {
                var longs = ReflectionUtils.GetArrayIndexesByString(lastItem.MemberName);
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
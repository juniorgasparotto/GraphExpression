using System.ComponentModel;

namespace GraphExpression.Serialization
{
    public class PrimitivesGetEntity : IGetEntity
    {
        public bool CanGetEntity(ItemDeserializer item)
        {
            return item.IsPrimitive;
        }

        public object GetEntity(ItemDeserializer item)
        {
            if (item.ValueRaw == null)
                return null;

            return TypeDescriptor.GetConverter(item.EntityType).ConvertFromInvariantString(item.ValueRaw);
        }
    }
}
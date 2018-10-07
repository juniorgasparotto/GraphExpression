using System.ComponentModel;

namespace GraphExpression
{
    public class PrimitivesGetValue : IGetValue
    {
        public bool CanGetValue(Entity item)
        {
            return item.IsPrimitive;
        }

        public object GetValue(Entity item)
        {
            if (item.ValueRaw == null)
                return null;

            return TypeDescriptor.GetConverter(item.Type).ConvertFromInvariantString(item.ValueRaw);
        }
    }
}
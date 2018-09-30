using GraphExpression.Utils;
using System;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;

namespace GraphExpression.Serialization
{
    public class ComplexEntityGetEntity : IGetEntity
    {
        public bool CanGetEntity(ItemDeserializer item)
        {
            return item.ComplexEntityId != null;
        }

        public object GetEntity(ItemDeserializer item)
        {
            var entityType = item.EntityType;

            if (ReflectionUtils.IsAnonymousType(item.EntityType))
                return new ExpandoObject();

            if (entityType.GetConstructors().Where(f => f.GetParameters().Length == 0).Any())
                return Activator.CreateInstance(entityType);
            else if (!entityType.IsInterface && !entityType.IsAbstract)
                return FormatterServices.GetUninitializedObject(entityType);

            return null;
        }
    }
}
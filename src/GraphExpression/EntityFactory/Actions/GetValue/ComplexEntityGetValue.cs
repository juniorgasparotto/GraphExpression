using GraphExpression.Utils;
using System;
using System.Dynamic;
using System.Linq;
using System.Runtime.Serialization;

namespace GraphExpression
{
    public class ComplexEntityGetValue : IGetValue
    {
        public bool CanGetValue(Entity item)
        {
            return item.ComplexEntityId != null;
        }

        public object GetValue(Entity item)
        {
            var entityType = item.Type;

            if (ReflectionUtils.IsAnonymousType(item.Type))
                return new ExpandoObject();

            if (entityType.GetConstructors().Where(f => f.GetParameters().Length == 0).Any())
                return Activator.CreateInstance(entityType);
            else if (!entityType.IsInterface && !entityType.IsAbstract)
                return FormatterServices.GetUninitializedObject(entityType);

            return null;
        }
    }
}
using System;
using System.Reflection;

namespace GraphExpression.Serialization
{
    public class MemberInfoSetChild : ISetChild
    {
        public bool CanSetChild(ItemDeserializer item, ItemDeserializer child)
        {
            return child.MemberInfo != null;
        }

        public void SetChild(ItemDeserializer item, ItemDeserializer child)
        {
            var factory = item.Factory;
            var entity = item.Entity;
            var entityType = item.EntityType;
            var value = child.Entity;

            if (entity == null && value != null)
            {
                var error = $"An instance of type '{entityType.FullName}' contains value, but not created. Make sure it is an interface or an abstract class, if so, set up a corresponding concrete class in the '{nameof(ComplexEntityFactoryDeserializer)}.{nameof(ComplexEntityFactoryDeserializer.MapTypes)}' configuration.";
                if (!factory.IgnoreErrors)
                    throw new Exception(error);
                else
                    factory.AddError(error);
                return;
            }

            if (child.MemberInfo is PropertyInfo prop)
                prop.SetValue(entity, value);
            else if (child.MemberInfo is FieldInfo field)
                field.SetValue(entity, value);
        }
    }
}
using System;
using System.Reflection;

namespace GraphExpression
{
    public class MemberInfoSetChild : ISetChild
    {
        public bool CanSetChild(Entity item, Entity child)
        {
            return child.MemberInfo != null;
        }

        public void SetChild(Entity item, Entity child)
        {
            var factory = item.Factory;
            var entity = item.Value;
            var entityType = item.Type;
            var value = child.Value;

            if (entity == null && value != null)
            {
                var error = $"An instance of type '{entityType.FullName}' contains value, but not created. Make sure it is an interface or an abstract class, if so, set up a corresponding concrete class in the '{nameof(ComplexEntityFactory)}.{nameof(ComplexEntityFactory.MapTypes)}' configuration.";
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
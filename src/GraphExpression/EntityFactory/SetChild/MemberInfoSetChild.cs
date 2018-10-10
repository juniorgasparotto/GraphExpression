using System;
using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class default to set value in MemberInfo
    /// </summary>
    public class MemberInfoSetChild : ISetChild
    {
        /// <summary>
        /// Verify if can set child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
        /// <returns>Return TRUE if can set</returns>
        public bool CanSet(Entity item, Entity child)
        {
            return child.MemberInfo != null;
        }

        /// <summary>
        /// Set a child in parent
        /// </summary>
        /// <param name="item">Parent item</param>
        /// <param name="child">Child item</param>
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
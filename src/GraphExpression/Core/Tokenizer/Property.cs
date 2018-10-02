using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GraphExpression.Utils;
using Mono.Reflection;

namespace GraphExpression.Serialization
{
    public class Member : TokenComplex
    {
        public MemberInfo MemberInfo => GetMemberInfo();

        public MemberInfo GetMemberInfo()
        {
            var memberInfo = Parent.EntityType
                       .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                       .Where(f => f.Name == Key)
                       .FirstOrDefault();

            memberInfo = memberInfo ?? throw new Exception($"Member not exists {item.MemberName} in {item.Parent.EntityType.FullName}");

            if (memberInfo is PropertyInfo prop)
            {
                if (prop.GetSetMethod(true) != null)
                    memberInfo = prop;
                else
                    memberInfo = prop.GetBackingField();
            }

            return memberInfo;
        }

        public void SetValue()
        {
            //var factory = item.EntityFactory;
            var entity = Parent.Entity;
            var entityType = Parent.EntityType;
            var value = Entity;

            if (entity == null && value != null)
            {
                //var error = $"An instance of type '{entityType.FullName}' contains value, but not created. Make sure it is an interface or an abstract class, if so, set up a corresponding concrete class in the '{nameof(ComplexEntityFactoryDeserializer)}.{nameof(ComplexEntityFactoryDeserializer.MapTypes)}' configuration.";
                //if (!factory.IgnoreErrors)
                //    throw new Exception(error);
                //else
                //    factory.AddError(error);
                //return;
            }

            if (child.MemberInfo is PropertyInfo prop)
                prop.SetValue(entity, value);
            else if (child.MemberInfo is FieldInfo field)
                field.SetValue(entity, value);
            return a;
        }

    }
}

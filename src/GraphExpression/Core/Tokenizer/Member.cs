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
    public class Member : Token
    {
        private MemberInfo memberInfo;
        public MemberInfo MemberInfo
        {
            get
            {
                if (memberInfo == null)
                {
                    memberInfo = Parent.Type
                               .GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                               .Where(f => f.Name == Name)
                               .FirstOrDefault();

                    memberInfo = memberInfo ?? throw new Exception($"Member not exists {Name} in {Parent.Type.FullName}");

                    if (memberInfo is PropertyInfo prop)
                    {
                        if (prop.GetSetMethod(true) != null)
                            memberInfo = prop;
                        else
                            memberInfo = prop.GetBackingField();
                    }
                }

                return memberInfo;
            }
        }

        public override Type Type
        {
            get
            {
                if (base.Type == null)
                    base.Type = ReflectionUtils.GetMemberType(MemberInfo);
                return base.Type;
            }
            protected set => base.Type = value;
        }

        public Member(string name, object value = null) : base(name, value, null)
        {
               
        }

        public override void AutoAddOnParent()
        {
            //var factory = item.EntityFactory;
            var parentValue = Parent.Value;
            var parentType = Parent.Type;

            if (parentType == null && Value != null)
            {
                //var error = $"An instance of type '{entityType.FullName}' contains value, but not created. Make sure it is an interface or an abstract class, if so, set up a corresponding concrete class in the '{nameof(ComplexEntityFactoryDeserializer)}.{nameof(ComplexEntityFactoryDeserializer.MapTypes)}' configuration.";
                //if (!factory.IgnoreErrors)
                //    throw new Exception(error);
                //else
                //    factory.AddError(error);
                //return;
            }

            if (this.MemberInfo is PropertyInfo prop)
                prop.SetValue(parentValue, Value);
            else if (this.MemberInfo is FieldInfo field)
                field.SetValue(parentValue, Value);
        }
    }
}

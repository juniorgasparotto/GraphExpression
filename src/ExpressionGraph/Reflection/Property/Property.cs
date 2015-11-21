using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ExpressionGraph.Reflection
{
    public class Property
    {
        public List<MethodValue> Values { get; private set; }
        public Type ParentType { get; private set; }
        public PropertyInfo PropertyInfo { get; private set; }

        public string Name { get; private set; }
        public bool IsGetPublic { get; set; }
        public bool IsGetPrivate { get; set; }
        public bool IsGetInternal { get; set; }
        public bool IsGetProtected { get; set; }
        public bool IsSetPublic { get; set; }
        public bool IsSetPrivate { get; set; }
        public bool IsSetInternal { get; set; }
        public bool IsSetProtected { get; set; }
        public bool IsOverride { get; set; }
        public bool IsStatic { get; set; }
        public bool IsExplicitlyImpl { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsNew { get; set; }
        public bool IsSealed { get; set; }

        public Property(string name, Type parentType, PropertyInfo propertyInfo, List<MethodValue> values)
        {
            this.Values = values;
            this.PropertyInfo = propertyInfo;
            this.Name = name;
            this.ParentType = parentType;
        }

        public override string ToString()
        {
            if (Values == null || Values.Count == 0)
                return Name + " = null";

            return Name + " = [" + Values.FirstOrDefault().Value.ToString() + "]";
        }
    }
}

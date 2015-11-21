using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ExpressionGraph.Reflection
{
    public class Method
    {
        public List<MethodValue> Values { get; private set; }
        public Type ParentType { get; private set; }
        public MethodInfo MethodInfo { get; private set; }

        public string Name { get; private set; }
        public bool IsPublic { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsInternal { get; set; }
        public bool IsProtected { get; set; }
        public bool IsOverride { get; set; }
        public bool IsStatic { get; set; }
        public bool IsExplicitlyImpl { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsAbstract { get; set; }
        public bool IsNew { get; set; }
        public bool IsSealed { get; set; }
        public bool IsExtern { get; set; }

        public Method(string name, Type parentType, MethodInfo methodInfo, List<MethodValue> values)
        {
            this.Values = values;
            this.MethodInfo = methodInfo;
            this.Name = name;
            this.ParentType = parentType;
        }

        public override string ToString()
        {
            if (Values == null || Values.Count == 0)
                return MethodInfo.ToString() + " = null";

            return MethodInfo.ToString() + " = [" + Values.FirstOrDefault().Value.ToString() + "]";
        }
    }
}

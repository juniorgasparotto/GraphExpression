using System;
using System.Linq;
using System.Collections.Generic;

namespace ExpressionGraph.Reflection
{
    public class InstanceType
    {
        public Type Type { get; private set; }
        public List<Property> Properties { get; private set; }
        public List<Method> Methods { get; private set; }
        public List<Field> Fields { get; private set; }

        public InstanceType(Type type)
        {
            this.Type = type;
            this.Properties = new List<Property>();
            this.Methods = new List<Method>();
            this.Fields = new List<Field>();
        }

        public override string ToString()
        {
            return Type.Name;
        }
    }
}

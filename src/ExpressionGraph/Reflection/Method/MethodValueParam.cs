using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class MethodValueParam
    {
        public string Name { get; private set; }
        public object Value { get; private set; }
        public ParameterInfo ParameterInfo { get; private set; }

        public MethodValueParam(string name, ParameterInfo parameterInfo, object value)
        {
            this.Name = name;
            this.ParameterInfo = parameterInfo;
            this.Value = value; 
        }

        public override string ToString()
        {
            if (Value != null)
                return Name + " = [" + Value.ToString() + "]";
            return Name + " = null";
        }
    }
}
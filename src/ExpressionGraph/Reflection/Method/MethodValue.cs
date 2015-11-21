using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class MethodValue
    {
        public MethodValueParam[] Parameters { get; private set; }
        public object Value { get; private set; }

        public MethodValue(object value, params MethodValueParam[] parameters)
        {
            this.Value = value;
            this.Parameters = parameters;
        }

        public override string ToString()
        {
            if (Value != null)
                return "[" + Value.ToString() + "]";
            return "null";
        }
    }
}
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
            return GetParameterToString(true, false, "[]");
        }

        public string GetParameterToString(bool showName = false, bool showType = false, string encloseValueChars = "\"\"")
        {
            var toString = default(string);
            var hasEnclose = !string.IsNullOrWhiteSpace(encloseValueChars) && encloseValueChars.Length == 2;
            var open = hasEnclose ? encloseValueChars[0].ToString() : "";
            var close = hasEnclose ? encloseValueChars[1].ToString() : "";

            toString += showType ? ReflectionHelper.CSharpName(ParameterInfo.ParameterType) + " " : "";
            toString += showName ? this.Name + "=" : "";
            toString += this.Value == null ? "null" : open + this.Value.ToString() + close;
            return toString;
        }
    }
}
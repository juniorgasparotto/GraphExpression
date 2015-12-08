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
            return GetValueToString("");
        }

        public string GetValueToString(string encloseChars = "\"\"")
        {
            var toString = default(string);
            var hasEnclose = !string.IsNullOrWhiteSpace(encloseChars) && encloseChars.Length == 2;
            var open = hasEnclose ? encloseChars[0].ToString() : "";
            var close = hasEnclose ? encloseChars[1].ToString() : "";
            toString = this.Value == null ? "null" : open + this.Value.ToString() + close;            
            return toString;
        }

        public string GetParametersToString(bool showName = false, bool showType = false, string encloseChars = "[]", string encloseValuesChars = "\"\"")
        {
            var hasEnclose = !string.IsNullOrWhiteSpace(encloseChars) && encloseChars.Length == 2;
            var open = hasEnclose ? encloseChars[0].ToString() : "";
            var close = hasEnclose ? encloseChars[1].ToString() : "";
            var toString = default(string);

            if (this.Parameters != null)
            {
                foreach (var param in this.Parameters)
                {
                    toString += toString == null ? "" : ", ";
                    toString += param.GetParameterToString(showName, showType, encloseValuesChars);
                }

                toString = open + toString + close;
            }

            return toString;
        }
    }
}
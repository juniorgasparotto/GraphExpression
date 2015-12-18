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

        public string NameWithParameters
        { 
            get 
            {
                return this.Name + this.GetParametersDefinition(true);
            } 
        }

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
        public bool IsIndexedProperty { get; set; }

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
                return NameWithParameters + " = null";

            return NameWithParameters + " = [" + Values.FirstOrDefault().Value.ToString() + "]";
        }

        public string GetParametersDefinition(bool showType = true, string encloseChars = "[]")
        {
            var parametersDefinition = default(string);
            var parameters = this.PropertyInfo.GetIndexParameters();
            var hasEnclose = !string.IsNullOrWhiteSpace(encloseChars) && encloseChars.Length == 2;
            var open = hasEnclose ? encloseChars[0].ToString() : "";
            var close = hasEnclose ? encloseChars[1].ToString() : "";

            if (parameters.Length > 0)
            {
                // Can be used for prevent duplicate indexer (Item) "this[int32 index]" or "this[int32 index, int32 index2]"
                foreach (var param in parameters)
                {
                    parametersDefinition += parametersDefinition == null ? "" : ", ";
                    parametersDefinition += showType ? ReflectionUtils.CSharpName(param.ParameterType) + " " : "";
                    parametersDefinition += param.Name;
                }

                parametersDefinition = open + parametersDefinition + close;
            }

            return parametersDefinition;
        }
    }
}

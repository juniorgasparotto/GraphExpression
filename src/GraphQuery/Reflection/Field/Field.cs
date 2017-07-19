using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace GraphQuery.Reflection
{
    public class Field
    {
        public object Value { get; private set; }
        public Type ParentType { get; private set; }
        public FieldInfo FieldInfo { get; private set; }

        public string Name { get; private set; }
        public bool IsPublic { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsInternal { get; set; }
        public bool IsProtected { get; set; }
        public bool IsStatic { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsConstant { get; set; }

        public Field(string name, Type parentType, FieldInfo fieldInfo, object value)
        {
            this.Value = value;
            this.FieldInfo = fieldInfo;
            this.Name = name;
            this.ParentType = parentType;
        }

        public override string ToString()
        {
            if (this.Value == null)
                return FieldInfo.Name + " = null";

            return FieldInfo.Name + " = [" + Value.ToString() + "]";
        }

    }
}

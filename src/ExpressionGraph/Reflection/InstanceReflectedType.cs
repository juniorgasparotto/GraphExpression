using System;
using System.Linq;
using System.Collections.Generic;

namespace ExpressionGraph.Reflection
{
    public class InstanceReflectedType
    {
        private List<Property> _properties;
        private List<Method> _methods;
        private List<Field> _fields;

        public Type Type { get; private set; }
        public IEnumerable<Field> Fields { get { return _fields; } }
        public IEnumerable<Property> Properties { get { return _properties; } }
        public IEnumerable<Method> Methods { get { return _methods; } }

        public InstanceReflectedType(Type type)
        {
            this.Type = type;
            this._fields = new List<Field>();
            this._properties = new List<Property>();
            this._methods = new List<Method>();
        }

        internal void Add(Field member)
        {
            this._fields.Add(member);
        }

        internal void Add(Property member)
        {
            this._properties.Add(member);
        }

        internal void Add(Method member)
        {
            this._methods.Add(member);
        }

        public override string ToString()
        {
            return Type.Name;
        }
    }
}

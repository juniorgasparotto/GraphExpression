using System;
using System.Linq;
using System.Collections.Generic;

namespace GraphQuery.Reflection
{
    public class ReflectedType
    {
        private List<Property> _properties;
        private List<Method> _methods;
        private List<Field> _fields;

        public Type Type { get; private set; }
        public IEnumerable<MethodValue> EnumeratorValues { get; private set; }
        public IEnumerable<Field> Fields { get { return _fields; } }
        public IEnumerable<Property> Properties { get { return _properties; } }
        public IEnumerable<Method> Methods { get { return _methods; } }

        public ReflectedType(Type type)
        {
            this.Type = type;
            this._fields = new List<Field>();
            this._properties = new List<Property>();
            this._methods = new List<Method>();
            this.EnumeratorValues = Enumerable.Empty<MethodValue>();
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

        internal void Add(IEnumerable<MethodValue> values)
        {
            this.EnumeratorValues = values;
        }

        public override string ToString()
        {
            return Type.Name;
        }
    }
}

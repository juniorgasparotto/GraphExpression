using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class Instance2
    {
        public string Name { get; private set; }
        public object Object { get; private set; }
        public Type ObjectType { get; private set; }
        public string ContainerName { get; private set; }
        public List<InstanceType> InstanceTypes { get; private set; }

        public IEnumerable<Property> GetAllProperties()
        {
            return this.InstanceTypes.SelectMany(f => f.Properties);
        }

        public IEnumerable<Method> GetAllMethods()
        {
            return this.InstanceTypes.SelectMany(f => f.Methods);
        }

        public IEnumerable<Field> GetAllFields()
        {
            return this.InstanceTypes.SelectMany(f => f.Fields);
        }

        #region Overrides

        public override string ToString()
        {
            if (this.Object != null)
                return this.Object.ToString();

            return null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || this.GetType() != obj.GetType())
                return false;

            var converted = obj as UnitReflaction;
            return (this.Object.Equals(converted.Object));
        }

        public override int GetHashCode()
        {
            if (this.Object != null)
                return this.Object.GetHashCode();

            return 0;
        }

        #endregion

        #region Operators

        public static bool operator ==(UnitReflaction a, UnitReflaction b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(UnitReflaction a, UnitReflaction b)
        {
            return !Equals(a, b);
        }

        #endregion
    }
}
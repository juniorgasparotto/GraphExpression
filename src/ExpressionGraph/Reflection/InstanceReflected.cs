using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class ReflectInstance
    {
        private List<InstanceReflectedType> _reflectedTypes;

        public string Name { get; private set; }
        public object Object { get; private set; }
        public Type ObjectType { get; private set; }
        public string ContainerName { get; private set; }
        public IEnumerable<InstanceReflectedType> ReflectedTypes { get { return _reflectedTypes; } }

        public ReflectInstance(object obj, string name = null, string containerName = null)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            this.ObjectType = obj.GetType();
            this.Name = name ?? this.ObjectType.Name + "_" + obj.GetHashCode();
            this.ContainerName = containerName;
            this.Object = obj;
            this._reflectedTypes = new List<InstanceReflectedType>();
        }

        public void Add(InstanceReflectedType instanceType)
        {
            this._reflectedTypes.Add(instanceType);
        }

        public IEnumerable<Property> GetAllProperties()
        {
            return this.ReflectedTypes.SelectMany(f => f.Properties);
        }

        public IEnumerable<Method> GetAllMethods()
        {
            return this.ReflectedTypes.SelectMany(f => f.Methods);
        }

        public IEnumerable<Field> GetAllFields()
        {
            return this.ReflectedTypes.SelectMany(f => f.Fields);
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

            var converted = obj as ReflectInstance;
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

        public static bool operator ==(ReflectInstance a, ReflectInstance b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(ReflectInstance a, ReflectInstance b)
        {
            return !Equals(a, b);
        }

        #endregion
    }
}
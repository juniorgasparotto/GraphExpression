using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class ReflectedInstance
    {
        private List<ReflectedType> _reflectedTypes;

        public string Name { get; private set; }
        public object Object { get; private set; }
        public Type ObjectType { get; private set; }
        public string ContainerName { get; private set; }
        public IEnumerable<ReflectedType> ReflectedTypes { get { return _reflectedTypes; } }

        public ReflectedInstance(object obj, string name = null, string containerName = null)
        {
            if (obj != null)
            { 
                this.ObjectType = obj.GetType();
                this.Name = name ?? this.ObjectType.Name + "_" + obj.GetHashCode();
            }

            this.ContainerName = containerName;
            this.Object = obj;
            this._reflectedTypes = new List<ReflectedType>();
        }

        public void Add(ReflectedType instanceType)
        {
            this._reflectedTypes.Add(instanceType);
        }

        public IEnumerable<Property> GetAllProperties()
        {
            return this._reflectedTypes.SelectMany(f => f.Properties);
        }

        public IEnumerable<Method> GetAllMethods()
        {
            return this._reflectedTypes.SelectMany(f => f.Methods);
        }

        public IEnumerable<Field> GetAllFields()
        {
            return this._reflectedTypes.SelectMany(f => f.Fields);
        }

        public IEnumerable<MethodValue> GetAllEnumeratorValues()
        {
            return this._reflectedTypes.SelectMany(f => f.EnumeratorValues);
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

            var converted = obj as ReflectedInstance;
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

        public static bool operator ==(ReflectedInstance a, ReflectedInstance b)
        {
            return Equals(a, b);
        }

        public static bool operator !=(ReflectedInstance a, ReflectedInstance b)
        {
            return !Equals(a, b);
        }

        #endregion
    }
}
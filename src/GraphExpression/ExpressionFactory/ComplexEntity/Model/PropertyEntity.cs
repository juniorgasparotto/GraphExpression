using System.Reflection;

namespace GraphExpression
{
    /// <summary>
    /// Class model representing a property member
    /// </summary>
    public class PropertyEntity : ComplexEntity
    {
        /// <summary>
        /// Property info
        /// </summary>
        public PropertyInfo Property { get; private set; }

        /// <summary>
        /// Check if property is static
        /// </summary>
        public bool IsStatic => (GetMethod(true) ?? GetMethod(false)).IsStatic;

        /// <summary>
        /// Check if property is virtual
        /// </summary>
        public bool IsVirtual => (GetMethod(true) ?? GetMethod(false)).IsVirtual;

        /// <summary>
        /// Check if property is abstract
        /// </summary>
        public bool IsAbstract => (GetMethod(true) ?? GetMethod(false)).IsAbstract;

        /// <summary>
        /// Check if property is new
        /// </summary>
        public bool IsNew => (GetMethod(true) ?? GetMethod(false)).IsHideBySig;

        /// <summary>
        /// Check if property is sealed
        /// </summary>
        public bool IsSealed => (GetMethod(true) ?? GetMethod(false)).IsFinal;

        /// <summary>
        /// Check if property get is public
        /// </summary>
        public bool? IsGetPublic => GetMethod(true)?.IsPublic;

        /// <summary>
        /// Check if property get is private
        /// </summary>
        public bool? IsGetPrivate => GetMethod(true)?.IsPrivate;

        /// <summary>
        /// Check if property get is protected
        /// </summary>
        public bool? IsGetProtected => GetMethod(true)?.IsFamily;

        /// <summary>
        /// Check if property get is internal
        /// </summary>
        public bool? IsGetInternal => GetMethod(true)?.IsAssembly;

        /// <summary>
        /// Check if property set is public
        /// </summary>
        public bool? IsSetPublic => GetMethod(false)?.IsPublic;

        /// <summary>
        /// Check if property set is private
        /// </summary>
        public bool? IsSetPrivate => GetMethod(false)?.IsPrivate;

        /// <summary>
        /// Check if property set is protected
        /// </summary>
        public bool? IsSetProtected => GetMethod(false)?.IsFamily;

        /// <summary>
        /// Check if property set is internal
        /// </summary>
        public bool? IsSetInternal => GetMethod(false)?.IsAssembly;

        /// <summary>
        /// Check if property is explicitly implemented
        /// </summary>
        public bool IsExplicitlyImpl => Property.Name.Contains(".");

        /// <summary>
        /// Check if property is indexed
        /// </summary>
        public bool IsIndexedProperty => Property.GetIndexParameters().Length > 0;

        /// <summary>
        /// Create a member property entity
        /// </summary>
        /// <param name="expression">Expression container</param>
        /// <param name="parent">Parent instance (PropertyInfo container)<</param>
        /// <param name="property">Property info</param>
        public PropertyEntity(Expression<object> expression, object parent, PropertyInfo property)
            : base(expression)
        {
            this.Property = property;
            if (parent != null)
                this.Entity = property.GetValue(parent);
        }

        public bool IsOverride(bool checkGetMethod = true)
        {
            var method = GetMethod(checkGetMethod);
            if (method.GetBaseDefinition() != method)
                return true;
            return false;
        }

        private MethodInfo GetMethod(bool getGetMethod)
        {
            return getGetMethod ? Property.GetMethod : Property.SetMethod;
        }
    }
}

using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    public class ComplexBuilder
    {
        public List<IReader> Readers { get;set; }
        public Func<object, IEnumerable<PropertyInfo>> GetProperties { get;set; }
        public Func<object, IEnumerable<FieldInfo>> GetFields { get;set; }

        public ComplexBuilder()
        {
            Readers = new List<IReader>();
            Readers.Add(new ArrayReader());
            Readers.Add(new DynamicReader());
            Readers.Add(new DictionaryReader());
            Readers.Add(new ListReader());

            GetProperties = (entity) => entity.GetType().GetProperties();
            GetFields = (entity) => entity.GetType().GetFields();
        }

        public bool CanContinue(Type type)
        {
            var typeName = type.Namespace ?? "";
            if (typeName == "System" || typeName.StartsWith("System.") ||
                typeName == "Microsoft" || typeName.StartsWith("Microsoft."))
                return true;
            return false;
        }
    }
}

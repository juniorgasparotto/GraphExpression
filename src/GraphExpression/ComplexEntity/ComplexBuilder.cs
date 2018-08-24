using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    public class ComplexBuilder
    {
        public List<IComplexItemReader> InstanceReaders { get;set; }
        public List<IComplexItemReader> MemberReaders { get;set; }
        public Func<object, IEnumerable<PropertyInfo>> GetProperties { get;set; }
        public Func<object, IEnumerable<FieldInfo>> GetFields { get;set; }
        public Func<object, bool> CanReadMembers { get; set; }

        public ComplexBuilder()
        {
            // is recommended this readers order:
            // 1) Array
            // 2) ExpandoObject (Dynamic): is IDictionary too
            // 3) IDictionary
            // 4) IList
            InstanceReaders = new List<IComplexItemReader>();
            InstanceReaders.Add(new ArrayReader());
            InstanceReaders.Add(new DynamicReader());
            InstanceReaders.Add(new DictionaryReader());
            InstanceReaders.Add(new ListReader());

            MemberReaders = new List<IComplexItemReader>();
            MemberReaders.Add(new PropertyReader());
            MemberReaders.Add(new FieldReader());

            GetProperties = (entity) => entity.GetType().GetProperties();
            GetFields = (entity) => entity.GetType().GetFields();
            CanReadMembers = (entity) => !ReflectionUtils.IsSystemType(entity.GetType());
        }
    }
}

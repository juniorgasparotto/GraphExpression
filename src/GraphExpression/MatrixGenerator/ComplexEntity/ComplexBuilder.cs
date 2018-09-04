using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    public class ComplexBuilder
    {
        public List<IEntityReader> Readers { get; private set; }
        public List<IMemberReader> MemberReaders { get; private set; }
        public Func<object, IEnumerable<PropertyInfo>> GetProperties { get; set; }
        public Func<object, IEnumerable<FieldInfo>> GetFields { get; set; }

        public ComplexBuilder(bool addDefaultConfig = true)
        {
            if (addDefaultConfig)
            {
                // This list should be used using "LastOrDefault", to give the user the chance to 
                // replace one "reader" with another one using "Reader.Add()"
                // that is, the last "reader" is the most important.
                // Important:
                // 1) Array inherit IList: Must be sorted below IList
                // 2) ExpandoObject inherit IDictionary: Must be sorted below IDictionary
                Readers = new List<IEntityReader>
                {
                    new DefaultReader(),
                    new CollectionReader(),
                    new ArrayReader(),
                    new DictionaryReader(),
                    new DynamicReader(),
                };

                MemberReaders = new List<IMemberReader>
                {
                    new PropertyReader(),
                    new FieldReader()
                };

                GetProperties = (entity) => entity.GetType().GetProperties();
                GetFields = (entity) => entity.GetType().GetFields();
            }
        }
    }
}

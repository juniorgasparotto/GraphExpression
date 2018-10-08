using GraphExpression.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GraphExpression
{
    public class ComplexExpressionFactory
    {
        public List<IEntityReader> Readers { get; private set; }
        public List<IMemberReader> MemberReaders { get; private set; }
        public Func<object, IEnumerable<PropertyInfo>> GetProperties { get; set; }
        public Func<object, IEnumerable<FieldInfo>> GetFields { get; set; }

        public ComplexExpressionFactory(bool addDefaultConfig = true)
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

        public Expression<object> Build(object entityRoot, bool deep = false)
        {
            var expression = new Expression<object>(expr => new ComplexEntity(expr, entityRoot), (expr, e) => GetChildren(expr, e), deep);
            expression.DefaultSerializer = new ComplexEntityExpressionSerializer(expression);
            return expression;
        }

        private IEnumerable<EntityItem<object>> GetChildren(Expression<object> expression, EntityItem<object> parent)
        {
            var entityParent = parent?.Entity;

            if (entityParent == null)
                yield break;

            // Find the reader, the last "reader" is the most important
            var instanceReader = 
                Readers
                .Where(f => f.CanRead(this, entityParent))
                .LastOrDefault();

            if (instanceReader != null)
                foreach (var item in instanceReader.GetChildren(this, expression, entityParent))
                    yield return item;
        }
    }
}

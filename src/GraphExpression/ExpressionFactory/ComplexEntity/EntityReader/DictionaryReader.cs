using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace GraphExpression
{
    public class DictionaryReader : IEntityReader
    {
        public bool CanRead(ComplexExpressionBuilder builder, object entity)
        {
            return entity is IDictionary || entity is DictionaryEntry;
        }

        public IEnumerable<ComplexEntity> GetChildren(ComplexExpressionBuilder builder, Expression<object> expression, object entity)
        {
            if (entity is IDictionary dic)
            {
                var count = 0;
                foreach (DictionaryEntry entry in dic)
                    yield return new CollectionItemEntity(expression, count++, entry);

                // read members, it may happen to be an instance of the 
                // user that inherits from IDictionary, so you need to read the members.
                foreach (var memberReader in builder.MemberReaders)
                {
                    var items = memberReader.GetMembers(builder, expression, entity);
                    foreach (var item in items)
                    {
                        // Ignore property "Values|Keys" because the values already specify 
                        if (item is PropertyEntity property
                            && (property.Property.Name == "Values" || property.Property.Name == "Keys"))
                            continue;

                        yield return item;
                    }
                }
            }
            else if (entity is DictionaryEntry entry)
            {
                // Read properties: "Key" and "Value"
                foreach (var memberReader in builder.MemberReaders)
                {
                    var items = memberReader.GetMembers(builder, expression, entity);
                    foreach (var item in items)
                        yield return item;
                }
            }
        }
    }
}
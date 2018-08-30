using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace GraphExpression
{
    public class DictionaryReader : IEntityReader
    {
        public bool CanRead(ComplexBuilder builder, object entity)
        {
            return entity is System.Collections.IDictionary;
        }

        public IEnumerable<ComplexEntity> GetChildren(ComplexBuilder builder, Expression<object> expression, object entity)
        {
            var dic = (System.Collections.IDictionary)entity;
            foreach (var key in dic.Keys)
                yield return new DictionaryItemEntity(expression, key, dic[key]);

            // read members, it may happen to be an instance of the 
            // user that inherits from IDictionary, so you need to read the members.
            foreach (var memberReader in builder.MemberReaders)
            {
                if (memberReader.CanRead(builder, entity))
                {
                    var items = memberReader.GetMembers(builder, expression, entity);
                    foreach (var item in items)
                        yield return item;
                }
            }
        }
    }
}
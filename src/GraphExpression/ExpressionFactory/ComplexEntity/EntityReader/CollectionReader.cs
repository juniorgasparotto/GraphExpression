using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    public class CollectionReader : IEntityReader
    {
        public bool CanRead(ComplexExpressionFactory builder, object entity)
        {
            return entity is System.Collections.ICollection;
        }

        public IEnumerable<ComplexEntity> GetChildren(ComplexExpressionFactory builder, Expression<object> expression, object entity)
        {
            var list = (System.Collections.ICollection)entity;
            var enumerator = list.GetEnumerator();
            var count = 0;
            while (enumerator.MoveNext())
                yield return new CollectionItemEntity(expression, count++, enumerator.Current);

            // read members, it may happen to be an instance of the 
            // user that inherits from IList, so you need to read the members.
            foreach (var memberReader in builder.MemberReaders)
            {
                var items = memberReader.GetMembers(builder, expression, entity);
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}
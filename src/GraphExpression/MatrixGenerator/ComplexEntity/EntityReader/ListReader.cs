using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public class ListReader : IEntityReader
    {
        public bool CanRead(ComplexBuilder builder, object entity)
        {
            return entity is System.Collections.IList;
        }

        public IEnumerable<ComplexEntity> GetChildren(ComplexBuilder builder, Expression<object> expression, object entity)
        {
            var list = (System.Collections.IList)entity;
            for (var i = 0; i < list.Count; i++)
                yield return new ListItemEntity(expression, i, list[i]);

            // read members, it may happen to be an instance of the 
            // user that inherits from IList, so you need to read the members.
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace GraphExpression
{
    public class ObjectReader : IEntityReader
    {
        public bool CanRead(ComplexBuilder builder, object entity)
        {
            return true;
        }

        public IEnumerable<ComplexEntity> GetChildren(ComplexBuilder builder, Expression<object> expression, object entity)
        {
            // read members
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
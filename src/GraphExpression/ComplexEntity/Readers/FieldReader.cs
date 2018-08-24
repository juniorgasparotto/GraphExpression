using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace GraphExpression
{
    public class FieldReader : IComplexItemReader
    {
        public bool CanRead(ComplexBuilder builder, object entity)
        {
            return builder.CanReadMembers(entity);
        }

        public IEnumerable<ComplexEntity> GetItems(ComplexBuilder builder, Expression<object> expression, object entity)
        {
            var fields = builder.GetFields(entity);
            foreach (var f in fields)
                yield return new FieldEntity(expression, entity, f);
        }
    }
}
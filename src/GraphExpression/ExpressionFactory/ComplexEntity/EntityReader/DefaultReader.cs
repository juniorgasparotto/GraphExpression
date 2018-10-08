using GraphExpression.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace GraphExpression
{
    public class DefaultReader : IEntityReader
    {
        public bool CanRead(ComplexExpressionFactory builder, object entity)
        {
            return !ReflectionUtils.IsSystemType(entity.GetType());
        }

        public IEnumerable<ComplexEntity> GetChildren(ComplexExpressionFactory builder, Expression<object> expression, object entity)
        {
            // read members
            foreach (var memberReader in builder.MemberReaders)
            {
                var items = memberReader.GetMembers(builder, expression, entity);
                foreach (var item in items)
                    yield return item;
            }
        }
    }
}
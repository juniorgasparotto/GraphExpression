using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression.Serialization
{
    public interface IEntityDeserialize
    {
        bool CanRead(ItemDeserializer builder, object entity);
        IEnumerable<ComplexEntity> GetChildren(ComplexBuilder builder, Expression<object> expression, object entity);
    }
}
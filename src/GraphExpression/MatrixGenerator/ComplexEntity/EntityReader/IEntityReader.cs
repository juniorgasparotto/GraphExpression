using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public interface IEntityReader
    {
        bool CanRead(ComplexBuilder builder, object entity);
        IEnumerable<ComplexEntity> GetChildren(ComplexBuilder builder, Expression<object> expression, object entity);
    }
}
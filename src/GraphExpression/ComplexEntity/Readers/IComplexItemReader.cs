using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public interface IComplexItemReader
    {
        bool CanRead(ComplexBuilder builder, object entity);
        IEnumerable<ComplexEntity> GetItems(ComplexBuilder builder, Expression<object> expression, object entity);
    }
}
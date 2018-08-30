using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public interface IMemberReader
    {
        bool CanRead(ComplexBuilder builder, object entity);
        IEnumerable<ComplexEntity> GetMembers(ComplexBuilder builder, Expression<object> expression, object entity);
    }
}
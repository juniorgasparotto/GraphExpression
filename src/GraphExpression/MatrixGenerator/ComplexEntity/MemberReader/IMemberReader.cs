using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public interface IMemberReader
    {
        IEnumerable<ComplexEntity> GetMembers(ComplexBuilder builder, Expression<object> expression, object entity);
    }
}
using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public interface IReader
    {
        bool CanRead(object entity);
        IEnumerable<ComplexEntity> GetValues(Expression<object> expression, object entity);
    }
}
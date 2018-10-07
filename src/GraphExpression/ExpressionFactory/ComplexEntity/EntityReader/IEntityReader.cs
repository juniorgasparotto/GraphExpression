using System;
using System.Collections.Generic;
using System.Reflection;


namespace GraphExpression
{
    public interface IEntityReader
    {
        bool CanRead(ComplexExpressionBuilder builder, object entity);
        IEnumerable<ComplexEntity> GetChildren(ComplexExpressionBuilder builder, Expression<object> expression, object entity);
    }
}
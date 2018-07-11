using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace GraphExpression
{
    public class ComplexEntity : EntityItem<object>
    {
        public ComplexEntity(Expression<object> expression) : base(expression)
        {
        }
    }
}

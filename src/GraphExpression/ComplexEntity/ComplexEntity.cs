using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace GraphExpression
{
    public class ComplexEntity
    {
        public object Entity { get; internal set; }

        public ComplexEntity(object entity = null)
        {
            this.Entity = entity;
        }
    }
}

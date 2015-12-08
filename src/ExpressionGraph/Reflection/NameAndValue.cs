using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace ExpressionGraph.Reflection
{
    public abstract class NameAndValue
    {
        public string Name { get; private set; }
        public List<MethodValue> Values { get; private set; }
    }
}

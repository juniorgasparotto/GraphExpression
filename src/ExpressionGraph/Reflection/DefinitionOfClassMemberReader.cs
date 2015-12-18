using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class DefinitionOfClassMemberReader<T>
    {
        public Func<object, Type, bool> CanRead { get; set; }
        public Func<object, Type, IEnumerable<T>> Get { get; set; }
    }
}
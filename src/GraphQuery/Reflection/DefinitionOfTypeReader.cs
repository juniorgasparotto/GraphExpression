using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GraphQuery.Reflection
{
    public class DefinitionOfTypeReader
    {
        public Func<object, bool> CanRead { get; set; }
        public Func<object, IEnumerable<Type>> Get { get; set; }
    }
}
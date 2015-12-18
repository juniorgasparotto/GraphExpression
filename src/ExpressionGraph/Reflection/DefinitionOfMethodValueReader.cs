using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionGraph.Reflection
{
    public class DefinitionOfMethodValueReader<TMemberType>
    {
        //public delegate bool CanReadMethodDelegate(object obj, Type type, TMemberType member);
        //public delegate IEnumerable<MethodValue> GetValuesMethodDelegate(object obj, Type type, TMemberType member);
        //public CanReadMethodDelegate CanRead { get; set; }
        //public GetValuesMethodDelegate GetValues { get; set; }

        public Func<UnitReflaction, Type, TMemberType, bool> CanRead { get; set; }
        public Func<UnitReflaction, Type, TMemberType, IEnumerable<MethodValue>> GetValues { get; set; }
    }
}
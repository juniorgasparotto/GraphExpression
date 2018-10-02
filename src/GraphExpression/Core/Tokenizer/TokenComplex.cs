using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using GraphExpression.Utils;

namespace GraphExpression.Serialization
{
    public class TokenComplex : Token
    {
        public object Entity { get; set; }
        public object EntityType { get; set; }

        public TokenComplex(string keyColonValue) : base(keyColonValue)
        {
            
        }

    }
}

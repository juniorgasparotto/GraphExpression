using System;

namespace GraphExpression
{
    public class ComplexEntityDeserializer
    {
        public Type TypeRoot { get; }

        public ComplexEntityDeserializer(Type typeRoot)
        {
            this.TypeRoot = typeRoot;
        }
    }
}

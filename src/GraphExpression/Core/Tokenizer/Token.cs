using GraphExpression.Utils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GraphExpression
{
    [DebuggerDisplay("{Key}")]
    public class Token
    {
        private readonly Dictionary<string, Token> children;
        protected Context context;

        public TokenRoot Root => context.Root;

        public Token Parent { get; private set; }
        public string Raw { get; private set; }
        public int Count => children.Count;
        public Token this[int index] => children.Values.ElementAt(index);
        public Token this[string key] => children[key];

        public string Value { get; private set; }
        public string Key { get; private set; }
        public bool IsPrimitive { get; private set; }
        public string ComplexEntityId { get; private set; }

        public object Data { get; set; }

        public Token(string keyColonValue)
        {
            this.children = new Dictionary<string, Token>();
            this.Raw = keyColonValue;
            this.ParseRaw();
        }

        public IEnumerable<Token> Children { get => children.Values; }

        public static Token operator +(Token a, Token b)
        {
            if (a.context == null && b.context == null)
            {
                a.context = new Context();
                b.context = a.context;
            }
            else if (a.context == null)
            {
                a.context = b.context;
            }
            else
            {
                b.context = a.context;
            }

            b.context = a.context;
            a.context.Edges.Add(new Edge(a, b));

            a.children.Add(b.Key, b);
            b.Parent = a;
            return a;
        }

        private void ParseRaw()
        {
            if (string.IsNullOrWhiteSpace(Raw))
                return;

            int index = Raw.IndexOf(":");
            string member, value, hashCode = null;
            bool isPrimitive;

            if (index == -1)
            {
                value = null;

                var parts = Raw.Split('.');
                if (StringUtils.IsNumber(parts.LastOrDefault()))
                {
                    // IS COMPLEX: "Prop.12345"
                    hashCode = parts.LastOrDefault();

                    // ShowType = None
                    if (parts.Length == 1)
                        member = parts[0];
                    // ShowType = [OTHERS]
                    else
                        member = parts[parts.Length - 2];

                    isPrimitive = false;
                }
                else
                {
                    // IS PRIMITIVE AND IS NULL: "Prop"
                    member = Raw;
                    isPrimitive = true;
                }
            }
            else
            {
                // IS PRIMITIVE AND HAS VALUE: "Prop: value"
                member = Raw.Substring(0, index);
                value = Raw.Substring(index + 2); // consider space after colon ": "
                isPrimitive = true;
            }

            // ShowType = FullTypeName || TypeName
            var partsWithType = member.Split('.');
            if (partsWithType.Length > 0)
                member = partsWithType.LastOrDefault();

            this.Key = member;
            this.Value = value;
            this.IsPrimitive = isPrimitive;
            this.ComplexEntityId = hashCode;
        }

        protected class Context
        {
            public List<Edge> Edges { get; }
            public TokenRoot Root { get; set; }

            public Context()
            {
                this.Edges = new List<Edge>();
            }
        }

        [DebuggerDisplay("{ToString()}")]
        protected class Edge
        {
            public Token Source { get; }
            public Token Target { get; }

            public Edge(Token source, Token target)
            {
                this.Source = source;
                this.Target = target;
            }

            public override string ToString()
            {
                return $"{Source?.Key}, {Target?.Key}";
            }
        }
    }
}

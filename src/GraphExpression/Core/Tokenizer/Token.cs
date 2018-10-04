using GraphExpression.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace GraphExpression
{
    [DebuggerDisplay("{Name}")]
    public class Token<T> : Token
    {
        public Token(string name, T value) : base(name, value, typeof(T))
        {

        }
    }

    [DebuggerDisplay("{Name}")]
    public class Token
    {
        private readonly Dictionary<string, Token> children;
        protected Context context;

        public TokenComplex Root => context.Root;
        public Token Parent { get; private set; }
        public IEnumerable<Token> Children { get => children.Values; }
        public Token this[int index] => children.Values.ElementAt(index);
        public Token this[string key] => children[key];
        public int Count => children.Count;

        public string Raw { get; private set; }
        public string Name { get; private set; }
        public virtual object Value { get; private set; }
        public string ValueRaw { get; private set; }
        public virtual Type Type { get; protected set; }
        public bool IsPrimitive { get; private set; }
        public string EntityId { get; private set; }
        public object Data { get; set; }

        public Token(string name, object value, Type type = null)
        {
            this.children = new Dictionary<string, Token>();
            this.Type = value?.GetType() ?? type;
            this.Name = name;
            this.Value = value;
            //this.Raw = nameColonValue;
            //this.ParseRaw();
        }

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

            if (!a.children.ContainsKey(b.Name))
                a.children.Add(b.Name, b);

            b.Parent = a;
            return a;
        }

        public virtual void AutoAddOnParent() { }

        #region Auxs

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

            this.Name = member;
            this.IsPrimitive = isPrimitive;
            this.EntityId = hashCode;
            this.ValueRaw = value;

            if (this.Type == null)
                this.Value = value;
            else
                this.Value = TypeDescriptor.GetConverter(Type).ConvertFromInvariantString(value);
        }
        
        #endregion

        #region nested classes

        protected class Context
        {
            public List<Edge> Edges { get; }
            public TokenComplex Root { get; set; }

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
                return $"{Source?.Name}, {Target?.Name}";
            }
        }

        #endregion
    }
}

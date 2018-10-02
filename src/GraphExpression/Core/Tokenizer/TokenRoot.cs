using System;
using System.Diagnostics;

namespace GraphExpression
{
    [DebuggerDisplay("{Key}")]
    public class TokenRoot : Token
    {
        public TokenRoot(string name) : base(name)
        {

        }

        public static TokenRoot operator +(TokenRoot a, Token b)
        {
            a = (TokenRoot)((Token)a + b);
            a.context.Root = a;
            return a;
        }

        public void ReRun(Action<Token, Token> factory)
        {
            foreach(var e in context.Edges)
            {
                factory(e.Source, e.Target);
            }
        }
    }
}

using System;
using System.Diagnostics;

namespace GraphExpression
{
    [DebuggerDisplay("{Value}")]
    public class TokenComplex<T> : TokenComplex
    {
        public TokenComplex(T value = default(T), bool autoReRun = true) : base(value, typeof(T), autoReRun)
        {

        }
    }

    [DebuggerDisplay("{Value}")]
    public class TokenComplex : Token
    {
        private bool autoReRun;

        public TokenComplex(object value, Type type = null, bool autoReRun = true) : base(null, value, type)
        {
            this.autoReRun = autoReRun;
        }

        public static TokenComplex operator +(TokenComplex a, Token b)
        {
            a = (TokenComplex)((Token)a + b);
            a.context.Root = a;
            if (a.autoReRun)
                a.ReRun();
            return a;
        }

        public void ReRun()
        {
            foreach(var e in context.Edges)
                e.Target.AutoAddOnParent();
        }
    }
}

using NCalc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace EntityGraph
{
    public class Token
    {
        public TokenValue TokenValue { get; private set; }
        public Token Parent { get; private set; }
        public int Level { get; private set; }

        public Token(TokenValue token, Token tokenPositionParent, int level)
        {
            this.TokenValue = token;
            this.Parent = tokenPositionParent;
            this.Level = level;
        }

        public override string ToString()
        {
            return this.TokenValue.ToString();
        }
    }
    
    public class TokenRecursive : Token
    {
        public TokenRecursive(TokenValue token, Token tokenPositionParent, int level)
            : base(token, tokenPositionParent, level) 
        { }
    }

    public class TokenValue
    {
        private Func<object, string> viewFunc;
        public object Value { get; private set; }

        public TokenValue(object value, Func<object, string> viewFunc)
        {
            this.Value = value;
            this.viewFunc = viewFunc;
        }

        public override string ToString()
        {
            var res = "";
            if (this is TokenValuePlus)
                res = " + ";
            else if (this is TokenValueOpenParenthesis)
                res = "(";
            else if (this is TokenValueCloseParenthesis)
                res = ")";
            else
                //res = viewFunc(this.Value);
                res = this.Value.ToString();

            return res;
        }
    }

    public class TokenValuePlus : TokenValue
    {
        public static TokenValuePlus Instance = new TokenValuePlus(null);
        private TokenValuePlus(Func<object, string> viewFunc) : base(null, viewFunc) { }
    }

    public class TokenValueOpenParenthesis : TokenValue
    {
        public static TokenValueOpenParenthesis Instance = new TokenValueOpenParenthesis(null);
        private TokenValueOpenParenthesis(Func<object, string> viewFunc) : base(null, viewFunc) { }
    }

    public class TokenValueCloseParenthesis : TokenValue
    {
        public static TokenValueCloseParenthesis Instance = new TokenValueCloseParenthesis(null);
        private TokenValueCloseParenthesis(Func<object, string> viewFunc) : base(null, viewFunc) { }
    }
}

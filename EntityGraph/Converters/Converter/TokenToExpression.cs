using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityGraph.Converter
{
    public class TokenToExpression : TokenToString
    {
        public TokenToExpression(Func<HierarchicalEntity, string> viewFunc, bool ignoreSubTokensOfMainTokens = true, string delimiterMainTokens = null, string delimiterSubTokensOfMainTokens = null)
            : base(viewFunc, ignoreSubTokensOfMainTokens, delimiterMainTokens, delimiterSubTokensOfMainTokens)
        {
        }

        public override string Convert(List<Token> tokens)
        {
            StringBuilder strBuilder = new StringBuilder();
            
            foreach (var token in tokens)
            {
                if (token is TokenRecursive)
                {
                    strBuilder.Append(viewFunc((HierarchicalEntity)token.TokenValue.Value));
                }
                else
                {
                    if (token.TokenValue is TokenValuePlus)
                        strBuilder.Append(" + ");
                    else if (token.TokenValue is TokenValueOpenParenthesis)
                        strBuilder.Append("(");
                    else if (token.TokenValue is TokenValueCloseParenthesis)
                        strBuilder.Append(")");
                    else
                        strBuilder.Append(viewFunc(((HierarchicalEntity)token.TokenValue.Value)));
                }
            }

            return strBuilder.ToString();
        }
    }
}

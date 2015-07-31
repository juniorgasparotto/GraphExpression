using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using EntityGraph.Converter;

namespace EntityGraph.Converter
{
    public class TokenToDebug : TokenToString
    {
        public TokenToDebug(Func<HierarchicalEntity, string> viewFunc, bool ignoreSubTokensOfMainTokens = true, string delimiterMainTokens = null, string delimiterSubTokensOfMainTokens = null)
            : base(viewFunc, 
                   ignoreSubTokensOfMainTokens, 
                   string.IsNullOrWhiteSpace(delimiterMainTokens) ? "\r\n-----\r\n" : delimiterMainTokens, 
                   string.IsNullOrWhiteSpace(delimiterSubTokensOfMainTokens) ? "\r\n\r\n" : delimiterSubTokensOfMainTokens
                  )
        {
        }

        public override string Convert(List<Token> tokens)
        {
            StringBuilder strBuilder = new StringBuilder();
            foreach (var token in tokens)
            {
                var resParent = "";

                if (token.Parent != null)
                    resParent = string.Format(" parent: (hashcode: {0}; value: {1})", token.Parent.GetHashCode(), token.Parent.TokenValue.ToString());

                strBuilder.Append(token.TokenValue.ToString().Trim());
                strBuilder.Append(string.Format(" hashcode: {0}", token.GetHashCode()));
                strBuilder.Append(resParent);
                strBuilder.Append(" level: " + token.Level);
                strBuilder.Append(" hashcodeValue: " + token.TokenValue.GetHashCode());
                strBuilder.AppendLine();
            }

            return Helper.TrimAll(strBuilder.ToString());
        }
    }
}

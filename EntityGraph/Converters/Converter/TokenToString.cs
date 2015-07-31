using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityGraph.Converter
{
    public abstract class TokenToString
    {
        private bool ignoreSubTokensOfMainTokens;
        private string delimiterMainTokens;
        private string delimiterSubTokensOfMainTokens;
        protected Func<HierarchicalEntity, string> viewFunc;

        public TokenToString(Func<HierarchicalEntity, string> viewFunc, bool ignoreSubTokensOfMainTokens = true, string delimiterMainTokens = null, string delimiterSubTokensOfMainTokens = null)
        {
            delimiterSubTokensOfMainTokens = string.IsNullOrEmpty(delimiterSubTokensOfMainTokens) ? "\r\n" : delimiterSubTokensOfMainTokens;
            delimiterMainTokens = string.IsNullOrEmpty(delimiterMainTokens) ? "\r\n" : delimiterMainTokens;

            this.ignoreSubTokensOfMainTokens = ignoreSubTokensOfMainTokens;
            this.delimiterMainTokens = delimiterMainTokens;
            this.delimiterSubTokensOfMainTokens = delimiterSubTokensOfMainTokens;
            this.viewFunc = viewFunc;
        }

        public abstract string Convert(List<Token> tokens);

        public virtual string Convert(TokenGroupCollection collection, HierarchicalEntity edoObject = null)
        {
            var strBuilder = new StringBuilder();
            List<TokenGroup> list;

            if (edoObject != null)
                list = collection.Where(f => f.MainEdoObject == edoObject).ToList();
            else
                list = collection.ToList();

            var last = list.LastOrDefault();

            foreach (var keyPair in list)
            {
                strBuilder.Append(this.Convert(keyPair));

                if (keyPair.GetHashCode() != last.GetHashCode())
                    strBuilder.Append(delimiterMainTokens);
            }

            return Helper.TrimAll(strBuilder.ToString());
        }

        protected virtual string Convert(TokenGroup tokenGroup)
        {
            var strBuilder = new StringBuilder();

            List<List<Token>> list;
            if (ignoreSubTokensOfMainTokens)
            {
                list = new List<List<Token>>();
                list.Add(tokenGroup.MainTokens);
            }
            else
            { 
                list = tokenGroup.ToList();
            }

            var last = list.LastOrDefault();

            foreach (var parsedToken in list)
            {
                strBuilder.Append(this.Convert(parsedToken));
                if (parsedToken.GetHashCode() != last.GetHashCode())
                    strBuilder.Append(this.delimiterSubTokensOfMainTokens);
            }

            return Helper.TrimAll(strBuilder.ToString());
        }
    }
}

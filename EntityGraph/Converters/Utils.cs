using EntityGraph.Converter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityGraph
{
    public class Utils
    {
        #region Writers

        public static ListOfHierarchicalEntity FromExpression(params string[] expressions)
        {
            var converter = new ExpressionToHierarchicalEntity();
            return converter.Convert(expressions);
        }

        public static ListOfHierarchicalEntity FromExpression(ListOfHierarchicalEntity paramsOfExpressions, params string[] expressions)
        {
            var converter = new ExpressionToHierarchicalEntity();
            return converter.Convert(paramsOfExpressions, expressions);
        }

        #endregion

        #region Readers (ignore sub tokens)

        public static string ToExpression(HierarchicalEntity entity, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal)
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToString = new TokenToExpression(viewFunc, CanIgnoreSubTokens(tokenizeType));

            var tokenGroupCollection = converterToToken.Convert(entity);
            var output = converterToString.Convert(tokenGroupCollection, entity);

            return output;
        }

        public static string ToExpression(ListOfHierarchicalEntity entities, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal, string delimiterMainTokens = "\r\n")
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToString = new TokenToExpression(viewFunc, CanIgnoreSubTokens(tokenizeType), delimiterMainTokens);
            var tokenGroupCollection = converterToToken.Convert(entities);
            var output = converterToString.Convert(tokenGroupCollection);

            return output;
        }

        public static string ToHierarchy(HierarchicalEntity entity, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal)
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToString = new TokenToHierarchy(viewFunc, CanIgnoreSubTokens(tokenizeType));

            var tokenGroupCollection = converterToToken.Convert(entity);
            var output = converterToString.Convert(tokenGroupCollection);

            return output;
        }

        public static string ToHierarchy(List<HierarchicalEntity> entities, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal)
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToString = new TokenToHierarchy(viewFunc, CanIgnoreSubTokens(tokenizeType));

            var tokenGroupCollection = converterToToken.Convert(entities);
            var output = converterToString.Convert(tokenGroupCollection);

            return output;
        }

        public static string ToHierarchyInverse(HierarchicalEntity entity, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal)
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToExp = new TokenToHierarchyInverse(viewFunc);

            var tokenGroupCollection = converterToToken.Convert(entity);
            var output = converterToExp.Convert(tokenGroupCollection, entity);
            return output;
        }

        public static string ToHierarchyInverse(List<HierarchicalEntity> entities, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal)
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToExp = new TokenToHierarchyInverse(viewFunc);

            var tokenGroupCollection = converterToToken.Convert(entities);
            var output = converterToExp.Convert(tokenGroupCollection);
            return output;
        }

        public static string ToDebug(HierarchicalEntity entity, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal)
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToString = new TokenToDebug(viewFunc, CanIgnoreSubTokens(tokenizeType));

            var tokenGroupCollection = converterToToken.Convert(entity);
            var output = converterToString.Convert(tokenGroupCollection);

            return output;
        }

        public static string ToDebug(List<HierarchicalEntity> entities, Func<object, string> viewFunc, TokenizeType tokenizeType = TokenizeType.Normal)
        {
            var converterToToken = new HierarchicalEntityToToken(viewFunc, tokenizeType);
            var converterToString = new TokenToDebug(viewFunc, CanIgnoreSubTokens(tokenizeType));

            var tokenGroupCollection = converterToToken.Convert(entities);
            var output = converterToString.Convert(tokenGroupCollection);

            return output;
        }

        private static bool CanIgnoreSubTokens(TokenizeType tokenizeType)
        {
            // It makes no sense not to list the sub tokens in "NeverRepeatDefinedTokenIfAlreadyParsed", can be lost information
            //if (tokenizeType == TokenizeType.NeverRepeatDefinedTokenIfAlreadyParsed)
            //    return false;

            return true;
        }

        #endregion
    }
}

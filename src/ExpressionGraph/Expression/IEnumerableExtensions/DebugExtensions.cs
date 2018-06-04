using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionGraph
{
    public static class DebugExtensions
    {
        public static string ToDebug<T>(this IEnumerable<ExpressionItem<T>> expression)
        {
            var str = "";
            foreach (var i in expression)
                str += i.ToString().Trim() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.LevelInExpression.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.Level.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.Index.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.IndexSameLevel.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
            {
                var t = i.PreviousInExpression is ExpressionItemCloseParenthesis<T>;
                t = t ? t : i.PreviousInExpression is ExpressionItemOpenParenthesis<T>;
                t = t ? t : i.PreviousInExpression is ExpressionItemPlus<T>;

                str += (i.PreviousInExpression == null) ? "  " : (t ? i.PreviousInExpression.ToString() : i.PreviousInExpression.ToString() + " ");
            }

            str += "\r\n";
            foreach (var i in expression)
                str += (i.PreviousInGraph == null) ? "  " : i.PreviousInGraph.ToString() + " ";

            return str;
        }
    }
}
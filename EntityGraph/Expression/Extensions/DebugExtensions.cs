using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public static class DebugExtensions
    {
        public static string ToDebug<T>(this Expression<T> expression)
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
                str += (i.Parent == null) ? "- " : i.Parent.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.Root.ToString() + " ";

            return str;
        }
    }
}
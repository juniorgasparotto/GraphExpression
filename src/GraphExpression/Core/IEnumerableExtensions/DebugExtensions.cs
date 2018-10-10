using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression
{
    /// <summary>
    ///  Debug extensions - only for developers
    /// </summary>
    public static class DebugExtensions
    {
        /// <summary>
        /// Debug a expression
        /// </summary>
        /// <typeparam name="T">Type of real entity</typeparam>
        /// <param name="expression">Expression to debug</param>
        /// <returns>String for debug</returns>
        public static string ToDebug<T>(this IEnumerable<EntityItem<T>> expression)
        {
            var str = "";
            foreach (var i in expression)
                str += i.ToString().Trim() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.Level.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.Level.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.Index.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
                str += i.IndexAtLevel.ToString() + " ";

            str += "\r\n";
            foreach (var i in expression)
            {
                str += (i.Previous == null) ? "  " : i.Previous.ToString() + " ";
            }

            str += "\r\n\r\n\r\n";
            foreach (var i in expression)
            {
                str += $"{i.ToString()} | {i.Level} | {i.IndexAtLevel} \r\n";
            }
            return str;
        }
    }
}
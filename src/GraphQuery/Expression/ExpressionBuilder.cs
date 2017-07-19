using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GraphQuery
{
    public class ExpressionBuilder<T>
    {
        public static IEnumerable<Expression<T>> Build
        (
            IEnumerable<T> source, 
            Func<T, IEnumerable<T>> 
            childrenCallback, 
            bool awaysRepeatDefined = true, 
            bool enableParenthesis = true, 
            bool enablePlus = true,
            Func<ExpressionItem<T>, string> entityToStringCallback = null,
            long maxItems = 0
        )
        {
            Expression<T> expression = null;
            var expressions = new List<Expression<T>>();

            var iteration = new Iteration<T>()
            {
                Enumerator = source.Distinct().GetEnumerator(),
                Level = 1,
                Index = -1
            };

            var iterations = new List<Iteration<T>>();
            iterations.Add(iteration);

            while (true)
            {
                while (iteration.Enumerator.MoveNext())
                {
                    var indexSameLevel = iteration.Index += 1;

                    // New expression
                    if (iteration.Level == 1)
                    {
                        expression = new Expression<T>(enableParenthesis, enablePlus, entityToStringCallback, maxItems);
                        expressions.Add(expression);
                    }

                    var entity = iteration.Enumerator.Current;

                    // Prevent recursion, infinite loop. eg: "A + B + [A]" where [A] already exists in path
                    // Get "last" of the same level of the parent of this current entity.
                    // A + (E + (C + (D + E) + E) + E)
                    // 1    2    3    4   5    4    3
                    //                ^   -             : Get last that has the level equals "4" (iteration.Level - 1), return "D" = continue recursion, but in the next rule will be tested the ancestors
                    //           ^             -        : Get last that has the level equals "3" (iteration.Level - 1), return "C" = continue recursion, but in the next rule will be tested the ancestors
                    //      ^                       -   : Get last that has the level equals "2" (iteration.Level - 1), return "E" = STOP recursion, because last parent is equals the current entity.
                    var last = expression.LastOrDefault(f => f.Level == iteration.Level - 1);
                    bool exists;

                    if (awaysRepeatDefined)
                    { 
                        exists = last != null && last.Entity != null && last.Entity.Equals(entity);
                        if (!exists)
                            exists = expression.Find(last)
                                               .AncestorsUntil((f,level) => f.Entity.Equals(entity))
                                               .Count(f=>f.Entity.Equals(entity)) == 0 ? false : true;
                    }
                    else
                    {
                        exists = expression.Count(f => f.Entity != null && f.Entity.Equals(entity)) == 0 ? false : true;
                    }

                    IEnumerable<T> children = null;
                    
                    if (!exists)
                        children = childrenCallback(entity);

                    var hasChildren = children != null && children.Count() > 0;

                    if (hasChildren)
                    {
                        // add parenthesis "(B" because exists children
                        var addParenthesis = iteration.Level > 1;

                        if (addParenthesis)
                            expression.OpenParenthesis();

                        iteration = new Iteration<T>()
                        {
                            Enumerator = children.GetEnumerator(),
                            Level = iteration.Level + 1,
                            EntityRootOfTheIterationForDebug = iteration.Enumerator.Current,
                            IterationParent = iteration,
                            HasOpenParenthesis = addParenthesis,
                            Index = -1
                        };

                        iterations.Add(iteration);
                    }

                    expression.AddItem(entity, indexSameLevel);
                }

                if (iteration.HasOpenParenthesis)
                    expression.CloseParenthesis();

                // Remove iteration because is empty
                iterations.Remove(iteration);

                if (iterations.Count == 0)
                    break;

                iteration = iterations.LastOrDefault();
            }

            return expressions;
        }

        public static void Build2
        (
            IEnumerable<T> source,
            Func<T, IEnumerable<T>> childrenCallback,
            bool awaysRepeatDefined = true
        )
        {
            var iteration = new Iteration<T>()
            {
                Enumerator = source.Distinct().GetEnumerator(),
                Level = 1,
            };

            var iterations = new List<Iteration<T>>();
            iterations.Add(iteration);

            while (true)
            {
                while (iteration.Enumerator.MoveNext())
                {
                    // New expression
                    if (iteration.Level == 1)
                    {
                        // initObject()
                    }

                    var entity = iteration.Enumerator.Current;
                    bool exists = false;

                    if (awaysRepeatDefined)
                    {
                        
                    }
                    else
                    {

                    }

                    IEnumerable<T> children = null;

                    if (!exists)
                        children = childrenCallback(entity);

                    var hasChildren = children != null && children.Count() > 0;

                    if (hasChildren)
                    {
                        // add parenthesis "(B" because exists children
                        var addParenthesis = iteration.Level > 1;

                        iteration = new Iteration<T>()
                        {
                            Enumerator = children.GetEnumerator(),
                            Level = iteration.Level + 1,
                            EntityRootOfTheIterationForDebug = iteration.Enumerator.Current,
                            IterationParent = iteration,
                            HasOpenParenthesis = addParenthesis
                        };

                        iterations.Add(iteration);
                    }

                    // addItem();
                }

                if (iteration.HasOpenParenthesis)
                {
                    // endPath();
                }

                // Remove iteration because is empty
                iterations.Remove(iteration);

                if (iterations.Count == 0)
                    break;

                iteration = iterations.LastOrDefault();
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EntityGraph
{
    public class ExpressionBuilder<T>
    {
        // Not accept multigraph
        public static IEnumerable<Expression<T>> Build(IEnumerable<T> source, Func<T, List<T>> childrenCallback, bool enableParenthesis = true, bool enablePlus = true, Func<T, string> entityToStringCallback = null)
        {
            Expression<T> expression = null;
            var expressions = new List<Expression<T>>();

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
                        expression = new Expression<T>(enableParenthesis, enablePlus, entityToStringCallback);
                        expressions.Add(expression);
                    }

                    var entity = iteration.Enumerator.Current;

                    // Prevent recursion, infinite loop. eg: "A + B + [A]" where [A] already exists in path
                    //var last = expression.LastOrDefault();
                    var last = expression.LastOrDefault(f => f.Level == iteration.Level - 1);

                    var exists = last != null && last.Entity != null && last.Entity.Equals(entity);
                    if (!exists)
                        exists = expression.Find(last)
                                           .AncestorsUntil((f,level) => f.Entity.Equals(entity))
                                           .Count(f=>f.Entity.Equals(entity)) == 0 ? false : true;

                    List<T> children = null;
                    
                    if (!exists)
                        children = childrenCallback(entity);

                    var hasChildren = children != null && children.Count > 0;

                    if (hasChildren)
                    {
                        // add parenthesis "(B" because exists children
                        var addParenthesis = iteration.Level > 1;

                        if (addParenthesis)
                            expression.OpenParenthesis();

                        expression.AddItem(entity);

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
                    else
                    {
                        expression.AddItem(entity);
                    }
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
    }
}
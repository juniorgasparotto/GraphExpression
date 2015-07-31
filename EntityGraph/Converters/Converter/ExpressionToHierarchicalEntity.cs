using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EntityGraph.Converter
{
    public class ExpressionToHierarchicalEntity
    {
        public ListOfHierarchicalEntity Convert(params string[] expressions)
        {
            return this.Convert(null, expressions);
        }

        public ListOfHierarchicalEntity Convert(ListOfHierarchicalEntity paramsOfExpressions, params string[] expressions)
        {
            expressions = expressions.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
            ListOfHierarchicalEntity result = new ListOfHierarchicalEntity();

            if (paramsOfExpressions == null)
                result = new ListOfHierarchicalEntity();
            else
                result = paramsOfExpressions.ToListOfHierarchicalEntity();


            foreach (var expression in expressions)
            {
                var e = this.Prepare(expression);

                e.EvaluateParameter += delegate(string name, ParameterArgs args)
                {
                    // FIX to back params name
                    name = name.Replace('_', '.');
                    var objectAdd = result.GetByIdentity(name);
                    if (objectAdd == null)
                    {
                        objectAdd = new HierarchicalEntity(name);
                        result.Add(objectAdd);
                    }

                    var param = new ExpressionParam(objectAdd);
                    args.Result = param;
                };

                e.Evaluate();
            }

            return result;
        }

        public Expression Prepare(string expression)
        {
            // FIX to resolve params with name contains ".", ex: "Namespace.ObjectName"
            expression = expression.Replace('.', '_');
            var e = new Expression(expression, EvaluateOptions.NoCache);

            e.EvaluateFunction += delegate(string name, FunctionArgs args)
            {
                var value = args.Parameters[0].Evaluate();
                args.Result = value;
            };

            return e;
        }
    }
}

using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ExpressionGraph.Serialization
{
    public class ExpressionSerializer
    {
        public IEnumerable<HierarchicalEntity> FromString(params string[] expressions)
        {
            return this.FromString(null, expressions);
        }

        public IEnumerable<HierarchicalEntity> FromString(IEnumerable<HierarchicalEntity> paramsOfExpressions, params string[] expressions)
        {
            expressions = expressions.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
            var result = new List<HierarchicalEntity>();

            if (paramsOfExpressions == null)
                result = new List<HierarchicalEntity>();
            else
                result = new List<HierarchicalEntity>(paramsOfExpressions);


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

                    var param = new StringParam(objectAdd);
                    args.Result = param;
                };

                e.Evaluate();
            }

            return result;
        }

        public string ToString<T>(ExpressionGraph.Expression<T> expression)
        {
            var output = "";

            expression.FirstOrDefault().IterationAll
                (
                    itemWhenStart =>
                    {
                        if (itemWhenStart.IsRoot())
                        {
                            output = itemWhenStart.ToString();
                        }
                        else
                        {
                            output += " + ";
                            if (itemWhenStart.HasChildren())
                                output += "(";

                            output += itemWhenStart.ToString();
                        }
                    },
                    itemWhenEnd =>
                    {
                        if (!itemWhenEnd.IsRoot())
                            output += ")";
                    }
                );

            return output;
        }

        #region Privates

        private Expression Prepare(string expression)
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

        #endregion
    }
}

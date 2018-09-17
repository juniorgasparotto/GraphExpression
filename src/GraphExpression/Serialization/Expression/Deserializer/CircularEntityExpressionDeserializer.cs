using NCalc;
using System.Collections.Generic;
using System.Linq;

namespace GraphExpression.Serialization
{
    public class CircularEntityExpressionDeserializer
    {
        public IEnumerable<CircularEntity> FromString(params string[] expressions)
        {
            return this.FromString(null, expressions);
        }

        public IEnumerable<CircularEntity> FromString(IEnumerable<CircularEntity> paramsOfExpressions, params string[] expressions)
        {
            expressions = expressions.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
            var result = new List<CircularEntity>();

            if (paramsOfExpressions == null)
                result = new List<CircularEntity>();
            else
                result = new List<CircularEntity>(paramsOfExpressions);

            foreach (var expression in expressions)
            {
                var e = this.Prepare(expression);

                e.EvaluateParameter += delegate(string name, ParameterArgs args)
                {
                    // FIX to back params name
                    name = name.Replace('_', '.');
                    var objectAdd = result.FirstOrDefault(f => f.Name == name);
                    if (objectAdd == null)
                    {
                        objectAdd = new CircularEntity(name);
                        result.Add(objectAdd);
                    }

                    var param = new StringParam(objectAdd);
                    args.Result = param;
                };

                e.Evaluate();
            }

            return result;
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

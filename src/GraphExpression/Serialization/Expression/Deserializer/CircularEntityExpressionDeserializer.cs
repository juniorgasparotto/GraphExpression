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



//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.CodeAnalysis;
//using Microsoft.CodeAnalysis.CSharp;
//using Microsoft.CodeAnalysis.CSharp.Scripting;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
//using Microsoft.CodeAnalysis.Scripting;
//using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

//namespace ConstructionCS
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var expression = "A.U.OI.TEST.KAKA + \"B\" + (I + O)";
//            var compile = CSharpScript.Create<EntityRepresentation>(expression).GetCompilation();
//            var root = compile.SyntaxTrees.Single().GetRoot();

//            var descentands = root.DescendantNodes().Where(n =>
//            {
//                if (n is MemberAccessExpressionSyntax || n is IdentifierNameSyntax || n is LiteralExpressionSyntax)
//                {
//                    // if is IdentifierNameSyntax but is child of MemberAccessExpressionSyntax
//                    // Test.DAL = MemberAccessExpressionSyntax
//                    // Test     = IdentifierNameSyntax -> IGNORE
//                    // DAL      = IdentifierNameSyntax -> IGNORE
//                    if (n.Parent is MemberAccessExpressionSyntax)
//                        return false;
//                    return true;
//                }
//                return false;
//            }).ToList();

//            var otherRoot = root.ReplaceNodes(descentands, (n1, n2) =>
//            {
//                // add a space to fix error when generate:
//                // from (error): newConstructionCS.EntityRepresentation
//                // to          : new ConstructionCS.EntityRepresentation
//                var clsName =
//                           QualifiedName(
//                               IdentifierName(" " + nameof(ConstructionCS)),
//                               IdentifierName(nameof(EntityRepresentation)));

//                var argumentName = Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(n1.ToString())));
//                var argumentContainer = Argument(IdentifierName(nameof(Globals.Container)));
//                var argumentsSeparatedList = SeparatedList(new[] { argumentName, argumentContainer });

//                var objInitiation = ObjectCreationExpression(clsName)
//                    .WithNewKeyword(Token(SyntaxKind.NewKeyword))
//                    .WithArgumentList(ArgumentList(argumentsSeparatedList));

//                return objInitiation;
//            });

//            var globals = new Globals();
//            var result2 = CSharpScript.EvaluateAsync<EntityRepresentation>(otherRoot.ToString()
//                , ScriptOptions.Default.WithReferences(typeof(EntityRepresentation).Assembly),
//                globals: globals).Result;
//            var entity = result2.GetEntity();
//        }
//    }
//    public class Globals
//    {
//        public List<CircularEntity> Container { get; set; } = new List<CircularEntity>();
//    }

//    [DebuggerDisplay("{Name}")]
//    public class EntityRepresentation
//    {
//        public List<CircularEntity> Container { get; private set; }
//        public string Name { get; private set; }

//        public EntityRepresentation(string name, List<CircularEntity> container)
//        {
//            this.Container = container;
//            this.Name = name.Trim('"');
//        }

//        public static EntityRepresentation operator +(EntityRepresentation a, EntityRepresentation b)
//        {
//            var result = a.GetEntity() + b.GetEntity();
//            return a;
//        }

//        public CircularEntity GetEntity()
//        {
//            var e = Container.FirstOrDefault(f => f.Name == Name);
//            if (e == null)
//            {
//                e = new CircularEntity(Name);
//                Container.Add(e);
//            }
//            return e;
//        }

//        public static EntityRepresentation operator -(EntityRepresentation a, EntityRepresentation b)
//        {
//            var result = a.GetEntity() - b.GetEntity();
//            return a;
//        }

//        public override string ToString()
//        {
//            return this.Name;
//        }
//    }

//    [DebuggerDisplay("{Name}")]
//    public class CircularEntity : List<CircularEntity>
//    {
//        public string Name { get; private set; }

//        public CircularEntity(string name)
//        {
//            this.Name = name;
//        }

//        // only didatic
//        public IEnumerable<CircularEntity> Children { get => this; }

//        public static CircularEntity operator +(CircularEntity a, CircularEntity b)
//        {
//            a.Add(b);
//            return a;
//        }

//        public static CircularEntity operator -(CircularEntity a, CircularEntity b)
//        {
//            a.Remove(b);
//            return a;
//        }

//        public override string ToString()
//        {
//            return this.Name;
//        }
//    }
//}

////using Microsoft.CodeAnalysis.CSharp.Scripting;
////using Microsoft.CodeAnalysis.Scripting;
////using System;
////using System.Collections.Generic;

////namespace ConsoleApp7
////{
////    public class Globals
////    {
////        public CircularEntity X;
////        public CircularEntity Y;
////    }

////    public class CircularEntity : List<CircularEntity>
////    {
////        public string Name { get; private set; }
////        public CircularEntity(string name) => this.Name = name;

////        // only didatic
////        public IEnumerable<CircularEntity> Children { get => this; }

////        public static CircularEntity operator +(CircularEntity a, CircularEntity b)
////        {
////            a.Add(b);
////            return a;
////        }

////        public static CircularEntity operator -(CircularEntity a, CircularEntity b)
////        {
////            a.Remove(b);
////            return a;
////        }

////        public override string ToString()
////        {
////            return this.Name;
////        }
////    }

////    class Program
////    {
////        static void Main(string[] args)
////        {
////            var X = new CircularEntity("X");
////            var Y = new CircularEntity("Y");
////            var Z = new CircularEntity("Z");
////            var global = new Globals
////            {
////                X = X,
////                Y = Y
////            };

////            var result = CSharpScript.EvaluateAsync<CircularEntity>("X+Y+X"
////                , globalsType: typeof(CircularEntity))
////                .ContinueWith(f => X).Result;

////        }
////    }
////}

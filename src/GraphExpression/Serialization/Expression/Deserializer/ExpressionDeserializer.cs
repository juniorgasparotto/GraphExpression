using GraphExpression.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GraphExpression.Serialization
{
    public class ExpressionDeserializer<T>
    {
        public T Deserialize(string expression, Func<string, T> createEntityCallback)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            Validation.ArgumentNotNull(createEntityCallback, nameof(createEntityCallback));

            var container = new ContainerDeserializer<T>(createEntityCallback, new Dictionary<string, T>());
            return Deserialize(expression, container);
        }

        public T Deserialize(string expression)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            
            var container = new ContainerDeserializer<T>(null, new Dictionary<string, T>());
            return Deserialize(expression, (ContainerDeserializer<T>)null);
        }

        public T Deserialize(string expression, ContainerDeserializer<T> container)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            Validation.ArgumentNotNull(container, nameof(container));

            var compile = CSharpScript.Create(expression).GetCompilation();
            var root = compile.SyntaxTrees.Single().GetRoot();

            var descentands = root.DescendantNodes().Where(n =>
            {
                if (n is MemberAccessExpressionSyntax || n is IdentifierNameSyntax || n is LiteralExpressionSyntax)
                {
                    // 1) if is IdentifierNameSyntax but is child of MemberAccessExpressionSyntax
                    // Example.DAL = MemberAccessExpressionSyntax
                    // Example     = IdentifierNameSyntax -> IGNORE
                    // DAL         = IdentifierNameSyntax -> IGNORE
                    // 2) if is IdentifierNameSyntax but is child of InvocationExpressionSyntax
                    // MyMethod(Param1, Param2) = InvocationExpressionSyntax
                    // MyMethod                 = IdentifierNameSyntax -> IGNORE because parent is InvocationExpressionSyntax
                    if (n.Parent is MemberAccessExpressionSyntax || n.Parent is InvocationExpressionSyntax)
                        return false;

                    return true;
                }
                return false;
            }).ToList();

            var otherRoot = root.ReplaceNodes(descentands, (n1, n2) =>
            {
                // if start with "'" is a string params and can be used in functions
                // GetEntity('create-entity-by-string') + "DirectEntity"
                if (n1 is LiteralExpressionSyntax && n1.ToString().StartsWith("'"))
                {
                    var strValue = RemoveQuotes(n1.ToString(), '\'');
                    return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(strValue));
                }
                else
                {
                    var argumentValueName = Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(RemoveQuotes(n1.ToString(), '"'))));
                    var argumentsSeparatedList = SeparatedList(new[] { argumentValueName });
                    var argumentsList = ArgumentList(argumentsSeparatedList);

                    return InvocationExpression(IdentifierName(nameof(ContainerDeserializer<T>.GetEntity)), argumentsList);
                }
            });

            var rootEntity = CSharpScript.EvaluateAsync<T>
            (
                otherRoot.ToString(),
                ScriptOptions.Default.WithReferences(typeof(ContainerDeserializer<T>).Assembly),
                globals: container
            ).Result;

            return rootEntity;
        }

        private string RemoveQuotes(string value, char quote)
        {
            // minimun: '' or ""
            if (value.Length >= 2 && value.StartsWith(quote.ToString()))
                return value.Substring(1, value.Length - 2);
            return value;
        }
    }
}
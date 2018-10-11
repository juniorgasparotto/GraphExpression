using GraphExpression.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GraphExpression.Serialization
{
    /// <summary>
    /// Class responsible for string compilation using Roslyn
    /// </summary>
    /// <typeparam name="T">Type of expression output</typeparam>
    public class RoslynExpressionDeserializer<T>
    {
        /// <summary>
        /// Set custom assemblies if necessary
        /// </summary>
        public List<Assembly> Assemblies { get; set; }

        /// <summary>
        /// Create a roslyn expression deserializer (compiler)
        /// </summary>
        public RoslynExpressionDeserializer()
        {
            Assemblies = new List<Assembly>() { typeof(T).Assembly };
        }

        /// <summary>
        /// Convert a expression string as c# delegate
        /// </summary>
        /// <param name="expression">Expression as string</param>
        /// <param name="typeFactory">Type of expression output</param>
        /// <returns>A delegate to be runned by user</returns>
        public ScriptRunner<T> GetDelegateExpression(string expression, Type typeFactory)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            Validation.ArgumentNotNull(typeFactory, nameof(typeFactory));

            var origTree = CSharpSyntaxTree.ParseText(expression, CSharpParseOptions.Default.WithKind(SourceCodeKind.Script));
            var root = origTree.GetRoot();

            var descentands = root.DescendantNodes().Where(n =>
            {
                // A + null     => Is C# Null
                // A + \"null\" => Is a string with "null" value
                if (n.ToString() == Constants.NULL_VALUE)
                    return false;

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

            var index = 0;
            var otherRoot = root.ReplaceNodes(descentands, (n1, n2) =>
            {
                var content = n1.ToString();
                // if start with "'" is a string params and can be used in factory
                // GetEntity('create-entity-by-string') + "DirectEntity"
                if (n1 is LiteralExpressionSyntax && content.StartsWith(Constants.CHAR_QUOTE.ToString()))
                {
                    var strValue = StringUtils.RemoveQuotes(content, Constants.CHAR_QUOTE).Replace("\\'", "'");
                    return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(strValue));
                }
                else
                {
                    var argumentValueName = Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(StringUtils.RemoveQuotes(content, Constants.DEFAULT_QUOTE))));
                    var argumentIdName = Argument(LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(index)));
                    var argumentsSeparatedList = SeparatedList(new[] { argumentValueName, argumentIdName });
                    var argumentsList = ArgumentList(argumentsSeparatedList);

                    index++;
                    return InvocationExpression(IdentifierName(nameof(IDeserializeFactory<T>.GetEntity)), argumentsList);
                }
            });

            // add factory assembly if not exists
            var assemblies = Assemblies;
            if (!Assemblies.Contains(typeFactory.Assembly))
            {
                assemblies = assemblies.ToList();
                assemblies.Add(typeFactory.Assembly);
            }

            var script = CSharpScript.Create<T>
            (
                otherRoot.ToString(),
                ScriptOptions.Default.WithReferences(assemblies.ToArray()),
                globalsType: typeFactory
            );

            return script.CreateDelegate();
        }
    }
}
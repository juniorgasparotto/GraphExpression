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
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace GraphExpression.Serialization
{
    public class ExpressionDeserializer<T>
    {
        public List<Assembly> Assemblies { get; set; }

        public ExpressionDeserializer()
        {
            Assemblies = new List<Assembly>() { typeof(T).Assembly };
        }

        public T Deserialize(string expression, Func<string, T> createEntityCallback)
        {
            return DeserializeAsync(expression, createEntityCallback).Result;
        }

        public async Task<T> DeserializeAsync(string expression, Func<string, T> createEntityCallback)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            Validation.ArgumentNotNull(createEntityCallback, nameof(createEntityCallback));

            var factory = new EntityFactoryDeserializer<T>(createEntityCallback, new Dictionary<string, T>());
            return await DeserializeAsync(expression, factory);
        }

        public T Deserialize(string expression)
        {
            return DeserializeAsync(expression).Result;
        }

        public async Task<T> DeserializeAsync(string expression)
        {
            Validation.ArgumentNotNull(expression, nameof(expression));
            
            var factory = new EntityFactoryDeserializer<T>(null, new Dictionary<string, T>());
            return await DeserializeAsync(expression, factory);
        }

        public T Deserialize(string expression, EntityFactoryDeserializer<T> factory)
        {
            return DeserializeAsync(expression, factory).Result;
        }

        public async Task<T> DeserializeAsync(string expression, EntityFactoryDeserializer<T> factory)
        {
            var runner = GetDelegate(expression, factory.GetType());
            return await runner(factory);
        }

        public ScriptRunner<T> GetDelegate(string expression, Type typeFactory)
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
                    var argumentIdName = Argument(LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(Guid.NewGuid().ToString())));
                    var argumentsSeparatedList = SeparatedList(new[] { argumentValueName, argumentIdName });
                    var argumentsList = ArgumentList(argumentsSeparatedList);

                    return InvocationExpression(IdentifierName(nameof(EntityFactoryDeserializer<T>.GetEntity)), argumentsList);
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
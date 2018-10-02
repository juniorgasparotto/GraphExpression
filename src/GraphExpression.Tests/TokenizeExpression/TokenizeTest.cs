using GraphExpression.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace GraphExpression.Tests.Serialization
{
    public partial class TokenizeTest
    {
        public TokenRoot A { get; set; } = new TokenRoot("A.123");
        public Token B { get; set; } = new Token("B: valueB");
        public Token C { get; set; } = new Token("C: valueC");
        public Token D { get; set; } = new Token("D: valueD");
        public Token E { get; set; } = new Token("E: valueE");
        public Token F { get; set; } = new Token("F: valueF");
        public Token G { get; set; } = new Token("G: valueG");

        [Fact]
        public void TokenTest_CheckProperties()
        {
            A = A + (B + C);
            TestToken(A, "A", null, "123", false, "A.123", A);
            TestToken(B, "B", "valueB", null, true, "B: valueB", A);
            TestToken(C, "C", "valueC", null, true, "C: valueC", A);

            Assert.Same(B, A[0]);
            Assert.Same(B, A["B"]);
            Assert.Same(C, A["B"][0]);
            Assert.Same(C, A["B"]["C"]);
        }

        //[Fact]
        //public void TokenTest_ExecuteTest_Dic()
        //{
        //    // B + C -> First
        //    // A + B
        //    A = A + (B + C);
        //    var dic = new Dictionary<string, int>()
        //    {
        //        { "Key1", 10 },
        //        { "Key2", 20 },
        //    };

        //    var deserialize = new ComplexEntityDeserializer(dic.GetType());

        //}

        [Fact]
        public void TokenTest_ExecuteTest_ChildPharentesisExecuteFirst()
        {
            // B + C -> First
            // A + B
            A = A + (B + C);
            List<Token> tokens = new List<Token>();
            A.ReRun((source, target) =>
            {
                if (source.Data == null)
                    source.Data = source == source.Root ? new TokenRoot(source.Raw) : new Token(source.Raw);

                if (target.Data == null)
                    target.Data = new Token(target.Raw);

                var r = ((Token)source.Data) + ((Token)target.Data);
                tokens.Add(source);
                tokens.Add(target);

                //if (source.Data == null)
                //    source.Data = new ItemDeserializer(source.Raw);

                //if (target.Data == null)
                //    target.Data = new ItemDeserializer(target.Raw);

                //var r = ((ItemDeserializer)source.Data) + ((ItemDeserializer)target.Data);
                //tokens.Add(source);
                //tokens.Add(target);
            });

            var root = tokens.OfType<TokenRoot>().First();
            TestToken(root, "A", null, "123", false, "A.123", root);
            TestToken(A["B"], "B", "valueB", null, true, "B: valueB", root);
            TestToken(A["B"]["C"], "C", "valueC", null, true, "C: valueC", root);

            //ComplexEntity GetComplex(Token token)
            //{
            //    var complex = token.Data as ComplexEntity;
            //    if (complex == null)
            //    {
            //        var parent = GetComplex(token.Parent) as ComplexEntity;
            //        parent.Entity.
            //    }
            //}
        }

        [Fact]
        public void TokenTest_ExecuteTest_RootPharentesisExecuteFirst()
        {
            // A + B -> First
            // A + C 
            A = A + B + C;
            List<Token> tokens = new List<Token>();
            A.ReRun((source, target) =>
            {
                if (source.Data == null)
                    source.Data = source == source.Root ? new TokenRoot(source.Raw) : new Token(source.Raw);

                if (target.Data == null)
                    target.Data = new Token(target.Raw);

                var r = ((Token)source.Data) + ((Token)target.Data);
                tokens.Add(source);
                tokens.Add(target);
            });

            var root = tokens.OfType<TokenRoot>().First();
            TestToken(root, "A", null, "123", false, "A.123", root);
            TestToken(A["B"], "B", "valueB", null, true, "B: valueB", root);
            TestToken(A["C"], "C", "valueC", null, true, "C: valueC", root);
        }

        private void TestToken(Token token, string key, string value, string complexId, bool isPrimitive,  string raw, Token root)
        {
            Assert.Equal(key, token.Key);
            Assert.Equal(complexId, token.ComplexEntityId);
            Assert.Equal(isPrimitive, token.IsPrimitive);
            Assert.Equal(value, token.Value);
            Assert.Equal(raw, token.Raw);
            Assert.Equal(root, token.Root);
        }
    }
}

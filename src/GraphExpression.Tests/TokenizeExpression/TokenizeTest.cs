using GraphExpression.Serialization;
using Xunit;

namespace GraphExpression.Tests.Tokenization
{
    public partial class TokenizationTest
    {
        public TokenComplex A { get; set; } = new TokenComplex("A.123");
        public Token B { get; set; } = new Token("B", "valueB");
        public Token C { get; set; } = new Token("C", "valueC");
        public Token D { get; set; } = new Token("D", "valueD");
        public Token E { get; set; } = new Token("E", "valueE");
        public Token F { get; set; } = new Token("F", "valueF");
        public Token G { get; set; } = new Token("G", "valueG");

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
            //A = A + (B + C);
            //List<Token> tokens = new List<Token>();
            //A.ReRun((source, target) =>
            //{
            //    if (source.Data == null)
            //        source.Data = source == source.Root ? new TokenRoot(source.Raw) : new Token(source.Raw);

            //    if (target.Data == null)
            //        target.Data = new Token(target.Raw);

            //    var r = ((Token)source.Data) + ((Token)target.Data);
            //    tokens.Add(source);
            //    tokens.Add(target);

            //    //if (source.Data == null)
            //    //    source.Data = new ItemDeserializer(source.Raw);

            //    //if (target.Data == null)
            //    //    target.Data = new ItemDeserializer(target.Raw);

            //    //var r = ((ItemDeserializer)source.Data) + ((ItemDeserializer)target.Data);
            //    //tokens.Add(source);
            //    //tokens.Add(target);
            //});

            //var root = tokens.OfType<TokenRoot>().First();
            //TestToken(root, "A", null, "123", false, "A.123", root);
            //TestToken(A["B"], "B", "valueB", null, true, "B: valueB", root);
            //TestToken(A["B"]["C"], "C", "valueC", null, true, "C: valueC", root);

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
            //A = A + B + C;
            //List<Token> tokens = new List<Token>();
            //A.ReRun((source, target) =>
            //{
            //    if (source.Data == null)
            //        source.Data = source == source.Root ? new TokenRoot(source.Raw) : new Token(source.Raw);

            //    if (target.Data == null)
            //        target.Data = new Token(target.Raw);

            //    var r = ((Token)source.Data) + ((Token)target.Data);
            //    tokens.Add(source);
            //    tokens.Add(target);
            //});

            //var root = tokens.OfType<TokenRoot>().First();
            //TestToken(root, "A", null, "123", false, "A.123", root);
            //TestToken(A["B"], "B", "valueB", null, true, "B: valueB", root);
            //TestToken(A["C"], "C", "valueC", null, true, "C: valueC", root);
        }

        [Fact]
        public void TokenTest_ExistsObject_ExecutePropertyMemberInObject()
        {
            var circularA = new CircularEntity("A");
            var root = new TokenComplex<CircularEntity>(circularA)
                + new Member("Name", "New Name") 
                + new Member("Name", "New Name 2");
            Assert.Equal("New name 2", root.Name);
        }

        [Fact]
        public void TokenTest_NonExistsObject_ExecutePropertyMemberInObject()
        {
            var root = new ItemDeserializerRoot<CircularEntity>() + new ItemDeserializer("Name: Test");
            Assert.Equal("Test", root.Entity.Name);
        }

        [Fact]
        public void TokenTest_NonExistsObject_ExecutePrivateFieldMemberInObject()
        {
            var root = new ItemDeserializerRoot<CircularEntity>() +
                        (
                            new ItemDeserializer("children.1") +
                                (
                                    new ItemDeserializer("[0].2") +
                                        new ItemDeserializer("Name: Name1") +
                                        new ItemDeserializer("Name: Name1 (last)")
                                ) +
                                (
                                    new ItemDeserializer("[1].3") +
                                        new ItemDeserializer("Name: Name2")
                                ) +
                                (
                                    new ItemDeserializer("[2].4") +
                                        new ItemDeserializer("Name: Name3")
                                )
                        ) +
                        new ItemDeserializer("Name: Root");
            Assert.Equal("Test", root.Entity.Name);
        }

        private void TestToken(Token token, string key, string value, string complexId, bool isPrimitive,  string raw, Token root)
        {
            Assert.Equal(key, token.Name);
            Assert.Equal(complexId, token.EntityId);
            Assert.Equal(isPrimitive, token.IsPrimitive);
            Assert.Equal(value, token.Value);
            Assert.Equal(raw, token.Raw);
            Assert.Equal(root, token.Root);
        }
    }
}

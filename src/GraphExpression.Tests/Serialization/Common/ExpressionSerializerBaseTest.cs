using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GraphExpression.Serialization;

namespace GraphExpression.Tests.Serialization
{
    public class ExpressionSerializerBaseTest
    {
        [Fact]
        public void CreateExpressionCircularValidName_RemoveQuotesWhenValidIdentifier_DEFAULT_ShouldNotEnclosed()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var output = expression.DefaultSerializer.Serialize();
            Assert.Equal(2, expression.Count);
            Assert.Equal("A + B", output);
        }

        [Fact]
        public void CreateExpressionCircularInvalidName_RemoveQuotesWhenValidIdentifier_DEFAULT_ShouldEnclosed()
        {
            var A = new CircularEntity("A-B"); // "-" is a special char, can throw in deserializer
            var B = new CircularEntity("if"); // special c# name, can throw in deserializer
            
            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var serializer = expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
            var output = serializer.Serialize();

            Assert.Equal(2, expression.Count);
            Assert.Equal("\"A-B\" + \"if\"", output);
        }

        [Fact]
        public void CreateExpressionCircularValidName_RemoveQuotesWhenValidIdentifier_FALSE_ShouldEnclosed()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var serializer = expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
            serializer.RemoveQuoteWhenValidIdentified = false;
            var output = serializer.Serialize();
            Assert.Equal(2, expression.Count);
            Assert.Equal("\"A\" + \"B\"", output);
        }

        [Fact]
        public void CreateExpressionCircularInvalidName_RemoveQuotesWhenValidIdentifier_FALSE_ShouldEnclosed()
        {
            var A = new CircularEntity("A-B"); // "-" is a special char, can throw in deserializer
            var B = new CircularEntity("if"); // special c# name, can throw in deserializer

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var serializer = expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
            serializer.RemoveQuoteWhenValidIdentified = false;
            Assert.Equal(2, expression.Count);
            Assert.Equal("\"A-B\" + \"if\"", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplex_RemoveQuotesWhenValidIdentifier_DEFAULT_ShouldEnclosed()
        {
            var A = new
            {
                P = 'A'
            };

            var expression = A.AsExpression();
            Assert.Equal($"\"{A.GetType().Name}.{A.GetHashCode()}\" + \"@P: A\"", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplex_RemoveQuotesWhenValidIdentifier_TRUE_ShouldEnclosed()
        {
            var A = new
            {
                P = 1
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.RemoveQuoteWhenValidIdentified = true;

            // in complex version aways exists quotes, because is aways invalid
            Assert.Equal($"\"{A.GetType().Name}.{A.GetHashCode()}\" + \"@P: 1\"", expression.DefaultSerializer.Serialize());
        }


        [Fact]
        public void CreateExpressionCircular_VerifyTrimQuotesWithQuotesInStringValue_ShouldRemoveQuotes()
        {
            var A = new CircularEntity("\"value with quotes\"");

            var expression = A.AsExpression(f => f.Children);
            Assert.Single(expression);
            Assert.Equal("\\\"value with quotes\\\"", expression[0].ToString());
        }

        [Fact]
        public void CreateExpressionComplex_VerifyTrimQuotesWithQuotesInStringValue_ShouldRemoveQuotes()
        {
            var A = new
            {
                P = "\"\"value\"\""
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            var a = expression[1].ToString();
            Assert.Equal($"\"{A.GetType().Name}.{A.GetHashCode()}\" + \"@P: \\\"\\\"value\\\"\\\"\"", expression.DefaultSerializer.Serialize());
        }
    }
}

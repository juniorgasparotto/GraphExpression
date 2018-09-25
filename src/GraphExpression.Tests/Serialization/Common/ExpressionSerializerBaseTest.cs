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
        public void CreateExpressionCircularValidName_ForceQuoteEvenWhenValidIdentified_DEFAULT_ShouldNotEnclosed()
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
        public void CreateExpressionCircularInvalidName_ForceQuoteEvenWhenValidIdentified_DEFAULT_ShouldEnclosed()
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
        public void CreateExpressionCircularValidName_ForceQuoteEvenWhenValidIdentified_TRUE_ShouldEnclosedBecauseIsForced()
        {
            var A = new CircularEntity("A");
            var B = new CircularEntity("B");

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var serializer = expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
            serializer.ForceQuoteEvenWhenValidIdentified = true;
            var output = serializer.Serialize();
            Assert.Equal(2, expression.Count);
            Assert.Equal("\"A\" + \"B\"", output);
        }

        [Fact]
        public void CreateExpressionCircularInvalidName_ForceQuoteEvenWhenValidIdentified_FALSE_ShouldEnclosedBecauseIsInvalid()
        {
            var A = new CircularEntity("A-B"); // "-" is a special char, can throw in deserializer
            var B = new CircularEntity("if"); // special c# name, can throw in deserializer

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var serializer = expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
            serializer.ForceQuoteEvenWhenValidIdentified = false;
            Assert.Equal(2, expression.Count);
            Assert.Equal("\"A-B\" + \"if\"", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplex_ForceQuoteEvenWhenValidIdentified_FALSE_ShouldEnclosedBecauseComplexAwaysHasSpaceAndIsInvalidChar()
        {
            var A = new
            {
                P = 'A'
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.ForceQuoteEvenWhenValidIdentified = false;
            Assert.Equal($"\"{A.GetType().Name}.{A.GetHashCode()}\" + \"P: A\"", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplex_ForceQuoteEvenWhenValidIdentified_TRUE_ShouldEnclosed()
        {
            var A = new
            {
                P = 1
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.ForceQuoteEvenWhenValidIdentified = true;

            // in complex version aways exists quotes, because is aways invalid
            Assert.Equal($"\"{A.GetType().Name}.{A.GetHashCode()}\" + \"P: 1\"", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplex_VerifyVerbatinInExpression_ShouldSerilizeStringToVerbatin()
        {
            var A = new
            {
                P = "\"\"value\"\""
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            Assert.Equal($"\"{A.GetType().Name}.{A.GetHashCode()}\" + \"P: \\\"\\\"value\\\"\\\"\"", expression.DefaultSerializer.Serialize());
        }

        [Fact]
        public void CreateExpressionComplex_VerifyVerbatinInEntityToString_ShouldSerilizeStringWithoutVerbatin()
        {
            var A = new
            {
                P = "\"\"value\"\""
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            var output = expression[1].ToString();
            Assert.Equal("P: \"\"value\"\"", output);
        }

        [Fact]
        public void CreateExpressionCircular_EmptyString_ShouldNothingInAEntity()
        {
            var A = new CircularEntity("");
            var B = new CircularEntity("B");

            var r = A + B;
            var expression = A.AsExpression(f => f.Children);
            var serializer = expression.GetSerializer<CircularEntityExpressionSerializer<CircularEntity>>();
            serializer.ForceQuoteEvenWhenValidIdentified = true;
            var output = serializer.Serialize();
            var outputEntity = expression[0].ToString();
            Assert.Equal(2, expression.Count);
            Assert.Equal("", outputEntity);
            Assert.Equal("\"\" + \"B\"", output);
        }

        [Fact]
        public void CreateExpressionComplex_EmptyString_ShouldNothingAfterColon()
        {
            var A = new
            {
                P = ""
            };

            var expression = A.AsExpression();
            var serializer = expression.GetSerializer<ComplexEntityExpressionSerializer>();
            var output = serializer.Serialize();
            var outputEntity = expression[1].ToString();
            Assert.Equal("P: ", outputEntity);
            Assert.Equal($"\"{A.GetType().Name}.{A.GetHashCode()}\" + \"P: \"", output);
        }
    }
}

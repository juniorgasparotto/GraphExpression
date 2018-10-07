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
    public partial class DeserializationComplexPrimitivesTest : EntitiesData
    {
        [Fact]
        public void DeserializeComplex_PrimitiveLong()
        {
            long longVal = long.MaxValue;
            var expressionStr = longVal.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<long>(expressionStr);
            Assert.Equal(longVal, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveBoolean()
        {
            var expressionStr = "\"Boolean: true\"";
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<bool>(expressionStr);
            Assert.True(deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveInt16()
        {
            short val = 10;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<short>(expressionStr);
            Assert.Equal(10, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveInt32()
        {
            int val = 10;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<int>(expressionStr);
            Assert.Equal(10, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveInt64()
        {
            long val = 10;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<long>(expressionStr);
            Assert.Equal(10, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveByte()
        {
            byte val = 1;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<byte>(expressionStr);
            Assert.Equal(1, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveSByte()
        {
            sbyte val = 1;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<sbyte>(expressionStr);
            Assert.Equal(1, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveUInt16()
        {
            ushort val = 1;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<UInt16>(expressionStr);
            Assert.Equal(1, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveUInt32()
        {
            ushort val = 1;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<ushort>(expressionStr);
            Assert.Equal(1, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveUInt64()
        {
            ulong val = 1;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            ulong deserialized = deserializer.Deserialize<ulong>(expressionStr);
            Assert.Equal(val, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveFloat()
        {
            float val = 99.99F;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<float>(expressionStr);
            Assert.Equal(val, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveDecimal()
        {
            decimal val = 99.99m;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<decimal>(expressionStr);
            Assert.Equal(val, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveDouble()
        {
            double val = 99.99D;
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<double>(expressionStr);
            Assert.Equal(val, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveDateTime()
        {
            var val = new DateTime(2000, 1, 1, 1, 1, 1, 999);
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<DateTime>(expressionStr);
            Assert.Equal(val, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveDateTimeOffset()
        {
            var val = new DateTimeOffset(2000, 1, 1, 1, 1, 1, new TimeSpan());
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<DateTime>(expressionStr);
            Assert.Equal(val, deserialized);
        }

        [Fact]
        public void DeserializeComplex_PrimitiveChar()
        {
            var val = 'º';
            var expressionStr = val.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<char>(expressionStr);
            Assert.Equal(val, deserialized);
        }
    }
}

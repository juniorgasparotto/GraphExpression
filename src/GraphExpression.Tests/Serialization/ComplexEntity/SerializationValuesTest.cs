using ExpressionGraph.Serialization;
using GraphExpression.Serialization;
using System;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class SerializationValuesTest
    {
        public DateTime DateTime { get; set; } = new DateTime(2000, 01, 01, 1, 1, 1, DateTimeKind.Utc);
        public DateTime? DateTimeNullable { get; set; } = new DateTime(2000, 01, 01, 1, 1, 1, DateTimeKind.Utc);

        public int Int { get; set; } = int.MinValue;
        public int? IntNullable { get; set; } = int.MinValue;

        public long Long { get; set; } = long.MaxValue;
        public long? LongNullable { get; set; } = long.MaxValue;

        public DateTimeOffset DateTimeOffset { get; set; } = new DateTimeOffset(2000, 1, 1, 0, 0, 0, 0, new TimeSpan());
        public DateTimeOffset? DateTimeOffsetNullable { get; set; } = new DateTimeOffset(2000, 1, 1, 0, 0, 0, 0, new TimeSpan());

        public IntPtr IntPtr { get; set; } = new IntPtr(long.MaxValue);
        public IntPtr? IntPtrNullable { get; set; } = new IntPtr(long.MaxValue);

        public UIntPtr UIntPtr { get; set; } = new System.UIntPtr(ulong.MaxValue);
        public UIntPtr? UIntPtrNullable { get; set; } = new System.UIntPtr(ulong.MaxValue);

        public Byte Byte { get; set; } = Byte.MaxValue;
        public Byte? ByteNullable { get; set; } = Byte.MaxValue;

        public SByte SByte { get; set; } = SByte.MinValue;
        public SByte? SByteNullable { get; set; } = SByte.MinValue;

        public UInt16 UInt16 { get; set; } = UInt16.MaxValue;
        public UInt16? UInt16Nullable { get; set; } = UInt16.MaxValue;

        public UInt32 UInt32 { get; set; } = UInt32.MinValue;
        public UInt32? UInt32Nullable { get; set; } = UInt32.MinValue;

        public UInt64 UInt64 { get; set; } = UInt64.MaxValue;
        public UInt64? UInt64Nullable { get; set; } = UInt64.MaxValue;

        public Int16 Int16 { get; set; } = Int16.MinValue;
        public Int16? Int16Nullable { get; set; } = Int16.MinValue;

        public Boolean Boolean { get; set; } = true;
        public Boolean? BooleanNullable { get; set; } = false;

        public Decimal Decimal { get; set; } = Decimal.MinValue;
        public Decimal? DecimalNullable { get; set; } = Decimal.MinValue/199;

        public Double Double { get; set; } = Double.MaxValue;
        public Double? DoubleNullable { get; set; } = Double.MaxValue / 99.999;

        public Single Single { get; set; } = Single.MaxValue;
        public Single? SingleNullable { get; set; } = Single.MaxValue;

        public Char Char { get; set; } = 'Ê';
        public Char? CharNullable { get; set; } = 'Ê';

        #region DateTime

        [Fact]
        public void CheckValue_DateTime()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DateTime"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@DateTime: \"2000-01-01T01:01:01.000+00:00\"}", result);
        }

        [Fact]
        public void CheckValue_DateTimeNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DateTimeNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.DateTimeNullable: \"2000-01-01T01:01:01.000+00:00\"}", result);

            serialization.ShowType = ShowTypeOptions.None;
            DateTimeNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DateTimeNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@DateTimeNullable: null}", result);
        }

        #endregion

        #region int

        [Fact]
        public void CheckValue_Int()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Int"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Int: -2147483648}", result);
        }

        [Fact]
        public void CheckValue_IntNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("IntNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.IntNullable: -2147483648}", result);

            serialization.ShowType = ShowTypeOptions.None;
            IntNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("IntNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@IntNullable: null}", result);
        }

        #endregion

        #region long

        [Fact]
        public void CheckValue_Long()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Long"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Long: 9223372036854775807}", result);
        }

        [Fact]
        public void CheckValue_LongNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("LongNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.LongNullable: 9223372036854775807}", result);

            serialization.ShowType = ShowTypeOptions.None;
            LongNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("LongNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@LongNullable: null}", result);
        }

        #endregion

        #region DateTimeOffset

        [Fact]
        public void CheckValue_DateTimeOffset()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DateTimeOffset"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@DateTimeOffset: \"2000-01-01T00:00:00.000+00:00\"}", result);
        }

        [Fact]
        public void CheckValue_DateTimeOffsetNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DateTimeOffsetNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.DateTimeOffsetNullable: \"2000-01-01T00:00:00.000+00:00\"}", result);

            serialization.ShowType = ShowTypeOptions.None;
            DateTimeOffsetNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DateTimeOffsetNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@DateTimeOffsetNullable: null}", result);
        }

        #endregion

        #region IntPtr

        [Fact]
        public void CheckValue_IntPtr()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("IntPtr"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@IntPtr: 9223372036854775807}", result);
        }

        [Fact]
        public void CheckValue_IntPtrNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("IntPtrNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.IntPtrNullable: 9223372036854775807}", result);

            serialization.ShowType = ShowTypeOptions.None;
            IntPtrNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("IntPtrNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@IntPtrNullable: null}", result);
        }

        #endregion

        #region UIntPtr

        [Fact]
        public void CheckValue_UIntPtr()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UIntPtr"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@UIntPtr: 18446744073709551615}", result);
        }

        [Fact]
        public void CheckValue_UIntPtrNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UIntPtrNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.UIntPtrNullable: 18446744073709551615}", result);

            serialization.ShowType = ShowTypeOptions.None;
            UIntPtrNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UIntPtrNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@UIntPtrNullable: null}", result);
        }

        #endregion

        #region Byte

        [Fact]
        public void CheckValue_Byte()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Byte"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Byte: 255}", result);
        }

        [Fact]
        public void CheckValue_ByteNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("ByteNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.ByteNullable: 255}", result);

            serialization.ShowType = ShowTypeOptions.None;
            ByteNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("ByteNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@ByteNullable: null}", result);
        }

        #endregion

        #region SByte

        [Fact]
        public void CheckValue_SByte()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("SByte"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@SByte: -128}", result);
        }

        [Fact]
        public void CheckValue_SByteNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("SByteNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.SByteNullable: -128}", result);

            serialization.ShowType = ShowTypeOptions.None;
            SByteNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("SByteNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@SByteNullable: null}", result);
        }

        #endregion

        #region UInt16

        [Fact]
        public void CheckValue_UInt16()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt16"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@UInt16: 65535}", result);
        }

        [Fact]
        public void CheckValue_UInt16Nullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt16Nullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.UInt16Nullable: 65535}", result);

            serialization.ShowType = ShowTypeOptions.None;
            UInt16Nullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt16Nullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@UInt16Nullable: null}", result);
        }

        #endregion

        #region UInt32

        [Fact]
        public void CheckValue_UInt32()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt32"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@UInt32: 0}", result);
        }

        [Fact]
        public void CheckValue_UInt32Nullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt32Nullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.UInt32Nullable: 0}", result);

            serialization.ShowType = ShowTypeOptions.None;
            UInt32Nullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt32Nullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@UInt32Nullable: null}", result);
        }

        #endregion

        #region UInt64

        [Fact]
        public void CheckValue_UInt64()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt64"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@UInt64: 18446744073709551615}", result);
        }

        [Fact]
        public void CheckValue_UInt64Nullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt64Nullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.UInt64Nullable: 18446744073709551615}", result);

            serialization.ShowType = ShowTypeOptions.None;
            UInt64Nullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("UInt64Nullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@UInt64Nullable: null}", result);
        }

        #endregion

        #region Int16

        [Fact]
        public void CheckValue_Int16()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Int16"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Int16: -32768}", result);
        }

        [Fact]
        public void CheckValue_Int16Nullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Int16Nullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.Int16Nullable: -32768}", result);

            serialization.ShowType = ShowTypeOptions.None;
            Int16Nullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Int16Nullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@Int16Nullable: null}", result);
        }

        #endregion

        #region Boolean

        [Fact]
        public void CheckValue_Boolean()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Boolean"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Boolean: true}", result);
        }

        [Fact]
        public void CheckValue_BooleanNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("BooleanNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.BooleanNullable: false}", result);

            serialization.ShowType = ShowTypeOptions.None;
            BooleanNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("BooleanNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@BooleanNullable: null}", result);
        }

        #endregion

        #region Decimal

        [Fact]
        public void CheckValue_Decimal()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Decimal"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Decimal: -79228162514264337593543950335}", result);
        }

        [Fact]
        public void CheckValue_DecimalNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DecimalNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.DecimalNullable: -398131469920926319565547489.12}", result);

            serialization.ShowType = ShowTypeOptions.None;
            DecimalNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("DecimalNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@DecimalNullable: null}", result);
        }

        #endregion

        #region SingleNullable

        [Fact]
        public void CheckValue_Single()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Single"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Single: 3.40282346638529E+38}", result);
        }

        [Fact]
        public void CheckValue_SingleNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("SingleNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.SingleNullable: 3.40282346638529E+38}", result);

            serialization.ShowType = ShowTypeOptions.None;
            SingleNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("SingleNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@SingleNullable: null}", result);
        }

        #endregion

        #region Char

        [Fact]
        public void CheckValue_Char()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("Char"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Char: \"Ê\"}", result);
        }

        [Fact]
        public void CheckValue_CharNullable()
        {
            var expression = Utils.CreateEmptyExpression();
            var serialization = Utils.GetSerialization(expression);
            serialization.ShowType = ShowTypeOptions.TypeName;

            var fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("CharNullable"));
            var result = fieldEntity.ToString();
            Assert.Equal("{@Nullable`1.CharNullable: \"Ê\"}", result);

            serialization.ShowType = ShowTypeOptions.None;
            CharNullable = null;
            fieldEntity = new PropertyEntity(expression, this, GetPropertyByName("CharNullable"));
            result = fieldEntity.ToString();
            Assert.Equal("{@CharNullable: null}", result);
        }

        #endregion

        private System.Reflection.PropertyInfo GetPropertyByName(string name)
        {
            return this.GetType().GetProperties().Where(p => p.Name == name).First();
        }
    }
}

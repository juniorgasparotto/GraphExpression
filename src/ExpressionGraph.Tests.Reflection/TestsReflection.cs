using ExpressionGraph.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpressionGraph;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.CSharp.RuntimeBinder;

namespace ExpressionGraph.Tests.Console
{
    [TestClass]
    public class TestsReflection
    {
        private bool failWithException;

        public TestsReflection()
        {
            this.failWithException = false;
        }

        public TestsReflection(bool failWithException)
        {
            this.failWithException = failWithException;
        }

        [TestMethod]
        public void TestReflection_Array()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();
            
            var expressionResult1 = testClass.FieldPublicArrayUni.AsReflection().Query().ToString();
            expectedTest = "{System.String[]_0} + [0]: \"[0]\" + [1]: \"[1]\"";
            json = ToJson(testClass.FieldPublicArrayUni);
            AssertTrue(expressionResult1 == expectedTest, "ArrayUni");

            var expressionResult2 = testClass.FieldPublicArrayTwo.AsReflection().Query().ToString();
            expectedTest = "{System.String[,]_0} + [0,0]: \"[0, 0]\" + [0,1]: \"[0, 1]\" + [1,0]: \"[1, 0]\" + [1,1]: \"[1, 1]\"";
            json = ToJson(testClass.FieldPublicArrayTwo);
            AssertTrue(expressionResult2 == expectedTest, "ArrayTwo");

            var expressionResult3 = testClass.FieldPublicArrayThree.AsReflection().Query().ToString();
            expectedTest = "{System.String[,,]_0} + [0,0,0]: \"[0, 0, 0]\" + [0,0,1]: \"[0, 0, 1]\"";
            json = ToJson(testClass.FieldPublicArrayThree);
            AssertTrue(expressionResult3 == expectedTest, "ArrayThree");

            var expressionResult4 = testClass.FieldPublicJaggedArrayTwo.AsReflection().Query().ToString();
            expectedTest = "{System.String[][]_0} + ([0]: {System.String[]_1} + [0]: \"a\" + [1]: \"b\" + [2]: \"c\" + [3]: \"d\" + [4]: \"e\") + ([1]: {System.String[]_7} + [0]: \"a1\" + [1]: \"b1\" + [2]: \"c1\" + [3]: \"d1\")";
            json = ToJson(testClass.FieldPublicJaggedArrayTwo);
            AssertTrue(expressionResult4 == expectedTest, "JaggedArrayTwo");

            var expressionResult5 = testClass.FieldPublicJaggedArrayThree.AsReflection().Query().ToString();
            expectedTest = "{System.String[][][]_0} + ([0]: {System.String[][]_1} + ([0]: {System.String[]_2} + [0]: \"[0][0][0]\" + [1]: \"[0][0][1]\"))";
            json = ToJson(testClass.FieldPublicJaggedArrayThree);
            AssertTrue(expressionResult5 == expectedTest, "JaggedArrayThree");

            var expressionResult6 = testClass.FieldPublicArrayListNonGeneric.AsReflection().Query().ToString();
            expectedTest = "{System.Collections.ArrayList_0} + [0]: 1 + [1]: \"a\" + [2]: 10.0 + [3]: \"2000-01-01T00:00:00.000-02:00\"";
            json = ToJson(testClass.FieldPublicArrayListNonGeneric);
            AssertTrue(expressionResult6 == expectedTest, "ArrayList");

            var expressionResult7 = testClass.FieldPublicBitArray.AsReflection().Query().ToString();
            expectedTest = "{System.Collections.BitArray_0} + [0]: false + [1]: false + [2]: true";
            json = ToJson(testClass.FieldPublicBitArray);
            AssertTrue(expressionResult7 == expectedTest, "BitArray");

            // is normal "int[,][]" is changed by "int[][,]". The .net make it.
            // ** Newtonsoft Json have different return expectations **
            var expressionResult8 = testClass.FieldPublicMixedArrayAndJagged.AsReflection().Query().ToString();
            expectedTest = "{System.Int32[,][]_0} + ([0]: {System.Int32[,]_1} + [0,0]: 1 + [0,1]: 3 + [1,0]: 5 + [1,1]: 7) + ([1]: {System.Int32[,]_6} + [0,0]: 0 + [0,1]: 2 + [1,0]: 4 + [1,1]: 6 + [2,0]: 8 + [2,1]: 10) + ([2]: {System.Int32[,]_13} + [0,0]: 11 + [0,1]: 22 + [1,0]: 99 + [1,1]: 88 + [2,0]: 0 + [2,1]: 9)";
            json = ToJson(testClass.FieldPublicMixedArrayAndJagged);
            AssertTrue(expressionResult8 == expectedTest, "MixedArrayAndJagged");
        }

        [TestMethod]
        public void TestReflection_Primitives()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();

            #region Types of Keywords

            var expressionResult1 = Extensions.AsReflection(testClass.FieldPublicDynamic).Query().ToString();
            expectedTest = "{<>f__AnonymousType1<System.String, System.Int32, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, System.String, <>f__AnonymousType0<System.Int32, System.Int32, System.Int32>>_0} + PropPublic1: \"A\" + PropPublic2: 1 + PropPublic3: \"B\" + PropPublic4: \"B\" + PropPublic5: \"B\" + PropPublic6: \"B\" + PropPublic7: \"B\" + PropPublic8: \"B\" + PropPublic9: \"B\" + PropPublic10: \"B\" + PropPublic11: \"B\" + (PropPublic12: {<>f__AnonymousType0<System.Int32, System.Int32, System.Int32>_12} + PropSubPublic1: 0 + PropSubPublic2: 1 + PropSubPublic3: 2)";
            json = ToJson(testClass.FieldPublicDynamic);
            AssertTrue(expressionResult1 == expectedTest, "Dynamic");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult2 = GetString(testClass.FieldPublicObject);
            expectedTest = "{System.Text.StringBuilder_0}";
            json = ToJson(testClass.FieldPublicObject);
            AssertTrue(expressionResult2 == expectedTest, "Object");

            var expressionResult3 = GetString(testClass.FieldPublicInt32);
            expectedTest = "2147483647";
            json = ToJson(testClass.FieldPublicInt32);
            AssertTrue(expressionResult3 == expectedTest, "Int32");

            var expressionResult4 = GetString(testClass.FieldPublicInt64);
            expectedTest = "9223372036854775807";
            json = ToJson(testClass.FieldPublicInt64);
            AssertTrue(expressionResult4 == expectedTest, "Int64");

            var expressionResult5 = GetString(testClass.FieldPublicULong);
            expectedTest = "18446744073709551615";
            json = ToJson(testClass.FieldPublicULong);
            AssertTrue(expressionResult5 == expectedTest, "ULong");

            var expressionResult6 = GetString(testClass.FieldPublicUInt);
            expectedTest = "4294967295";
            json = ToJson(testClass.FieldPublicUInt);
            AssertTrue(expressionResult6 == expectedTest, "UInt");

            var expressionResult7 = GetString(testClass.FieldPublicDecimal);
            expectedTest = "100000.999999";
            json = ToJson(testClass.FieldPublicDecimal);
            AssertTrue(expressionResult7 == expectedTest, "Decimal");

            var expressionResult8 = GetString(testClass.FieldPublicDouble);
            expectedTest = "100000.999999";
            json = ToJson(testClass.FieldPublicDouble);
            AssertTrue(expressionResult8 == expectedTest, "Double");

            var expressionResult9 = GetString(testClass.FieldPublicChar);
            expectedTest = "\"A\"";
            json = ToJson(testClass.FieldPublicChar);
            AssertTrue(expressionResult9 == expectedTest, "Char");

            var expressionResult10 = GetString(testClass.FieldPublicByte);
            expectedTest = "255";
            json = ToJson(testClass.FieldPublicByte);
            AssertTrue(expressionResult10 == expectedTest, "Byte");

            var expressionResult11 = GetString(testClass.FieldPublicBoolean);
            expectedTest = "true";
            json = ToJson(testClass.FieldPublicBoolean);
            AssertTrue(expressionResult11 == expectedTest, "Boolean");

            var expressionResult12 = GetString(testClass.FieldPublicSByte);
            expectedTest = "127";
            json = ToJson(testClass.FieldPublicSByte);
            AssertTrue(expressionResult12 == expectedTest, "SByte");

            var expressionResult13 = GetString(testClass.FieldPublicShort);
            expectedTest = "32767";
            json = ToJson(testClass.FieldPublicShort);
            AssertTrue(expressionResult13 == expectedTest, "Short");

            var expressionResult14 = GetString(testClass.FieldPublicUShort);
            expectedTest = "65535";
            json = ToJson(testClass.FieldPublicUShort);
            AssertTrue(expressionResult14 == expectedTest, "UShort");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult15 = GetString(testClass.FieldPublicFloat);
            expectedTest = "100000.671875";
            json = ToJson(testClass.FieldPublicFloat);
            AssertTrue(expressionResult15 == expectedTest, "Float");

            var expressionResult16 = GetString(testClass.FieldPublicInt32Nullable);
            expectedTest = "2147483647";
            json = ToJson(testClass.FieldPublicInt32Nullable);
            AssertTrue(expressionResult16 == expectedTest, "Int32Nullable");

            var expressionResult17 = GetString(testClass.FieldPublicInt64Nullable);
            expectedTest = "2";
            json = ToJson(testClass.FieldPublicInt64Nullable);
            AssertTrue(expressionResult17 == expectedTest, "Int64Nullable");

            var expressionResult18 = GetString(testClass.FieldPublicULongNullable);
            expectedTest = "18446744073709551615";
            json = ToJson(testClass.FieldPublicULongNullable);
            AssertTrue(expressionResult18 == expectedTest, "ULongNullable");

            var expressionResult19 = GetString(testClass.FieldPublicUIntNullable);
            expectedTest = "4294967295";
            json = ToJson(testClass.FieldPublicUIntNullable);
            AssertTrue(expressionResult19 == expectedTest, "UIntNullable");

            var expressionResult20 = GetString(testClass.FieldPublicDecimalNullable);
            expectedTest = "100000.999999";
            json = ToJson(testClass.FieldPublicDecimalNullable);
            AssertTrue(expressionResult20 == expectedTest, "DecimalNullable");

            var expressionResult21 = GetString(testClass.FieldPublicDoubleNullable);
            expectedTest = "100000.999999";
            json = ToJson(testClass.FieldPublicDoubleNullable);
            AssertTrue(expressionResult21 == expectedTest, "DoubleNullable");

            var expressionResult22 = GetString(testClass.FieldPublicCharNullable);
            expectedTest = "\"A\"";
            json = ToJson(testClass.FieldPublicCharNullable);
            AssertTrue(expressionResult22 == expectedTest, "CharNullable");
            
            var expressionResult23 = GetString(testClass.FieldPublicByteNullable);
            expectedTest = "255";
            json = ToJson(testClass.FieldPublicByteNullable);
            AssertTrue(expressionResult23 == expectedTest, "ByteNullable");

            var expressionResult24 = GetString(testClass.FieldPublicBooleanNullable);
            expectedTest = "true";
            json = ToJson(testClass.FieldPublicBooleanNullable);
            AssertTrue(expressionResult24 == expectedTest, "BooleanNullable");

            var expressionResult25 = GetString(testClass.FieldPublicSByteNullable);
            expectedTest = "127";
            json = ToJson(testClass.FieldPublicSByteNullable);
            AssertTrue(expressionResult25 == expectedTest, "SByteNullable");

            var expressionResult26 = GetString(testClass.FieldPublicShortNullable);
            expectedTest = "32767";
            json = ToJson(testClass.FieldPublicShortNullable);
            AssertTrue(expressionResult26 == expectedTest, "ShortNullable");

            var expressionResult27 = GetString(testClass.FieldPublicUShortNullable);
            expectedTest = "65535";
            json = ToJson(testClass.FieldPublicUShortNullable);
            AssertTrue(expressionResult27 == expectedTest, "UShortNullable");
            
            var expressionResult28 = GetString(testClass.FieldPublicFloatNullable);
            expectedTest = "100000.671875";
            json = ToJson(testClass.FieldPublicFloatNullable);
            AssertTrue(expressionResult28 == expectedTest, "FloatNullable");

            #endregion

            #region System

            // ** Newtonsoft Json have different return expectations **
            var expressionResult29 = GetString(testClass.FieldPublicSingle);
            expectedTest = "3.40282346638529E+38";
            json = ToJson(testClass.FieldPublicSingle);
            AssertTrue(expressionResult29 == expectedTest, "Single");

            // ** Newtonsoft Json have different return expectations **
            // ** Is different because Newtonsoft use "System.Runtime.Serialization.SerializationInfo" to get properties ** 
            var expressionResult30 = GetString(testClass.FieldPublicException);
            expectedTest = "{System.Exception_0}";
            json = ToJson(testClass.FieldPublicException);
            AssertTrue(expressionResult30 == expectedTest, "Exception");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult31 = GetString(testClass.FieldPublicEnumNonGeneric);
            expectedTest = "ValueA";
            json = ToJson(testClass.FieldPublicEnumNonGeneric);
            AssertTrue(expressionResult31 == expectedTest, "EnumNonGeneric");

            var expressionResult32 = GetString(testClass.FieldPublicAction);
            expectedTest = "{System.Action_0}";
            json = ToJson(testClass.FieldPublicAction);
            AssertTrue(expressionResult32 == expectedTest, "Action");

            var expressionResult33 = GetString(testClass.FieldPublicAction2);
            expectedTest = "{System.Action<System.String, System.Int32>_0}";
            json = ToJson(testClass.FieldPublicAction);
            AssertTrue(expressionResult33 == expectedTest, "Action<string, int>");

            var expressionResult34 = GetString(testClass.FieldPublicFunc);
            expectedTest = "{System.Func<System.Boolean>_0}";
            json = ToJson(testClass.FieldPublicFunc);
            AssertTrue(expressionResult34 == expectedTest, "Func");

            var expressionResult35 = GetString(testClass.FieldPublicFunc2);
            expectedTest = "{System.Func<System.String, System.Int32, System.Boolean>_0}";
            json = ToJson(testClass.FieldPublicFunc);
            AssertTrue(expressionResult35 == expectedTest, "Func<string, int, bool>");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult36 = GetString(testClass.FieldPublicDateTime);
            expectedTest = "\"2000-01-01T01:01:01.000-02:00\"";
            json = ToJson(testClass.FieldPublicDateTime);
            AssertTrue(expressionResult36 == expectedTest, "DateTime)");

            var expressionResult37 = GetString(testClass.FieldPublicTimeSpan);
            expectedTest = "{System.TimeSpan_0}";
            json = ToJson(testClass.FieldPublicTimeSpan);
            AssertTrue(expressionResult37 == expectedTest, "TimeSpan");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult38 = GetString(testClass.FieldPublicEnumDateTimeKind);
            expectedTest = "Local";
            json = ToJson(testClass.FieldPublicEnumDateTimeKind);
            AssertTrue(expressionResult38 == expectedTest, "EnumDateTimeKind");

            var expressionResult39 = GetString(testClass.FieldPublicDateTimeOffset);
            expectedTest = "\"2008-05-01T08:06:32.545+01:00\"";
            json = ToJson(testClass.FieldPublicDateTimeOffset);
            AssertTrue(expressionResult39 == expectedTest, "DateTimeOffset");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult40 = GetString(testClass.FieldPublicIntPtr);
            expectedTest = "100";
            json = ToJson(testClass.FieldPublicIntPtr);
            AssertTrue(expressionResult40 == expectedTest, "IntPtr");

            var expressionResult41 = GetString(testClass.FieldPublicTimeZone);
            expectedTest = "{System.CurrentSystemTimeZone_0}";
            json = ToJson(testClass.FieldPublicTimeZone);
            AssertTrue(expressionResult41 == expectedTest, "TimeZone");

            // ** Newtonsoft Json have different return expectations **
            // ** Is different because Newtonsoft use "System.Runtime.Serialization.SerializationInfo" to get properties ** 
            var expressionResult42 = GetString(testClass.FieldPublicTimeZoneInfo);
            expectedTest = "{System.TimeZoneInfo_0}";
            json = ToJson(testClass.FieldPublicTimeZoneInfo);
            AssertTrue(expressionResult42 == expectedTest, "TimeZoneInfo");

            var expressionResult43 = GetString(testClass.FieldPublicTuple);
            expectedTest = "{System.Tuple<System.String, System.Int32, System.Decimal>_0}";
            json = ToJson(testClass.FieldPublicTuple);
            AssertTrue(expressionResult43 == expectedTest, "Tuple");

            // ** Newtonsoft Json have different return expectations **
            // ** ERROR 
            var expressionResult44 = GetString(testClass.FieldPublicType);
            expectedTest = "{System.RuntimeType_0}";
            json = ToJson(testClass.FieldPublicType);
            AssertTrue(expressionResult44 == expectedTest, "Type");

            var expressionResult45 = GetString(testClass.FieldPublicUIntPtr);
            expectedTest = "100";
            json = ToJson(testClass.FieldPublicUIntPtr);
            AssertTrue(expressionResult45 == expectedTest, "UIntPtr");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult46 = GetString(testClass.FieldPublicUri);
            expectedTest = "{System.Uri_0}";
            json = ToJson(testClass.FieldPublicUri);
            AssertTrue(expressionResult46 == expectedTest, "Uri");

            var expressionResult47 = GetString(testClass.FieldPublicVersion);
            expectedTest = "{System.Version_0}";
            json = ToJson(testClass.FieldPublicVersion);
            AssertTrue(expressionResult47 == expectedTest, "Version");

            var expressionResult48 = GetString(testClass.FieldPublicGuid, true);
            expectedTest = "{System.Guid_0}";
            json = ToJson(testClass.FieldPublicGuid);
            AssertTrue(expressionResult48 == expectedTest, "Guid");

            #endregion
        }

        [TestMethod]
        public void TestReflection_Custom()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();

            var expressionResult1 = GetString(testClass.FieldPublicMyCollectionPublicGetEnumerator);
            expectedTest = "{MyCollectionPublicGetEnumerator_0} + [0]: \"a\" + [1]: \"b\" + [2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionPublicGetEnumerator);
            AssertTrue(expressionResult1 == expectedTest, "MyCollectionPublicGetEnumerator");

            var expressionResult2 = GetString(testClass.FieldPublicMyCollectionInheritsPublicGetEnumerator);
            expectedTest = "{MyCollectionInheritsPublicGetEnumerator_0} + [0]: \"a\" + [1]: \"b\" + [2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionInheritsPublicGetEnumerator);
            AssertTrue(expressionResult2 == expectedTest, "MyCollectionInheritsPublicGetEnumerator");

            var expressionResult3 = GetString(testClass.FieldPublicMyCollectionExplicitGetEnumerator);
            expectedTest = "{MyCollectionExplicitGetEnumerator_0} + [0]: \"a\" + [1]: \"b\" + [2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionExplicitGetEnumerator);
            AssertTrue(expressionResult3 == expectedTest, "MyCollectionExplicitGetEnumerator");

            var expressionResult4 = GetString(testClass.FieldPublicMyCollectionInheritsExplicitGetEnumerator);
            expectedTest = "{MyCollectionInheritsExplicitGetEnumerator_0} + [0]: \"a\" + [1]: \"b\" + [2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionInheritsExplicitGetEnumerator);
            AssertTrue(expressionResult4 == expectedTest, "MyCollectionInheritsExplicitGetEnumerator");

            var expressionResult5 = GetString(testClass.FieldPublicMyCollectionInheritsTooIEnumerable);
            expectedTest = "{MyCollectionInheritsTooIEnumerable_0} + [0]: \"A\" + [1]: \"B\" + [2]: \"C\"";
            json = ToJson(testClass.FieldPublicMyCollectionInheritsTooIEnumerable);
            AssertTrue(expressionResult5 == expectedTest, "FieldPublicMyCollectionInheritsTooIEnumerable");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult6 = GetString(testClass.FieldPublicEnumSpecific);
            expectedTest = "ValueB";
            json = ToJson(testClass.FieldPublicEnumSpecific);
            AssertTrue(expressionResult6 == expectedTest, "EnumSpecific");

            // ** Newtonsoft Json throw exception with delegates **
            var expressionResult7 = GetString(testClass.MyDelegate);
            expectedTest = "{DelegateTest_0}";
            //json = ToJson(testClass.MyDelegate);
            AssertTrue(expressionResult7 == expectedTest, "Func<string, int, bool>");

            var expressionResult8 = GetString(testClass.EmptyClass);
            expectedTest = "{EmptyClass_0}";
            json = ToJson(testClass.EmptyClass);
            AssertTrue(expressionResult8 == expectedTest, "Func<string, int, bool>");

            var expressionResult9 = GetString(testClass.StructGeneric);
            expectedTest = "{ThreeTuple<System.Int32>_0}";
            json = ToJson(testClass.StructGeneric);
            AssertTrue(expressionResult9 == expectedTest, "StructGeneric");

            var expressionResult10 = GetString(testClass.StructGenericNullable);
            expectedTest = "{ThreeTuple<System.Int32>_0}";
            json = ToJson(testClass.StructGenericNullable);
            AssertTrue(expressionResult10 == expectedTest, "StructGenericNullable");

            var expressionResult11 = GetString(testClass.FieldPublicNullable);
            expectedTest = "{ThreeTuple<System.Int32>_0}";
            json = ToJson(testClass.StructGenericNullable);
            AssertTrue(expressionResult11 == expectedTest, "Nullable");
        }

        [TestMethod]
        public void TestReflection_Lists()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();

            var expressionResult1 = GetString(testClass.FieldPublicDictionary);
            expectedTest = "{System.Collections.Generic.Dictionary<System.String, System.String>_0} + [\"Key1\"]: \"Value1\" + [\"Key2\"]: \"Value2\" + [\"Key3\"]: \"Value3\" + [\"Key4\"]: \"Value4\"";
            json = ToJson(testClass.FieldPublicDictionary);
            AssertTrue(expressionResult1 == expectedTest, "Dictionary");

            var expressionResult2 = GetString(testClass.FieldPublicList);
            expectedTest = "{System.Collections.Generic.List<System.Int32>_0} + [0]: 0 + [1]: 1 + [2]: 2";
            json = ToJson(testClass.FieldPublicList);
            AssertTrue(expressionResult2 == expectedTest, "List<int>");

            var expressionResult3 = GetString(testClass.FieldPublicQueue);
            expectedTest = "{System.Collections.Generic.Queue<System.Int32>_0} + [0]: 10 + [1]: 11 + [2]: 12";
            json = ToJson(testClass.FieldPublicQueue);
            AssertTrue(expressionResult3 == expectedTest, "Queue<int>");

            var expressionResult4 = GetString(testClass.FieldPublicHashSet);
            expectedTest = "{System.Collections.Generic.HashSet<System.String>_0} + [0]: \"HashSet1\" + [1]: \"HashSet2\"";
            json = ToJson(testClass.FieldPublicHashSet);
            AssertTrue(expressionResult4 == expectedTest, "HashSet<string>");

            var expressionResult5 = GetString(testClass.FieldPublicSortedSet);
            expectedTest = "{System.Collections.Generic.SortedSet<System.String>_0} + [0]: \"SortedSet1\" + [1]: \"SortedSet2\" + [2]: \"SortedSet3\"";
            json = ToJson(testClass.FieldPublicSortedSet);
            AssertTrue(expressionResult5 == expectedTest, "SortedSet<string>");

            var expressionResult6 = GetString(testClass.FieldPublicStack);
            expectedTest = "{System.Collections.Generic.Stack<System.String>_0} + [0]: \"Stack3\" + [1]: \"Stack2\" + [2]: \"Stack1\"";
            json = ToJson(testClass.FieldPublicStack);
            AssertTrue(expressionResult6 == expectedTest, "Stack<string>");

            var expressionResult7 = GetString(testClass.FieldPublicLinkedList);
            expectedTest = "{System.Collections.Generic.LinkedList<System.String>_0} + [0]: \"LinkedList1\" + [1]: \"LinkedList1.1\" + [2]: \"LinkedList2\"";
            json = ToJson(testClass.FieldPublicLinkedList);
            AssertTrue(expressionResult7 == expectedTest, "LinkedList<string>");

            var expressionResult8 = GetString(testClass.FieldPublicObservableCollection);
            expectedTest = "{System.Collections.ObjectModel.ObservableCollection<System.String>_0} + [0]: \"ObservableCollection1\" + [1]: \"ObservableCollection2\"";
            json = ToJson(testClass.FieldPublicObservableCollection);
            AssertTrue(expressionResult8 == expectedTest, "ObservableCollection<string>");

            var expressionResult9 = GetString(testClass.FieldPublicKeyedCollection);
            expectedTest = "{MyDataKeyedCollection_0} + ([0]: {MyData_1} + Id: 0 + Data: \"data1\") + ([1]: {MyData_4} + Id: 1 + Data: \"data2\")";
            json = ToJson(testClass.FieldPublicKeyedCollection);
            AssertTrue(expressionResult9 == expectedTest, "KeyedCollection<int, MyData>");

            var expressionResult10 = GetString(testClass.FieldPublicReadOnlyCollection);
            expectedTest = "{System.Collections.ObjectModel.ReadOnlyCollection<System.String>_0} + [0]: \"list1\" + [1]: \"list2\" + [2]: \"list3\"";
            json = ToJson(testClass.FieldPublicReadOnlyCollection);
            AssertTrue(expressionResult10 == expectedTest, "ReadOnlyCollection<string>");

            var expressionResult11 = GetString(testClass.FieldPublicReadOnlyDictionary);
            expectedTest = "{System.Collections.ObjectModel.ReadOnlyDictionary<System.String, System.String>_0} + [\"Key1\"]: \"Value1\" + [\"Key2\"]: \"Value2\" + [\"Key3\"]: \"Value3\" + [\"Key4\"]: \"Value4\"";
            json = ToJson(testClass.FieldPublicReadOnlyDictionary);
            AssertTrue(expressionResult11 == expectedTest, "ReadOnlyDictionary<string, string>");

            var expressionResult12 = GetString(testClass.FieldPublicArrayListNonGeneric);
            expectedTest = "{System.Collections.ArrayList_0} + [0]: 1 + [1]: \"a\" + [2]: 10.0 + [3]: \"2000-01-01T00:00:00.000-02:00\"";
            json = ToJson(testClass.FieldPublicArrayListNonGeneric);
            AssertTrue(expressionResult12 == expectedTest, "ArrayList");

            var expressionResult13 = GetString(testClass.FieldPublicBitArray);
            expectedTest = "{System.Collections.BitArray_0} + [0]: false + [1]: false + [2]: true";
            json = ToJson(testClass.FieldPublicBitArray);
            AssertTrue(expressionResult13 == expectedTest, "BitArray");

            var expressionResult14 = GetString(testClass.FieldPublicSortedList);
            expectedTest = "{System.Collections.SortedList_0} + [\"key1\"]: 1 + [\"key2\"]: 2 + [\"key3\"]: 3 + [\"key4\"]: 4";
            json = ToJson(testClass.FieldPublicSortedList);
            AssertTrue(expressionResult14 == expectedTest, "SortedList");

            var expressionResult15 = GetString(testClass.FieldPublicHashtableNonGeneric);
            expectedTest = "{System.Collections.Hashtable_0} + [\"key2\"]: 2 + [\"key3\"]: 3 + [\"key1\"]: 1 + [\"key4\"]: 4";
            json = ToJson(testClass.FieldPublicHashtableNonGeneric);
            AssertTrue(expressionResult15 == expectedTest, "Hashtable");

            var expressionResult16 = GetString(testClass.FieldPublicQueueNonGeneric);
            expectedTest = "{System.Collections.Queue_0} + [0]: \"QueueNonGeneric1\" + [1]: \"QueueNonGeneric2\" + [2]: \"QueueNonGeneric3\"";
            json = ToJson(testClass.FieldPublicQueueNonGeneric);
            AssertTrue(expressionResult16 == expectedTest, "Queue");

            var expressionResult17 = GetString(testClass.FieldPublicStackNonGeneric);
            expectedTest = "{System.Collections.Stack_0} + [0]: \"StackNonGeneric2\" + [1]: \"StackNonGeneric1\"";
            json = ToJson(testClass.FieldPublicStackNonGeneric);
            AssertTrue(expressionResult17 == expectedTest, "Stack");

            var expressionResult18 = GetString(testClass.FieldPublicIEnumerable);
            expectedTest = "{System.Collections.SortedList_0} + [\"key1\"]: 1 + [\"key2\"]: 2 + [\"key3\"]: 3 + [\"key4\"]: 4";
            json = ToJson(testClass.FieldPublicIEnumerable);
            AssertTrue(expressionResult18 == expectedTest, "IEnumerable");

            var expressionResult19 = GetString(testClass.FieldPublicBlockingCollection);
            expectedTest = "{System.Collections.Concurrent.BlockingCollection<System.String>_0} + [0]: \"BlockingCollection1\" + [1]: \"BlockingCollection2\"";
            json = ToJson(testClass.FieldPublicBlockingCollection);
            AssertTrue(expressionResult19 == expectedTest, "BlockingCollection<string>");

            var expressionResult20 = GetString(testClass.FieldPublicConcurrentBag);
            expectedTest = "{System.Collections.Concurrent.ConcurrentBag<System.String>_0} + [0]: \"ConcurrentBag3\" + [1]: \"ConcurrentBag2\" + [2]: \"ConcurrentBag1\"";
            json = ToJson(testClass.FieldPublicConcurrentBag);
            AssertTrue(expressionResult20 == expectedTest, "ConcurrentBag<string>");

            var expressionResult21 = GetString(testClass.FieldPublicConcurrentDictionary);
            expectedTest = "{System.Collections.Concurrent.ConcurrentDictionary<System.String, System.Int32>_0} + [\"ConcurrentDictionary2\"]: 0 + [\"ConcurrentDictionary1\"]: 0";
            json = ToJson(testClass.FieldPublicConcurrentDictionary);
            AssertTrue(expressionResult21 == expectedTest, "ConcurrentDictionary<string, int>");

            var expressionResult22 = GetString(testClass.FieldPublicConcurrentQueue);
            expectedTest = "{System.Collections.Concurrent.ConcurrentQueue<System.String>_0} + [0]: \"ConcurrentQueue1\" + [1]: \"ConcurrentQueue2\"";
            json = ToJson(testClass.FieldPublicConcurrentQueue);
            AssertTrue(expressionResult22 == expectedTest, "ConcurrentQueue<string>");

            var expressionResult23 = GetString(testClass.FieldPublicConcurrentStack);
            expectedTest = "{System.Collections.Concurrent.ConcurrentStack<System.String>_0} + [0]: \"ConcurrentStack2\" + [1]: \"ConcurrentStack1\"";
            json = ToJson(testClass.FieldPublicConcurrentStack);
            AssertTrue(expressionResult23 == expectedTest, "ConcurrentStack<string>");

            var expressionResult24 = GetString(testClass.FieldPublicHybridDictionary);
            expectedTest = "{System.Collections.Specialized.HybridDictionary_0} + [\"HybridDictionaryKey1\"]: \"HybridDictionary1\" + [\"HybridDictionaryKey2\"]: \"HybridDictionary2\"";
            json = ToJson(testClass.FieldPublicHybridDictionary);
            AssertTrue(expressionResult24 == expectedTest, "HybridDictionary");

            var expressionResult25 = GetString(testClass.FieldPublicListDictionary);
            expectedTest = "{System.Collections.Specialized.ListDictionary_0} + [\"ListDictionaryKey1\"]: \"ListDictionary1\" + [\"ListDictionaryKey2\"]: \"ListDictionary2\"";
            json = ToJson(testClass.FieldPublicListDictionary);
            AssertTrue(expressionResult25 == expectedTest, "ListDictionary");

            var expressionResult26 = GetString(testClass.FieldPublicNameValueCollection);
            expectedTest = "{System.Collections.Specialized.NameValueCollection_0} + [0]: \"Key1\" + [1]: \"Key2\"";
            json = ToJson(testClass.FieldPublicNameValueCollection);
            AssertTrue(expressionResult26 == expectedTest, "NameValueCollection");

            var expressionResult27 = GetString(testClass.FieldPublicOrderedDictionary);
            expectedTest = "{System.Collections.Specialized.OrderedDictionary_0} + [1]: \"OrderedDictionary1\" + [2]: \"OrderedDictionary1\" + [\"OrderedDictionaryKey2\"]: \"OrderedDictionary2\"";
            json = ToJson(testClass.FieldPublicOrderedDictionary);
            AssertTrue(expressionResult27 == expectedTest, "OrderedDictionary");

            var expressionResult28 = GetString(testClass.FieldPublicStringCollection);
            expectedTest = "{System.Collections.Specialized.StringCollection_0} + [0]: \"StringCollection1\" + [1]: \"StringCollection2\"";
            json = ToJson(testClass.FieldPublicStringCollection);
            AssertTrue(expressionResult28 == expectedTest, "StringCollection");
        }

        [TestMethod]
        public void TestReflection_SelectPropertiesAndSelectFields()
        {
            var value = "Test";

            //  load all properties for "System.String"
            var query = value.AsReflection()
                .SelectProperties
                (
                    (obj, type) =>
                    {
                        return obj.GetType() == typeof(string);
                    },
                    (obj, type) =>
                    {
                        return obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    }
                )
                .SelectFields
                (
                    (obj, type) =>
                    {
                        return obj.GetType() == typeof(int);
                    },
                    (obj, type) =>
                    {
                        return obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
                    }
                )
                .Query();

            var expressionResult = query.ToString();

            var output = "String_0 + Chars[0]: \"T\" + Chars[1]: \"e\" + Chars[2]: \"s\" + Chars[3]: \"t\" + (Length: Int32_5 + m_value: 4 + (MaxValue: Int32_7 + m_value: 2147483647 + MaxValue: 2147483647 + (MinValue: Int32_10 + m_value: -2147483648 + MaxValue: 2147483647 + MinValue: -2147483648)) + (MinValue: Int32_14 + m_value: -2147483648 + (MaxValue: Int32_16 + m_value: 2147483647 + MaxValue: 2147483647 + MinValue: -2147483648) + MinValue: -2147483648))";
            AssertTrue(expressionResult == output, "Test SelectPropertiesAndSelectFields: deepen load in string properties and int fields.");
        }

        [TestMethod]
        public void TestReflection_CallMethod()
        {
            // test load all methods of 'Sun' class, don't load properties or fields and add specify reader for "Beep Method"
            var obj = new Son();
            var reflection = obj.AsReflection()
                .SelectProperties(null)
                .SelectMethods((value, type) => value is Son, (value, type) => type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
                .AddValueReaderForMethods(new BeepMethodReader());
           
            var result = reflection.Query().ToString();
            var expected = "";
            AssertTrue(result == expected, "Test public and extern method");

            //reflection.AddValueReaderForProperties(new PropertyReadIndexersTestClass());
        }

        public string GetString(object obj, bool loadFields = false)
        {
            if (!loadFields)
                return obj
                    .AsReflection()
                    //.Settings(SettingsFlags.ShowFullNameOfType | SettingsFlags.ShowParameterName)
                    .SetMaxItems(2000)
                    .Query()
                    .ToString();

            return obj
                .AsReflection()
                .SelectFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Query()
                .ToString();
        }

        public void AssertTrue(bool condition, string message)
        {
            if (!condition)
            {
                if (this.failWithException)
                { 
                    throw new Exception(message);
                }
                else
                {
                    Assert.Fail(message);
                }
            }
        }

        public string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}

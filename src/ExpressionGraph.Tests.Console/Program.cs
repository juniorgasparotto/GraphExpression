using ExpressionGraph.Reflection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpressionGraph;
//using Microsoft.CSharp.RuntimeBinder;

namespace ExpressionGraph.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Test_Commoms();
            Test_Primitives();
            Test_Array();

            //var classTestTypes = new ClassTestTypes();
            //dynamic anonymous = new { PropPublic1 = "A", PropPublic2 = 1, PropPublic3 = "B", PropPublic4 = "B", PropPublic5 = "B", PropPublic6 = "B", PropPublic7 = "B", PropPublic8 = "B", PropPublic9 = "B", PropPublic10 = "B", PropPublic11 = "B", PropPublic12 = new { PropSubPublic1 = 0, PropSubPublic2 = 1, PropSubPublic3 = 2 } };
            //classTestTypes.FieldPublicDynamic = anonymous;
            //TestProperties(Extensions.AsReflection(anonymous).Reflect());

            //var objects = Enumerable.ToList(Extensions.AsReflection(classTestTypes.FieldPublicDynamic).ReflectTree());
            //var expression = Extensions.AsReflection(classTestTypes.FieldPublicDynamic).Query().ToString();

            //var testClass3 = new ClassTest3();
            //var str = testClass3.AsReflection().Query().ToString();

            //var testClass4 = new ClassTest3();
            //var str2 = testClass3.AsReflection().Query().ToString();

            //testClass4 = new ClassTest3();
            //str2 = testClass3.AsReflection().SelectProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Query().ToString();

            //var vDateTime = DateTime.Now;
            //var eDateTime = vDateTime.AsReflection().Query().ToString();

            //var vDecimal = 10.0001m;
            //var eDecimal = vDecimal.AsReflection().Query().ToString();

            //var vFloat = 10.0001f;
            //var eFloat = vFloat.AsReflection().Query().ToString();

            // block load from namespace "System"
            
            
            //////////
            var lst = new List<object>();
            lst.Add(new TestClass3());
            lst.Add("Test");

            var query = lst.AsReflection()
                .SelectTypes
                (
                    (obj) =>
                    {
                        return obj.GetType() == typeof(TestClass3);
                    },
                    (obj) =>
                    {
                        return ReflectionUtils.GetAllParentTypes(obj.GetType());
                    }
                )
                .Query();
        }

        private static void Test_Array()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();

            var expressionResult1 = testClass.FieldPublicArrayUni.AsReflection().Query().ToString();
            expectedTest = "String[]_0 + Item[0]: \"[0]\" + Item[1]: \"[1]\"";
            json = ToJson(testClass.FieldPublicArrayUni);
            AssertTrue(expressionResult1 == expectedTest, "ArrayUni");

            var expressionResult2 = testClass.FieldPublicArrayTwo.AsReflection().Query().ToString();
            expectedTest = "String[,]_0 + Item[0,0]: \"[0, 0]\" + Item[0,1]: \"[0, 1]\" + Item[1,0]: \"[1, 0]\" + Item[1,1]: \"[1, 1]\"";
            json = ToJson(testClass.FieldPublicArrayTwo);
            AssertTrue(expressionResult2 == expectedTest, "ArrayTwo");

            var expressionResult3 = testClass.FieldPublicArrayThree.AsReflection().Query().ToString();
            expectedTest = "String[,,]_0 + Item[0,0,0]: \"[0, 0, 0]\" + Item[0,0,1]: \"[0, 0, 1]\"";
            json = ToJson(testClass.FieldPublicArrayThree);
            AssertTrue(expressionResult3 == expectedTest, "ArrayThree");

            var expressionResult4 = testClass.FieldPublicJaggedArrayTwo.AsReflection().Query().ToString();
            expectedTest = "String[][]_0 + (Item[0]: String[]_1 + Item[0]: \"a\" + Item[1]: \"b\" + Item[2]: \"c\" + Item[3]: \"d\" + Item[4]: \"e\") + (Item[1]: String[]_7 + Item[0]: \"a1\" + Item[1]: \"b1\" + Item[2]: \"c1\" + Item[3]: \"d1\")";
            json = ToJson(testClass.FieldPublicJaggedArrayTwo);
            AssertTrue(expressionResult4 == expectedTest, "JaggedArrayTwo");

            var expressionResult5 = testClass.FieldPublicJaggedArrayThree.AsReflection().Query().ToString();
            expectedTest = "String[][][]_0 + (Item[0]: String[][]_1 + (Item[0]: String[]_2 + Item[0]: \"[0][0][0]\" + Item[1]: \"[0][0][1]\"))";
            json = ToJson(testClass.FieldPublicJaggedArrayThree);
            AssertTrue(expressionResult5 == expectedTest, "JaggedArrayThree");

            var expressionResult6 = testClass.FieldPublicArrayListNonGeneric.AsReflection().Query().ToString();
            expectedTest = "ArrayList_0 + Item[0]: 1 + Item[1]: \"a\" + Item[2]: 10.0 + Item[3]: \"2000-01-01T00:00:00.000-02:00\"";
            json = ToJson(testClass.FieldPublicArrayListNonGeneric);
            AssertTrue(expressionResult6 == expectedTest, "ArrayList");

            var expressionResult7 = testClass.FieldPublicBitArray.AsReflection().Query().ToString();
            expectedTest = "BitArray_0 + Item[0]: false + Item[1]: false + Item[2]: true";
            json = ToJson(testClass.FieldPublicBitArray);
            AssertTrue(expressionResult7 == expectedTest, "BitArray");

            // is normal "int[,][]" is changed by "int[][,]". The .net make it.
            // ** Newtonsoft Json have different return expectations **
            var expressionResult8 = testClass.FieldPublicMixedArrayAndJagged.AsReflection().Query().ToString();
            expectedTest = "Int32[,][]_0 + (Item[0]: Int32[,]_1 + Item[0,0]: 1 + Item[0,1]: 3 + Item[1,0]: 5 + Item[1,1]: 7) + (Item[1]: Int32[,]_6 + Item[0,0]: 0 + Item[0,1]: 2 + Item[1,0]: 4 + Item[1,1]: 6 + Item[2,0]: 8 + Item[2,1]: 10) + (Item[2]: Int32[,]_13 + Item[0,0]: 11 + Item[0,1]: 22 + Item[1,0]: 99 + Item[1,1]: 88 + Item[2,0]: 0 + Item[2,1]: 9)";
            json = ToJson(testClass.FieldPublicMixedArrayAndJagged);
            AssertTrue(expressionResult8 == expectedTest, "MixedArrayAndJagged");
        }

        private static void Test_Primitives()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();

            #region Types of Keywords

            //var expressionResult1 = Extensions.AsReflection(testClass.FieldPublicDynamic).Query().ToString();
            //expectedTest = "\"Dynamic - String\"";
            //json = ToJson(testClass.FieldPublicDynamic);
            //AssertTrue(expressionResult1 == expectedTest, "Dynamic");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult2 = GetString(testClass.FieldPublicObject);
            expectedTest = "\"Object - StringBuilder\"";
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

            #endregion

            #region System

            // ** Newtonsoft Json have different return expectations **
            var expressionResult16 = GetString(testClass.FieldPublicDateTime);
            expectedTest = "\"2000-01-01T01:01:01.000-02:00\"";
            json = ToJson(testClass.FieldPublicDateTime);
            AssertTrue(expressionResult16 == expectedTest, "DateTime)");

            var expressionResult17 = GetString(testClass.FieldPublicTimeSpan);
            expectedTest = "\"01:10:40\"";
            json = ToJson(testClass.FieldPublicTimeSpan);
            AssertTrue(expressionResult17 == expectedTest, "TimeSpan");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult18 = GetString(testClass.FieldPublicEnumDateTimeKind);
            expectedTest = "Local";
            json = ToJson(testClass.FieldPublicEnumDateTimeKind);
            AssertTrue(expressionResult18 == expectedTest, "EnumDateTimeKind");

            var expressionResult19 = GetString(testClass.FieldPublicDateTimeOffset);
            expectedTest = "\"2008-05-01T08:06:32.545+01:00\"";
            json = ToJson(testClass.FieldPublicDateTimeOffset);
            AssertTrue(expressionResult19 == expectedTest, "DateTimeOffset");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult20 = GetString(testClass.FieldPublicIntPtr);
            expectedTest = "100";
            json = ToJson(testClass.FieldPublicIntPtr);
            AssertTrue(expressionResult20 == expectedTest, "IntPtr");

            var expressionResult21 = GetString(testClass.FieldPublicTimeZone);
            expectedTest = "CurrentSystemTimeZone_0 + StandardName: \"Hora oficial do Brasil\" + DaylightName: \"Horário brasileiro de verão\"";
            json = ToJson(testClass.FieldPublicTimeZone);
            AssertTrue(expressionResult21 == expectedTest, "TimeZone");

            // ** Newtonsoft Json have different return expectations **
            // ** Is different because Newtonsoft use "System.Runtime.Serialization.SerializationInfo" to get properties ** 
            var expressionResult22 = GetString(testClass.FieldPublicTimeZoneInfo);
            expectedTest = "TimeZoneInfo_0 + Id: \"UTC\" + DisplayName: \"UTC\" + StandardName: \"UTC\" + DaylightName: \"UTC\" + BaseUtcOffset: \"00:00:00\" + SupportsDaylightSavingTime: false";
            json = ToJson(testClass.FieldPublicTimeZoneInfo);
            AssertTrue(expressionResult22 == expectedTest, "TimeZoneInfo");

            var expressionResult23 = GetString(testClass.FieldPublicTuple);
            expectedTest = "Tuple<String, Int32, Decimal>_0 + Item1: \"T-string\" + Item2: 1 + Item3: 1.1";
            json = ToJson(testClass.FieldPublicTuple);
            AssertTrue(expressionResult23 == expectedTest, "Tuple");

            // ** Newtonsoft Json have different return expectations **
            // ** ERROR 
            var expressionResult24 = GetString(testClass.FieldPublicType);
            expectedTest = "\"System.Object\"";
            json = ToJson(testClass.FieldPublicType);
            AssertTrue(expressionResult24 == expectedTest, "Type");

            var expressionResult25 = GetString(testClass.FieldPublicUIntPtr);
            expectedTest = "100";
            json = ToJson(testClass.FieldPublicUIntPtr);
            AssertTrue(expressionResult25 == expectedTest, "UIntPtr");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult26 = GetString(testClass.FieldPublicUri);
            expectedTest = "\"http://www.site.com/\"";
            json = ToJson(testClass.FieldPublicUri);
            AssertTrue(expressionResult26 == expectedTest, "Uri");

            var expressionResult27 = GetString(testClass.FieldPublicVersion);
            expectedTest = "Version_0 + Major: 1 + Minor: 0 + Build: 100 + Revision: 1 + MajorRevision: 0 + MinorRevision: 1";
            json = ToJson(testClass.FieldPublicVersion);
            AssertTrue(expressionResult27 == expectedTest, "Version");

            var expressionResult28 = GetString(testClass.FieldPublicGuid, true);
            expectedTest = "\"d5010f5b-0cd1-44ca-aacb-5678b9947e6c\"";
            json = ToJson(testClass.FieldPublicGuid);
            AssertTrue(expressionResult28 == expectedTest, "Guid");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult29 = GetString(testClass.FieldPublicSingle);
            expectedTest = "3.40282346638529E+38";
            json = ToJson(testClass.FieldPublicSingle);
            AssertTrue(expressionResult29 == expectedTest, "Single");

            // ** Newtonsoft Json have different return expectations **
            // ** Is different because Newtonsoft use "System.Runtime.Serialization.SerializationInfo" to get properties ** 
            var expressionResult30 = GetString(testClass.FieldPublicException);
            expectedTest = "Exception_0 + Message: \"Test error\" + Data: \"System.Collections.ListDictionaryInternal\" + (InnerException: Exception_3 + Message: \"inner exception\" + Data: \"System.Collections.ListDictionaryInternal\" + InnerException: null + TargetSite: null + StackTrace: null + HelpLink: null + Source: null + HResult: -2146233088) + TargetSite: null + StackTrace: null + HelpLink: null + Source: null + HResult: -2146233088";
            json = ToJson(testClass.FieldPublicException);
            AssertTrue(expressionResult30 == expectedTest, "Exception");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult31 = GetString(testClass.FieldPublicEnumNonGeneric);
            expectedTest = "ValueA";
            json = ToJson(testClass.FieldPublicEnumNonGeneric);
            AssertTrue(expressionResult31 == expectedTest, "EnumNonGeneric");

            var expressionResult32 = GetString(testClass.FieldPublicAction);
            expectedTest = "\"System.Action\"";
            json = ToJson(testClass.FieldPublicAction);
            AssertTrue(expressionResult32 == expectedTest, "Action");

            var expressionResult33 = GetString(testClass.FieldPublicAction2);
            expectedTest = "\"System.Action`2[System.String,System.Int32]\"";
            json = ToJson(testClass.FieldPublicAction);
            AssertTrue(expressionResult33 == expectedTest, "Action<string, int>");

            var expressionResult34 = GetString(testClass.FieldPublicFunc);
            expectedTest = "\"System.Func`1[System.Boolean]\"";
            json = ToJson(testClass.FieldPublicFunc);
            AssertTrue(expressionResult34 == expectedTest, "Func");

            var expressionResult35 = GetString(testClass.FieldPublicFunc2);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicFunc);
            AssertTrue(expressionResult35 == expectedTest, "Func<string, int, bool>");

            #endregion
        }

        private static void Test_Commoms()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();

            var expressionResult1 = GetString(testClass.FieldPublicMyCollectionPublicGetEnumerator);
            expectedTest = "MyCollectionPublicGetEnumerator_0 + GetEnumerator[0]: \"a\" + GetEnumerator[1]: \"b\" + GetEnumerator[2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionPublicGetEnumerator);
            AssertTrue(expressionResult1 == expectedTest, "MyCollectionPublicGetEnumerator");

            var expressionResult2 = GetString(testClass.FieldPublicMyCollectionInheritsPublicGetEnumerator);
            expectedTest = "MyCollectionInheritsPublicGetEnumerator_0 + GetEnumerator[0]: \"a\" + GetEnumerator[1]: \"b\" + GetEnumerator[2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionInheritsPublicGetEnumerator);
            AssertTrue(expressionResult2 == expectedTest, "MyCollectionInheritsPublicGetEnumerator");

            var expressionResult3 = GetString(testClass.FieldPublicMyCollectionExplicitGetEnumerator);
            expectedTest = "MyCollectionExplicitGetEnumerator_0 + System.Collections.IEnumerable.GetEnumerator[0]: \"a\" + System.Collections.IEnumerable.GetEnumerator[1]: \"b\" + System.Collections.IEnumerable.GetEnumerator[2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionExplicitGetEnumerator);
            AssertTrue(expressionResult3 == expectedTest, "MyCollectionExplicitGetEnumerator");

            var expressionResult4 = GetString(testClass.FieldPublicMyCollectionInheritsExplicitGetEnumerator);
            expectedTest = "MyCollectionInheritsExplicitGetEnumerator_0 + System.Collections.IEnumerable.GetEnumerator[0]: \"a\" + System.Collections.IEnumerable.GetEnumerator[1]: \"b\" + System.Collections.IEnumerable.GetEnumerator[2]: \"c\"";
            json = ToJson(testClass.FieldPublicMyCollectionInheritsExplicitGetEnumerator);
            AssertTrue(expressionResult4 == expectedTest, "MyCollectionInheritsExplicitGetEnumerator");

            // ** Newtonsoft Json have different return expectations **
            var expressionResult5 = GetString(testClass.FieldPublicEnumSpecific);
            expectedTest = "ValueB";
            json = ToJson(testClass.FieldPublicEnumSpecific);
            AssertTrue(expressionResult5 == expectedTest, "EnumSpecific");

            // ** Newtonsoft Json throw exception with delegates **
            var expressionResult6 = GetString(testClass.MyDelegate);
            expectedTest = "\"DelegateTest\"";
            //json = ToJson(testClass.MyDelegate);
            AssertTrue(expressionResult6 == expectedTest, "Func<string, int, bool>");
        }

        private static void Test_Lists()
        {
            var expectedTest = "";
            var json = "";
            var testClass = new SeveralTypesTest();
            testClass.Populate();

            var expressionResult1 = GetString(testClass.FieldPublicDictionary);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicDictionary);
            AssertTrue(expressionResult1 == expectedTest, "Dictionary");

            var expressionResult2 = GetString(testClass.FieldPublicList);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicList);
            AssertTrue(expressionResult2 == expectedTest, "List<int>");

            var expressionResult3 = GetString(testClass.FieldPublicQueue);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicQueue);
            AssertTrue(expressionResult3 == expectedTest, "Queue<int>");

            var expressionResult4 = GetString(testClass.FieldPublicHashSet);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicHashSet);
            AssertTrue(expressionResult1 == expectedTest, "HashSet<string>");


            var expressionResult5 = GetString(testClass.FieldPublicSortedSet);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicSortedSet);
            AssertTrue(expressionResult5 == expectedTest, "SortedSet<string>");

            var expressionResult6 = GetString(testClass.FieldPublicStack);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicStack);
            AssertTrue(expressionResult6 == expectedTest, "Stack<string>");

            var expressionResult7 = GetString(testClass.FieldPublicLinkedList);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicLinkedList);
            AssertTrue(expressionResult7 == expectedTest, "LinkedList<string>");

            var expressionResult8 = GetString(testClass.FieldPublicObservableCollection);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicObservableCollection);
            AssertTrue(expressionResult8 == expectedTest, "ObservableCollection<string>");

            var expressionResult9 = GetString(testClass.FieldPublicKeyedCollection);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicKeyedCollection);
            AssertTrue(expressionResult9 == expectedTest, "KeyedCollection<int, MyData>");

            var expressionResult10 = GetString(testClass.FieldPublicReadOnlyCollection);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicReadOnlyCollection);
            AssertTrue(expressionResult10 == expectedTest, "ReadOnlyCollection<string>");

            var expressionResult11 = GetString(testClass.FieldPublicReadOnlyDictionary);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicReadOnlyDictionary);
            AssertTrue(expressionResult11 == expectedTest, "ReadOnlyDictionary<string, string>");

            var expressionResult12 = GetString(testClass.FieldPublicArrayListNonGeneric);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicArrayListNonGeneric);
            AssertTrue(expressionResult12 == expectedTest, "ArrayList");

            var expressionResult13 = GetString(testClass.FieldPublicBitArray);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicBitArray);
            AssertTrue(expressionResult13 == expectedTest, "BitArray");

            var expressionResult14 = GetString(testClass.FieldPublicSortedList);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicSortedList);
            AssertTrue(expressionResult14 == expectedTest, "SortedList");

            var expressionResult15 = GetString(testClass.FieldPublicHashtableNonGeneric);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicHashtableNonGeneric);
            AssertTrue(expressionResult15 == expectedTest, "Hashtable");

            var expressionResult16 = GetString(testClass.FieldPublicQueueNonGeneric);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicQueueNonGeneric);
            AssertTrue(expressionResult16 == expectedTest, "Queue");

            var expressionResult17 = GetString(testClass.FieldPublicStackNonGeneric);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicStackNonGeneric);
            AssertTrue(expressionResult17 == expectedTest, "Stack");

            var expressionResult18 = GetString(testClass.FieldPublicIEnumerable);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicIEnumerable);
            AssertTrue(expressionResult18 == expectedTest, "IEnumerable");

            var expressionResult19 = GetString(testClass.FieldPublicBlockingCollection);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicBlockingCollection);
            AssertTrue(expressionResult19 == expectedTest, "BlockingCollection<string>");

            var expressionResult20 = GetString(testClass.FieldPublicConcurrentBag);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicConcurrentBag);
            AssertTrue(expressionResult20 == expectedTest, "ConcurrentBag<string>");

            var expressionResult21 = GetString(testClass.FieldPublicConcurrentDictionary);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicConcurrentDictionary);
            AssertTrue(expressionResult21 == expectedTest, "ConcurrentDictionary<string, int>");

            var expressionResult22 = GetString(testClass.FieldPublicConcurrentQueue);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicConcurrentQueue);
            AssertTrue(expressionResult22 == expectedTest, "ConcurrentQueue<string>");

            var expressionResult23 = GetString(testClass.FieldPublicConcurrentStack);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicConcurrentStack);
            AssertTrue(expressionResult23 == expectedTest, "ConcurrentStack<string>");

            var expressionResult24 = GetString(testClass.FieldPublicHybridDictionary);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicHybridDictionary);
            AssertTrue(expressionResult24 == expectedTest, "HybridDictionary");

            var expressionResult25 = GetString(testClass.FieldPublicListDictionary);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicListDictionary);
            AssertTrue(expressionResult25 == expectedTest, "ListDictionary");

            var expressionResult26 = GetString(testClass.FieldPublicNameValueCollection);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicNameValueCollection);
            AssertTrue(expressionResult26 == expectedTest, "NameValueCollection");

            var expressionResult27 = GetString(testClass.FieldPublicOrderedDictionary);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicOrderedDictionary);
            AssertTrue(expressionResult27 == expectedTest, "OrderedDictionary");

            var expressionResult28 = GetString(testClass.FieldPublicStringCollection);
            expectedTest = "\"System.Func`3[System.String,System.Int32,System.Boolean]\"";
            json = ToJson(testClass.FieldPublicStringCollection);
            AssertTrue(expressionResult28 == expectedTest, "StringCollection");
        }

        private static void TestSelectPropertiesAndSelectFields()
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

        private static string GetString(object obj, bool loadFields = false)
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

        public static void AssertTrue(bool condition, string message)
        {
            if (!condition)
                throw new Exception(message);
        }

        private static void TestProperties(ReflectedInstance instance)
        {
            var i = 1;

            foreach (var prop in instance.GetAllProperties())
            {
                var b = prop.Name == "PropPublic" + i;

                if (!b)
                    throw new Exception("Erro");

                i++;
            }
        }

        private static void TestFields(ReflectedInstance instance)
        {
            var i = 1;

            foreach (var field in instance.GetAllFields())
            {
                var b = field.Name == "Field" + i;

                if (!b)
                    throw new Exception("Erro");

                i++;
            }
        }

        private static string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
    }
}

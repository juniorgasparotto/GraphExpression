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
    public partial class DeserializationComplexTest : EntitiesData
    {        
        [Fact]
        public void DeserializeComplex_Null_Expression()
        {
            PublicConstructorWithParameters[] array = null;
            var expressionStr = array.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PublicConstructorWithParameters>(expressionStr);

            Assert.Null(deserialized);
        }

        [Fact]
        public void DeserializeComplex_EmptyString()
        {
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PublicConstructorWithParameters>("");
            Assert.Null(deserialized);
        }

        [Fact]
        public void DeserializeComplex_EmptyStringWithSpace()
        {
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PublicConstructorWithParameters>("");
            Assert.Null(deserialized);
        }

        [Fact]
        public void DeserializeComplex_AnonymousObjectWithPrimitivesOnly_NoTyped()
        {
            var obj = new
            {
                Prop1 = "Value",
                Prop2 = 100,
                Prop3 = new
                {
                    Prop4 = 10.10d,
                    Prop5 = new
                    {
                        Prop6 = 1
                    },
                    Prop7 = new
                    {
                        Prop8 = 'a',
                        Prop9 = (string)null,
                    },
                }
            };

            var expressionStr = obj.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize(expressionStr);
            var cast = Dynamic(deserialized);

            Assert.IsType<ExpandoObject>(deserialized);
            Assert.Equal("Value", cast.Prop1);
            Assert.Equal("100", cast.Prop2);

            var prop3 = Dynamic(cast.Prop3);
            Assert.Equal("10.1", prop3.Prop4);

            var prop5 = Dynamic(prop3.Prop5);
            Assert.Equal("1", prop5.Prop6);

            var prop7 = Dynamic(prop3.Prop7);
            Assert.Equal("a", prop7.Prop8);
            Assert.Equal(null, prop7.Prop9);

            // var serializeFinal = ((object)cast).AsExpression().DefaultSerializer.Serialize();
        }

        [Fact]
        public void DeserializeComplex_AnonymousObjectWithComplex_NoTyped()
        {
            var obj = new
            {
                Prop1 = "Value",
                Prop2 = new RecursiveClass()
                {
                    MyProp = 12
                }
            };

            var expressionStr = obj.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize(expressionStr);
            var cast = Dynamic(deserialized);

            Assert.IsType<ExpandoObject>(deserialized);
            Assert.Equal("Value", cast.Prop1);
            var prop2 = Dynamic(cast.Prop2);
            Assert.Equal("12", prop2.MyProp);
        }

        [Fact]
        public void DeserializeComplex_AnonymousObjectWithComplex_Typed()
        {
            var obj = new
            {
                Prop1 = "Value",
                Prop2 = 12,
                Prop3 = new RecursiveClass()
                {
                    MyProp = 100,
                    RecursiveProperty = null
                }
            };

            var expressionStr = obj.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize(expressionStr, obj.GetType());

            var cast = Dynamic(deserialized);
            Assert.IsType<ExpandoObject>(deserialized);
            Assert.Equal("Value", cast.Prop1);
            Assert.Equal(12, (int)cast.Prop2);

            var prop3 = (RecursiveClass)cast.Prop3;
            Assert.Equal(100, prop3.MyProp);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_RecursiveAntiparalell()
        {
            var objTyped1 = new RecursiveClass()
            {
                MyProp = 100,
                RecursiveProperty = null
            };

            var objTyped2 = new RecursiveClass()
            {
                MyProp = 120,
                RecursiveProperty = objTyped1
            };

            objTyped1.RecursiveProperty = objTyped2;

            var expressionStr = objTyped1.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = (RecursiveClass)deserializer.Deserialize(expressionStr, objTyped1.GetType());

            Assert.IsType<RecursiveClass>(deserialized);
            Assert.Equal(100, deserialized.MyProp);
            Assert.Equal(120, deserialized.RecursiveProperty.MyProp);
            Assert.Same(deserialized, deserialized.RecursiveProperty.RecursiveProperty);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_MultiLevel()
        {
            var objTyped = new RecursiveClass()
            {
                MyProp = 1,
                RecursiveProperty = new RecursiveClass()
                {
                    MyProp = 2,
                    RecursiveProperty = new RecursiveClass()
                    {
                        MyProp = 3,
                        RecursiveProperty = new RecursiveClass()
                        {
                            MyProp = 4,
                            RecursiveProperty = null,
                            ZProp = "D"
                        },
                        ZProp = "C"
                    },
                    ZProp = "B"
                },
                ZProp = "A"
            };

            var expressionStr = objTyped.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = (RecursiveClass)deserializer.Deserialize(expressionStr, objTyped.GetType());

            Assert.IsType<RecursiveClass>(deserialized);
            Assert.Equal(1, deserialized.MyProp);
            Assert.Equal("A", deserialized.ZProp);

            Assert.Equal(2, deserialized.RecursiveProperty.MyProp);
            Assert.Equal("B", deserialized.RecursiveProperty.ZProp);

            Assert.Equal(3, deserialized.RecursiveProperty.RecursiveProperty.MyProp);
            Assert.Equal("C", deserialized.RecursiveProperty.RecursiveProperty.ZProp);

            Assert.Equal(4, deserialized.RecursiveProperty.RecursiveProperty.RecursiveProperty.MyProp);
            Assert.Equal("D", deserialized.RecursiveProperty.RecursiveProperty.RecursiveProperty.ZProp);
        }

        [Fact]
        public void DeserializeComplex_ListObject()
        {
            var list = new List<RecursiveClass>()
            {
                new RecursiveClass
                {
                    MyProp = 999,
                    ZProp = "value1"
                },
                new RecursiveClass
                {
                    MyProp = 1000,
                    ZProp = "value2"
                }
            };

            var expressionStr = list.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = (List<RecursiveClass>)deserializer.Deserialize(expressionStr, list.GetType());

            Assert.Equal(2, deserialized.Count);
            Assert.Equal(999, deserialized[0].MyProp);
            Assert.Null(deserialized[0].RecursiveProperty);
            Assert.Equal("value1", deserialized[0].ZProp);

            Assert.Equal(1000, deserialized[1].MyProp);
            Assert.Null(deserialized[1].RecursiveProperty);
            Assert.Equal("value2", deserialized[1].ZProp);
        }

        [Fact]
        public void DeserializeComplex_AnonymousListObject_NoTyped()
        {
            var list = new
            {
                A = new List<RecursiveClass>()
                    {
                        new RecursiveClass
                        {
                            MyProp = 999,
                            ZProp = "value1"
                        },
                        new RecursiveClass
                        {
                            MyProp = 1000,
                            ZProp = "value2"
                        }
                    }
            };

            var expressionStr = list.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = (ExpandoObject)deserializer.Deserialize(expressionStr);

            Assert.Equal("999", GetExpandoObjectValue(deserialized, "A", "[0]", "MyProp"));
            Assert.Equal("value1", GetExpandoObjectValue(deserialized, "A", "[0]", "ZProp"));

            Assert.Equal("1000", GetExpandoObjectValue(deserialized, "A", "[1]", "MyProp"));
            Assert.Equal("value2", GetExpandoObjectValue(deserialized, "A", "[1]", "ZProp"));
        }

        [Fact]
        public void DeserializeComplex_AnonymousListObject_Typed()
        {
            var listAnonymous = new
            {
                A = new List<RecursiveClass>()
                    {
                        new RecursiveClass
                        {
                            MyProp = 999,
                            ZProp = "value1"
                        },
                        new RecursiveClass
                        {
                            MyProp = 1000,
                            ZProp = "value2"
                        }
                    }
            };

            var expressionStr = listAnonymous.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = (ExpandoObject)deserializer.Deserialize(expressionStr, listAnonymous.GetType());

            var list = GetExpandoObjectValue<List<RecursiveClass>>(deserialized, "A");
            Assert.Equal(999, list[0].MyProp);
            Assert.Equal("value1", list[0].ZProp);

            Assert.Equal(1000, list[1].MyProp);
            Assert.Equal("value2", list[1].ZProp);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_PublicPropertyNoSetter()
        {
            var expressionStr = "\"Complex.1\" + \"PropOnlySet: 2000\" + \"Prop1: 1\" + \"Prop3: 3\" + \"Prop2: 2\" + \"Prop4: 4\" + \"PropNoBackingField: 999\"";
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PropertyPrivateWithoutSetter>(expressionStr);

            Assert.Equal(1, deserialized.Prop1);
            Assert.Equal(2, deserialized.Prop2);
            Assert.Equal(3, deserialized.GetType().GetProperty("Prop3", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(deserialized));
            Assert.Equal(4, deserialized.Prop4);
            Assert.Equal(2000, deserialized.propOnlyValue);
            Assert.Equal(2, deserialized.PropNoBackingField);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_ConstructorOnlyWithParameters()
        {
            var expressionStr = "\"Complex.1\" + \"Prop1: 2000\"";
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PublicConstructorWithParameters>(expressionStr);
            Assert.Equal(2000, deserialized.Prop1);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_NonPublicConstructor()
        {
            var expressionStr = "\"Complex.1\" + \"Prop1: 2000\"";
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PrivateConstructor>(expressionStr);
            Assert.Equal(2000, deserialized.Prop1);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_PropertyIEnumerableAndBackFieldIsList()
        {
            var obj = new PropertyTypeDiffBackField();
            obj.Set();

            var expressionStr = obj.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PropertyTypeDiffBackField>(expressionStr);

            Assert.Equal(1, deserialized.IEnumerable.ElementAt(0));
            Assert.Equal(2, deserialized.IEnumerable.ElementAt(1));
            Assert.Equal(3, deserialized.IEnumerable.ElementAt(2));
        }

        [Fact]
        public void DeserializeComplex_TypedObject_CircularEntityMultiLevel()
        {
            TestMultiLevel(() => A);
            TestMultiLevel(() => A + B);
            TestMultiLevel(() => A + (B + C));

            // recursive
            TestMultiLevel(() => A + A);
            TestMultiLevel(() => A + (B + A));
            TestMultiLevel(() => A + (B + B));
            TestMultiLevel(() => A + (B + A) + A);
            TestMultiLevel(() => A + (A + A) + A);
            TestMultiLevel(() => A + (B + (C + B)));

            // multiples closed pharenthesis (no repeat)
            TestMultiLevel(() => A + (B + (C + (D + (E + F) + G) + H) + I) + J);
            TestMultiLevel(() => A + (B + (C + (D + (E + F)) + G) + H) + I);
            TestMultiLevel(() => A + (B + (C + (D + (E + F))) + G) + H);
            TestMultiLevel(() => A + (B + (C + (D + (E + F)))) + G);
            TestMultiLevel(() => A + (B + (C + (J + (I + H)))));

            // multiples entities
            TestMultiLevel(() => A + (B + C + (J + (I + H + S + W + Y) + L) + S) + K + (D + E + (P + (U + Y) + R) + O + Q + S + V));
        }

        [Fact]
        public void DeserializeComplex_DictionaryPrimitiveObject()
        {
            var dic = new Dictionary<string, int>()
            {
                { "A", 1 },
                { "B", 2 },
            };

            var expressionStr = dic.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<Dictionary<string, int>>(expressionStr);

            Assert.Equal(2, deserialized.Count);
            Assert.Equal(1, deserialized["A"]);
            Assert.Equal(2, deserialized["B"]);
        }

        [Fact]
        public void DeserializeComplex_DictionaryComplexKeyAndValueObject()
        {   
            var key = new PublicConstructorWithParameters("value") { Prop1 = 1 };
            var value = new PublicConstructorWithParameters("value") { Prop1 = 2 };

            var dic = new Dictionary<PublicConstructorWithParameters, PublicConstructorWithParameters>()
            {
                { key, value },                                
            };

            var anonymous = new
            {
                A = dic,
                PropKey = key,
                PropValue = value
            };

            var expressionStr = anonymous.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = (ExpandoObject)deserializer.Deserialize(expressionStr, anonymous.GetType());

            var A = GetExpandoObjectValue<Dictionary<PublicConstructorWithParameters, PublicConstructorWithParameters>>(deserialized, "A");
            var propKey = GetExpandoObjectValue<PublicConstructorWithParameters>(deserialized, "PropKey");
            var propValue = GetExpandoObjectValue<PublicConstructorWithParameters>(deserialized, "PropValue");

            Assert.Equal(1, A.Keys.ElementAt(0).Prop1);
            Assert.Equal(2, A.Values.ElementAt(0).Prop1);

            Assert.Same(propKey, A.Keys.ElementAt(0));
            Assert.Same(propValue, A.Values.ElementAt(0));
        }

        [Fact]
        public void DeserializeComplex_ArrayIntEmpty()
        {
            var array = new int[0];
            var expressionStr = array.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<int[]>(expressionStr);

            Assert.Empty(deserialized);
        }

        [Fact]
        public void DeserializeComplex_ArrayInt()
        {
            var array = new int[] { 1, 2, 3 };
            var expressionStr = array.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<int[]>(expressionStr);

            Assert.Equal(1, deserialized[0]);
            Assert.Equal(2, deserialized[1]);
            Assert.Equal(3, deserialized[2]);
        }

        [Fact]
        public void DeserializeComplex_ArrayComplex()
        {
            var value1 = new PublicConstructorWithParameters("value") { Prop1 = 1 };
            var value2 = new PublicConstructorWithParameters("value") { Prop1 = 2 };
            var value3 = new PublicConstructorWithParameters("value") { Prop1 = 3 };

            var array = new PublicConstructorWithParameters[] { value1, value2, value3 };
            var expressionStr = array.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PublicConstructorWithParameters[]>(expressionStr);

            Assert.Equal(1, deserialized[0].Prop1);
            Assert.Equal(2, deserialized[1].Prop1);
            Assert.Equal(3, deserialized[2].Prop1);
        }

        [Fact]
        public void DeserializeComplex_ArrayComplexMultidimetional()
        {
            var value1 = new PublicConstructorWithParameters("value") { Prop1 = 1 };
            var value2 = new PublicConstructorWithParameters("value") { Prop1 = 2 };
            var value3 = new PublicConstructorWithParameters("value") { Prop1 = 3 };
            var value4 = new PublicConstructorWithParameters("value") { Prop1 = 4 };

            var array = new PublicConstructorWithParameters[2,2] { { value1, value2 }, { value3, value4 } };
            var expressionStr = array.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<PublicConstructorWithParameters[,]>(expressionStr);

            Assert.Equal(1, deserialized[0, 0].Prop1);
            Assert.Equal(2, deserialized[0, 1].Prop1);
            Assert.Equal(3, deserialized[1, 0].Prop1);
            Assert.Equal(4, deserialized[1, 1].Prop1);
        }


        [Fact]
        public void DeserializeComplex_ArrayJaggedComplexMultidimetional()
        {
            int[][] jaggedArray = new int[2][];
            jaggedArray[0] = new int[2];
            jaggedArray[0][0] = 10;
            jaggedArray[0][1] = 20;
            jaggedArray[1] = new int[1];
            jaggedArray[1][0] = 30;

            var expressionStr = jaggedArray.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<int[][]>(expressionStr);

            Assert.Equal(10, deserialized[0][0]);
            Assert.Equal(20, deserialized[0][1]);
            Assert.Equal(30, deserialized[1][0]);
        }

        [Fact]
        public void DeserializeComplex_AbstractAndInterfaceClass()
        {
            var obj = new ClassWithAbstractAndInterface
            {
                A = new ImplementAbstractAndInterface { MyProp = 10 },
                B = new ImplementAbstractAndInterface { MyProp = 20 },
            };

            var expressionStr = obj.AsExpression().DefaultSerializer.Serialize();

            var factory = new ComplexEntityFactoryDeserializer(obj.GetType());
            factory.AddMapType<Interface, ImplementAbstractAndInterface>();
            factory.AddMapType<AbstractClass, ImplementAbstractAndInterface>();

            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<ClassWithAbstractAndInterface>(expressionStr, factory);

            Assert.Equal(10, deserialized.A.MyProp);
            Assert.Equal(20, deserialized.B.MyProp);
        }

        [Fact]
        public void DeserializeComplex_AbstractAndInterfaceClass_NoFactory()
        {
            var obj = new ClassWithAbstractAndInterface
            {
                A = new ImplementAbstractAndInterface { MyProp = 10 },
                B = new ImplementAbstractAndInterface { MyProp = 20 },
            };

            var factory = new ComplexEntityFactoryDeserializer(obj.GetType());
            factory.IgnoreErrors = true;

            var expressionStr = obj.AsExpression().DefaultSerializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<ClassWithAbstractAndInterface>(expressionStr, factory);

            Assert.Null(deserialized.A);
            Assert.Null(deserialized.B);
            Assert.Equal(2, factory.Errors.Count);
            Assert.Equal($"An instance of type '{typeof(Interface).FullName}' contains value, but not created. Make sure it is an interface or an abstract class, if so, set up a corresponding concrete class in the '{nameof(ComplexEntityFactoryDeserializer)}.{nameof(ComplexEntityFactoryDeserializer.MapTypes)}' configuration.", factory.Errors[0]);
            Assert.Equal($"An instance of type '{typeof(AbstractClass).FullName}' contains value, but not created. Make sure it is an interface or an abstract class, if so, set up a corresponding concrete class in the '{nameof(ComplexEntityFactoryDeserializer)}.{nameof(ComplexEntityFactoryDeserializer.MapTypes)}' configuration.", factory.Errors[1]);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_ShowFullTypeName()
        {
            var obj = new RecursiveClass
            {
                MyProp = 10,
                RecursiveProperty = new RecursiveClass() { MyProp = 20 },
                ZProp = "value"
            };

            var serializer = obj.AsExpression().GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.ShowType = ShowTypeOptions.FullTypeName;
            var expressionStr = serializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<RecursiveClass>(expressionStr);

            Assert.Equal(10, deserialized.MyProp);
            Assert.Equal("value", deserialized.ZProp);
            Assert.Equal(20, deserialized.RecursiveProperty.MyProp);
        }


        [Fact]
        public void DeserializeComplex_TypedObject_ShowTypeName()
        {
            var obj = new RecursiveClass
            {
                MyProp = 10,
                RecursiveProperty = new RecursiveClass() { MyProp = 20 },
                ZProp = "value"
            };

            var serializer = obj.AsExpression().GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.ShowType = ShowTypeOptions.TypeName;
            var expressionStr = serializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<RecursiveClass>(expressionStr);

            Assert.Equal(10, deserialized.MyProp);
            Assert.Equal("value", deserialized.ZProp);
            Assert.Equal(20, deserialized.RecursiveProperty.MyProp);
        }

        [Fact]
        public void DeserializeComplex_TypedObject_ShowType_None()
        {
            var obj = new RecursiveClass
            {
                MyProp = 10,
                RecursiveProperty = new RecursiveClass() { MyProp = 20 },
                ZProp = "value"
            };

            var serializer = obj.AsExpression().GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.ShowType = ShowTypeOptions.None;
            var expressionStr = serializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();
            var deserialized = deserializer.Deserialize<RecursiveClass>(expressionStr);

            Assert.Equal(10, deserialized.MyProp);
            Assert.Equal("value", deserialized.ZProp);
            Assert.Equal(20, deserialized.RecursiveProperty.MyProp);
        }

        //[Fact]
        //public void JsonTests()
        //{
        //    var a = new PropertyPrivateWithoutSetter();
        //    a.Set();

        //    var json = Newtonsoft.Json.JsonConvert.SerializeObject(a);
        //    json = "{\"Name\":\"A\",\"Count\":3,\"Children\":[{\"Name\":\"B\",\"Count\":3,\"Children\":[{\"Name\":\"C\",\"Count\":0,\"Children\":[]},{\"Name\":\"J\",\"Count\":2,\"Children\":[{\"Name\":\"I\",\"Count\":4,\"Children\":[{\"Name\":\"H\",\"Count\":0,\"Children\":[]},{\"Name\":\"S\",\"Count\":0,\"Children\":[]},{\"Name\":\"W\",\"Count\":0,\"Children\":[]},{\"Name\":\"Y\",\"Count\":0,\"Children\":[]}]},{\"Name\":\"L\",\"Count\":0,\"Children\":[]}]},{\"Name\":\"S\",\"Count\":0,\"Children\":[]}]},{\"Name\":\"K\",\"Count\":0,\"Children\":[]},{\"Name\":\"D\",\"Count\":6,\"Children\":[{\"Name\":\"E\",\"Count\":0,\"Children\":[]},{\"Name\":\"P\",\"Count\":2,\"Children\":[{\"Name\":\"U\",\"Count\":1,\"Children\":[{\"Name\":\"Y\",\"Count\":0,\"Children\":[]}]},{\"Name\":\"R\",\"Count\":0,\"Children\":[]}]},{\"Name\":\"O\",\"Count\":0,\"Children\":[]},{\"Name\":\"Q\",\"Count\":0,\"Children\":[]},{\"Name\":\"S\",\"Count\":0,\"Children\":[]},{\"Name\":\"V\",\"Count\":0,\"Children\":[]}]}]}";
        //    //json = "{\"Prop1\":20000,\"Prop2\":200,\"Prop4\":10000,\"PropNoBackingField\":2}";

        //    List<string> errors = new List<string>();
        //    var jsonObj = JsonConvert.DeserializeObject<CircularEntity>(json,
        //        new JsonSerializerSettings
        //        {
        //            Error = delegate (object sender, ErrorEventArgs args)
        //            {
        //                errors.Add(args.ErrorContext.Error.Message);
        //                args.ErrorContext.Handled = true;
        //            }
        //        });
        //}

        #region Auxs

        private static dynamic Dynamic(object deserialized)
        {
            return deserialized as ExpandoObject;
        }

        private object GetExpandoObjectValue(ExpandoObject eo, params string[] keys)
        {
            object value = null;
            var dic = (IDictionary<string, object>)eo;

            foreach (var k in keys)
            {
                value = dic[k];
                if (dic[k] is IDictionary<string, object> dic2)
                    dic = dic2;
            }

            return value;
        }

        private T GetExpandoObjectValue<T>(ExpandoObject eo, params string[] keys)
        {
            object value = null;
            var dic = (IDictionary<string, object>)eo;

            foreach (var k in keys)
            {
                value = dic[k];
                if (dic[k] is IDictionary<string, object> dic2)
                    dic = dic2;
            }

            return (T)value;
        }

        private void TestMultiLevel(Func<CircularEntity> action)
        {
            Clear();

            var objA = action();
            var serializer = objA.AsExpression().GetSerializer<ComplexEntityExpressionSerializer>();
            serializer.GetEntityIdCallback = item => VertexContainer<object>.GetEntityId(item.Entity).Id;

            var expressionStr = serializer.Serialize();
            var deserializer = new ComplexEntityExpressionDeserializer();

            var objB = deserializer.Deserialize<CircularEntity>(expressionStr);
            var jsonSettings = new JsonSerializerSettings { Formatting = Formatting.Indented, PreserveReferencesHandling = PreserveReferencesHandling.Objects };

            var jsonA = JsonConvert.SerializeObject(objA, jsonSettings);
            var jsonB = JsonConvert.SerializeObject(objB, jsonSettings);
            Assert.Equal(jsonA, jsonB);
        }

        #region Classes abstracts and interface

        public interface Interface
        {
            int MyProp { get; set; }
        }

        public abstract class AbstractClass : Interface
        {
            public int MyProp { get; set; }
        }

        public class ImplementAbstractAndInterface : AbstractClass
        {
        }

        public class ClassWithAbstractAndInterface
        {
            public Interface A { get; set; }
            public AbstractClass B { get; set; }
        }

        #endregion

        public class RecursiveClass
        {
            public int MyProp { get; set; }
            public RecursiveClass RecursiveProperty { get; set; }
            public string ZProp { get; set; }
            public RecursiveClass() { }
        }

        public class PrivateConstructor
        {
            public int Prop1 { get; set; }
            private PrivateConstructor() { }
        }

        public class PublicConstructorWithParameters
        {
            public int Prop1 { get; set; }
            public PublicConstructorWithParameters(string prop1) { }
        }

        public class PropertyTypeDiffBackField
        {
            private List<int> list;
            public IEnumerable<int> IEnumerable
            {
                get
                {
                    return list;
                }
            }

            public void Set()
            {
                list = new List<int>() { 1, 2, 3 };
            }
        }

        public class PropertyPrivateWithoutSetter
        {
            private int prop4;
            public int propOnlyValue;

            public int Prop1 { get; }
            public int Prop2 { get; private set; }
            private int Prop3 { get; set; }
            public int Prop4 { get => prop4; }
            public int PropOnlySet { set => propOnlyValue = value; }

            public int PropNoBackingField => 2;

            public void Set()
            {
                Prop2 = 200;
                Prop3 = 300;
                prop4 = 400;
            }
        }

        #endregion
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace GraphExpression.Tests
{
    public class DictionaryTest
    {
        private class MyList2
        {
        }

        [Fact]
        public void CreateDictionary_ReturnExpressionAsString()
        {
            var dic = new Dictionary<string, string>();
            dic["key1"] = "value1";
            dic["key2"] = "value2";
            dic["key3"] = "value3";

            var enumerator = ((IDictionary)dic).GetEnumerator();
            enumerator.MoveNext();
            var key1 = enumerator.Current.GetHashCode();
            enumerator.MoveNext();
            var key2 = enumerator.Current.GetHashCode();
            enumerator.MoveNext();
            var key3 = enumerator.Current.GetHashCode();

            var expression = dic.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{dic.GetType().Name}.{dic.GetHashCode()}}} + ({{[0].{key1}}} + {{@Key: \"key1\"}} + {{@Value: \"value1\"}}) + ({{[1].{key2}}} + {{@Key: \"key2\"}} + {{@Value: \"value2\"}}) + ({{[2].{key3}}} + {{@Key: \"key3\"}} + {{@Value: \"value3\"}}) + {{@Comparer.{dic.Comparer.GetHashCode()}}} + {{@Count: 3}}";
            Assert.Equal(12, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<CollectionItemEntity>(expression[1]);
            Assert.IsType<PropertyEntity>(expression[2]);
            Assert.IsType<PropertyEntity>(expression[3]);
            Assert.IsType<CollectionItemEntity>(expression[4]);
            Assert.IsType<PropertyEntity>(expression[5]);
            Assert.IsType<PropertyEntity>(expression[6]);
            Assert.IsType<CollectionItemEntity>(expression[7]);
            Assert.IsType<PropertyEntity>(expression[8]);
            Assert.IsType<PropertyEntity>(expression[9]);
            Assert.IsType<PropertyEntity>(expression[10]);
            Assert.IsType<PropertyEntity>(expression[11]);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateComplexDictionary_ReturnExpressionAsString()
        {
            var dic = new Dictionary<MyList2, MyList2>();
            dic[new MyList2()] = new MyList2();
            dic[new MyList2()] = new MyList2();
            dic[new MyList2()] = new MyList2();

            var enumerator = ((IDictionary)dic).GetEnumerator();
            enumerator.MoveNext();
            var key1 = enumerator.Current.GetHashCode();
            enumerator.MoveNext();
            var key2 = enumerator.Current.GetHashCode();
            enumerator.MoveNext();
            var key3 = enumerator.Current.GetHashCode();

            var expression = dic.AsExpression();
            var result = expression.DefaultSerializer.Serialize();
            var expected = $"{{{dic.GetType().Name}.{dic.GetHashCode()}}} + ({{[0].{key1}}} + {{@Key.{dic.Keys.ElementAt(0).GetHashCode()}}} + {{@Value.{dic.Values.ElementAt(0).GetHashCode()}}}) + ({{[1].{key2}}} + {{@Key.{dic.Keys.ElementAt(1).GetHashCode()}}} + {{@Value.{dic.Values.ElementAt(1).GetHashCode()}}}) + ({{[2].{key3}}} + {{@Key.{dic.Keys.ElementAt(2).GetHashCode()}}} + {{@Value.{dic.Values.ElementAt(2).GetHashCode()}}}) + {{@Comparer.{dic.Comparer.GetHashCode()}}} + {{@Count: 3}}";
            Assert.Equal(12, expression.Count);
            Assert.IsType<ComplexEntity>(expression[0]);
            Assert.IsType<CollectionItemEntity>(expression[1]);
            Assert.IsType<PropertyEntity>(expression[2]);
            Assert.IsType<PropertyEntity>(expression[3]);
            Assert.IsType<CollectionItemEntity>(expression[4]);
            Assert.IsType<PropertyEntity>(expression[5]);
            Assert.IsType<PropertyEntity>(expression[6]);
            Assert.IsType<CollectionItemEntity>(expression[7]);
            Assert.IsType<PropertyEntity>(expression[8]);
            Assert.IsType<PropertyEntity>(expression[9]);
            Assert.IsType<PropertyEntity>(expression[10]);
            Assert.IsType<PropertyEntity>(expression[11]);
            Assert.Equal(expected, result);
        }
    }
}

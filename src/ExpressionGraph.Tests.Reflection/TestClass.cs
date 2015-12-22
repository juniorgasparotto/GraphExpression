using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ExpressionGraph.Tests.Reflection
{
    internal enum EnumTest
    {
        ValueA,
        ValueB,
        ValueC
    }

    internal delegate void DelegateTest(int a, string b);

    internal class ClassTestVisibility
    {
        public string FieldPublicString;
        private string FieldPrivateString;
        internal string FieldInternalString;

        public string PropPublicString { get; set; }
        private string PropPrivateString { get; set; }
        internal string PropInternalString { get; set; }
    }

    internal class ClassTestTypes
    {
        #region Types of Keywords

        public dynamic FieldPublicDynamic;
        public object FieldPublicObject;
        public int FieldPublicInt32;
        public long FieldPublicInt64;
        public ulong FieldPublicULong;
        public uint FieldPublicUInt;
        public decimal FieldPublicDecimal;
        public double FieldPublicDouble;
        public char FieldPublicChar;
        public byte FieldPublicByte;
        public bool FieldPublicBoolean;
        public sbyte FieldPublicSByte;
        public short FieldPublicShort;
        public ushort FieldPublicUShort;
        public float FieldPublicFloat;

        #endregion

        #region System

        public System.DateTime FieldPublicDateTime;
        public System.TimeSpan FieldPublicTimeSpan;
        public System.DateTimeKind FieldPublicEnumDateTimeKind;
        public System.DateTimeOffset FieldPublicDateTimeOffset;
        
        public System.IntPtr FieldPublicIntPtr;
        public System.TimeZone FieldPublicTimeZone;
        public System.TimeZoneInfo FieldPublicTimeZoneInfo;
        public System.Tuple<string, int, decimal> FieldPublicTuple;
        public System.Type FieldPublicType;
        public System.UIntPtr FieldPublicUIntPtr;
        public System.Uri FieldPublicUri;
        public System.Version FieldPublicVersion;
        public System.Guid FieldPublicGuid;
        public System.Single FieldPublicSingle;
        public System.Exception FieldPublicException;
        public System.Enum FieldPublicEnumNonGeneric;
        public System.Action FieldPublicAction;
        public System.Func<bool> FieldPublicFunc;

        #endregion

        #region Arrays and Collections

        public string[] FieldPublicArrayUni;
        public string[,] FieldPublicArrayTwo;
        public string[, ,] FieldPublicArrayThree;

        public string[][] FieldPublicJaggedArrayTwo;
        public string[][][] FieldPublicJaggedArrayThree;
        public int[][,] FieldPublicMixedArrayAndJagged;

        public System.Collections.Generic.Dictionary<string, string> FieldPublicDictionary;
        public System.Collections.Generic.List<int> FieldPublicList;
        public System.Collections.Generic.Queue<int> FieldPublicQueue;
        public System.Collections.Generic.HashSet<string> FieldPublicHashSet;
        public System.Collections.Generic.SortedSet<string> FieldPublicSortedSet;
        public System.Collections.Generic.Stack<string> FieldPublicStack;
        public System.Collections.Generic.LinkedList<string> FieldPublicLinkedList;

        public System.Collections.ObjectModel.ObservableCollection<string> FieldPublicObservableCollection;
        public System.Collections.ObjectModel.KeyedCollection<int, MyData> FieldPublicKeyedCollection;
        public System.Collections.ObjectModel.ReadOnlyCollection<string> FieldPublicReadOnlyCollection;
        public System.Collections.ObjectModel.ReadOnlyDictionary<string, string> FieldPublicReadOnlyDictionary;
        public System.Collections.ObjectModel.ReadOnlyObservableCollection<string> FieldPublicReadOnlyObservableCollection;
        public System.Collections.ObjectModel.Collection<string> FieldPublicCollection;

        public System.Collections.ArrayList FieldPublicArrayListNonGeneric;
        public System.Collections.BitArray FieldPublicBitArray;
        public System.Collections.SortedList FieldPublicSortedList;
        public System.Collections.Hashtable FieldPublicHashtableNonGeneric;
        public System.Collections.Queue FieldPublicQueueNonGeneric;
        public System.Collections.Stack FieldPublicStackNonGeneric;
        public System.Collections.IEnumerable FieldPublicIEnumerable;

        public System.Collections.Concurrent.BlockingCollection<string> FieldPublicBlockingCollection;
        public System.Collections.Concurrent.ConcurrentBag<string> FieldPublicConcurrentBag;
        public System.Collections.Concurrent.ConcurrentDictionary<string, int> FieldPublicConcurrentDictionary;
        public System.Collections.Concurrent.ConcurrentQueue<string> FieldPublicConcurrentQueue;
        public System.Collections.Concurrent.ConcurrentStack<string> FieldPublicConcurrentStack;
        //public System.Collections.Concurrent.OrderablePartitioner<string> FieldPublicOrderablePartitioner;
        //public System.Collections.Concurrent.Partitioner<string> FieldPublicPartitioner;
        //public System.Collections.Concurrent.Partitioner FieldPublicPartitionerNonGeneric;

        //public System.Collections.Specialized.BitVector32 FieldPublicBitVector32;
        public System.Collections.Specialized.HybridDictionary FieldPublicHybridDictionary;
        public System.Collections.Specialized.ListDictionary FieldPublicListDictionary;
        public System.Collections.Specialized.NameValueCollection FieldPublicNameValueCollection;
        public System.Collections.Specialized.OrderedDictionary FieldPublicOrderedDictionary;
        public System.Collections.Specialized.StringCollection FieldPublicStringCollection;

        #endregion

        #region Several

        public System.Xml.XmlDocument PropXmlDocument { get; set; }
        public System.Xml.Linq.XDocument PropXDocument { get; set; }
        public System.IO.Stream PropStream { get; set; }
        public System.Numerics.BigInteger PropBigInteger { get; set; }
        public System.Text.StringBuilder PropStringBuilder { get; set; }
        public System.Linq.IQueryable FieldPublicIQueryable;

        #endregion

        #region Custom

        public MyCollection FieldPublicMyCollection;
        public EnumTest FieldPublicEnumSpecific;

        #endregion

        public void PopulateSingles()
        {

            #region Types of Keywords

            FieldPublicDynamic = "content:FieldPublicDynamic";
            FieldPublicObject = new StringBuilder("content:FieldPublicObject - type StringBuilder");
            FieldPublicInt32 = int.MaxValue;
            FieldPublicInt64 = long.MaxValue;
            FieldPublicULong = ulong.MaxValue;
            FieldPublicUInt = uint.MaxValue;
            FieldPublicDecimal = decimal.MaxValue;
            FieldPublicDouble = double.MaxValue;
            FieldPublicChar = char.MaxValue;
            FieldPublicByte = byte.MaxValue;
            FieldPublicBoolean = true;
            FieldPublicSByte = sbyte.MaxValue;
            FieldPublicShort = short.MaxValue;
            FieldPublicUShort = ushort.MaxValue;
            FieldPublicFloat = float.MaxValue;

            #endregion

            #region System

            FieldPublicDateTime = DateTime.Now;
            FieldPublicTimeSpan = new TimeSpan(DateTime.Now.Day, DateTime.Now.Minute, DateTime.Now.Second);
            FieldPublicEnumDateTimeKind = DateTimeKind.Local;
            FieldPublicDateTimeOffset = new DateTimeOffset(FieldPublicDateTime, FieldPublicTimeSpan);

            FieldPublicIntPtr = new IntPtr(100);
            FieldPublicTimeZone = TimeZone.CurrentTimeZone;
            FieldPublicTimeZoneInfo = TimeZoneInfo.Utc;
            FieldPublicTuple = Tuple.Create<string, int, decimal>("T-string", 1, 1.1m);
            FieldPublicType = typeof(object);
            FieldPublicUIntPtr = new UIntPtr(100);
            FieldPublicUri = new Uri("http://www.g1.com.br");
            FieldPublicVersion = new Version(1, 0, 100, 1);
            FieldPublicGuid = Guid.NewGuid();
            FieldPublicSingle = Single.MaxValue;
            FieldPublicException = new Exception("Test error", new Exception("inner exception"));
            FieldPublicEnumNonGeneric = EnumTest.ValueA;
            FieldPublicAction = () => true.Equals(true);
            FieldPublicFunc = () => true;

            #endregion
            
            #region Arrays and Collections

            FieldPublicArrayUni = new string[1];
            FieldPublicArrayUni[0] = "FieldPublicArrayUni[0]";

            FieldPublicArrayTwo = new string[1,1];
            FieldPublicArrayTwo[0, 0] = "FieldPublicArrayTwo[0, 0]";

            FieldPublicArrayThree = new string[1,1,1];
            FieldPublicArrayThree[0, 0, 0] = "FieldPublicArrayThree[0, 0, 0]";

            FieldPublicJaggedArrayTwo = new string[2][];
            FieldPublicJaggedArrayTwo[0] = new string[5] { "a", "b", "c", "d", "e" };
            FieldPublicJaggedArrayTwo[1] = new string[4] { "a1", "b1", "c1", "d1" };

            FieldPublicJaggedArrayThree = new string[1][][];
            FieldPublicJaggedArrayThree[0] = new string[1][];
            FieldPublicJaggedArrayThree[0][0] = new string[1];
            FieldPublicJaggedArrayThree[0][0][0] = "string jagged array";

            FieldPublicMixedArrayAndJagged = new int[3][,]
            {
                new int[,] { {1,3}, {5,7} },
                new int[,] { {0,2}, {4,6}, {8,10} },
                new int[,] { {11,22}, {99,88}, {0,9} } 
            };

            ;

            FieldPublicDictionary = new System.Collections.Generic.Dictionary<string, string>();
            FieldPublicDictionary.Add("Key1", "Value1");
            FieldPublicDictionary.Add("Key2", "Value2");
            FieldPublicDictionary.Add("Key3", "Value3");
            FieldPublicDictionary.Add("Key4", "Value4");

            FieldPublicList = new System.Collections.Generic.List<int>();
            FieldPublicList.Add(0);
            FieldPublicList.Add(1);
            FieldPublicList.Add(2);

            FieldPublicQueue = new System.Collections.Generic.Queue<int>();
            FieldPublicQueue.Enqueue(10);
            FieldPublicQueue.Enqueue(11);
            FieldPublicQueue.Enqueue(12);

            FieldPublicHashSet = new System.Collections.Generic.HashSet<string>();
            FieldPublicHashSet.Add("FieldPublicHashSet1");
            FieldPublicHashSet.Add("FieldPublicHashSet2");
            
            FieldPublicSortedSet = new System.Collections.Generic.SortedSet<string>();
            FieldPublicSortedSet.Add("FieldPublicSortedSet1");
            FieldPublicSortedSet.Add("FieldPublicSortedSet2");
            FieldPublicSortedSet.Add("FieldPublicSortedSet3");

            FieldPublicStack = new System.Collections.Generic.Stack<string>();
            FieldPublicStack.Push("FieldPublicStack1");
            FieldPublicStack.Push("FieldPublicStack2");
            FieldPublicStack.Push("FieldPublicStack3");

            FieldPublicLinkedList = new System.Collections.Generic.LinkedList<string>();
            FieldPublicLinkedList.AddFirst("FieldPublicLinkedList1");
            FieldPublicLinkedList.AddLast("FieldPublicLinkedList2");
            FieldPublicLinkedList.AddAfter(FieldPublicLinkedList.Find("FieldPublicLinkedList1"), "FieldPublicLinkedList1.1");

            FieldPublicObservableCollection = new System.Collections.ObjectModel.ObservableCollection<string>();
            FieldPublicObservableCollection.Add("FieldPublicObservableCollection1");
            FieldPublicObservableCollection.Add("FieldPublicObservableCollection2");

            FieldPublicKeyedCollection = new MyDataKeyedCollection();
            FieldPublicKeyedCollection.Add(new MyData() { Data = "data1", Id = 0 });
            FieldPublicKeyedCollection.Add(new MyData() { Data = "data2", Id = 0 });

            var list = new List<string>();
            list.Add("list1");
            list.Add("list2");
            list.Add("list3");

            FieldPublicReadOnlyCollection = new ReadOnlyCollection<string>(list);

            FieldPublicReadOnlyDictionary = new ReadOnlyDictionary<string, string>(FieldPublicDictionary);
            FieldPublicReadOnlyObservableCollection = new ReadOnlyObservableCollection<string>(FieldPublicObservableCollection);
            FieldPublicCollection = new Collection<string>();
            FieldPublicCollection.Add("collection1");
            FieldPublicCollection.Add("collection2");
            FieldPublicCollection.Add("collection3");

            FieldPublicArrayListNonGeneric = new System.Collections.ArrayList();
            FieldPublicArrayListNonGeneric.Add(1);
            FieldPublicArrayListNonGeneric.Add("a");
            FieldPublicArrayListNonGeneric.Add(10m);

            FieldPublicBitArray = new System.Collections.BitArray(new int[] { 1,2,3 });

            FieldPublicSortedList = new System.Collections.SortedList();
            FieldPublicSortedList.Add("key1", 1);
            FieldPublicSortedList.Add("key2", 2);
            FieldPublicSortedList.Add("key3", 3);
            FieldPublicSortedList.Add(0, "value1");

            FieldPublicHashtableNonGeneric = new System.Collections.Hashtable();
            FieldPublicHashtableNonGeneric.Add("key1", 1);
            FieldPublicHashtableNonGeneric.Add("key2", 2);
            FieldPublicHashtableNonGeneric.Add("key3", 3);
            FieldPublicHashtableNonGeneric.Add(0, "value1");

            FieldPublicQueueNonGeneric = new System.Collections.Queue();
            FieldPublicQueueNonGeneric.Enqueue("FieldPublicQueueNonGeneric1");
            FieldPublicQueueNonGeneric.Enqueue("FieldPublicQueueNonGeneric2");
            FieldPublicQueueNonGeneric.Enqueue("FieldPublicQueueNonGeneric3");
            
            FieldPublicStackNonGeneric = new System.Collections.Stack();
            FieldPublicStackNonGeneric.Push("FieldPublicStackNonGeneric1");
            FieldPublicStackNonGeneric.Push("FieldPublicStackNonGeneric2");

            FieldPublicIEnumerable = FieldPublicSortedList;

            FieldPublicBlockingCollection = new System.Collections.Concurrent.BlockingCollection<string>();
            FieldPublicBlockingCollection.Add("FieldPublicBlockingCollection1");
            FieldPublicBlockingCollection.Add("FieldPublicBlockingCollection2");

            FieldPublicConcurrentBag = new System.Collections.Concurrent.ConcurrentBag<string>();
            FieldPublicConcurrentBag.Add("FieldPublicConcurrentBag1");
            FieldPublicConcurrentBag.Add("FieldPublicConcurrentBag2");
            FieldPublicConcurrentBag.Add("FieldPublicConcurrentBag3");

            FieldPublicConcurrentDictionary = new System.Collections.Concurrent.ConcurrentDictionary<string,int>();
            FieldPublicConcurrentDictionary.GetOrAdd("FieldPublicConcurrentDictionary1", 0);
            FieldPublicConcurrentDictionary.GetOrAdd("FieldPublicConcurrentDictionary2", 0);

            FieldPublicConcurrentQueue = new System.Collections.Concurrent.ConcurrentQueue<string>();
            FieldPublicConcurrentQueue.Enqueue("FieldPublicConcurrentQueue1");
            FieldPublicConcurrentQueue.Enqueue("FieldPublicConcurrentQueue2");

            FieldPublicConcurrentStack = new System.Collections.Concurrent.ConcurrentStack<string>();
            FieldPublicConcurrentStack.Push("FieldPublicConcurrentStack1");
            FieldPublicConcurrentStack.Push("FieldPublicConcurrentStack2");

            // FieldPublicOrderablePartitioner = new OrderablePartitioner();
            // FieldPublicPartitioner;
            // FieldPublicPartitionerNonGeneric;

            FieldPublicHybridDictionary = new System.Collections.Specialized.HybridDictionary();
            FieldPublicHybridDictionary.Add("FieldPublicHybridDictionaryKey1", "FieldPublicHybridDictionary1");
            FieldPublicHybridDictionary.Add("FieldPublicHybridDictionaryKey2", "FieldPublicHybridDictionary2");

            FieldPublicListDictionary = new System.Collections.Specialized.ListDictionary();
            FieldPublicListDictionary.Add("FieldPublicListDictionaryKey1", "FieldPublicListDictionary1");
            FieldPublicListDictionary.Add("FieldPublicListDictionaryKey2", "FieldPublicListDictionary2");
            FieldPublicNameValueCollection = new System.Collections.Specialized.NameValueCollection();
            FieldPublicNameValueCollection.Add(new System.Collections.Specialized.NameValueCollection());

            FieldPublicOrderedDictionary = new System.Collections.Specialized.OrderedDictionary();
            FieldPublicOrderedDictionary.Add("FieldPublicOrderedDictionaryKey1", "FieldPublicOrderedDictionary1");
            FieldPublicOrderedDictionary.Add("FieldPublicOrderedDictionaryKey2", "FieldPublicOrderedDictionary2");

            FieldPublicStringCollection = new System.Collections.Specialized.StringCollection();
            FieldPublicStringCollection.Add("FieldPublicStringCollection1");
            FieldPublicStringCollection.Add("FieldPublicStringCollection2");

            #endregion


            #region Several

            PropXmlDocument = new XmlDocument();
            PropXmlDocument.LoadXml("<xml>something</xml>");

            var tr = new StringReader("<Root>Content</Root>");
            PropXDocument = XDocument.Load(tr);
            PropStream = GenerateStreamFromString("PropStream");
            PropBigInteger = new System.Numerics.BigInteger(100);
            PropStringBuilder = new StringBuilder("PropStringBuilder");
            FieldPublicIQueryable = FieldPublicStringCollection.AsQueryable();

            #endregion
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}

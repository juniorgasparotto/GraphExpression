using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

internal enum EnumTest
{
    ValueA,
    ValueB,
    ValueC
}

internal delegate string DelegateTest(int a, string b);

internal class ClassTestVisibility
{
    public string FieldPublicString;
    private string FieldPrivateString;
    internal string FieldInternalString;

    public string PropPublicString { get; set; }
    private string PropPrivateString { get; set; }
    internal string PropInternalString { get; set; }
}

internal class SeveralTypesTest
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
    public System.Action<string, int> FieldPublicAction2;
    public System.Func<bool> FieldPublicFunc;
    public System.Func<string, int, bool> FieldPublicFunc2;

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

    public MyCollectionPublicGetEnumerator FieldPublicMyCollectionPublicGetEnumerator;
    public MyCollectionInheritsPublicGetEnumerator FieldPublicMyCollectionInheritsPublicGetEnumerator;
    public MyCollectionExplicitGetEnumerator FieldPublicMyCollectionExplicitGetEnumerator;
    public MyCollectionInheritsExplicitGetEnumerator FieldPublicMyCollectionInheritsExplicitGetEnumerator;

    public EnumTest FieldPublicEnumSpecific;
    public DelegateTest MyDelegate { get; set; }
    public event DelegateTest MyEvent;

    #endregion

    public void Populate()
    {
        #region Types of Keywords

        FieldPublicDynamic = "Dynamic - String";
        FieldPublicObject = new StringBuilder("Object - StringBuilder");
        FieldPublicInt32 = int.MaxValue;
        FieldPublicInt64 = long.MaxValue;
        FieldPublicULong = ulong.MaxValue;
        FieldPublicUInt = uint.MaxValue;
        FieldPublicDecimal = 100000.999999m;
        FieldPublicDouble = 100000.999999d;
        FieldPublicChar = 'A';
        FieldPublicByte = byte.MaxValue;
        FieldPublicBoolean = true;
        FieldPublicSByte = sbyte.MaxValue;
        FieldPublicShort = short.MaxValue;
        FieldPublicUShort = ushort.MaxValue;
        FieldPublicFloat = 100000.675555f;

        #endregion

        #region System

        FieldPublicDateTime = new DateTime(2000, 1, 1, 1, 1, 1);
        FieldPublicTimeSpan = new TimeSpan(1, 10, 40);
        FieldPublicEnumDateTimeKind = DateTimeKind.Local;

        // Instantiate date and time using Persian calendar with years,
        // months, days, hours, minutes, seconds, and milliseconds
        FieldPublicDateTimeOffset = new DateTimeOffset(1387, 2, 12, 8, 6, 32, 545,
                                 new System.Globalization.PersianCalendar(),
                                 new TimeSpan(1, 0, 0));
            
        FieldPublicIntPtr = new IntPtr(100);
        FieldPublicTimeZone = TimeZone.CurrentTimeZone;
        FieldPublicTimeZoneInfo = TimeZoneInfo.Utc;
        FieldPublicTuple = Tuple.Create<string, int, decimal>("T-string", 1, 1.1m);
        FieldPublicType = typeof(object);
        FieldPublicUIntPtr = new UIntPtr(100);
        FieldPublicUri = new Uri("http://www.site.com");
        FieldPublicVersion = new Version(1, 0, 100, 1);
        FieldPublicGuid = new Guid("d5010f5b-0cd1-44ca-aacb-5678b9947e6c");
        FieldPublicSingle = Single.MaxValue;
        FieldPublicException = new Exception("Test error", new Exception("inner exception"));
        FieldPublicEnumNonGeneric = EnumTest.ValueA;
        FieldPublicAction = () => true.Equals(true);
        FieldPublicAction2 = (a, b) => true.Equals(true);
        FieldPublicFunc = () => true;
        FieldPublicFunc2 = (a, b) => true;

        #endregion
            
        #region Arrays and Collections

        FieldPublicArrayUni = new string[2];
        FieldPublicArrayUni[0] = "[0]";
        FieldPublicArrayUni[1] = "[1]";

        FieldPublicArrayTwo = new string[2,2];
        FieldPublicArrayTwo[0, 0] = "[0, 0]";
        FieldPublicArrayTwo[0, 1] = "[0, 1]";
        FieldPublicArrayTwo[1, 0] = "[1, 0]";
        FieldPublicArrayTwo[1, 1] = "[1, 1]";

        FieldPublicArrayThree = new string[1,1,2];
        FieldPublicArrayThree[0, 0, 0] = "[0, 0, 0]";
        FieldPublicArrayThree[0, 0, 1] = "[0, 0, 1]";

        FieldPublicJaggedArrayTwo = new string[2][];
        FieldPublicJaggedArrayTwo[0] = new string[5] { "a", "b", "c", "d", "e" };
        FieldPublicJaggedArrayTwo[1] = new string[4] { "a1", "b1", "c1", "d1" };

        FieldPublicJaggedArrayThree = new string[1][][];
        FieldPublicJaggedArrayThree[0] = new string[1][];
        FieldPublicJaggedArrayThree[0][0] = new string[2];
        FieldPublicJaggedArrayThree[0][0][0] = "[0][0][0]";
        FieldPublicJaggedArrayThree[0][0][1] = "[0][0][1]";

        FieldPublicMixedArrayAndJagged = new int[3][,]
        {
            new int[,] { {1,3}, {5,7} },
            new int[,] { {0,2}, {4,6}, {8,10} },
            new int[,] { {11,22}, {99,88}, {0,9} } 
        };

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
        FieldPublicHashSet.Add("HashSet1");
        FieldPublicHashSet.Add("HashSet2");
            
        FieldPublicSortedSet = new System.Collections.Generic.SortedSet<string>();
        FieldPublicSortedSet.Add("SortedSet1");
        FieldPublicSortedSet.Add("SortedSet2");
        FieldPublicSortedSet.Add("SortedSet3");

        FieldPublicStack = new System.Collections.Generic.Stack<string>();
        FieldPublicStack.Push("Stack1");
        FieldPublicStack.Push("Stack2");
        FieldPublicStack.Push("Stack3");

        FieldPublicLinkedList = new System.Collections.Generic.LinkedList<string>();
        FieldPublicLinkedList.AddFirst("LinkedList1");
        FieldPublicLinkedList.AddLast("LinkedList2");
        FieldPublicLinkedList.AddAfter(FieldPublicLinkedList.Find("LinkedList1"), "LinkedList1.1");

        FieldPublicObservableCollection = new System.Collections.ObjectModel.ObservableCollection<string>();
        FieldPublicObservableCollection.Add("ObservableCollection1");
        FieldPublicObservableCollection.Add("ObservableCollection2");

        FieldPublicKeyedCollection = new MyDataKeyedCollection();
        FieldPublicKeyedCollection.Add(new MyData() { Data = "data1", Id = 0 });
        FieldPublicKeyedCollection.Add(new MyData() { Data = "data2", Id = 1 });

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
        FieldPublicArrayListNonGeneric.Add(10.0m);
        FieldPublicArrayListNonGeneric.Add(new DateTime(2000, 01, 01));

        FieldPublicBitArray = new System.Collections.BitArray(3);
        FieldPublicBitArray[2] = true;

        FieldPublicSortedList = new System.Collections.SortedList();
        FieldPublicSortedList.Add("key1", 1);
        FieldPublicSortedList.Add("key2", 2);
        FieldPublicSortedList.Add("key3", 3);
        FieldPublicSortedList.Add("key4", 4);

        FieldPublicHashtableNonGeneric = new System.Collections.Hashtable();
        FieldPublicHashtableNonGeneric.Add("key1", 1);
        FieldPublicHashtableNonGeneric.Add("key2", 2);
        FieldPublicHashtableNonGeneric.Add("key3", 3);
        FieldPublicHashtableNonGeneric.Add("key4", 4);

        FieldPublicQueueNonGeneric = new System.Collections.Queue();
        FieldPublicQueueNonGeneric.Enqueue("QueueNonGeneric1");
        FieldPublicQueueNonGeneric.Enqueue("QueueNonGeneric2");
        FieldPublicQueueNonGeneric.Enqueue("QueueNonGeneric3");
            
        FieldPublicStackNonGeneric = new System.Collections.Stack();
        FieldPublicStackNonGeneric.Push("StackNonGeneric1");
        FieldPublicStackNonGeneric.Push("StackNonGeneric2");

        FieldPublicIEnumerable = FieldPublicSortedList;

        FieldPublicBlockingCollection = new System.Collections.Concurrent.BlockingCollection<string>();
        FieldPublicBlockingCollection.Add("BlockingCollection1");
        FieldPublicBlockingCollection.Add("BlockingCollection2");

        FieldPublicConcurrentBag = new System.Collections.Concurrent.ConcurrentBag<string>();
        FieldPublicConcurrentBag.Add("ConcurrentBag1");
        FieldPublicConcurrentBag.Add("ConcurrentBag2");
        FieldPublicConcurrentBag.Add("ConcurrentBag3");

        FieldPublicConcurrentDictionary = new System.Collections.Concurrent.ConcurrentDictionary<string,int>();
        FieldPublicConcurrentDictionary.GetOrAdd("ConcurrentDictionary1", 0);
        FieldPublicConcurrentDictionary.GetOrAdd("ConcurrentDictionary2", 0);

        FieldPublicConcurrentQueue = new System.Collections.Concurrent.ConcurrentQueue<string>();
        FieldPublicConcurrentQueue.Enqueue("ConcurrentQueue1");
        FieldPublicConcurrentQueue.Enqueue("ConcurrentQueue2");

        FieldPublicConcurrentStack = new System.Collections.Concurrent.ConcurrentStack<string>();
        FieldPublicConcurrentStack.Push("ConcurrentStack1");
        FieldPublicConcurrentStack.Push("ConcurrentStack2");

        // FieldPublicOrderablePartitioner = new OrderablePartitioner();
        // FieldPublicPartitioner;
        // FieldPublicPartitionerNonGeneric;

        FieldPublicHybridDictionary = new System.Collections.Specialized.HybridDictionary();
        FieldPublicHybridDictionary.Add("HybridDictionaryKey1", "HybridDictionary1");
        FieldPublicHybridDictionary.Add("HybridDictionaryKey2", "HybridDictionary2");

        FieldPublicListDictionary = new System.Collections.Specialized.ListDictionary();
        FieldPublicListDictionary.Add("ListDictionaryKey1", "ListDictionary1");
        FieldPublicListDictionary.Add("ListDictionaryKey2", "ListDictionary2");
        FieldPublicNameValueCollection = new System.Collections.Specialized.NameValueCollection();
        FieldPublicNameValueCollection.Add(new System.Collections.Specialized.NameValueCollection());

        FieldPublicOrderedDictionary = new System.Collections.Specialized.OrderedDictionary();
        FieldPublicOrderedDictionary.Add("OrderedDictionaryKey1", "OrderedDictionary1");
        FieldPublicOrderedDictionary.Add("OrderedDictionaryKey2", "OrderedDictionary2");

        FieldPublicStringCollection = new System.Collections.Specialized.StringCollection();
        FieldPublicStringCollection.Add("StringCollection1");
        FieldPublicStringCollection.Add("StringCollection2");

        #endregion

        #region Several

        PropXmlDocument = new XmlDocument();
        PropXmlDocument.LoadXml("<xml>something</xml>");

        var tr = new StringReader("<Root>Content</Root>");
        PropXDocument = XDocument.Load(tr);
        PropStream = GenerateStreamFromString("Stream");
        PropBigInteger = new System.Numerics.BigInteger(100);
        PropStringBuilder = new StringBuilder("StringBuilder");
        FieldPublicIQueryable = new List<string>() { "IQueryable" }.AsQueryable();

        #endregion

        #region Custom

        FieldPublicMyCollectionPublicGetEnumerator = new MyCollectionPublicGetEnumerator("a b c", new char[] { ' ' } );
        FieldPublicMyCollectionInheritsPublicGetEnumerator = new MyCollectionInheritsPublicGetEnumerator("a b c", new char[] { ' ' });
        FieldPublicMyCollectionExplicitGetEnumerator = new MyCollectionExplicitGetEnumerator("a b c", new char[] { ' ' });
        FieldPublicMyCollectionInheritsExplicitGetEnumerator = new MyCollectionInheritsExplicitGetEnumerator("a b c", new char[] { ' ' });
        FieldPublicEnumSpecific = EnumTest.ValueB;
        MyDelegate = MethodDelegate;
        MyEvent += new DelegateTest(MyDelegate);
        MyEvent += new DelegateTest(MyDelegate);

        #endregion
    }

    public string MethodDelegate(int a, string b)
    {
        return "A";
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

public class MyCollectionExplicitGetEnumerator : IEnumerable
{
    private string[] elements;

    public MyCollectionExplicitGetEnumerator(string source, char[] delimiters)
    {
        // The constructor parses the string argument into tokens.
        elements = source.Split(delimiters);
    }

    // The IEnumerable interface requires implementation of method GetEnumerator.
    IEnumerator IEnumerable.GetEnumerator()
    {
        return elements.GetEnumerator();
    }
}

public class MyCollectionInheritsExplicitGetEnumerator : MyCollectionExplicitGetEnumerator
{
    public MyCollectionInheritsExplicitGetEnumerator(string source, char[] delimiters)
        : base(source, delimiters)
    {
    }
}


public class MyCollectionPublicGetEnumerator : IEnumerable
{
    private string[] elements;

    public MyCollectionPublicGetEnumerator(string source, char[] delimiters)
    {
        // The constructor parses the string argument into tokens.
        elements = source.Split(delimiters);
    }

    // The IEnumerable interface requires implementation of method GetEnumerator.
    public IEnumerator GetEnumerator()
    {
        return elements.GetEnumerator();
    }
}

public class MyCollectionInheritsPublicGetEnumerator : MyCollectionPublicGetEnumerator
{
    public MyCollectionInheritsPublicGetEnumerator(string source, char[] delimiters)
        : base(source, delimiters)
    {
    }
}

public class MyDataKeyedCollection : KeyedCollection<int, MyData>
{
    protected override int GetKeyForItem(MyData item)
    {
        return item.Id;
    }
}

public class MyData
{
    public int Id { get; set; }
    public string Data { get; set; }
}

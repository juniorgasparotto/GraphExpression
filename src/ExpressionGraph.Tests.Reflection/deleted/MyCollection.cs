//using System.Collections;
//using System.Collections.ObjectModel;

//// Declare the Tokens class. The class implements the IEnumerable interface.
//public class MyCollection : IEnumerable
//{
//    private string[] elements;

//    MyCollection(string source, char[] delimiters)
//    {
//        // The constructor parses the string argument into tokens.
//        elements = source.Split(delimiters);
//    }

//    // The IEnumerable interface requires implementation of method GetEnumerator.
//    public IEnumerator GetEnumerator()
//    {
//        return new MyCollectionEnumerator(this);
//    }


//    // Declare an inner class that implements the IEnumerator interface.
//    private class MyCollectionEnumerator : IEnumerator
//    {
//        private int position = -1;
//        private MyCollection t;

//        public MyCollectionEnumerator(MyCollection t)
//        {
//            this.t = t;
//        }

//        // The IEnumerator interface requires a MoveNext method.
//        public bool MoveNext()
//        {
//            if (position < t.elements.Length - 1)
//            {
//                position++;
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        // The IEnumerator interface requires a Reset method.
//        public void Reset()
//        {
//            position = -1;
//        }

//        // The IEnumerator interface requires a Current method.
//        public object Current
//        {
//            get
//            {
//                return t.elements[position];
//            }
//        }
//    }
//}


//// KeyedCollection is an abstract class, so have to derive
//public class MyDataKeyedCollection : KeyedCollection<int, MyData>
//{
//    protected override int GetKeyForItem(MyData item)
//    {
//        return item.Id;
//    }
//}

//public class MyData
//{
//    public int Id { get; set; }
//    public string Data { get; set; }
//}
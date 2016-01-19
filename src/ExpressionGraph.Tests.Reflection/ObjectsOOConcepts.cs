using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ExpressionGraph.Tests
{
    public interface ILevel3
    {
        string C { get; }
    }

    public interface ILevel2 : ILevel3
    {
        string B { get; }
        
        string this[int index]
        {
            get;
        }
    }

    public interface ILevel1 : ILevel2
    {
        string A { get; }
    }

    public interface IGrandfather
    {
        string BaseProp { get; }
    }

    public abstract class Grandfather: IGrandfather
    {
        private string Field1 = "field1";
        private const string Constant1 = "constant1a";

        private string BaseA
        {
            get { return "base.base.a"; }
        }

        public virtual string BaseB
        {
            get { return "base.base.b"; }
        }

        protected string BaseC
        {
            get { return "base.base.c"; }
        }

        private string D
        {
            get { return "base.base.d"; }
        }

        public virtual string E
        {
            get { return "base.base.e"; }
        }

        public string F
        {
            get
            {
                return "base.base.f";
            }
        }

        string IGrandfather.BaseProp
        {
            get { return "base.IGrandfather.BaseProp"; }
        }

        public static string StaticA
        {
            get
            {
                return "base.BaseStaticA";
            }
        }

        public string this[int index]
        {
            get
            {
                return "base.base.this[int index]";
            }
        }

        public string this[int index, int index2]
        {
            get
            {
                return "base.base.this[int index, int index2]";
            }
        }

        public string this[string contains]
        {
            get
            {
                return "base.base.this[string contains]";
            }
        }

        internal string GrandGatherMethod()
        {
            return "Hi!";
        }
    }

    public class Father : Grandfather, IGrandfather
    {
        private string Field1 = "field1";
        private const string Constant1 = "constant1a";

        private string BaseA
        {
            get { return "base.a"; }
        }

        sealed public override string BaseB
        {
            get { return "base.b"; }
        }

        protected string BaseC
        {
            get { return "base.c"; }
        }

        private string D
        {
            get { return "base.d"; }
        }

        public virtual string E
        {
            get { return "base.e"; }
        }

        public string F
        {
            get
            {
                return "base.f";
            }
        }

        string IGrandfather.BaseProp
        {
            get { return "IGrandfather.BaseProp"; }
        }

        public static string StaticA
        {
            get
            {
                return "BaseStaticA";
            }
        }

        public string this[int index]
        {
            get
            {
                return "base.this[int index]";
            }
        }

        public string this[int index, int index2]
        {
            get
            {
                return "base.this[int index, int index2]";
            }
        }

        public string this[string contains]
        {
            get
            {
                return "base.this[string contains]";
            }
        }
    }

    public class Son : Father, ILevel1
    {
        private string Field1 = "field1";
        private const string Constant1 = "constant1a";

        public List<string> list = new List<string>();
        public static List<string> listStatic = new List<string>();

        public Son()
        {
            list.Add("list 0");
            list.Add("list 1");
            list.Add("list 2");
            list.Add("list 3");

            listStatic.Add("list 0");
            listStatic.Add("list 1");
            listStatic.Add("list 2");
            listStatic.Add("list 3");
        }

        private string StaticA
        {
            get
            {
                return "StaticA";
            }
        }

        public string StaticB
        {
            get
            {
                return "StaticB";
            }
        }

        public List<string> List
        {
            get
            {
                return list;
            }
        }

        [System.Runtime.CompilerServices.IndexerName("TesteIndexer")]
        public string this[int index]
        {
            get
            {
                return list[index];
            }
        }

        [System.Runtime.CompilerServices.IndexerName("TesteIndexer")]
        public string this[int index, int index2]
        {
            get
            {
                return list[index];
            }
        }

        [System.Runtime.CompilerServices.IndexerName("TesteIndexer")]
        public string this[string contains]
        {
            get
            {
                return list.FirstOrDefault(f => f == contains);
            }
        }

        public void Add(string value)
        {
            this.list.Add(value);
        }

        string ILevel1.A
        {
            get
            {
                return "A.A";
            }
        }

        private string A
        {
            get
            {
                return "A";
            }
        }

        public string B
        {
            get
            {
                return "B";
            }
        }

        public string C
        {
            get
            {
                return "C";
            }
        }

        public string D
        {
            get
            {
                return "D";
            }
        }

        public override string E
        {
            get
            {
                return "E";
            }
        }


        public string F
        {
            get
            {
                return "F";
            }
        }

        protected internal int OnlySetProperty
        {
            set
            {

            }
        }

        [DllImport("kernel32.dll")]
        public static extern bool Beep(int frequency, int duration);
        public bool Public(bool param1) 
        {
            return false;
        }
        private bool Private(bool param1)
        {
            return false;
        }
    }
}
